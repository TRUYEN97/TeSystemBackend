using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class AclEntryConfiguration : IEntityTypeConfiguration<AclEntry>
{
    public void Configure(EntityTypeBuilder<AclEntry> builder)
    {
        builder.ToTable("AclEntries");

        builder.HasKey(ae => ae.Id);

        builder.Property(ae => ae.ResourceType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(ae => ae.ResourceId)
            .IsRequired();

        builder.Property(ae => ae.PrincipalType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(ae => ae.PrincipalId)
            .IsRequired();

        builder.Property(ae => ae.Permission)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ae => ae.AceOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(ae => ae.IsAllow)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(ae => ae.IsDeny)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ae => ae.IsInherited)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(ae => new { ae.ResourceType, ae.ResourceId });
        builder.HasIndex(ae => new { ae.PrincipalType, ae.PrincipalId });
        builder.HasIndex(ae => ae.AceOrder);
        builder.HasIndex(ae => new { ae.ResourceType, ae.ResourceId, ae.PrincipalType, ae.PrincipalId, ae.Permission });

        builder.HasOne(ae => ae.CreatedByUser)
            .WithMany(u => u.AclEntries)
            .HasForeignKey(ae => ae.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

