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




