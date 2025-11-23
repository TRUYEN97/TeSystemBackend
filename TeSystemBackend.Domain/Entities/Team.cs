namespace TeSystemBackend.Domain.Entities;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentTeamId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Team? ParentTeam { get; set; }
    public virtual List<Team> ChildTeams { get; set; } = new List<Team>();
    public virtual List<AppUser> Users { get; set; } = new List<AppUser>();
    public virtual List<AclEntry> AclEntries { get; set; } = new List<AclEntry>();
}

