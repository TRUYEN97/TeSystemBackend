namespace TeSystemBackend.Domain.Entities;

public class ResourceType
{
    public int Id { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual List<AclEntry> AclEntries { get; set; } = new List<AclEntry>();
}

