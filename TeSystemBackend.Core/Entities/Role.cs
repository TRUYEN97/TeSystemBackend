namespace TeSystemBackend.Core.Entities
{
    public class Role
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<UserModelRole> UserRoles { get; set; } = new();
        public List<RoleMixPermission> RoleMixPermissions { get; set; } = new();
    }
}
