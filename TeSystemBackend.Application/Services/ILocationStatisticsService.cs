using TeSystemBackend.Application.DTOs.Locations;

namespace TeSystemBackend.Application.Services;

public interface ILocationStatisticsService
{
    Task<LocationStatisticsDto> GetLocationStatisticsAsync(int locationId, bool includeChildren = false);
    Task<Dictionary<string, int>> GetResourceCountsAsync(int locationId, bool includeChildren = false);
    Task<LocationResourcesDto> GetLocationResourcesAsync(int locationId, bool includeChildren = false);
    Task<object?> GetResourcesByTypeAsync(int locationId, string resourceType, bool includeChildren = false);
}

