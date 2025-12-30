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
    private readonly IUserTeamService _userTeamService;

    public TeamService(
        ITeamRepository teamRepository,
        IDepartmentRepository departmentRepository,
        IUnitOfWork unitOfWork,
        IUserTeamService userTeamService)
    {
        _teamRepository = teamRepository;
        _departmentRepository = departmentRepository;
        _unitOfWork = unitOfWork;
        _userTeamService = userTeamService;
    }

    public async Task<List<TeamDto>> GetAllAsync()
    {
        var teams = await _teamRepository.GetAllAsync();
        var teamDtos = new List<TeamDto>();
        
        foreach (var team in teams)
        {
            var memberCount = await GetTeamMemberCountAsync(team.Id);
            teamDtos.Add(MapToDto(team, memberCount));
        }
        
        return teamDtos;
    }

    public async Task<TeamDto> GetByIdAsync(int id)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
        {
            throw new KeyNotFoundException(ErrorMessages.TeamNotFound);
        }

        var memberCount = await GetTeamMemberCountAsync(team.Id);
        return MapToDtoIgnoreDepartmentName(team, memberCount);
    }

    public async Task<List<TeamDto>> GetByDepartmentIdAsync(int departmentId)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId);
        if (department == null)
        {
            throw new KeyNotFoundException(ErrorMessages.DepartmentNotFound);
        }

        var teams = await _teamRepository.GetByDepartmentIdAsync(departmentId);
        var teamDtos = new List<TeamDto>();
        
        foreach (var team in teams)
        {
            var memberCount = await GetTeamMemberCountAsync(team.Id);
            teamDtos.Add(MapToDto(team, memberCount));
        }
        
        return teamDtos;
    }

    public async Task<TeamDto> CreateAsync(CreateTeamDto request)
    {
        var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
        if (department == null)
        {
            throw new KeyNotFoundException(ErrorMessages.DepartmentNotFound);
        }

        request.Name = request.Name.ToUpper();
        var existing = await _teamRepository.GetByNameAndDepartmentId(request.DepartmentId, request.Name);
        if (existing != null)
        {
            throw new InvalidOperationException(ErrorMessages.TeamAlreadyExists);
        }

        var team = new Team
        {
            DepartmentId = request.DepartmentId,
            Name = request.Name,
        };

        await _teamRepository.AddAsync(team);
        await _unitOfWork.SaveChangesAsync();

        var created = await _teamRepository.GetByIdAsync(team.Id);
        var memberCount = await GetTeamMemberCountAsync(created!.Id);
        return MapToDto(created, memberCount);
    }

    public async Task<TeamDto> UpdateAsync(int id, UpdateTeamDto request)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
        {
            throw new KeyNotFoundException(ErrorMessages.TeamNotFound);
        }

        team.Name = request.Name;

        await _teamRepository.UpdateAsync(team);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _teamRepository.GetByIdAsync(id);
        var memberCount = await GetTeamMemberCountAsync(updated!.Id);
        return MapToDtoIgnoreDepartmentName(updated, memberCount);
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

    private async Task<int> GetTeamMemberCountAsync(int teamId)
    {
        var userIds = await _userTeamService.GetTeamUserIdsAsync(teamId);
        return userIds.Count;
    }

    private static TeamDto MapToDto(Team team, int memberCount)
    {
        return new TeamDto
        {
            Id = team.Id,
            DepartmentId = team.DepartmentId,
            DepartmentName = team.Department.Name,
            Name = $"{team.Department.Name}_{team.Name}",
            MemberCount = memberCount
        };
    }

    private static TeamDto MapToDtoIgnoreDepartmentName(Team team, int memberCount)
    {
        return new TeamDto
        {
            Id = team.Id,
            DepartmentId = team.DepartmentId,
            DepartmentName = team.Department.Name,
            Name = team.Name,
            MemberCount = memberCount
        };
    }
}

