namespace TeSystemBackend.Domain.Entities;

public class SoftwareFile
{
    public int Id { get; set; }
    public int SoftwareVersionId { get; set; }
    public string RelativePath { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Md5 { get; set; } = string.Empty;
    public int? OwnerTeamId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual SoftwareVersion SoftwareVersion { get; set; } = null!;
    public virtual Team? OwnerTeam { get; set; }
    public virtual List<SoftwareFileLocation> Locations { get; set; } = [];
}

