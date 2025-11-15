namespace TeSystemBackend.Core.Entities
{
    public class AclEntry
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ResourceId { get; set; } 
        public long PermissionId { get; set; } 
        public bool IsAllowed { get; set; } = true;

        public AppUser User { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
