using TeSystemBackend.Application.DTOs.Users;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(MapToDto).ToList();
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return MapToDto(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email);
        if (existing != null)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            Name = request.Name
        };

        var created = await _userRepository.CreateAsync(user, request.Password);

        return MapToDto(created);
    }

    public async Task<UserDto> UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _userRepository.GetByEmailAsync(request.Email);
            if (existing != null && existing.Id != id)
            {
                throw new InvalidOperationException("Email already exists.");
            }
        }

        user.Name = request.Name;
        user.Email = request.Email;
        user.UserName = request.Email;

        var updated = await _userRepository.UpdateAsync(user);

        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return;
        }

        await _userRepository.DeleteAsync(user);
    }

    private static UserDto MapToDto(AppUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            Name = user.Name
        };
    }
}



