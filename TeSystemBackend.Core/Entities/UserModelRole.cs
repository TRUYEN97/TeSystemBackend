namespace TeSystemBackend.Core.Entities
{
    public class UserModelRole
    {
        public long UserId { get; set; }
        public AppUser User { get; set; } = null!;

        public long ModelId { get; set; }
        public Model Model { get; set; } = null!;

        public long RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public bool IsTemporary { get; set; } = false;
        public DateTime? ExpireAt { get; set; }
    }
}
