using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs.Locations;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LocationService(ILocationRepository locationRepository, IUnitOfWork unitOfWork)
    {
        _locationRepository = locationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LocationDto> CreateAsync(CreateLocationDto request)
    {
        // Validate parent location if provided
        if (request.ParentId.HasValue)
        {
            var parent = await _locationRepository.GetByIdAsync(request.ParentId.Value);
            if (parent == null)
            {
                throw new KeyNotFoundException(ErrorMessages.LocationNotFound);
            }
        }

        // Check for duplicate name (optional, depending on business rules)
        var existing = await _locationRepository.GetByNameAsync(request.Name);
        if (existing != null)
        {
            throw new InvalidOperationException(ErrorMessages.LocationNameAlreadyExists);
        }

        var location = new Location
        {
            Name = request.Name,
            ParentId = request.ParentId
        };

        await _locationRepository.AddAsync(location);
        await _unitOfWork.SaveChangesAsync();

        var created = await _locationRepository.GetByIdAsync(location.Id);
        return MapToDto(created!);
    }

    public async Task<List<LocationDto>> GetAllAsync()
    {
        var locations = await _locationRepository.GetAllAsync();
        return locations.Select(MapToDto).ToList();
    }

    public async Task<LocationDto> GetByIdAsync(int id)
    {
        var location = await _locationRepository.GetByIdAsync(id);
        if (location == null)
        {
            throw new KeyNotFoundException(ErrorMessages.LocationNotFound);
        }

        return MapToDto(location);
    }

    private static LocationDto MapToDto(Location location)
    {
        return new LocationDto
        {
            Id = location.Id,
            Name = location.Name,
            ParentId = location.ParentId,
            ParentName = location.Parent?.Name
        };
    }
}

