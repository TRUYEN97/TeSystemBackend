using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.API.Extensions;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Locations;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/locations")]
[Authorize]
public class LocationsController : ControllerBase
{
    private readonly ILocationStatisticsService _locationStatisticsService;

    public LocationsController(ILocationStatisticsService locationStatisticsService)
    {
        _locationStatisticsService = locationStatisticsService;
    }

    [HttpGet("{id:int}/statistics")]
    public async Task<ApiResponse<LocationStatisticsDto>> GetStatistics(
        int id,
        [FromQuery] bool includeChildren = false)
    {
        var statistics = await _locationStatisticsService.GetLocationStatisticsAsync(id, includeChildren);
        return ApiResponse<LocationStatisticsDto>.Success(statistics);
    }

    [HttpGet("{id:int}/resources")]
    public async Task<ApiResponse<LocationResourcesDto>> GetResources(
        int id,
        [FromQuery] bool includeChildren = false)
    {
        var resources = await _locationStatisticsService.GetLocationResourcesAsync(id, includeChildren);
        return ApiResponse<LocationResourcesDto>.Success(resources);
    }

    [HttpGet("{id:int}/resources/{resourceType}")]
    public async Task<ApiResponse<object>> GetResourcesByType(
        int id,
        string resourceType,
        [FromQuery] bool includeChildren = false)
    {
        var resources = await _locationStatisticsService.GetResourcesByTypeAsync(id, resourceType, includeChildren);
        
        if (resources == null)
        {
            throw new KeyNotFoundException($"Resource type '{resourceType}' is not supported");
        }

        return ApiResponse<object>.Success(resources);
    }
}

