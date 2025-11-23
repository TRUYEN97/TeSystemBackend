namespace TeSystemBackend.Domain.Entities;

public class Computer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? MacAddress { get; set; }
    public string? IpAddress { get; set; }
    public int? OwnerTeamId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Team? OwnerTeam { get; set; }
    public virtual List<ComputerSoftware> ComputerSoftwares { get; set; } = [];
    public virtual List<InstallationHistory> InstallationHistories { get; set; } = [];
}

