using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Text;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Auth;
using TeSystemBackend.Application.DTOs.Users;
using TeSystemBackend.Application.Helpers;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Application.Services;
using TeSystemBackend.Application.Validators.Auth;
using TeSystemBackend.Application.Validators.Users;
using TeSystemBackend.Application.Validators.Computers;
using TeSystemBackend.Application.Validators.Departments;
using TeSystemBackend.Application.Validators.Teams;
using TeSystemBackend.Application.DTOs.Computers;
using TeSystemBackend.Application.DTOs.Departments;
using TeSystemBackend.Application.DTOs.Teams;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString(ConfigurationKeys.DefaultConnection);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException(ErrorMessages.ConnectionStringNotFound);
                }
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));
            });

            builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            var jwtConfig = JwtConfiguration.FromConfiguration(builder.Configuration);
            builder.Services.AddSingleton(jwtConfig);

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = !string.IsNullOrWhiteSpace(jwtConfig.Issuer),
                        ValidateAudience = !string.IsNullOrWhiteSpace(jwtConfig.Audience),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1),
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
                    };
                });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IComputerRepository, ComputerRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<ITeamRepository, TeamRepository>();
            
            builder.Services.AddScoped<IIdentityRoleService, IdentityRoleService>();
            builder.Services.AddScoped<IUserTeamService, UserTeamService>();
            builder.Services.AddScoped<ITeamRoleLocationService, TeamRoleLocationService>();
            builder.Services.AddScoped<IAppAuthorizationService, AppAuthorizationService>();
            builder.Services.AddScoped<IPermissionService, PermissionService>();
            builder.Services.AddScoped<IAdminSeedService, AdminSeedService>();
            
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IComputerService, ComputerService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<ITeamService, TeamService>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
            builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            builder.Services.AddScoped<IValidator<RefreshTokenRequest>, RefreshTokenRequestValidator>();
            builder.Services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordRequestValidator>();
            builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
            builder.Services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();
            builder.Services.AddScoped<IValidator<CreateComputerDto>, CreateComputerDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateComputerDto>, UpdateComputerDtoValidator>();
            builder.Services.AddScoped<IValidator<CreateDepartmentDto>, CreateDepartmentDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateDepartmentDto>, UpdateDepartmentDtoValidator>();
            builder.Services.AddScoped<IValidator<CreateTeamDto>, CreateTeamDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateTeamDto>, UpdateTeamDtoValidator>();

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                            .SelectMany(kvp => kvp.Value!.Errors
                                .Select(e => new
                                {
                                    Field = kvp.Key,
                                    Error = e.ErrorMessage
                                }))
                            .ToList();

                        var message = errors.Count > 0
                            ? errors[0].Error
                            : ErrorMessages.ValidationFailed;

                        var response = ApiResponse<object>.Fail(ErrorCodes.ValidationFailed, message);
                        response.Data = errors;

                        return new ObjectResult(response)
                        {
                            StatusCode = ErrorCodes.ValidationFailed
                        };
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", policy => 
                    policy.RequireRole(Roles.Admin));
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
                    await permissionService.EnsureRolesAndPermissionsExistAsync();

                    var adminSeedService = scope.ServiceProvider.GetRequiredService<IAdminSeedService>();
                    await adminSeedService.EnsureAdminExistsAsync();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while initializing system");
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
