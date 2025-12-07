using TeSystemBackend.Application.DTOs.Computers;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class ComputerLocationResourceProvider : ILocationResourceProvider<ComputerDto>, IResourceCounter
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppAuthorizationService _authService;

    public ComputerLocationResourceProvider(
        IUnitOfWork unitOfWork,
        IAppAuthorizationService authService)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public string ResourceType => "Computer";

    public async Task<int> CountByLocationIdAsync(int locationId)
    {
        return await _unitOfWork.Computers.CountByLocationIdAsync(locationId);
    }

    public async Task<Dictionary<int, int>> CountByLocationIdsAsync(List<int> locationIds)
    {
        return await _unitOfWork.Computers.CountByLocationIdsAsync(locationIds);
    }

    public async Task<List<ComputerDto>> GetByLocationIdAsync(int locationId)
    {
        var computers = await _unitOfWork.Computers.GetByLocationIdAsync(locationId);
        return computers.Select(MapToDto).ToList();
    }

    public async Task<Dictionary<int, List<ComputerDto>>> GetByLocationIdsAsync(List<int> locationIds)
    {
        var computersByLocation = await _unitOfWork.Computers.GetByLocationIdsAsync(locationIds);
        return computersByLocation.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Select(MapToDto).ToList()
        );
    }

    public async Task<List<ComputerDto>> GetByLocationIdWithAuthAsync(
        int locationId,
        int userId,
        IAppAuthorizationService authService)
    {
        var computers = await _unitOfWork.Computers.GetByLocationIdAsync(locationId);

        var filteredComputers = new List<Computer>();
        foreach (var computer in computers)
        {
            if (await authService.CanViewComputerAsync(userId, computer.Id))
            {
                filteredComputers.Add(computer);
            }
        }

        return filteredComputers.Select(MapToDto).ToList();
    }

    private static ComputerDto MapToDto(Computer computer)
    {
        return new ComputerDto
        {
            Id = computer.Id,
            IpAddress = computer.IpAddress,
            Name = computer.Name,
            LocationId = computer.LocationId,
            Description = computer.Description
        };
    }
}

