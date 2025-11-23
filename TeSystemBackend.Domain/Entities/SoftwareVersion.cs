namespace TeSystemBackend.Domain.Entities;

public class SoftwareVersion
{
    public int Id { get; set; }
    public int SoftwareId { get; set; }
    public string Version { get; set; } = string.Empty;
    public string? ReleaseNotes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime ReleasedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Software Software { get; set; } = null!;
    public virtual List<SoftwareFile> SoftwareFiles { get; set; } = new List<SoftwareFile>();
    public virtual List<ComputerSoftware> ComputerSoftwares { get; set; } = new List<ComputerSoftware>();
    public virtual List<InstallationHistory> InstallationHistories { get; set; } = new List<InstallationHistory>();
}

