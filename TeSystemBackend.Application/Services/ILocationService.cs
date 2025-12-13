using TeSystemBackend.Application.DTOs.Locations;

namespace TeSystemBackend.Application.Services;

public interface ILocationService
{
    Task<LocationDto> CreateAsync(CreateLocationDto request);
    Task<List<LocationDto>> GetAllAsync();
    Task<LocationDto> GetByIdAsync(int id);
}

