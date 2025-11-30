namespace TeSystemBackend.Domain.Entities;

public class TeamRoleLocation
{
    public int Id { get; set; }
    public int TeamId { get; set; }
    public int RoleId { get; set; }
    public int LocationId { get; set; }

    public virtual Team Team { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
    public virtual Location Location { get; set; } = null!;
}




