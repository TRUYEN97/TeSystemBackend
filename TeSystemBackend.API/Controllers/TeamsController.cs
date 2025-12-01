using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.API.Extensions;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Teams;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/teams")]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IValidator<CreateTeamDto> _createValidator;
    private readonly IValidator<UpdateTeamDto> _updateValidator;

    public TeamsController(
        ITeamService teamService,
        IValidator<CreateTeamDto> createValidator,
        IValidator<UpdateTeamDto> updateValidator)
    {
        _teamService = teamService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<TeamDto>>> GetAll()
    {
        var teams = await _teamService.GetAllAsync();
        return ApiResponse<List<TeamDto>>.Success(teams);
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResponse<TeamDto>> GetById(int id)
    {
        var team = await _teamService.GetByIdAsync(id);
        return ApiResponse<TeamDto>.Success(team);
    }

    [HttpGet("department/{departmentId:int}")]
    public async Task<ApiResponse<List<TeamDto>>> GetByDepartmentId(int departmentId)
    {
        var teams = await _teamService.GetByDepartmentIdAsync(departmentId);
        return ApiResponse<List<TeamDto>>.Success(teams);
    }

    [HttpPost]
    public async Task<ApiResponse<TeamDto>> Create(CreateTeamDto request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var team = await _teamService.CreateAsync(request);
        return ApiResponse<TeamDto>.Success(team);
    }

    [HttpPut("{id:int}")]
    public async Task<ApiResponse<TeamDto>> Update(int id, UpdateTeamDto request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var team = await _teamService.UpdateAsync(id, request);
        return ApiResponse<TeamDto>.Success(team);
    }

    [HttpDelete("{id:int}")]
    public async Task<ApiResponse<object>> Delete(int id)
    {
        await _teamService.DeleteAsync(id);
        return ApiResponse<object>.Success(null!);
    }
}

