namespace TeSystemBackend.Application.DTOs.Teams;

public class AssignTeamRoleLocationRequest
{
    public int TeamId { get; set; }
    public int RoleId { get; set; }
    public int LocationId { get; set; }
}
