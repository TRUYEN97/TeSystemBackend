using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Domain.Entities;

public class AclSid
{
    public int Id { get; set; }
    public PrincipalType Principal { get; set; }
    public string SidName { get; set; } = string.Empty;

    public virtual List<AclObjectIdentity> OwnedObjectIdentities { get; set; } = new();
    public virtual List<AclEntry> Entries { get; set; } = new();
}



