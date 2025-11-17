using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Data.Entities;
using TeSystemBackend.Service.Exceptions;
using TeSystemBackend.Service.Interfaces;

namespace TeSystemBackend.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUserEntity> _userManager;

        public UserService(UserManager<AppUserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUserEntity> RegisterAsync(
            string userName,
            string email,
            string password,
            string fullName)
        {
            if (await _userManager.FindByNameAsync(userName) != null)
                throw new BadRequestException("Tên đăng nhập đã tồn tại");

            if (await _userManager.FindByEmailAsync(email) != null)
                throw new BadRequestException("Email đã tồn tại");

            var identityUser = new AppUserEntity
            {
                UserName = userName,
                Email = email,
                FullName = fullName,
                Rank = string.Empty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(identityUser, password);
            if (!result.Succeeded)
                throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));

            return identityUser;
        }

        public async Task<List<AppUserEntity>> GetAllUsersAsync()
        {
            return await _userManager.Users
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
