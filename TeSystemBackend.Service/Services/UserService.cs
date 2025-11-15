using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Core.Entities;
using TeSystemBackend.Data;
using TeSystemBackend.Data.Entities;

namespace TeSystemBackend.Service
{
    public class UserService
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUserEntity> userManager, AppDbContext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _mapper = mapper;
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

            var newUser = _mapper.Map<AppUser>(identityUser);
            newUser.FullName = fullName;
            newUser.EmployeeCode = employeeCode;
            newUser.Rank = "";

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
