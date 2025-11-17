using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.API.DTOs.User;
using TeSystemBackend.API.Responses;
using TeSystemBackend.Service.Interfaces;

namespace TeSystemBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var (accessToken, refreshToken) = await _authService.LoginAsync(
                request.UserName!,
                request.Password!,
                ip
            );

            var response = new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return ApiResponse.Success(response, "Đăng nhập thành công");
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var (newAccessToken, newRefreshToken) =
                await _authService.RefreshTokenAsync(request.RefreshToken!, ip);

            var response = new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return ApiResponse.Success(response, "Làm mới token thành công");
        }
    }
}
