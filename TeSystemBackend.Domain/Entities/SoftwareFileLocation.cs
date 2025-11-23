namespace TeSystemBackend.Domain.Entities;

public class SoftwareFileLocation
{
    public int Id { get; set; }
    public int SoftwareFileId { get; set; }
    public string LocationType { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual SoftwareFile SoftwareFile { get; set; } = null!;
}

