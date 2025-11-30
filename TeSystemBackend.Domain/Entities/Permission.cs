namespace TeSystemBackend.Domain.Entities;

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual List<PerRole> PerRoles { get; set; } = new();
    public virtual List<AclEntry> AclEntries { get; set; } = new();
}




