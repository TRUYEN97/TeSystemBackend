namespace TeSystemBackend.Application.DTOs.Teams;

public class UpdateTeamDto
{
    public int DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}

