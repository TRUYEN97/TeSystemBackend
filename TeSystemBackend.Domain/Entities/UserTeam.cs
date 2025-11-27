namespace TeSystemBackend.Domain.Entities;

public class UserTeam
{
    public int UserId { get; set; }
    public int TeamId { get; set; }

    public virtual AppUser User { get; set; } = null!;
    public virtual Team Team { get; set; } = null!;
}



