using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Domain.Entities;

public class InstallationHistory
{
    public int Id { get; set; }
    public int ComputerId { get; set; }
    public int SoftwareId { get; set; }
    public int SoftwareVersionId { get; set; }
    public InstallationAction Action { get; set; }
    public int? InstalledBy { get; set; }
    public DateTime InstalledAt { get; set; } = DateTime.UtcNow;

    public virtual Computer Computer { get; set; } = null!;
    public virtual Software Software { get; set; } = null!;
    public virtual SoftwareVersion SoftwareVersion { get; set; } = null!;
    public virtual AppUser? InstalledByUser { get; set; }
}

