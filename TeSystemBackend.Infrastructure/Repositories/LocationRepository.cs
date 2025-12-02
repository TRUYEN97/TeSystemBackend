using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _context;

    public LocationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Location?> GetByIdAsync(int id)
    {
        return await _context.Locations
            .Include(l => l.Parent)
            .Include(l => l.Children)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Location?> GetByNameAsync(string name)
    {
        return await _context.Locations
            .FirstOrDefaultAsync(l => l.Name == name);
    }

    public async Task<List<Location>> GetAllAsync()
    {
        return await _context.Locations
            .Include(l => l.Parent)
            .ToListAsync();
    }
}
