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

        builder.Property(c => c.IpAddress)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.HasIndex(c => c.IpAddress)
            .IsUnique();

        builder.HasIndex(c => c.LocationId);

        builder.HasOne(c => c.Location)
            .WithMany(l => l.Computers)
            .HasForeignKey(c => c.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

