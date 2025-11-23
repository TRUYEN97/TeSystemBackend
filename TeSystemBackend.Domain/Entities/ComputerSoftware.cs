namespace TeSystemBackend.Domain.Entities;

public class ComputerSoftware
{
    public int Id { get; set; }
    public int ComputerId { get; set; }
    public int SoftwareId { get; set; }
    public int? InstalledSoftwareVersionId { get; set; }
    public DateTime InstalledAt { get; set; } = DateTime.UtcNow;

    public virtual Computer Computer { get; set; } = null!;
    public virtual Software Software { get; set; } = null!;
    public virtual SoftwareVersion? InstalledSoftwareVersion { get; set; }
}

