using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.API.Extensions;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Departments;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly IValidator<CreateDepartmentDto> _createValidator;
    private readonly IValidator<UpdateDepartmentDto> _updateValidator;

    public DepartmentsController(
        IDepartmentService departmentService,
        IValidator<CreateDepartmentDto> createValidator,
        IValidator<UpdateDepartmentDto> updateValidator)
    {
        _departmentService = departmentService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<DepartmentDto>>> GetAll()
    {
        var departments = await _departmentService.GetAllAsync();
        return ApiResponse<List<DepartmentDto>>.Success(departments);
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResponse<DepartmentDto>> GetById(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        return ApiResponse<DepartmentDto>.Success(department);
    }

    [HttpPost]
    public async Task<ApiResponse<DepartmentDto>> Create(CreateDepartmentDto request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var department = await _departmentService.CreateAsync(request);
        return ApiResponse<DepartmentDto>.Success(department);
    }

    [HttpPut("{id:int}")]
    public async Task<ApiResponse<DepartmentDto>> Update(int id, UpdateDepartmentDto request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var department = await _departmentService.UpdateAsync(id, request);
        return ApiResponse<DepartmentDto>.Success(department);
    }

    [HttpDelete("{id:int}")]
    public async Task<ApiResponse<object>> Delete(int id)
    {
        await _departmentService.DeleteAsync(id);
        return ApiResponse<object>.Success(null!);
    }
}

