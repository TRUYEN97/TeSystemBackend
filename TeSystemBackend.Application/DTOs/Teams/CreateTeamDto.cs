namespace TeSystemBackend.Application.DTOs.Teams;

public class CreateTeamDto
{
    public int DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty;
}

