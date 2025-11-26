using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class AclEntryConfiguration : IEntityTypeConfiguration<AclEntry>
{
    public void Configure(EntityTypeBuilder<AclEntry> builder)
    {
        builder.ToTable("Acl_Entry");

        builder.HasKey(ae => ae.Id);

        builder.Property(ae => ae.Granting)
            .IsRequired();

        builder.Property(ae => ae.AuditSuccess)
            .IsRequired();

        builder.Property(ae => ae.AuditFailure)
            .IsRequired();

        builder.HasIndex(ae => new { ae.ObjectIdentityId, ae.SidId, ae.PermissionId });

        builder.HasOne(ae => ae.ObjectIdentity)
            .WithMany(oi => oi.Entries)
            .HasForeignKey(ae => ae.ObjectIdentityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ae => ae.Sid)
            .WithMany(s => s.Entries)
            .HasForeignKey(ae => ae.SidId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ae => ae.Permission)
            .WithMany(p => p.AclEntries)
            .HasForeignKey(ae => ae.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

