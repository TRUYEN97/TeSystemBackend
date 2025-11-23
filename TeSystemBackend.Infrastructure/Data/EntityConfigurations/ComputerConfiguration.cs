using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class ComputerConfiguration : IEntityTypeConfiguration<Computer>
{
    public void Configure(EntityTypeBuilder<Computer> builder)
    {
        builder.ToTable("Computers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.MacAddress)
            .HasMaxLength(50);

        builder.Property(c => c.IpAddress)
            .HasMaxLength(50);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.MacAddress);
        builder.HasIndex(c => c.OwnerTeamId);

        builder.HasOne(c => c.OwnerTeam)
            .WithMany()
            .HasForeignKey(c => c.OwnerTeamId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.ComputerSoftwares)
            .WithOne(cs => cs.Computer)
            .HasForeignKey(cs => cs.ComputerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.InstallationHistories)
            .WithOne(ih => ih.Computer)
            .HasForeignKey(ih => ih.ComputerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

