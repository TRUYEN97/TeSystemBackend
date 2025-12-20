using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.Infrastructure.Data;

public class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _context;

    public TeamRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Team?> GetByIdAsync(int id)
    {
        return await _context.Teams
            .Include(t => t.Department)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Team?> GetByFullNameAsync(string fullName)
    {
        return await _context.Teams
            .Include(t => t.Department)
            .FirstOrDefaultAsync(t => t.FullName == fullName);
    }

    public async Task<Team?> GetByNameAndDepartmentId(int departmentId, string name)
    {
        return await _context.Teams
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower() && t.DepartmentId == departmentId);
    }

    public async Task<List<Team>> GetAllAsync()
    {
        return await _context.Teams
            .Include(t => t.Department)
            .ToListAsync();
    }

    public async Task<List<Team>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _context.Teams
            .Include(t => t.Department)
            .Where(t => t.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task AddAsync(Team team)
    {
        await _context.Teams.AddAsync(team);
    }

    public Task UpdateAsync(Team team)
    {
        _context.Teams.Update(team);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Team team)
    {
        _context.Teams.Remove(team);
        return Task.CompletedTask;
    }
}

