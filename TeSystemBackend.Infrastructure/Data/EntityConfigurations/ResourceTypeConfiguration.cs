using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class AclClassConfiguration : IEntityTypeConfiguration<AclClass>
{
    public void Configure(EntityTypeBuilder<AclClass> builder)
    {
        builder.ToTable("Acl_Class");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(rt => rt.Name)
            .IsUnique();

        builder.HasMany(rt => rt.ObjectIdentities)
            .WithOne(oi => oi.ResourceType)
            .HasForeignKey(oi => oi.ResourceTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

