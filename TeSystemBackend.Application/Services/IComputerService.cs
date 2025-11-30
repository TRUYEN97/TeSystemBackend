using TeSystemBackend.Application.DTOs.Computers;

namespace TeSystemBackend.Application.Services;

public interface IComputerService
{
    Task<List<ComputerDto>> GetAllAsync();
    Task<ComputerDto> GetByIdAsync(int id);
    Task<ComputerDto> CreateAsync(CreateComputerDto request);
    Task<ComputerDto> UpdateAsync(int id, UpdateComputerDto request);
    Task DeleteAsync(int id);
}

