namespace TeSystemBackend.Domain.Entities;

public class Team
{
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;

    public virtual Department Department { get; set; } = null!;
    public virtual List<UserTeam> UserTeams { get; set; } = new();
    public virtual List<TeamRoleLocation> TeamRoleLocations { get; set; } = new();
}

