using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class SoftwareConfiguration : IEntityTypeConfiguration<Software>
{
    public void Configure(EntityTypeBuilder<Software> builder)
    {
        builder.ToTable("Softwares");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.Vendor)
            .HasMaxLength(200);

        builder.Property(s => s.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(s => s.Name);

        builder.HasMany(s => s.SwVersions)
            .WithOne(sv => sv.Software)
            .HasForeignKey(sv => sv.SoftwareId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.ComputerSoftwares)
            .WithOne(cs => cs.Software)
            .HasForeignKey(cs => cs.SoftwareId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

