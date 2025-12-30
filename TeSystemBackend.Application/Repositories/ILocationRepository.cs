using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(int id);
    Task<Location?> GetByNameAsync(string name);
    Task<Location?> GetByNameAndParentIdAsync(string name, int? parentId);
    Task<List<Location>> GetAllAsync();
    Task AddAsync(Location location);
    Task UpdateAsync(Location location);
    Task DeleteAsync(Location location);
}
