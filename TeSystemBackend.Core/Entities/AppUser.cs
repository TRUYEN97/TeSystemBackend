namespace TeSystemBackend.Core.Entities
{
    public class AppUser
    {
        public long Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;

        public List<UserMixGroupUser> Groups { get; set; } = new();
        public List<UserModelRole> UserRoles { get; set; } = new();
    }
}
