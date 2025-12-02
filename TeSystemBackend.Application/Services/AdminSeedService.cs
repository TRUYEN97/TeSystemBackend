using Microsoft.Extensions.Configuration;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class AdminSeedService : IAdminSeedService
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityRoleService _identityRoleService;
    private readonly IConfiguration _configuration;

    public AdminSeedService(
        IUserRepository userRepository,
        IIdentityRoleService identityRoleService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _identityRoleService = identityRoleService;
        _configuration = configuration;
    }

    public async Task EnsureAdminExistsAsync()
    {
        var adminEmail = _configuration["AdminSettings:Email"];
        var adminUsername = _configuration["AdminSettings:Username"];
        var adminPassword = _configuration["AdminSettings:Password"];
        var adminName = _configuration["AdminSettings:Name"];

        if (string.IsNullOrWhiteSpace(adminEmail))
        {
            return;
        }

        var existingAdmin = await _userRepository.GetByEmailAsync(adminEmail);
        if (existingAdmin != null)
        {
            return;
        }

        await _identityRoleService.EnsureRoleExistsAsync(Roles.Admin);

        var adminUser = new AppUser
        {
            UserName = adminUsername ?? adminEmail,
            Email = adminEmail,
            Name = adminName ?? "I Am Admin"
        };

        var created = await _userRepository.CreateAsync(adminUser, adminPassword ?? "Admin1");

        await _identityRoleService.AssignRoleToUserAsync(created.Id, Roles.Admin);
    }
}

