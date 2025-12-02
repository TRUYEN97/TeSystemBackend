using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.Infrastructure.Repositories;

public class UserTeamRepository : IUserTeamRepository
{
    private readonly ApplicationDbContext _context;

    public UserTeamRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserTeam?> GetByUserIdAndTeamIdAsync(int userId, int teamId)
    {
        return await _context.UserTeams
            .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TeamId == teamId);
    }

    public async Task<List<int>> GetUserTeamIdsAsync(int userId)
    {
        return await _context.UserTeams
            .Where(ut => ut.UserId == userId)
            .Select(ut => ut.TeamId)
            .ToListAsync();
    }

    public async Task<List<int>> GetTeamUserIdsAsync(int teamId)
    {
        return await _context.UserTeams
            .Where(ut => ut.TeamId == teamId)
            .Select(ut => ut.UserId)
            .ToListAsync();
    }

    public async Task AddAsync(UserTeam userTeam)
    {
        await _context.UserTeams.AddAsync(userTeam);
    }

    public Task RemoveAsync(UserTeam userTeam)
    {
        _context.UserTeams.Remove(userTeam);
        return Task.CompletedTask;
    }

    public async Task<bool> IsUserInTeamAsync(int userId, int teamId)
    {
        return await _context.UserTeams
            .AnyAsync(ut => ut.UserId == userId && ut.TeamId == teamId);
    }
}
