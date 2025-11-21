namespace TeSystemBackend.Domain.Entities;

public class Software
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Vendor { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual List<SwVersion> SwVersions { get; set; } = new List<SwVersion>();
    public virtual List<ComputerSoftware> ComputerSoftwares { get; set; } = new List<ComputerSoftware>();
}

