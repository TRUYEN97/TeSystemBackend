using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Repositories;

public interface IUserRepository
{
    Task<AppUser?> GetByEmailAsync(string email);
    Task<AppUser?> GetByIdAsync(int id);
    Task<AppUser> CreateAsync(AppUser user, string password);
    Task<bool> CheckPasswordAsync(AppUser user, string password);
    Task<List<AppUser>> GetAllAsync();
    Task<AppUser> UpdateAsync(AppUser user);
    Task DeleteAsync(AppUser user);
    Task<AppUser?> GetByUserNameAsync(string userName);
    Task ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
}



