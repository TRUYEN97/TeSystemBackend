namespace TeSystemBackend.Application.Services;

public interface IUserTeamService
{
    Task AddUserToTeamAsync(int userId, int teamId);
    Task RemoveUserFromTeamAsync(int userId, int teamId);
    Task<List<int>> GetUserTeamIdsAsync(int userId);
    Task<List<int>> GetTeamUserIdsAsync(int teamId);
    Task<bool> IsUserInTeamAsync(int userId, int teamId);
}
