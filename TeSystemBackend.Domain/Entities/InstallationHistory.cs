using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Domain.Entities;

public class InstallationHistory
{
    public int Id { get; set; }
    public int ComputerId { get; set; }
    public int SoftwareId { get; set; }
    public int SwVersionId { get; set; }
    public InstallationAction Action { get; set; }
    public int? InstalledBy { get; set; }
    public DateTime InstalledAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    public virtual Computer Computer { get; set; } = null!;
    public virtual Software Software { get; set; } = null!;
    public virtual SwVersion SwVersion { get; set; } = null!;
    public virtual AppUser? InstalledByUser { get; set; }
}

