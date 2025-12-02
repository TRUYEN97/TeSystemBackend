using Microsoft.AspNetCore.Identity;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class IdentityRoleService : IIdentityRoleService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public IdentityRoleService(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task EnsureRoleExistsAsync(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
        }
    }

    public async Task AssignRoleToUserAsync(int userId, string roleName)
    {
        await EnsureRoleExistsAsync(roleName);

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KeyNotFoundException(ErrorMessages.UserNotFound);

        if (!await _userManager.IsInRoleAsync(user, roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
    }

    public async Task RemoveRoleFromUserAsync(int userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KeyNotFoundException(ErrorMessages.UserNotFound);

        if (await _userManager.IsInRoleAsync(user, roleName))
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }
    }

    public async Task<bool> UserHasRoleAsync(int userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

        return await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<List<string>> GetUserRolesAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return new List<string>();

        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }
}
