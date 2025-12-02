using TeSystemBackend.Application.DTOs.Teams;

namespace TeSystemBackend.Application.Services;

public interface ITeamRoleLocationService
{
    Task AssignRoleToTeamAtLocationAsync(int teamId, int roleId, int locationId);
    Task RemoveRoleFromTeamAtLocationAsync(int teamId, int roleId, int locationId);
    Task<List<TeamRoleLocationDto>> GetTeamRolesAtLocationAsync(int teamId, int locationId);
    Task<List<TeamRoleLocationDto>> GetLocationsForTeamRoleAsync(int teamId, int roleId);
    Task<List<TeamRoleLocationDto>> GetTeamsWithRoleAtLocationAsync(int roleId, int locationId);
    Task<bool> TeamHasRoleAtLocationAsync(int teamId, int roleId, int locationId);
    Task<List<string>> GetTeamPermissionsAtLocationAsync(int teamId, int locationId);
}
