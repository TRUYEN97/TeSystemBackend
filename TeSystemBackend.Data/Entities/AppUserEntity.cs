using Microsoft.AspNetCore.Identity;

namespace TeSystemBackend.Data.Entities
{
    public class AppUserEntity : IdentityUser<long>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public string? CurrentAccessToken { get; set; }
        public DateTime? CurrentAccessTokenExpiresAt { get; set; }

    }
}
