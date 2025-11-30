using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Application.DTOs.Teams;
using TeSystemBackend.Application.Services;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.Infrastructure.Services;

public class TeamService : ITeamService
{
    private readonly ApplicationDbContext _context;

    public TeamService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TeamDto>> GetAllAsync()
    {
        var teams = await _context.Teams.ToListAsync();
        return teams.Select(MapToDto).ToList();
    }

    public async Task<TeamDto> GetByIdAsync(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        if (team == null)
        {
            throw new KeyNotFoundException("Team không tồn tại.");
        }

        return MapToDto(team);
    }

    public async Task<List<TeamUserDto>> GetTeamUsersAsync(int teamId)
    {
        var team = await _context.Teams.FindAsync(teamId);
        if (team == null)
        {
            throw new KeyNotFoundException("Team không tồn tại.");
        }

        var userTeams = await _context.UserTeams
            .Include(ut => ut.User)
            .Where(ut => ut.TeamId == teamId)
            .ToListAsync();

        return userTeams.Select(ut => new TeamUserDto
        {
            UserId = ut.User.Id,
            UserName = ut.User.UserName ?? string.Empty,
            Email = ut.User.Email ?? string.Empty,
            Name = ut.User.Name
        }).ToList();
    }

    public async Task AddUserToTeamAsync(int teamId, int userId)
    {
        var team = await _context.Teams.FindAsync(teamId);
        if (team == null)
        {
            throw new KeyNotFoundException("Team không tồn tại.");
        }

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User không tồn tại.");
        }

        var existing = await _context.UserTeams
            .FirstOrDefaultAsync(ut => ut.TeamId == teamId && ut.UserId == userId);

        if (existing != null)
        {
            throw new InvalidOperationException("User đã có trong team này.");
        }

        var userTeam = new UserTeam
        {
            TeamId = teamId,
            UserId = userId
        };

        await _context.UserTeams.AddAsync(userTeam);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveUserFromTeamAsync(int teamId, int userId)
    {
        var userTeam = await _context.UserTeams
            .FirstOrDefaultAsync(ut => ut.TeamId == teamId && ut.UserId == userId);

        if (userTeam == null)
        {
            return;
        }

        _context.UserTeams.Remove(userTeam);
        await _context.SaveChangesAsync();
    }

    private static TeamDto MapToDto(Team team)
    {
        return new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            FullName = team.FullName,
            DepartmentId = team.DepartmentId
        };
    }
}

