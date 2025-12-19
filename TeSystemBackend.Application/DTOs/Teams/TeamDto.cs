namespace TeSystemBackend.Application.DTOs.Teams;

public class TeamDto
{
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MemberCount { get; set; }
}

