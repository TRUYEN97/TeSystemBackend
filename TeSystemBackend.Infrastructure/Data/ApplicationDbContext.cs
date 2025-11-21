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

    public DbSet<Software> Softwares { get; set; }
    public DbSet<SwVersion> SwVersions { get; set; }
    public DbSet<SwFile> SwFiles { get; set; }
    public DbSet<Computer> Computers { get; set; }
    public DbSet<ComputerSoftware> ComputerSoftwares { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<AclEntry> AclEntries { get; set; }
    public DbSet<InstallationHistory> InstallationHistories { get; set; }
    public DbSet<ChangeLog> ChangeLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

