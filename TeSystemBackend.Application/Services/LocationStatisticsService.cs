using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs.Locations;
using TeSystemBackend.Application.Repositories;

namespace TeSystemBackend.Application.Services;

public class LocationStatisticsService : ILocationStatisticsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocationResourceProviderFactory _providerFactory;
    private readonly IAppAuthorizationService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocationStatisticsService(
        IUnitOfWork unitOfWork,
        ILocationResourceProviderFactory providerFactory,
        IAppAuthorizationService authService,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _providerFactory = providerFactory;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<LocationStatisticsDto> GetLocationStatisticsAsync(int locationId, bool includeChildren = false)
    {
        var location = await _unitOfWork.Locations.GetByIdAsync(locationId);
        if (location == null)
        {
            throw new KeyNotFoundException(ErrorMessages.LocationNotFound);
        }

        var locationIds = includeChildren
            ? await GetLocationAndChildrenIdsAsync(locationId)
            : new List<int> { locationId };

        var resourceCounts = await GetResourceCountsAsync(locationIds);

        return new LocationStatisticsDto
        {
            LocationId = locationId,
            LocationName = location.Name,
            ResourceCounts = resourceCounts,
            IncludeChildren = includeChildren
        };
    }

    public async Task<Dictionary<string, int>> GetResourceCountsAsync(int locationId, bool includeChildren = false)
    {
        var locationIds = includeChildren
            ? await GetLocationAndChildrenIdsAsync(locationId)
            : new List<int> { locationId };

        return await GetResourceCountsAsync(locationIds);
    }

    public async Task<LocationResourcesDto> GetLocationResourcesAsync(int locationId, bool includeChildren = false)
    {
        var location = await _unitOfWork.Locations.GetByIdAsync(locationId);
        if (location == null)
        {
            throw new KeyNotFoundException(ErrorMessages.LocationNotFound);
        }

        var locationIds = includeChildren
            ? await GetLocationAndChildrenIdsAsync(locationId)
            : new List<int> { locationId };

        var resourceCounts = await GetResourceCountsAsync(locationIds);
        var resources = await GetResourcesByLocationIdsAsync(locationIds);

        return new LocationResourcesDto
        {
            LocationId = locationId,
            LocationName = location.Name,
            ResourceCounts = resourceCounts,
            Resources = resources,
            IncludeChildren = includeChildren
        };
    }

    public async Task<object?> GetResourcesByTypeAsync(int locationId, string resourceType, bool includeChildren = false)
    {
        var location = await _unitOfWork.Locations.GetByIdAsync(locationId);
        if (location == null)
        {
            throw new KeyNotFoundException(ErrorMessages.LocationNotFound);
        }

        var locationIds = includeChildren
            ? await GetLocationAndChildrenIdsAsync(locationId)
            : new List<int> { locationId };

        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);
        }

        if (resourceType == "Computer")
        {
            var provider = _providerFactory.GetProvider<DTOs.Computers.ComputerDto>(resourceType);
            if (provider == null)
            {
                return null;
            }

            var allResources = new List<DTOs.Computers.ComputerDto>();
            foreach (var id in locationIds)
            {
                var resources = await provider.GetByLocationIdWithAuthAsync(id, userId.Value, _authService);
                allResources.AddRange(resources);
            }

            return allResources;
        }

        return null;
    }

    private async Task<Dictionary<string, int>> GetResourceCountsAsync(List<int> locationIds)
    {
        var counters = _providerFactory.GetAllCounters();
        var result = new Dictionary<string, int>();

        foreach (var counter in counters)
        {
            var totalCount = 0;
            foreach (var locationId in locationIds)
            {
                totalCount += await counter.CountByLocationIdAsync(locationId);
            }
            result[counter.ResourceType] = totalCount;
        }

        return result;
    }

    private async Task<Dictionary<string, object>> GetResourcesByLocationIdsAsync(List<int> locationIds)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);
        }

        var result = new Dictionary<string, object>();
        var resourceTypes = _providerFactory.GetAvailableResourceTypes();

        foreach (var resourceType in resourceTypes)
        {
            if (resourceType == "Computer")
            {
                var provider = _providerFactory.GetProvider<DTOs.Computers.ComputerDto>(resourceType);
                if (provider == null) continue;

                var allResources = new List<DTOs.Computers.ComputerDto>();
                foreach (var locationId in locationIds)
                {
                    var resources = await provider.GetByLocationIdWithAuthAsync(locationId, userId.Value, _authService);
                    allResources.AddRange(resources);
                }

                result[resourceType] = allResources;
            }
        }

        return result;
    }

    private async Task<List<int>> GetLocationAndChildrenIdsAsync(int locationId)
    {
        var location = await _unitOfWork.Locations.GetByIdAsync(locationId);
        if (location == null)
        {
            return new List<int> { locationId };
        }

        var ids = new List<int> { locationId };

        if (location.Children != null && location.Children.Any())
        {
            foreach (var child in location.Children)
            {
                var childIds = await GetLocationAndChildrenIdsAsync(child.Id);
                ids.AddRange(childIds);
            }
        }

        return ids;
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId) ? userId : null;
    }
}

