using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.API.Extensions;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Reports;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IValidator<CreateReportDto> _createValidator;
    private readonly IValidator<UpdateReportDto> _updateValidator;
    private readonly IValidator<ChangeReportStatusRequest> _changeStatusValidator;

    public ReportsController(
        IReportService reportService,
        IValidator<CreateReportDto> createValidator,
        IValidator<UpdateReportDto> updateValidator,
        IValidator<ChangeReportStatusRequest> changeStatusValidator)
    {
        _reportService = reportService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _changeStatusValidator = changeStatusValidator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<ReportDto>>> GetAll()
    {
        var reports = await _reportService.GetAllAsync();
        return ApiResponse<List<ReportDto>>.Success(reports);
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResponse<ReportDto>> GetById(int id)
    {
        var report = await _reportService.GetByIdAsync(id);
        return ApiResponse<ReportDto>.Success(report);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<ApiResponse<List<ReportDto>>> GetByUserId(int userId)
    {
        var reports = await _reportService.GetByUserIdAsync(userId);
        return ApiResponse<List<ReportDto>>.Success(reports);
    }

    [HttpGet("location/{locationId:int}")]
    public async Task<ApiResponse<List<ReportDto>>> GetByLocationId(int locationId)
    {
        var reports = await _reportService.GetByLocationIdAsync(locationId);
        return ApiResponse<List<ReportDto>>.Success(reports);
    }

    [HttpGet("date-range")]
    public async Task<ApiResponse<List<ReportDto>>> GetByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var reports = await _reportService.GetByDateRangeAsync(startDate, endDate);
        return ApiResponse<List<ReportDto>>.Success(reports);
    }

    [HttpGet("type/{type:int}")]
    public async Task<ApiResponse<List<ReportDto>>> GetByType(int type)
    {
        var reports = await _reportService.GetByTypeAsync(type);
        return ApiResponse<List<ReportDto>>.Success(reports);
    }

    [HttpGet("status/{status:int}")]
    public async Task<ApiResponse<List<ReportDto>>> GetByStatus(int status)
    {
        var reports = await _reportService.GetByStatusAsync(status);
        return ApiResponse<List<ReportDto>>.Success(reports);
    }

    [HttpGet("team/{teamId:int}")]
    public async Task<ApiResponse<List<ReportDto>>> GetByTeamId(int teamId)
    {
        var reports = await _reportService.GetByTeamIdAsync(teamId);
        return ApiResponse<List<ReportDto>>.Success(reports);
    }

    [HttpGet("team/{teamId:int}/role/{roleName}")]
    public async Task<ApiResponse<List<ReportDto>>> GetByTeamIdAndRole(int teamId, string roleName)
    {
        var reports = await _reportService.GetByTeamIdAndRoleAsync(teamId, roleName);
        return ApiResponse<List<ReportDto>>.Success(reports);
    }

    [HttpPost]
    public async Task<ApiResponse<ReportDto>> Create(CreateReportDto request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var report = await _reportService.CreateAsync(request);
        return ApiResponse<ReportDto>.Success(report);
    }

    [HttpPut("{id:int}")]
    public async Task<ApiResponse<ReportDto>> Update(int id, UpdateReportDto request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var report = await _reportService.UpdateAsync(id, request);
        return ApiResponse<ReportDto>.Success(report);
    }

    [HttpPut("{id:int}/status")]
    public async Task<ApiResponse<ReportDto>> ChangeStatus(int id, [FromBody] ChangeReportStatusRequest request)
    {
        await _changeStatusValidator.ValidateAndThrowAsync(request);

        var report = await _reportService.ChangeStatusAsync(id, request.Status);
        return ApiResponse<ReportDto>.Success(report);
    }

    [HttpDelete("{id:int}")]
    public async Task<ApiResponse<object>> Delete(int id)
    {
        await _reportService.DeleteAsync(id);
        return ApiResponse<object>.Success(null!);
    }
}

