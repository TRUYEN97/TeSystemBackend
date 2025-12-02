using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PermissionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task EnsurePermissionsExistAsync()
    {
        var allPermissions = new[]
        {
            Permissions.ComputerView,
            Permissions.ComputerCreate,
            Permissions.ComputerUpdate,
            Permissions.ComputerDelete,
            Permissions.TeamView,
            Permissions.TeamCreate,
            Permissions.TeamUpdate,
            Permissions.TeamDelete,
            Permissions.DepartmentView,
            Permissions.DepartmentCreate,
            Permissions.DepartmentUpdate,
            Permissions.DepartmentDelete,
            Permissions.LocationView,
            Permissions.LocationCreate,
            Permissions.LocationUpdate,
            Permissions.LocationDelete
        };

        foreach (var permissionName in allPermissions)
        {
            var exists = await _unitOfWork.Permissions.ExistsAsync(permissionName);
            
            if (!exists)
            {
                await _unitOfWork.Permissions.AddAsync(new Permission { Name = permissionName });
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task EnsureRolesAndPermissionsExistAsync()
    {
        await EnsurePermissionsExistAsync();

        var pcManagerRole = await EnsureRoleExistsAsync(Roles.PcManager);
        var pcManagerPermissions = new[]
        {
            Permissions.ComputerView,
            Permissions.ComputerCreate,
            Permissions.ComputerUpdate,
            Permissions.ComputerDelete
        };
        await AssignPermissionsToRoleByNameAsync(pcManagerRole.Id, pcManagerPermissions);

        var pcViewerRole = await EnsureRoleExistsAsync(Roles.PcViewer);
        var pcViewerPermissions = new[] { Permissions.ComputerView };
        await AssignPermissionsToRoleByNameAsync(pcViewerRole.Id, pcViewerPermissions);

        var teamLeadRole = await EnsureRoleExistsAsync(Roles.TeamLead);
        var teamLeadPermissions = new[]
        {
            Permissions.TeamView,
            Permissions.TeamCreate,
            Permissions.TeamUpdate,
            Permissions.ComputerView
        };
        await AssignPermissionsToRoleByNameAsync(teamLeadRole.Id, teamLeadPermissions);
    }

    private async Task<Role> EnsureRoleExistsAsync(string roleName)
    {
        var role = await _unitOfWork.Roles.GetByNameAsync(roleName);
        
        if (role == null)
        {
            role = new Role { Name = roleName };
            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();
        }

        return role;
    }

    private async Task AssignPermissionsToRoleByNameAsync(int roleId, string[] permissionNames)
    {
        var permissions = await _unitOfWork.Permissions.GetByNameListAsync(permissionNames.ToList());
        var permissionIds = permissions.Select(p => p.Id).ToList();

        if (permissionIds.Count != permissionNames.Length)
        {
            var foundNames = permissions.Select(p => p.Name).ToList();
            var missing = permissionNames.Where(name => !foundNames.Contains(name)).ToList();
            
            if (missing.Any())
            {
                throw new InvalidOperationException($"Missing permissions: {string.Join(", ", missing)}");
            }
        }

        await AssignPermissionsToRoleAsync(roleId, permissionIds);
    }

    public async Task AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
    {
        var existingPerRoles = await _unitOfWork.PerRoles.GetByRoleIdAsync(roleId);
        await _unitOfWork.PerRoles.RemoveRangeAsync(existingPerRoles);

        var newPerRoles = permissionIds.Select(permissionId => new PerRole
        {
            RoleId = roleId,
            PermissionId = permissionId
        }).ToList();

        await _unitOfWork.PerRoles.AddRangeAsync(newPerRoles);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<string>> GetRolePermissionsAsync(int roleId)
    {
        return await _unitOfWork.PerRoles.GetPermissionNamesByRoleIdAsync(roleId);
    }
}
