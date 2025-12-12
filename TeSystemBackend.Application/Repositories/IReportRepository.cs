using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface IReportRepository
{
    Task<Report?> GetByIdAsync(int id);
    Task<List<Report>> GetAllAsync();
    Task<List<Report>> GetByUserIdAsync(int userId);
    Task<List<Report>> GetByLocationIdAsync(int locationId);
    Task<List<Report>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<Report>> GetByTypeAsync(int reportType);
    Task<List<Report>> GetByStatusAsync(int status);
    Task<List<Report>> GetByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
    Task<List<Report>> GetByUserIdsAsync(List<int> userIds);
    Task AddAsync(Report report);
    Task UpdateAsync(Report report);
    Task DeleteAsync(Report report);
}

