using System.ComponentModel.DataAnnotations;

namespace TeSystemBackend.API.DTOs.Users
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "UserName không được để trống")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password không được để trống")]
        [MinLength(6, ErrorMessage = "Password phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "FullName không được để trống")]
        public string FullName { get; set; } = string.Empty;
    }
}
