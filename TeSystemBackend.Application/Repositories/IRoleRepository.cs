using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id);
    Task<Role?> GetByNameAsync(string name);
    Task<List<Role>> GetAllAsync();
    Task AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task<bool> ExistsAsync(string name);
}
