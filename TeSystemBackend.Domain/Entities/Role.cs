namespace TeSystemBackend.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual List<PerRole> PerRoles { get; set; } = new();
    public virtual List<TeamRoleLocation> TeamRoleLocations { get; set; } = new();
}




