namespace TeSystemBackend.Domain.Entities;

public class SwFile
{
    public int Id { get; set; }
    public int SwVersionId { get; set; }
    public string RelativePath { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Md5 { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual SwVersion SwVersion { get; set; } = null!;
}

