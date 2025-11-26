using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface IComputerRepository
{
    Task<Computer?> GetByIdAsync(int id);
    Task<Computer?> GetByCodeAsync(string code);
    Task AddAsync(Computer computer);
}


