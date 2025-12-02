using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Teams;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/team-role-locations")]
[Authorize]
public class TeamRoleLocationsController : ControllerBase
{
    private readonly ITeamRoleLocationService _teamRoleLocationService;

    public TeamRoleLocationsController(ITeamRoleLocationService teamRoleLocationService)
    {
        _teamRoleLocationService = teamRoleLocationService;
    }

    [HttpPost]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<ApiResponse<TeamRoleLocationDto>> AssignRoleToTeam(
        [FromBody] AssignTeamRoleLocationRequest request)
    {
        await _teamRoleLocationService.AssignRoleToTeamAtLocationAsync(
            request.TeamId,
            request.RoleId,
            request.LocationId);

        var result = await _teamRoleLocationService.GetTeamRolesAtLocationAsync(
            request.TeamId,
            request.LocationId);

        var assigned = result.FirstOrDefault(r =>
            r.RoleId == request.RoleId &&
            r.LocationId == request.LocationId);

        return ApiResponse<TeamRoleLocationDto>.Success(assigned!, ErrorMessages.Success);
    }

    [HttpDelete("{teamId:int}/role/{roleId:int}/location/{locationId:int}")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<ApiResponse<object>> RemoveRoleFromTeam(
        int teamId, int roleId, int locationId)
    {
        await _teamRoleLocationService.RemoveRoleFromTeamAtLocationAsync(
            teamId, roleId, locationId);

        return ApiResponse<object>.Success(null!, ErrorMessages.Success);
    }

    [HttpGet("team/{teamId:int}/location/{locationId:int}")]
    public async Task<ApiResponse<List<TeamRoleLocationDto>>> GetTeamRolesAtLocation(
        int teamId, int locationId)
    {
        var result = await _teamRoleLocationService.GetTeamRolesAtLocationAsync(
            teamId, locationId);

        return ApiResponse<List<TeamRoleLocationDto>>.Success(result);
    }

    [HttpGet("team/{teamId:int}/location/{locationId:int}/permissions")]
    public async Task<ApiResponse<List<string>>> GetTeamPermissionsAtLocation(
        int teamId, int locationId)
    {
        var result = await _teamRoleLocationService.GetTeamPermissionsAtLocationAsync(
            teamId, locationId);

        return ApiResponse<List<string>>.Success(result);
    }
}
