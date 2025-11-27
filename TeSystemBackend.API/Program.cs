using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Text;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Auth;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Application.Services;
using TeSystemBackend.Application.Validators.Auth;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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

            var jwtSection = builder.Configuration.GetSection("Jwt");
            var key = Environment.GetEnvironmentVariable("JWT_KEY")
                      ?? jwtSection["Key"];
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("JWT configuration is missing 'Key'.");
            }

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
                        ValidateIssuer = !string.IsNullOrWhiteSpace(issuer),
                        ValidateAudience = !string.IsNullOrWhiteSpace(audience),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1),
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IComputerRepository, ComputerRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
            builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            builder.Services.AddScoped<IValidator<RefreshTokenRequest>, RefreshTokenRequestValidator>();

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

                        var response = ApiResponse<object>.Fail(ErrorCodes.ValidationFailed, "Validation failed");
                        response.Data = errors;

                        return new ObjectResult(response)
                        {
                            StatusCode = ErrorCodes.ValidationFailed
                        };
                    };
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
