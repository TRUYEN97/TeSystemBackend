namespace TeSystemBackend.Domain.Entities;

public class Software
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OwnerTeamId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Team? OwnerTeam { get; set; }
    public virtual List<SoftwareVersion> SoftwareVersions { get; set; } = new List<SoftwareVersion>();
    public virtual List<ComputerSoftware> ComputerSoftwares { get; set; } = new List<ComputerSoftware>();
}

