namespace TeSystemBackend.Application.Services;

public interface IPermissionService
{
    Task EnsurePermissionsExistAsync();
    Task EnsureRolesAndPermissionsExistAsync();
    Task AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);
    Task<List<string>> GetRolePermissionsAsync(int roleId);
}
