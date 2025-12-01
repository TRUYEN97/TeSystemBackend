using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs.Computers;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class ComputerService : IComputerService
{
    private readonly IComputerRepository _computerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ComputerService(IComputerRepository computerRepository, IUnitOfWork unitOfWork)
    {
        _computerRepository = computerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ComputerDto>> GetAllAsync()
    {
        var computers = await _computerRepository.GetAllAsync();
        return computers.Select(MapToDto).ToList();
    }

    public async Task<ComputerDto> GetByIdAsync(int id)
    {
        var computer = await _computerRepository.GetByIdAsync(id);
        if (computer == null)
        {
            throw new KeyNotFoundException(ErrorMessages.ComputerNotFound);
        }

        return MapToDto(computer);
    }

    public async Task<ComputerDto> CreateAsync(CreateComputerDto request)
    {
        var existing = await _computerRepository.GetByCodeAsync(request.Code);
        if (existing != null)
        {
            throw new InvalidOperationException(ErrorMessages.ComputerCodeAlreadyExists);
        }

        var computer = new Computer
        {
            Code = request.Code,
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
        var computer = await _computerRepository.GetByIdAsync(id);
        if (computer == null)
        {
            throw new KeyNotFoundException(ErrorMessages.ComputerNotFound);
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
            Code = computer.Code,
            Name = computer.Name,
            LocationId = computer.LocationId,
            Description = computer.Description
        };
    }
}



