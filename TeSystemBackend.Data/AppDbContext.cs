using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Data.Entities;

namespace TeSystemBackend.Data
{
    public class AppDbContext: IdentityDbContext<AppUserEntity, IdentityRole<long>, long>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
