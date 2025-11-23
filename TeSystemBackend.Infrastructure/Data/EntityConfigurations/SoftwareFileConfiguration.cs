using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class SoftwareFileConfiguration : IEntityTypeConfiguration<SoftwareFile>
{
    public void Configure(EntityTypeBuilder<SoftwareFile> builder)
    {
        builder.ToTable("SoftwareFiles");

        builder.HasKey(sf => sf.Id);

        builder.Property(sf => sf.RelativePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(sf => sf.Size)
            .IsRequired();

        builder.Property(sf => sf.Md5)
            .IsRequired()
            .HasMaxLength(32);

        builder.HasIndex(sf => sf.Md5);
        builder.HasIndex(sf => new { sf.SoftwareVersionId, sf.RelativePath })
            .IsUnique();
        builder.HasIndex(sf => sf.SoftwareVersionId);
        builder.HasIndex(sf => sf.OwnerTeamId);

        builder.HasOne(sf => sf.SoftwareVersion)
            .WithMany(sv => sv.SoftwareFiles)
            .HasForeignKey(sf => sf.SoftwareVersionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sf => sf.OwnerTeam)
            .WithMany()
            .HasForeignKey(sf => sf.OwnerTeamId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(sf => sf.Locations)
            .WithOne(sfl => sfl.SoftwareFile)
            .HasForeignKey(sfl => sfl.SoftwareFileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

