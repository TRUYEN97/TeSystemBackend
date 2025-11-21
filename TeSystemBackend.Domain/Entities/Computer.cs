namespace TeSystemBackend.Domain.Entities;

public class Computer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MacAddress { get; set; }
    public string? IpAddress { get; set; }
    public string? OperatingSystem { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastSeenAt { get; set; }

    public virtual List<ComputerSoftware> ComputerSoftwares { get; set; } = new List<ComputerSoftware>();
    public virtual List<InstallationHistory> InstallationHistories { get; set; } = new List<InstallationHistory>();
}

