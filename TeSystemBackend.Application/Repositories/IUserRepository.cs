using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface IUserRepository
{
    Task<AppUser?> GetByEmailAsync(string email);
    Task<AppUser?> GetByIdAsync(int id);
    Task<AppUser> CreateAsync(AppUser user, string password);
    Task<bool> CheckPasswordAsync(AppUser user, string password);
}


