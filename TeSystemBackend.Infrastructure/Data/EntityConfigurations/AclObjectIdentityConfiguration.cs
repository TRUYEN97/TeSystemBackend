using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class AclObjectIdentityConfiguration : IEntityTypeConfiguration<AclObjectIdentity>
{
    public void Configure(EntityTypeBuilder<AclObjectIdentity> builder)
    {
        builder.ToTable("Acl_Object_Identity");

        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.EntriesInheriting)
            .IsRequired();

        builder.HasIndex(oi => new { oi.ResourceTypeId, oi.ResourceId })
            .IsUnique();

        builder.HasOne(oi => oi.ResourceType)
            .WithMany(c => c.ObjectIdentities)
            .HasForeignKey(oi => oi.ResourceTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(oi => oi.ParentObject)
            .WithMany(p => p.Children)
            .HasForeignKey(oi => oi.ParentObjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(oi => oi.Owner)
            .WithMany(s => s.OwnedObjectIdentities)
            .HasForeignKey(oi => oi.OwnerSid)
            .OnDelete(DeleteBehavior.Restrict);
    }
}




