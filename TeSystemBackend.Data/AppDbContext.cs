using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TeSystemBackend.Core.Entities;
using TeSystemBackend.Data.Entities;

namespace TeSystemBackend.Data
{
    public class AppDbContext : IdentityDbContext<AppUserEntity, IdentityRole<long>, long>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; } = null!;
        public DbSet<GroupUser> GroupUsers { get; set; } = null!;
        public DbSet<UserMixGroupUser> UserMixGroupUsers { get; set; } = null!;
        public DbSet<Model> Models { get; set; } = null!;
        public DbSet<Role> AppRoles { get; set; } = null!;
        public DbSet<Permission> AppPermissions { get; set; } = null!;
        public DbSet<UserModelRole> UserModelRoles { get; set; } = null!;
        public DbSet<RoleMixPermission> RoleMixPermissions { get; set; } = null!;
        public DbSet<AclEntry> AclEntries { get; set; } = null!;
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserMixGroupUser>()
                .HasKey(um => new { um.UserId, um.GroupUserId });

            builder.Entity<UserMixGroupUser>()
                .HasOne(um => um.User)
                .WithMany(u => u.Groups)
                .HasForeignKey(um => um.UserId);

            builder.Entity<UserMixGroupUser>()
                .HasOne(um => um.GroupUser)
                .WithMany(g => g.Members)
                .HasForeignKey(um => um.GroupUserId);

            builder.Entity<UserModelRole>()
                .HasKey(umr => new { umr.UserId, umr.ModelId, umr.RoleId });

            builder.Entity<UserModelRole>()
                .HasOne(umr => umr.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(umr => umr.UserId);

            builder.Entity<UserModelRole>()
                .HasOne(umr => umr.Model)
                .WithMany(m => m.UserRoles)
                .HasForeignKey(umr => umr.ModelId);

            builder.Entity<UserModelRole>()
                .HasOne(umr => umr.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(umr => umr.RoleId);

            builder.Entity<RoleMixPermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RoleMixPermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RoleMixPermissions)
                .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RoleMixPermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RoleMixPermissions)
                .HasForeignKey(rp => rp.PermissionId);

            builder.Entity<AppUser>()
                .Property(u => u.FullName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Entity<AppUser>()
                .Property(u => u.EmployeeCode)
                .HasMaxLength(50);

            builder.Entity<GroupUser>()
                .Property(g => g.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Entity<Model>()
                .Property(m => m.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Entity<Role>()
                .Property(r => r.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Entity<Permission>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Entity<Permission>()
                .Property(p => p.Description)
                .HasMaxLength(500);

            builder.Entity<AclEntry>(ae =>
            {
                ae.HasKey(x => x.Id);
                ae.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
                ae.HasOne(x => x.Permission).WithMany().HasForeignKey(x => x.PermissionId);
            });

            builder.Entity<RefreshTokenEntity>(entity =>
            {
                entity.ToTable("RefreshTokens");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Token)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.HasIndex(e => e.Token)
                      .IsUnique();

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
