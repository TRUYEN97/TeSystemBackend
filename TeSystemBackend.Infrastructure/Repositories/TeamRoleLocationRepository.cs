using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.Infrastructure.Repositories;

public class TeamRoleLocationRepository : ITeamRoleLocationRepository
{
    private readonly ApplicationDbContext _context;

    public TeamRoleLocationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TeamRoleLocation?> GetByTeamRoleLocationAsync(int teamId, int roleId, int locationId)
    {
        return await _context.TeamRoleLocations
            .FirstOrDefaultAsync(trl => 
                trl.TeamId == teamId && 
                trl.RoleId == roleId && 
                trl.LocationId == locationId);
    }

    public async Task<List<TeamRoleLocation>> GetByTeamIdAndLocationIdAsync(int teamId, int locationId)
    {
        return await _context.TeamRoleLocations
            .Include(trl => trl.Team)
            .Include(trl => trl.Role)
            .ThenInclude(r => r.PerRoles)
            .ThenInclude(pr => pr.Permission)
            .Include(trl => trl.Location)
            .Where(trl => trl.TeamId == teamId && trl.LocationId == locationId)
            .ToListAsync();
    }

    public async Task<List<TeamRoleLocation>> GetByTeamIdAndRoleIdAsync(int teamId, int roleId)
    {
        return await _context.TeamRoleLocations
            .Include(trl => trl.Team)
            .Include(trl => trl.Role)
            .Include(trl => trl.Location)
            .Where(trl => trl.TeamId == teamId && trl.RoleId == roleId)
            .ToListAsync();
    }

    public async Task<List<TeamRoleLocation>> GetByRoleIdAndLocationIdAsync(int roleId, int locationId)
    {
        return await _context.TeamRoleLocations
            .Include(trl => trl.Team)
            .Include(trl => trl.Role)
            .Include(trl => trl.Location)
            .Where(trl => trl.RoleId == roleId && trl.LocationId == locationId)
            .ToListAsync();
    }

    public async Task AddAsync(TeamRoleLocation teamRoleLocation)
    {
        await _context.TeamRoleLocations.AddAsync(teamRoleLocation);
    }

    public Task RemoveAsync(TeamRoleLocation teamRoleLocation)
    {
        _context.TeamRoleLocations.Remove(teamRoleLocation);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int teamId, int roleId, int locationId)
    {
        return await _context.TeamRoleLocations
            .AnyAsync(trl => 
                trl.TeamId == teamId && 
                trl.RoleId == roleId && 
                trl.LocationId == locationId);
    }

    public async Task<List<string>> GetTeamPermissionsAtLocationAsync(int teamId, int locationId)
    {
        return await _context.TeamRoleLocations
            .Include(trl => trl.Role)
            .ThenInclude(r => r.PerRoles)
            .ThenInclude(pr => pr.Permission)
            .Where(trl => trl.TeamId == teamId && trl.LocationId == locationId)
            .SelectMany(trl => trl.Role.PerRoles.Select(pr => pr.Permission.Name))
            .Distinct()
            .ToListAsync();
    }

    public async Task<bool> TeamHasPermissionAtLocationAsync(int teamId, string permission, int locationId)
    {
        return await _context.TeamRoleLocations
            .Include(trl => trl.Role)
            .ThenInclude(r => r.PerRoles)
            .ThenInclude(pr => pr.Permission)
            .Where(trl => trl.TeamId == teamId && trl.LocationId == locationId)
            .AnyAsync(trl => trl.Role.PerRoles.Any(pr => pr.Permission.Name == permission));
    }

    public async Task<bool> TeamsHavePermissionAsync(List<int> teamIds, string permission, int? locationId = null)
    {
        var query = _context.TeamRoleLocations
            .Include(trl => trl.Role)
            .ThenInclude(r => r.PerRoles)
            .ThenInclude(pr => pr.Permission)
            .Where(trl => teamIds.Contains(trl.TeamId));

        if (locationId.HasValue)
        {
            query = query.Where(trl => trl.LocationId == locationId.Value);
        }

        return await query.AnyAsync(trl => trl.Role.PerRoles.Any(pr => pr.Permission.Name == permission));
    }
}
