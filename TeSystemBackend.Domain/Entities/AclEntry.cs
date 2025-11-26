namespace TeSystemBackend.Domain.Entities;

public class AclEntry
{
    public int Id { get; set; }
    public int ObjectIdentityId { get; set; }
    public int SidId { get; set; }
    public int PermissionId { get; set; }
    public bool Granting { get; set; }
    public bool AuditSuccess { get; set; }
    public bool AuditFailure { get; set; }

    public virtual AclObjectIdentity ObjectIdentity { get; set; } = null!;
    public virtual AclSid Sid { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}

