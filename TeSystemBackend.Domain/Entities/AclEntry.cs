using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Domain.Entities;

public class AclEntry
{
    public int Id { get; set; }
    public int ResourceTypeId { get; set; }
    public int ResourceId { get; set; }
    public PrincipalType PrincipalType { get; set; }
    public int PrincipalId { get; set; }
    public string Permission { get; set; } = string.Empty;
    public int AceOrder { get; set; }
    public bool IsAllow { get; set; } = true;
    public bool IsDeny { get; set; } = false;
    public bool IsInherited { get; set; } = false;
    public int? GrantFromScopeId { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ResourceType ResourceType { get; set; } = null!;
    public virtual AppUser CreatedByUser { get; set; } = null!;
}

