using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AppUser?> GetByEmailAsync(string email)
    {
        return await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<AppUser?> GetByIdAsync(int id)
    {
        return await _userManager.Users
            .Include(u => u.UserTeams)
            .ThenInclude(ut => ut.Team)
            .ThenInclude(team => team.Department)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<AppUser> CreateAsync(AppUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(";", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }

        return user;
    }

    public async Task<bool> CheckPasswordAsync(AppUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<List<AppUser>> GetAllAsync()
    {
        return await _userManager.Users
            .Include(u => u.UserTeams)
            .ThenInclude(ut => ut.Team)
            .ThenInclude(team => team.Department)
            .ToListAsync();
    }

    public async Task<AppUser> UpdateAsync(AppUser user)
    {
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(";", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }

        return user;
    }

    public async Task DeleteAsync(AppUser user)
    {
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(";", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }
    }

    public async Task<AppUser?> GetByUserNameAsync(string userName)
    {
        return await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
    {
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(";", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }
    }
}



