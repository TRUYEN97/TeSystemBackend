using TeSystemBackend.Application.DTOs.Teams;

namespace TeSystemBackend.Application.Services;

public interface ITeamService
{
    Task<List<TeamDto>> GetAllAsync();
    Task<TeamDto> GetByIdAsync(int id);
    Task<List<TeamUserDto>> GetTeamUsersAsync(int teamId);
    Task AddUserToTeamAsync(int teamId, int userId);
    Task RemoveUserFromTeamAsync(int teamId, int userId);
}

