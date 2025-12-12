using TeSystemBackend.Application.DTOs.Reports;

namespace TeSystemBackend.Application.Services;

public interface IReportService
{
    Task<ReportDto> CreateAsync(CreateReportDto request);
    Task<ReportDto> UpdateAsync(int id, UpdateReportDto request);
    Task<ReportDto> GetByIdAsync(int id);
    Task<List<ReportDto>> GetAllAsync();
    Task<List<ReportDto>> GetByUserIdAsync(int userId);
    Task<List<ReportDto>> GetByLocationIdAsync(int locationId);
    Task<List<ReportDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<ReportDto>> GetByTypeAsync(int reportType);
    Task<List<ReportDto>> GetByStatusAsync(int status);
    Task<List<ReportDto>> GetByTeamIdAsync(int teamId);
    Task<List<ReportDto>> GetByTeamIdAndRoleAsync(int teamId, string roleName);
    Task DeleteAsync(int id);
    Task<ReportDto> ChangeStatusAsync(int id, int status);
}

