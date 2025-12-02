using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(int id);
    Task<Location?> GetByNameAsync(string name);
    Task<List<Location>> GetAllAsync();
}
