using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("Locations");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(l => l.ParentId);

        builder.HasOne(l => l.Parent)
            .WithMany(p => p.Children)
            .HasForeignKey(l => l.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}



