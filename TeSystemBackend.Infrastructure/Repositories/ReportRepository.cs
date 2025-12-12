using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.Infrastructure.Data;

public class ReportRepository : IReportRepository
{
    private readonly ApplicationDbContext _context;

    public ReportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Report?> GetByIdAsync(int id)
    {
        return await _context.Reports
            .Include(r => r.CreatedBy)
            .Include(r => r.UpdatedBy)
            .Include(r => r.Location)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Report>> GetAllAsync()
    {
        return await _context.Reports
            .Include(r => r.CreatedBy)
            .Include(r => r.UpdatedBy)
            .Include(r => r.Location)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Report>> GetByUserIdAsync(int userId)
    {
        return await _context.Reports
            .Include(r => r.CreatedBy)
            .Include(r => r.UpdatedBy)
            .Include(r => r.Location)
            .Where(r => r.CreatedById == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Report>> GetByLocationIdAsync(int locationId)
    {
        return await _context.Reports
            .Include(r => r.CreatedBy)
            .Include(r => r.UpdatedBy)
            .Include(r => r.Location)
            .Where(r => r.LocationId == locationId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Report>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Reports
            .Include(r => r.CreatedBy)
            .Include(r => r.UpdatedBy)
            .Include(r => r.Location)
            .Where(r => r.ReportDate >= startDate && r.ReportDate <= endDate)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Report>> GetByTypeAsync(int reportType)
    {
        return await _context.Reports
            .Include(r => r.CreatedBy)
            .Include(r => r.UpdatedBy)
            .Include(r => r.Location)
            .Where(r => (int)r.Type == reportType)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Report>> GetByStatusAsync(int status)
    {
        return await _context.Reports
            .Include(r => r.CreatedBy)
            .Include(r => r.UpdatedBy)
            .Include(r => r.Location)
            .Where(r => (int)r.Status == status)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Report>> GetByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
    {
        return await _context.Reports
            .Include(r => r.CreatedBy)
            .Include(r => r.UpdatedBy)
            .Include(r => r.Location)
            .Where(r => r.CreatedById == userId && r.ReportDate >= startDate && r.ReportDate <= endDate)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Report>> GetByUserIdsAsync(List<int> userIds)
    {
        return await _context.Reports
            .Include(r => r.CreatedBy)
            .Include(r => r.UpdatedBy)
            .Include(r => r.Location)
            .Where(r => userIds.Contains(r.CreatedById))
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Report report)
    {
        await _context.Reports.AddAsync(report);
    }

    public Task UpdateAsync(Report report)
    {
        _context.Reports.Update(report);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Report report)
    {
        _context.Reports.Remove(report);
        return Task.CompletedTask;
    }
}

