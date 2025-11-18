using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.API.DTOs.Users;
using TeSystemBackend.API.Responses;
using TeSystemBackend.Service.Interfaces;

namespace TeSystemBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class usersController : ControllerBase
    {
        private readonly IUserService _userService;

        public usersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _userService.RegisterAsync(
                request.UserName,
                request.Email,
                request.Password,
                request.FullName
            );

            var response = new RegisterResponse
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName
            };

            return ApiResponse.Success(response, "Đăng ký thành công");
        }
    }
}
