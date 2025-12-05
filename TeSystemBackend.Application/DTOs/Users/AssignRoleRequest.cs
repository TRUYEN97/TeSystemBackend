namespace TeSystemBackend.Application.DTOs.Users;

public class AssignRoleRequest
{
    public int UserId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

