using Microsoft.AspNetCore.Identity;

namespace TeSystemBackend.Data.Entities
{
    public class AppUserEntity : IdentityUser<long>
    {
        public string FullName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;

    }
}
