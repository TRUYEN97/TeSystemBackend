using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs.Departments;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentService(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
    {
        _departmentRepository = departmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<DepartmentDto>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(MapToDto).ToList();
    }

    public async Task<DepartmentDto> GetByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            throw new KeyNotFoundException(ErrorMessages.DepartmentNotFound);
        }

        return MapToDto(department);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto request)
    {
        var existing = await _departmentRepository.GetByNameAsync(request.Name);
        if (existing != null)
        {
            throw new InvalidOperationException(ErrorMessages.DepartmentNameAlreadyExists);
        }

        var department = new Department
        {
            Name = request.Name
        };

        await _departmentRepository.AddAsync(department);
        await _unitOfWork.SaveChangesAsync();

        var created = await _departmentRepository.GetByIdAsync(department.Id);
        return MapToDto(created!);
    }

    public async Task<DepartmentDto> UpdateAsync(int id, UpdateDepartmentDto request)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            throw new KeyNotFoundException(ErrorMessages.DepartmentNotFound);
        }

        if (!string.Equals(department.Name, request.Name, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _departmentRepository.GetByNameAsync(request.Name);
            if (existing != null && existing.Id != id)
            {
                throw new InvalidOperationException(ErrorMessages.DepartmentNameAlreadyExists);
            }
        }

        department.Name = request.Name;

        await _departmentRepository.UpdateAsync(department);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _departmentRepository.GetByIdAsync(id);
        return MapToDto(updated!);
    }

    public async Task DeleteAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            return;
        }

        await _departmentRepository.DeleteAsync(department);
        await _unitOfWork.SaveChangesAsync();
    }

    private static DepartmentDto MapToDto(Department department)
    {
        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name
        };
    }
}

