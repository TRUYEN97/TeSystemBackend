using TeSystemBackend.Application.DTOs.Auth;

namespace TeSystemBackend.Application.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}


