using TeSystemBackend.Application.DTOs.Departments;

namespace TeSystemBackend.Application.Services;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto> GetByIdAsync(int id);
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto request);
    Task<DepartmentDto> UpdateAsync(int id, UpdateDepartmentDto request);
    Task DeleteAsync(int id);
}

