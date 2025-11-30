using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class PerRoleConfiguration : IEntityTypeConfiguration<PerRole>
{
    public void Configure(EntityTypeBuilder<PerRole> builder)
    {
        builder.ToTable("PerRoles");

        builder.HasKey(pr => pr.Id);

        builder.HasIndex(pr => new { pr.RoleId, pr.PermissionId })
            .IsUnique();

        builder.HasOne(pr => pr.Role)
            .WithMany(r => r.PerRoles)
            .HasForeignKey(pr => pr.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pr => pr.Permission)
            .WithMany(p => p.PerRoles)
            .HasForeignKey(pr => pr.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}




