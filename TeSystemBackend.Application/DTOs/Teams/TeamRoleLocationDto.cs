namespace TeSystemBackend.Application.DTOs.Teams;

public class TeamRoleLocationDto
{
    public int Id { get; set; }
    public int TeamId { get; set; }
    public int RoleId { get; set; }
    public int LocationId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
}
