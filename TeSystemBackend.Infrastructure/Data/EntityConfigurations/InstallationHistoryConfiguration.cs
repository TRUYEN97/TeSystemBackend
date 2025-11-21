using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class InstallationHistoryConfiguration : IEntityTypeConfiguration<InstallationHistory>
{
    public void Configure(EntityTypeBuilder<InstallationHistory> builder)
    {
        builder.ToTable("InstallationHistories");

        builder.HasKey(ih => ih.Id);

        builder.Property(ih => ih.Action)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(ih => ih.Notes)
            .HasMaxLength(2000);

        builder.HasIndex(ih => ih.ComputerId);
        builder.HasIndex(ih => ih.SoftwareId);
        builder.HasIndex(ih => ih.SwVersionId);
        builder.HasIndex(ih => ih.InstalledBy);
        builder.HasIndex(ih => ih.InstalledAt);

        builder.HasOne(ih => ih.Computer)
            .WithMany(c => c.InstallationHistories)
            .HasForeignKey(ih => ih.ComputerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ih => ih.Software)
            .WithMany()
            .HasForeignKey(ih => ih.SoftwareId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(ih => ih.SwVersion)
            .WithMany(sv => sv.InstallationHistories)
            .HasForeignKey(ih => ih.SwVersionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ih => ih.InstalledByUser)
            .WithMany(u => u.InstallationHistories)
            .HasForeignKey(ih => ih.InstalledBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

