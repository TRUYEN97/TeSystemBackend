using TeSystemBackend.Application.DTOs.Teams;

namespace TeSystemBackend.Application.Services;

public interface ITeamService
{
    Task<List<TeamDto>> GetAllAsync();
    Task<TeamDto> GetByIdAsync(int id);
    Task<List<TeamDto>> GetByDepartmentIdAsync(int departmentId);
    Task<TeamDto> CreateAsync(CreateTeamDto request);
    Task<TeamDto> UpdateAsync(int id, UpdateTeamDto request);
    Task DeleteAsync(int id);
}

