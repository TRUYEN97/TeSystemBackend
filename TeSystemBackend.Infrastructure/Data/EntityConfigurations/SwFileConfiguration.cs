using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class SwFileConfiguration : IEntityTypeConfiguration<SwFile>
{
    public void Configure(EntityTypeBuilder<SwFile> builder)
    {
        builder.ToTable("SwFiles");

        builder.HasKey(sf => sf.Id);

        builder.Property(sf => sf.RelativePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(sf => sf.Size)
            .IsRequired();

        builder.Property(sf => sf.Md5)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(sf => sf.DownloadUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasIndex(sf => sf.Md5);

        builder.HasIndex(sf => new { sf.SwVersionId, sf.RelativePath })
            .IsUnique();

        builder.HasIndex(sf => sf.SwVersionId);
    }
}

