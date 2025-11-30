using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class AclSidConfiguration : IEntityTypeConfiguration<AclSid>
{
    public void Configure(EntityTypeBuilder<AclSid> builder)
    {
        builder.ToTable("Acl_Sid");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Principal)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(s => s.SidName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(s => new { s.Principal, s.SidName })
            .IsUnique();
    }
}




