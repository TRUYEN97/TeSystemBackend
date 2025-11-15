using System;

namespace TeSystemBackend.Data.Entities
{
    public class RefreshTokenEntity
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public AppUserEntity User { get; set; } = null!;

        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? CreatedByIp { get; set; }

        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public string? RevokedByIp { get; set; }
    }
}
