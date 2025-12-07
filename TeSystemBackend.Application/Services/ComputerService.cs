using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs.Computers;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class ComputerService : IComputerService
{
    private readonly IComputerRepository _computerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppAuthorizationService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ComputerService(
        IComputerRepository computerRepository,
        IUnitOfWork unitOfWork,
        IAppAuthorizationService authService,
        IHttpContextAccessor httpContextAccessor)
    {
        _computerRepository = computerRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId) ? userId : null;
    }

    public async Task<List<ComputerDto>> GetAllAsync()
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);

        var computers = await _computerRepository.GetAllAsync();
        
        var filteredComputers = new List<Computer>();
        foreach (var computer in computers)
        {
            if (await _authService.CanViewComputerAsync(userId.Value, computer.Id))
            {
                filteredComputers.Add(computer);
            }
        }

        if (filteredComputers.Count == 0 && computers.Count > 0)
        {
            throw new UnauthorizedAccessException(ErrorMessages.PermissionDenied);
        }

        return filteredComputers.Select(MapToDto).ToList();
    }

    public async Task<ComputerDto> GetByIdAsync(int id)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);

        if (!await _authService.CanViewComputerAsync(userId.Value, id))
        {
            throw new UnauthorizedAccessException(ErrorMessages.PermissionDenied);
        }

        var computer = await _computerRepository.GetByIdAsync(id);
        if (computer == null)
        {
            throw new KeyNotFoundException(ErrorMessages.ComputerNotFound);
        }

        return MapToDto(computer);
    }

    public async Task<ComputerDto> CreateAsync(CreateComputerDto request)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);

        if (!await _authService.CanCreateComputerAsync(userId.Value, request.LocationId))
        {
            throw new UnauthorizedAccessException(ErrorMessages.PermissionDenied);
        }

        var location = await _unitOfWork.Locations.GetByIdAsync(request.LocationId);
        if (location == null)
        {
            throw new KeyNotFoundException(ErrorMessages.LocationNotFound);
        }

        var existing = await _computerRepository.GetByIpAsync(request.IpAddress);
        if (existing != null)
        {
            throw new InvalidOperationException(ErrorMessages.ComputerIpAlreadyExists);
        }

        var computer = new Computer
        {
            IpAddress = request.IpAddress,
            Name = request.Name,
            LocationId = request.LocationId,
            Description = request.Description
        };

        await _computerRepository.AddAsync(computer);
        await _unitOfWork.SaveChangesAsync();

        var created = await _computerRepository.GetByIdAsync(computer.Id);
        return MapToDto(created!);
    }

    public async Task<ComputerDto> UpdateAsync(int id, UpdateComputerDto request)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);

        if (!await _authService.CanUpdateComputerAsync(userId.Value, id))
        {
            throw new UnauthorizedAccessException(ErrorMessages.PermissionDenied);
        }

        var computer = await _computerRepository.GetByIdAsync(id);
        if (computer == null)
        {
            throw new KeyNotFoundException(ErrorMessages.ComputerNotFound);
        }

        var location = await _unitOfWork.Locations.GetByIdAsync(request.LocationId);
        if (location == null)
        {
            throw new KeyNotFoundException(ErrorMessages.LocationNotFound);
        }

        computer.Name = request.Name;
        computer.LocationId = request.LocationId;
        computer.Description = request.Description;

        await _computerRepository.UpdateAsync(computer);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _computerRepository.GetByIdAsync(id);
        return MapToDto(updated!);
    }

    public async Task DeleteAsync(int id)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);

        if (!await _authService.CanDeleteComputerAsync(userId.Value, id))
        {
            throw new UnauthorizedAccessException(ErrorMessages.PermissionDenied);
        }

        var computer = await _computerRepository.GetByIdAsync(id);
        if (computer == null)
        {
            return;
        }

        await _computerRepository.DeleteAsync(computer);
        await _unitOfWork.SaveChangesAsync();
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
