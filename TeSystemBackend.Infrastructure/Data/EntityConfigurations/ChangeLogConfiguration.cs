using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class ChangeLogConfiguration : IEntityTypeConfiguration<ChangeLog>
{
    public void Configure(EntityTypeBuilder<ChangeLog> builder)
    {
        builder.ToTable("ChangeLogs");

        builder.HasKey(cl => cl.Id);

        builder.Property(cl => cl.EntityType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cl => cl.EntityId)
            .IsRequired();

        builder.Property(cl => cl.ChangeType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(cl => cl.OldValue)
            .HasColumnType("TEXT");

        builder.Property(cl => cl.NewValue)
            .HasColumnType("TEXT");

        builder.HasIndex(cl => new { cl.EntityType, cl.EntityId });
        builder.HasIndex(cl => cl.ChangedBy);
        builder.HasIndex(cl => cl.ChangedAt);

        builder.HasOne(cl => cl.ChangedByUser)
            .WithMany(u => u.ChangeLogs)
            .HasForeignKey(cl => cl.ChangedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

