using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs.Teams;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class TeamRoleLocationService : ITeamRoleLocationService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TeamRoleLocationService(
        ITeamRepository teamRepository,
        IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task AssignRoleToTeamAtLocationAsync(int teamId, int roleId, int locationId)
    {
        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            throw new KeyNotFoundException(ErrorMessages.TeamNotFound);

        var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
        if (role == null)
            throw new KeyNotFoundException(ErrorMessages.RoleNotFound);

        var location = await _unitOfWork.Locations.GetByIdAsync(locationId);
        if (location == null)
            throw new KeyNotFoundException(ErrorMessages.LocationNotFound);

        var exists = await _unitOfWork.TeamRoleLocations.ExistsAsync(teamId, roleId, locationId);
        if (exists)
            throw new InvalidOperationException(ErrorMessages.TeamAlreadyHasRoleAtLocation);

        var teamRoleLocation = new TeamRoleLocation
        {
            TeamId = teamId,
            RoleId = roleId,
            LocationId = locationId
        };

        await _unitOfWork.TeamRoleLocations.AddAsync(teamRoleLocation);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveRoleFromTeamAtLocationAsync(int teamId, int roleId, int locationId)
    {
        var teamRoleLocation = await _unitOfWork.TeamRoleLocations.GetByTeamRoleLocationAsync(
            teamId, roleId, locationId);
        
        if (teamRoleLocation == null)
            return;

        await _unitOfWork.TeamRoleLocations.RemoveAsync(teamRoleLocation);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<TeamRoleLocationDto>> GetTeamRolesAtLocationAsync(int teamId, int locationId)
    {
        var teamRoleLocations = await _unitOfWork.TeamRoleLocations.GetByTeamIdAndLocationIdAsync(
            teamId, locationId);
        
        return teamRoleLocations.Select(MapToDto).ToList();
    }

    public async Task<List<TeamRoleLocationDto>> GetLocationsForTeamRoleAsync(int teamId, int roleId)
    {
        var teamRoleLocations = await _unitOfWork.TeamRoleLocations.GetByTeamIdAndRoleIdAsync(
            teamId, roleId);
        
        return teamRoleLocations.Select(MapToDto).ToList();
    }

    public async Task<List<TeamRoleLocationDto>> GetTeamsWithRoleAtLocationAsync(int roleId, int locationId)
    {
        var teamRoleLocations = await _unitOfWork.TeamRoleLocations.GetByRoleIdAndLocationIdAsync(
            roleId, locationId);
        
        return teamRoleLocations.Select(MapToDto).ToList();
    }

    public async Task<bool> TeamHasRoleAtLocationAsync(int teamId, int roleId, int locationId)
    {
        return await _unitOfWork.TeamRoleLocations.ExistsAsync(teamId, roleId, locationId);
    }

    public async Task<List<string>> GetTeamPermissionsAtLocationAsync(int teamId, int locationId)
    {
        return await _unitOfWork.TeamRoleLocations.GetTeamPermissionsAtLocationAsync(
            teamId, locationId);
    }

    private static TeamRoleLocationDto MapToDto(TeamRoleLocation trl)
    {
        return new TeamRoleLocationDto
        {
            Id = trl.Id,
            TeamId = trl.TeamId,
            RoleId = trl.RoleId,
            LocationId = trl.LocationId,
            TeamName = trl.Team.Name,
            RoleName = trl.Role.Name,
            LocationName = trl.Location.Name
        };
    }
}
