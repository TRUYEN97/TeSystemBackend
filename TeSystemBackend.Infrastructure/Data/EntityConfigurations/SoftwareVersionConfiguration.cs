using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class SoftwareVersionConfiguration : IEntityTypeConfiguration<SoftwareVersion>
{
    public void Configure(EntityTypeBuilder<SoftwareVersion> builder)
    {
        builder.ToTable("SoftwareVersions");

        builder.HasKey(sv => sv.Id);

        builder.Property(sv => sv.Version)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(sv => sv.ReleaseNotes)
            .HasMaxLength(5000);

        builder.Property(sv => sv.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(sv => new { sv.SoftwareId, sv.Version })
            .IsUnique();

        builder.HasOne(sv => sv.Software)
            .WithMany(s => s.SoftwareVersions)
            .HasForeignKey(sv => sv.SoftwareId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(sv => sv.SoftwareFiles)
            .WithOne(sf => sf.SoftwareVersion)
            .HasForeignKey(sf => sf.SoftwareVersionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(sv => sv.ComputerSoftwares)
            .WithOne(cs => cs.InstalledSoftwareVersion)
            .HasForeignKey(cs => cs.InstalledSoftwareVersionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(sv => sv.InstallationHistories)
            .WithOne(ih => ih.SoftwareVersion)
            .HasForeignKey(ih => ih.SoftwareVersionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


