using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Computer> Computers { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<UserTeam> UserTeams { get; set; }
    public DbSet<Role> AppRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<PerRole> PerRoles { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<TeamRoleLocation> TeamRoleLocations { get; set; }
    public DbSet<AclClass> AclClasses { get; set; }
    public DbSet<AclObjectIdentity> AclObjectIdentities { get; set; }
    public DbSet<AclSid> AclSids { get; set; }
    public DbSet<AclEntry> AclEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

