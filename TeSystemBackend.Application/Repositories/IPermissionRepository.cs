using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(int id);
    Task<Permission?> GetByNameAsync(string name);
    Task<List<Permission>> GetByNameListAsync(List<string> names);
    Task<List<Permission>> GetAllAsync();
    Task AddAsync(Permission permission);
    Task<bool> ExistsAsync(string name);
}
