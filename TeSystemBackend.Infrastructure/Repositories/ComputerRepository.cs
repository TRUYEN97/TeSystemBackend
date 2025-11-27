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

    public async Task<Computer?> GetByCodeAsync(string code)
    {
        return await _context.Computers
            .Include(c => c.Location)
            .FirstOrDefaultAsync(c => c.Code == code);
    }

    public async Task AddAsync(Computer computer)
    {
        await _context.Computers.AddAsync(computer);
    }
}



