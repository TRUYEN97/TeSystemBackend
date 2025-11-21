using Microsoft.AspNetCore.Identity;

namespace TeSystemBackend.Domain.Entities;

public class AppUser : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? TeamId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual Team? Team { get; set; }
    public virtual List<AclEntry> AclEntries { get; set; } = new List<AclEntry>();
    public virtual List<InstallationHistory> InstallationHistories { get; set; } = new List<InstallationHistory>();
    public virtual List<ChangeLog> ChangeLogs { get; set; } = new List<ChangeLog>();
}

