using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class TeamRoleLocationConfiguration : IEntityTypeConfiguration<TeamRoleLocation>
{
    public void Configure(EntityTypeBuilder<TeamRoleLocation> builder)
    {
        builder.ToTable("TeamRoleLocations");

        builder.HasKey(trl => trl.Id);

        builder.HasIndex(trl => new { trl.TeamId, trl.RoleId, trl.LocationId })
            .IsUnique();

        builder.HasOne(trl => trl.Team)
            .WithMany(t => t.TeamRoleLocations)
            .HasForeignKey(trl => trl.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(trl => trl.Role)
            .WithMany(r => r.TeamRoleLocations)
            .HasForeignKey(trl => trl.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(trl => trl.Location)
            .WithMany(l => l.TeamRoleLocations)
            .HasForeignKey(trl => trl.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


