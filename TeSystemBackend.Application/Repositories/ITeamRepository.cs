using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(int id);
    Task<Team?> GetByFullNameAsync(string fullName);
    Task<List<Team>> GetAllAsync();
    Task<List<Team>> GetByDepartmentIdAsync(int departmentId);
    Task AddAsync(Team team);
    Task UpdateAsync(Team team);
    Task DeleteAsync(Team team);
}


