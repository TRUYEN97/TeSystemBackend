using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class ResourceTypeConfiguration : IEntityTypeConfiguration<ResourceType>
{
    public void Configure(EntityTypeBuilder<ResourceType> builder)
    {
        builder.ToTable("ResourceTypes");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.TypeName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(rt => rt.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(rt => rt.TypeName)
            .IsUnique();

        builder.HasMany(rt => rt.AclEntries)
            .WithOne(ae => ae.ResourceType)
            .HasForeignKey(ae => ae.ResourceTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

