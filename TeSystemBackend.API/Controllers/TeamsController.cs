using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.API.Helpers;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Teams;
using TeSystemBackend.Application.Services;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/teams")]
[Authorize]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IValidator<AddUserToTeamRequest> _addUserValidator;
    private readonly UserManager<AppUser> _userManager;

    public TeamsController(
        ITeamService teamService,
        IValidator<AddUserToTeamRequest> addUserValidator,
        UserManager<AppUser> userManager)
    {
        _teamService = teamService;
        _addUserValidator = addUserValidator;
        _userManager = userManager;
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

    [HttpGet("{id:int}/users")]
    public async Task<ApiResponse<List<TeamUserDto>>> GetTeamUsers(int id)
    {
        var users = await _teamService.GetTeamUsersAsync(id);
        return ApiResponse<List<TeamUserDto>>.Success(users);
    }

    [HttpPost("{id:int}/users")]
    public async Task<ApiResponse<object>> AddUserToTeam(int id, AddUserToTeamRequest request)
    {
        await ControllerHelper.EnsureAdminAsync(User, _userManager);

        var validationResult = await _addUserValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        await _teamService.AddUserToTeamAsync(id, request.UserId);
        return ApiResponse<object>.Success(null!, "Đã thêm user vào team");
    }

    [HttpDelete("{id:int}/users/{userId:int}")]
    public async Task<ApiResponse<object>> RemoveUserFromTeam(int id, int userId)
    {
        await ControllerHelper.EnsureAdminAsync(User, _userManager);

        await _teamService.RemoveUserFromTeamAsync(id, userId);
        return ApiResponse<object>.Success(null!, "Đã xóa user khỏi team");
    }
}

