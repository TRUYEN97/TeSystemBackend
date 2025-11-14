using TeSystemBackend.Data;
using TeSystemBackend.Data.Entities;
using TeSystemBackend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TeSystemBackend.Service
{
    public class UserService
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly AppDbContext _dbContext;

        public UserService(UserManager<AppUserEntity> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<AppUser> RegisterAsync(
            string userName,
            string email,
            string password,
            string fullName,
            string employeeCode)
        {
            if (await _userManager.FindByNameAsync(userName) != null)
                throw new Exception("Username đã tồn tại");

            if (await _userManager.FindByEmailAsync(email) != null)
                throw new Exception("Email đã tồn tại");

            var identityUser = new AppUserEntity
            {
                UserName = userName,
                Email = email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(identityUser, password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            var newUser = new AppUser
            {
                Id = identityUser.Id,
                UserName = identityUser.UserName,
                Email = identityUser.Email,
                FullName = fullName,
                EmployeeCode = employeeCode,
                Rank = ""
            };

            _dbContext.AppUsers.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return newUser;
        }

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            return await _dbContext.AppUsers.AsNoTracking().ToListAsync();
        }
    }
}
