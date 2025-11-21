using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .HasMaxLength(100);

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(u => u.TeamId);

        builder.HasMany(u => u.AclEntries)
            .WithOne(ae => ae.CreatedByUser)
            .HasForeignKey(ae => ae.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.InstallationHistories)
            .WithOne(ih => ih.InstalledByUser)
            .HasForeignKey(ih => ih.InstalledBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.ChangeLogs)
            .WithOne(cl => cl.ChangedByUser)
            .HasForeignKey(cl => cl.ChangedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

