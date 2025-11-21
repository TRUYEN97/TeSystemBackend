using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(t => t.Name);

        builder.HasIndex(t => t.ParentTeamId);
        builder.HasIndex(t => t.DepartmentId);

        builder.HasOne(t => t.ParentTeam)
            .WithMany(t => t.ChildTeams)
            .HasForeignKey(t => t.ParentTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Department)
            .WithMany(d => d.Teams)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(t => t.Users)
            .WithOne(u => u.Team)
            .HasForeignKey(u => u.TeamId)
            .OnDelete(DeleteBehavior.SetNull);

    }
}

