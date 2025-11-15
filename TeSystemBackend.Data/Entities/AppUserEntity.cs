using Microsoft.AspNetCore.Identity;
using TeSystemBackend.Data.Entities;

public class AppUserEntity : IdentityUser<long>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public List<RefreshTokenEntity> RefreshTokens { get; set; } = new();
}
