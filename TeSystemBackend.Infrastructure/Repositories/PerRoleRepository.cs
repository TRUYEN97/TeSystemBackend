using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.Infrastructure.Repositories;

public class PerRoleRepository : IPerRoleRepository
{
    private readonly ApplicationDbContext _context;

    public PerRoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PerRole>> GetByRoleIdAsync(int roleId)
    {
        return await _context.PerRoles
            .Include(pr => pr.Permission)
            .Where(pr => pr.RoleId == roleId)
            .ToListAsync();
    }

    public async Task<List<string>> GetPermissionNamesByRoleIdAsync(int roleId)
    {
        return await _context.PerRoles
            .Include(pr => pr.Permission)
            .Where(pr => pr.RoleId == roleId)
            .Select(pr => pr.Permission.Name)
            .ToListAsync();
    }

    public async Task AddAsync(PerRole perRole)
    {
        await _context.PerRoles.AddAsync(perRole);
    }

    public async Task AddRangeAsync(List<PerRole> perRoles)
    {
        await _context.PerRoles.AddRangeAsync(perRoles);
    }

    public Task RemoveRangeAsync(List<PerRole> perRoles)
    {
        _context.PerRoles.RemoveRange(perRoles);
        return Task.CompletedTask;
    }
}
