namespace TeSystemBackend.Service.Interfaces
{
    public interface IAuthService
    {
        Task<(string accessToken, string refreshToken)> LoginAsync(string username, string password, string ipAddress);
        Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string oldRefreshToken, string ipAddress);
    }
}

