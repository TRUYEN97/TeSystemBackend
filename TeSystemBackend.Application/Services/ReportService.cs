using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs.Reports;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Application.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IUserTeamService _userTeamService;
    private readonly ITeamRepository _teamRepository;
    private readonly IIdentityRoleService _identityRoleService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReportService(
        IReportRepository reportRepository,
        IUserRepository userRepository,
        ILocationRepository locationRepository,
        IUserTeamService userTeamService,
        ITeamRepository teamRepository,
        IIdentityRoleService identityRoleService,
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor)
    {
        _reportRepository = reportRepository;
        _userRepository = userRepository;
        _locationRepository = locationRepository;
        _userTeamService = userTeamService;
        _teamRepository = teamRepository;
        _identityRoleService = identityRoleService;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                         ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub");
        return userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId) ? userId : null;
    }

    public async Task<ReportDto> CreateAsync(CreateReportDto request)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);
        }

        if (request.LocationId.HasValue)
        {
            var location = await _locationRepository.GetByIdAsync(request.LocationId.Value);
            if (location == null)
            {
                throw new KeyNotFoundException(ErrorMessages.LocationNotFound);
            }
        }

        var report = new Report
        {
            Title = request.Title,
            Content = request.Content,
            Type = (ReportType)request.Type,
            Status = ReportStatus.Draft,
            CreatedById = userId.Value,
            LocationId = request.LocationId,
            ReportDate = request.ReportDate,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _reportRepository.AddAsync(report);
        await _unitOfWork.SaveChangesAsync();

        var created = await _reportRepository.GetByIdAsync(report.Id);
        return MapToDto(created!);
    }

    public async Task<ReportDto> UpdateAsync(int id, UpdateReportDto request)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);
        }

        var report = await _reportRepository.GetByIdAsync(id);
        if (report == null)
        {
            throw new KeyNotFoundException(ErrorMessages.ReportNotFound);
        }

        if (report.CreatedById != userId.Value)
        {
            throw new UnauthorizedAccessException(ErrorMessages.PermissionDenied);
        }

        if (report.Status != ReportStatus.Draft)
        {
            throw new InvalidOperationException("Only draft reports can be updated");
        }

        if (request.LocationId.HasValue)
        {
            var location = await _locationRepository.GetByIdAsync(request.LocationId.Value);
            if (location == null)
            {
                throw new KeyNotFoundException(ErrorMessages.LocationNotFound);
            }
            report.LocationId = request.LocationId;
        }

        if (!string.IsNullOrEmpty(request.Title))
        {
            report.Title = request.Title;
        }

        if (!string.IsNullOrEmpty(request.Content))
        {
            report.Content = request.Content;
        }

        if (request.ReportDate.HasValue)
        {
            report.ReportDate = request.ReportDate.Value;
        }

        if (request.StartDate.HasValue)
        {
            report.StartDate = request.StartDate;
        }

        if (request.EndDate.HasValue)
        {
            report.EndDate = request.EndDate;
        }

        report.UpdatedAt = DateTime.UtcNow;
        report.UpdatedById = userId;

        await _reportRepository.UpdateAsync(report);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _reportRepository.GetByIdAsync(id);
        return MapToDto(updated!);
    }

    public async Task<ReportDto> GetByIdAsync(int id)
    {
        var report = await _reportRepository.GetByIdAsync(id);
        if (report == null)
        {
            throw new KeyNotFoundException(ErrorMessages.ReportNotFound);
        }

        return MapToDto(report);
    }

    public async Task<List<ReportDto>> GetAllAsync()
    {
        var reports = await _reportRepository.GetAllAsync();
        return reports.Select(MapToDto).ToList();
    }

    public async Task<List<ReportDto>> GetByUserIdAsync(int userId)
    {
        var reports = await _reportRepository.GetByUserIdAsync(userId);
        return reports.Select(MapToDto).ToList();
    }

    public async Task<List<ReportDto>> GetByLocationIdAsync(int locationId)
    {
        var reports = await _reportRepository.GetByLocationIdAsync(locationId);
        return reports.Select(MapToDto).ToList();
    }

    public async Task<List<ReportDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var reports = await _reportRepository.GetByDateRangeAsync(startDate, endDate);
        return reports.Select(MapToDto).ToList();
    }

    public async Task<List<ReportDto>> GetByTypeAsync(int reportType)
    {
        var reports = await _reportRepository.GetByTypeAsync(reportType);
        return reports.Select(MapToDto).ToList();
    }

    public async Task<List<ReportDto>> GetByStatusAsync(int status)
    {
        var reports = await _reportRepository.GetByStatusAsync(status);
        return reports.Select(MapToDto).ToList();
    }

    public async Task<List<ReportDto>> GetByTeamIdAsync(int teamId)
    {
        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
        {
            throw new KeyNotFoundException(ErrorMessages.TeamNotFound);
        }

        var userIds = await _userTeamService.GetTeamUserIdsAsync(teamId);
        if (userIds.Count == 0)
        {
            return new List<ReportDto>();
        }

        var reports = await _reportRepository.GetByUserIdsAsync(userIds);
        return reports.Select(MapToDto).ToList();
    }

    public async Task<List<ReportDto>> GetByTeamIdAndRoleAsync(int teamId, string roleName)
    {
        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
        {
            throw new KeyNotFoundException(ErrorMessages.TeamNotFound);
        }

        var userIds = await _userTeamService.GetTeamUserIdsAsync(teamId);
        if (userIds.Count == 0)
        {
            return new List<ReportDto>();
        }

        var filteredUserIds = new List<int>();
        foreach (var userId in userIds)
        {
            if (await _identityRoleService.UserHasRoleAsync(userId, roleName))
            {
                filteredUserIds.Add(userId);
            }
        }

        if (filteredUserIds.Count == 0)
        {
            return new List<ReportDto>();
        }

        var reports = await _reportRepository.GetByUserIdsAsync(filteredUserIds);
        return reports.Select(MapToDto).ToList();
    }

    public async Task DeleteAsync(int id)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);
        }

        var report = await _reportRepository.GetByIdAsync(id);
        if (report == null)
        {
            return;
        }

        if (report.CreatedById != userId.Value)
        {
            throw new UnauthorizedAccessException(ErrorMessages.PermissionDenied);
        }

        if (report.Status != ReportStatus.Draft)
        {
            throw new InvalidOperationException("Only draft reports can be deleted");
        }

        await _reportRepository.DeleteAsync(report);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ReportDto> ChangeStatusAsync(int id, int status)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);
        }

        if (!Enum.IsDefined(typeof(ReportStatus), status))
        {
            throw new ArgumentException("Invalid report status");
        }

        var report = await _reportRepository.GetByIdAsync(id);
        if (report == null)
        {
            throw new KeyNotFoundException(ErrorMessages.ReportNotFound);
        }

        if (report.CreatedById != userId.Value)
        {
            throw new UnauthorizedAccessException(ErrorMessages.PermissionDenied);
        }

        report.Status = (ReportStatus)status;
        report.UpdatedAt = DateTime.UtcNow;
        report.UpdatedById = userId;

        await _reportRepository.UpdateAsync(report);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _reportRepository.GetByIdAsync(id);
        return MapToDto(updated!);
    }

    private static ReportDto MapToDto(Report report)
    {
        return new ReportDto
        {
            Id = report.Id,
            Title = report.Title,
            Content = report.Content,
            Type = (int)report.Type,
            TypeName = report.Type.ToString(),
            Status = (int)report.Status,
            StatusName = report.Status.ToString(),
            CreatedById = report.CreatedById,
            CreatedByName = report.CreatedBy.Name,
            LocationId = report.LocationId,
            LocationName = report.Location?.Name,
            ReportDate = report.ReportDate,
            StartDate = report.StartDate,
            EndDate = report.EndDate,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt,
            UpdatedById = report.UpdatedById,
            UpdatedByName = report.UpdatedBy?.Name
        };
    }
}

