using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;
using TeSystemBackend.Infrastructure.Helpers;

namespace TeSystemBackend.API.Helpers;

public static class ControllerHelper
{
    public static async Task<AppUser> GetCurrentUserAsync(
        ClaimsPrincipal user,
        UserManager<AppUser> userManager)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) 
            ?? user.FindFirst("sub");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("Không thể xác định user.");
        }

        var appUser = await userManager.FindByIdAsync(userId.ToString());
        if (appUser == null)
        {
            throw new UnauthorizedAccessException("User không tồn tại.");
        }

        return appUser;
    }

    public static async Task EnsureAdminAsync(
        ClaimsPrincipal user,
        UserManager<AppUser> userManager)
    {
        var currentUser = await GetCurrentUserAsync(user, userManager);
        if (!AuthorizationHelper.IsAdmin(currentUser))
        {
            throw new UnauthorizedAccessException("Chỉ admin mới có quyền thực hiện thao tác này.");
        }
    }
}

