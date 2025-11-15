namespace TeSystemBackend.Data.Entities
{
    public class RefreshTokenEntity
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public AppUserEntity User { get; set; } = null!;

        public string Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
    }
}
