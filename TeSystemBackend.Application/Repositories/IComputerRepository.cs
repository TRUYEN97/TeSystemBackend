using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface IComputerRepository
{
    Task<Computer?> GetByIdAsync(int id);
    Task<Computer?> GetByIpAsync(string code);
    Task<List<Computer>> GetAllAsync();
    Task AddAsync(Computer computer);
    Task UpdateAsync(Computer computer);
    Task DeleteAsync(Computer computer);
}




