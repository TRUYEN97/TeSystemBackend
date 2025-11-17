using System.ComponentModel.DataAnnotations;

namespace TeSystemBackend.API.DTOs.User
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "RefreshToken không được để trống")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
