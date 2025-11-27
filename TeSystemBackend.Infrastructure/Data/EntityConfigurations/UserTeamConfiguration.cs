using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Infrastructure.Data.EntityConfigurations;

public class UserTeamConfiguration : IEntityTypeConfiguration<UserTeam>
{
    public void Configure(EntityTypeBuilder<UserTeam> builder)
    {
        builder.ToTable("UserTeams");

        builder.HasKey(ut => new { ut.UserId, ut.TeamId });

        builder.HasIndex(ut => new { ut.UserId, ut.TeamId })
            .IsUnique();

        builder.HasOne(ut => ut.User)
            .WithMany(u => u.UserTeams)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ut => ut.Team)
            .WithMany(t => t.UserTeams)
            .HasForeignKey(ut => ut.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}



