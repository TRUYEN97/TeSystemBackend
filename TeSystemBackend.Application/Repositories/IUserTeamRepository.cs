using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface IUserTeamRepository
{
    Task<UserTeam?> GetByUserIdAndTeamIdAsync(int userId, int teamId);
    Task<List<int>> GetUserTeamIdsAsync(int userId);
    Task<List<int>> GetTeamUserIdsAsync(int teamId);
    Task AddAsync(UserTeam userTeam);
    Task RemoveAsync(UserTeam userTeam);
    Task<bool> IsUserInTeamAsync(int userId, int teamId);
}
