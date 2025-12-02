using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class UserTeamService : IUserTeamService
{
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserTeamService(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task AddUserToTeamAsync(int userId, int teamId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException(ErrorMessages.UserNotFound);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            throw new KeyNotFoundException(ErrorMessages.TeamNotFound);

        var exists = await _unitOfWork.UserTeams.IsUserInTeamAsync(userId, teamId);
        if (exists)
            throw new InvalidOperationException(ErrorMessages.UserAlreadyInTeam);

        var userTeam = new UserTeam
        {
            UserId = userId,
            TeamId = teamId
        };

        await _unitOfWork.UserTeams.AddAsync(userTeam);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveUserFromTeamAsync(int userId, int teamId)
    {
        var userTeam = await _unitOfWork.UserTeams.GetByUserIdAndTeamIdAsync(userId, teamId);
        
        if (userTeam == null)
            return;

        await _unitOfWork.UserTeams.RemoveAsync(userTeam);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<int>> GetUserTeamIdsAsync(int userId)
    {
        return await _unitOfWork.UserTeams.GetUserTeamIdsAsync(userId);
    }

    public async Task<List<int>> GetTeamUserIdsAsync(int teamId)
    {
        return await _unitOfWork.UserTeams.GetTeamUserIdsAsync(teamId);
    }

    public async Task<bool> IsUserInTeamAsync(int userId, int teamId)
    {
        return await _unitOfWork.UserTeams.IsUserInTeamAsync(userId, teamId);
    }
}
