using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs.Users;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Application.DTOs.Teams;

namespace TeSystemBackend.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
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
            throw new KeyNotFoundException(ErrorMessages.UserNotFound);
        }

        return MapToDto(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request)
    {
        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingEmail != null)
        {
            throw new InvalidOperationException(ErrorMessages.EmailAlreadyExists);
        }

        var existingUsername = await _userRepository.GetByUserNameAsync(request.Username);
        if (existingUsername != null)
        {
            throw new InvalidOperationException(ErrorMessages.UsernameAlreadyExists);
        }

        var user = new AppUser
        {
            UserName = request.Username,
            Email = request.Email,
            Name = request.Name
        };

        var newUser = await _userRepository.CreateAsync(user, request.Password);

        if (request.Teams.Count > 0)
        {
            foreach (int teamId in request.Teams)
            {
                var userTeam = new UserTeam
                {
                    TeamId = teamId,
                    UserId = newUser.Id
                };
                await _unitOfWork.UserTeams.AddAsync(userTeam);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        var newUpdatedUser = await _userRepository.GetByIdAsync(newUser.Id);

        return MapToDto(newUpdatedUser);
    }

    public async Task<UserDto> UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException(ErrorMessages.UserNotFound);
        }

        if (!string.IsNullOrEmpty(request.Email) && 
            !string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _userRepository.GetByEmailAsync(request.Email);
            if (existing != null && existing.Id != id)
            {
                throw new InvalidOperationException(ErrorMessages.EmailAlreadyExists);
            }
            user.Email = request.Email;
            user.UserName = request.Email;
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            user.Name = request.Name;
        }

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
            Name = user.Name,
            Teams = user.UserTeams.Select(ut => new TeamDto
            {
                Id = ut.Team.Id,
                DepartmentId = ut.Team.DepartmentId,
                Name = ut.Team.Name,
                FullName = ut.Team.FullName,
                MemberCount = 0
            }).ToList()
        };
    }
}





