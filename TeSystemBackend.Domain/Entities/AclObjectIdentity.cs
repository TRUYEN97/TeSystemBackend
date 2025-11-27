namespace TeSystemBackend.Domain.Entities;

public class AclObjectIdentity
{
    public int Id { get; set; }
    public int ResourceTypeId { get; set; }
    public int ResourceId { get; set; }
    public int? ParentObjectId { get; set; }
    public int OwnerSid { get; set; }
    public bool EntriesInheriting { get; set; }

    public virtual AclClass ResourceType { get; set; } = null!;
    public virtual AclObjectIdentity? ParentObject { get; set; }
    public virtual List<AclObjectIdentity> Children { get; set; } = new();
    public virtual AclSid Owner { get; set; } = null!;
    public virtual List<AclEntry> Entries { get; set; } = new();
}



