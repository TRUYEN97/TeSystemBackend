using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class SoftwareFileLocationConfiguration : IEntityTypeConfiguration<SoftwareFileLocation>
{
    public void Configure(EntityTypeBuilder<SoftwareFileLocation> builder)
    {
        builder.ToTable("SoftwareFileLocations");

        builder.HasKey(sfl => sfl.Id);

        builder.Property(sfl => sfl.LocationType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(sfl => sfl.DownloadUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(sfl => sfl.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(sfl => sfl.SoftwareFileId);
        builder.HasIndex(sfl => new { sfl.SoftwareFileId, sfl.IsPrimary })
            .HasFilter("[IsPrimary] = 1")
            .IsUnique();

        builder.HasOne(sfl => sfl.SoftwareFile)
            .WithMany(sf => sf.Locations)
            .HasForeignKey(sfl => sfl.SoftwareFileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

