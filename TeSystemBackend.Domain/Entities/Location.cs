namespace TeSystemBackend.Domain.Entities;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }

    public virtual Location? Parent { get; set; }
    public virtual List<Location> Children { get; set; } = new();
    public virtual List<Computer> Computers { get; set; } = new();
    public virtual List<TeamRoleLocation> TeamRoleLocations { get; set; } = new();
}



