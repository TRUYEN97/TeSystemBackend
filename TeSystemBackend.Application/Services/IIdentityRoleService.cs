namespace TeSystemBackend.Application.Services;

public interface IIdentityRoleService
{
    Task EnsureRoleExistsAsync(string roleName);
    Task AssignRoleToUserAsync(int userId, string roleName);
    Task RemoveRoleFromUserAsync(int userId, string roleName);
    Task<bool> UserHasRoleAsync(int userId, string roleName);
    Task<List<string>> GetUserRolesAsync(int userId);
    Task<int> GetUserCountByRoleAsync(string roleName);
}
