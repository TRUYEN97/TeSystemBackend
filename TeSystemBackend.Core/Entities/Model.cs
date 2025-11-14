namespace TeSystemBackend.Core.Entities
{
    public class Model
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<UserModelRole> UserRoles { get; set; } = new();
    }
}
