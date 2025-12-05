namespace TeSystemBackend.Application.DTOs.Roles;

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
    public int UserCount { get; set; }
}
