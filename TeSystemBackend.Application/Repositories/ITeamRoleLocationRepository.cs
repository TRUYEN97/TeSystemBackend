using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface ITeamRoleLocationRepository
{
    Task<TeamRoleLocation?> GetByTeamRoleLocationAsync(int teamId, int roleId, int locationId);
    Task<List<TeamRoleLocation>> GetByTeamIdAndLocationIdAsync(int teamId, int locationId);
    Task<List<TeamRoleLocation>> GetByTeamIdAndRoleIdAsync(int teamId, int roleId);
    Task<List<TeamRoleLocation>> GetByRoleIdAndLocationIdAsync(int roleId, int locationId);
    Task AddAsync(TeamRoleLocation teamRoleLocation);
    Task RemoveAsync(TeamRoleLocation teamRoleLocation);
    Task<bool> ExistsAsync(int teamId, int roleId, int locationId);
    Task<List<string>> GetTeamPermissionsAtLocationAsync(int teamId, int locationId);
    Task<bool> TeamHasPermissionAtLocationAsync(int teamId, string permission, int locationId);
    Task<bool> TeamsHavePermissionAsync(List<int> teamIds, string permission, int? locationId = null);
}
