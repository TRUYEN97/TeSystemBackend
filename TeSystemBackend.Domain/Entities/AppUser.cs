using Microsoft.AspNetCore.Identity;

namespace TeSystemBackend.Domain.Entities;

public class AppUser : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;

    public virtual List<UserTeam> UserTeams { get; set; } = new();
}

