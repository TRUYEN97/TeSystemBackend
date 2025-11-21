using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class ComputerSoftwareConfiguration : IEntityTypeConfiguration<ComputerSoftware>
{
    public void Configure(EntityTypeBuilder<ComputerSoftware> builder)
    {
        builder.ToTable("ComputerSoftwares");

        builder.HasKey(cs => cs.Id);

        builder.HasIndex(cs => new { cs.ComputerId, cs.SoftwareId })
            .IsUnique();

        builder.HasIndex(cs => cs.ComputerId);
        builder.HasIndex(cs => cs.SoftwareId);
        builder.HasIndex(cs => cs.InstalledSwVersionId);
    }
}

