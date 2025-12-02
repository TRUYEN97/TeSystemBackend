using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.API.Extensions;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Computers;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/computers")]
[Authorize]
public class ComputersController : ControllerBase
{
    private readonly IComputerService _computerService;
    private readonly IValidator<CreateComputerDto> _createValidator;
    private readonly IValidator<UpdateComputerDto> _updateValidator;

    public ComputersController(
        IComputerService computerService,
        IValidator<CreateComputerDto> createValidator,
        IValidator<UpdateComputerDto> updateValidator)
    {
        _computerService = computerService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<ComputerDto>>> GetAll()
    {
        var computers = await _computerService.GetAllAsync();
        return ApiResponse<List<ComputerDto>>.Success(computers);
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResponse<ComputerDto>> GetById(int id)
    {
        var computer = await _computerService.GetByIdAsync(id);
        return ApiResponse<ComputerDto>.Success(computer);
    }

    [HttpPost]
    public async Task<ApiResponse<ComputerDto>> Create(CreateComputerDto request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var computer = await _computerService.CreateAsync(request);
        return ApiResponse<ComputerDto>.Success(computer);
    }

    [HttpPut("{id:int}")]
    public async Task<ApiResponse<ComputerDto>> Update(int id, UpdateComputerDto request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var computer = await _computerService.UpdateAsync(id, request);
        return ApiResponse<ComputerDto>.Success(computer);
    }

    [HttpDelete("{id:int}")]
    public async Task<ApiResponse<object>> Delete(int id)
    {
        await _computerService.DeleteAsync(id);
        return ApiResponse<object>.Success(null!);
    }
}

