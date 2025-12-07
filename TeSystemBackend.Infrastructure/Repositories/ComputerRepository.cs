using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.Infrastructure.Data;

public class ComputerRepository : IComputerRepository
{
    private readonly ApplicationDbContext _context;

    public ComputerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Computer?> GetByIdAsync(int id)
    {
        return await _context.Computers
            .Include(c => c.Location)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Computer?> GetByIpAsync(string ipAddress)
    {
        return await _context.Computers
            .Include(c => c.Location)
            .FirstOrDefaultAsync(c => c.IpAddress == ipAddress);
    }

    public async Task<List<Computer>> GetAllAsync()
    {
        return await _context.Computers
            .Include(c => c.Location)
            .ToListAsync();
    }

    public async Task<List<Computer>> GetByLocationIdAsync(int locationId)
    {
        return await _context.Computers
            .Include(c => c.Location)
            .Where(c => c.LocationId == locationId)
            .ToListAsync();
    }

    public async Task<Dictionary<int, List<Computer>>> GetByLocationIdsAsync(List<int> locationIds)
    {
        var computers = await _context.Computers
            .Include(c => c.Location)
            .Where(c => locationIds.Contains(c.LocationId))
            .ToListAsync();

        return computers
            .GroupBy(c => c.LocationId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    public async Task<int> CountByLocationIdAsync(int locationId)
    {
        return await _context.Computers
            .CountAsync(c => c.LocationId == locationId);
    }

    public async Task<Dictionary<int, int>> CountByLocationIdsAsync(List<int> locationIds)
    {
        var counts = await _context.Computers
            .Where(c => locationIds.Contains(c.LocationId))
            .GroupBy(c => c.LocationId)
            .Select(g => new { LocationId = g.Key, Count = g.Count() })
            .ToListAsync();

        return counts.ToDictionary(x => x.LocationId, x => x.Count);
    }

    public async Task AddAsync(Computer computer)
    {
        await _context.Computers.AddAsync(computer);
    }

    public Task UpdateAsync(Computer computer)
    {
        _context.Computers.Update(computer);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Computer computer)
    {
        _context.Computers.Remove(computer);
        return Task.CompletedTask;
    }
}




