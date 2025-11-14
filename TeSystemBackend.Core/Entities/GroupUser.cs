namespace TeSystemBackend.Core.Entities
{
    public class GroupUser
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<UserMixGroupUser> Members { get; set; } = new();
    }
}
