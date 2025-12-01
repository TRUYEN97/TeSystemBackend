using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs.Teams;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TeamService(
        ITeamRepository teamRepository,
        IDepartmentRepository departmentRepository,
        IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _departmentRepository = departmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TeamDto>> GetAllAsync()
    {
        var teams = await _teamRepository.GetAllAsync();
        return teams.Select(MapToDto).ToList();
    }

    public async Task<TeamDto> GetByIdAsync(int id)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
        {
            throw new KeyNotFoundException(ErrorMessages.TeamNotFound);
        }

        return MapToDto(team);
    }

    public async Task<List<TeamDto>> GetByDepartmentIdAsync(int departmentId)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId);
        if (department == null)
        {
            throw new KeyNotFoundException(ErrorMessages.DepartmentNotFound);
        }

        var teams = await _teamRepository.GetByDepartmentIdAsync(departmentId);
        return teams.Select(MapToDto).ToList();
    }

    public async Task<TeamDto> CreateAsync(CreateTeamDto request)
    {
        var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
        if (department == null)
        {
            throw new KeyNotFoundException(ErrorMessages.DepartmentNotFound);
        }

        var existing = await _teamRepository.GetByFullNameAsync(request.FullName);
        if (existing != null)
        {
            throw new InvalidOperationException(ErrorMessages.TeamFullNameAlreadyExists);
        }

        var team = new Team
        {
            DepartmentId = request.DepartmentId,
            Name = request.Name,
            FullName = request.FullName
        };

        await _teamRepository.AddAsync(team);
        await _unitOfWork.SaveChangesAsync();

        var created = await _teamRepository.GetByIdAsync(team.Id);
        return MapToDto(created!);
    }

    public async Task<TeamDto> UpdateAsync(int id, UpdateTeamDto request)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
        {
            throw new KeyNotFoundException(ErrorMessages.TeamNotFound);
        }

        var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
        if (department == null)
        {
            throw new KeyNotFoundException(ErrorMessages.DepartmentNotFound);
        }

        if (!string.Equals(team.FullName, request.FullName, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _teamRepository.GetByFullNameAsync(request.FullName);
            if (existing != null && existing.Id != id)
            {
                throw new InvalidOperationException(ErrorMessages.TeamFullNameAlreadyExists);
            }
        }

        team.DepartmentId = request.DepartmentId;
        team.Name = request.Name;
        team.FullName = request.FullName;

        await _teamRepository.UpdateAsync(team);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _teamRepository.GetByIdAsync(id);
        return MapToDto(updated!);
    }

    public async Task DeleteAsync(int id)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
        {
            return;
        }

        await _teamRepository.DeleteAsync(team);
        await _unitOfWork.SaveChangesAsync();
    }

    private static TeamDto MapToDto(Team team)
    {
        return new TeamDto
        {
            Id = team.Id,
            DepartmentId = team.DepartmentId,
            Name = team.Name,
            FullName = team.FullName
        };
    }
}

