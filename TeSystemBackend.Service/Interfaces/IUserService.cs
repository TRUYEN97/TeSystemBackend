using TeSystemBackend.Data.Entities;

namespace TeSystemBackend.Service.Interfaces
{
    public interface IUserService
    {
        Task<AppUserEntity> RegisterAsync(string userName, string email, string password, string fullName);
        Task<List<AppUserEntity>> GetAllUsersAsync();
    }
}

