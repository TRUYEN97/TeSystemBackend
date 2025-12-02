using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.AppRoles
            .Include(r => r.PerRoles)
            .ThenInclude(pr => pr.Permission)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.AppRoles
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<List<Role>> GetAllAsync()
    {
        return await _context.AppRoles
            .Include(r => r.PerRoles)
            .ThenInclude(pr => pr.Permission)
            .ToListAsync();
    }

    public async Task AddAsync(Role role)
    {
        await _context.AppRoles.AddAsync(role);
    }

    public Task UpdateAsync(Role role)
    {
        _context.AppRoles.Update(role);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string name)
    {
        return await _context.AppRoles
            .AnyAsync(r => r.Name == name);
    }
}
