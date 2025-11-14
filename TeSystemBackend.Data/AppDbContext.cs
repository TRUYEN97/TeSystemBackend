using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Core.Entities;
using TeSystemBackend.Data.Entities;

namespace TeSystemBackend.Data
{
    public class AppDbContext: IdentityDbContext<AppUserEntity, IdentityRole<long>, long>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        DbSet<GroupUser> GroupUsers { get; set; }
        DbSet<Model> Models { get; set; }
        DbSet<Role> Roles { get;set; }
        DbSet<Permission> Permissions { get; set; }
    }
}
