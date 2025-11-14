using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.Service;
using TeSystemBackend.API.DTOs.Users;

namespace TeSystemBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService) => _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.RegisterAsync(
                    request.UserName,
                    request.Email,
                    request.Password,
                    request.FullName,
                    request.EmployeeCode
                );

                var response = new RegisterResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    EmployeeCode = user.EmployeeCode
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
