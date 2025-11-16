using Microsoft.AspNetCore.Identity;
using TeSystemBackend.Core.Entities;
using TeSystemBackend.Data.Entities;

namespace TeSystemBackend.Data.Entities
{
    public class AppUserEntity : IdentityUser<long>
    {
        public string FullName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public List<RefreshTokenEntity> RefreshTokens { get; set; } = new();
        public List<UserMixGroupUser> Groups { get; set; } = [];
        public List<UserModelRole> UserRoles { get; set; } = [];
    }
}
