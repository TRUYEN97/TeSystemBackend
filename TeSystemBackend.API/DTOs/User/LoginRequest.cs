using System.ComponentModel.DataAnnotations;

namespace TeSystemBackend.API.DTOs.User
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "UserName không được để trống")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password không được để trống")]
        public string Password { get; set; } = string.Empty;
    }
}
