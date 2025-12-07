using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface IComputerRepository
{
    Task<Computer?> GetByIdAsync(int id);
    Task<Computer?> GetByIpAsync(string ipAddress);
    Task<List<Computer>> GetAllAsync();
    Task<List<Computer>> GetByLocationIdAsync(int locationId);
    Task<Dictionary<int, List<Computer>>> GetByLocationIdsAsync(List<int> locationIds);
    Task<int> CountByLocationIdAsync(int locationId);
    Task<Dictionary<int, int>> CountByLocationIdsAsync(List<int> locationIds);
    Task AddAsync(Computer computer);
    Task UpdateAsync(Computer computer);
    Task DeleteAsync(Computer computer);
}




