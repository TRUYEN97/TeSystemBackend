using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.UserTeams;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/user-teams")]
[Authorize]
public class UserTeamsController : ControllerBase
{
    private readonly IUserTeamService _userTeamService;

    public UserTeamsController(IUserTeamService userTeamService)
    {
        _userTeamService = userTeamService;
    }

    [HttpPost]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<ApiResponse<object>> AddUserToTeam([FromBody] AddUserToTeamRequest request)
    {
        await _userTeamService.AddUserToTeamAsync(request.UserId, request.TeamId);
        return ApiResponse<object>.Success(null!, ErrorMessages.Success);
    }

    [HttpDelete("user/{userId:int}/team/{teamId:int}")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<ApiResponse<object>> RemoveUserFromTeam(int userId, int teamId)
    {
        await _userTeamService.RemoveUserFromTeamAsync(userId, teamId);
        return ApiResponse<object>.Success(null!, ErrorMessages.Success);
    }
}
