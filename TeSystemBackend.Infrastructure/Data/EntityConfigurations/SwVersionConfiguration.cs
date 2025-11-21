using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class SwVersionConfiguration : IEntityTypeConfiguration<SwVersion>
{
    public void Configure(EntityTypeBuilder<SwVersion> builder)
    {
        builder.ToTable("SwVersions");

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

        builder.HasMany(sv => sv.SwFiles)
            .WithOne(sf => sf.SwVersion)
            .HasForeignKey(sf => sf.SwVersionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(sv => sv.ComputerSoftwares)
            .WithOne(cs => cs.InstalledSwVersion)
            .HasForeignKey(cs => cs.InstalledSwVersionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(sv => sv.InstallationHistories)
            .WithOne(ih => ih.SwVersion)
            .HasForeignKey(ih => ih.SwVersionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

