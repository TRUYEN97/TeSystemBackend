namespace TeSystemBackend.Core.Entities
{
    public class UserMixGroupUser
    {
        public long UserId { get; set; }

        public long GroupUserId { get; set; }
        public GroupUser GroupUser { get; set; } = null!;

        public bool IsApproved { get; set; } = false;
    }
}
