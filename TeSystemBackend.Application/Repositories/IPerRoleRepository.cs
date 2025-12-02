using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface IPerRoleRepository
{
    Task<List<PerRole>> GetByRoleIdAsync(int roleId);
    Task<List<string>> GetPermissionNamesByRoleIdAsync(int roleId);
    Task AddAsync(PerRole perRole);
    Task AddRangeAsync(List<PerRole> perRoles);
    Task RemoveRangeAsync(List<PerRole> perRoles);
}
