namespace TeSystemBackend.Application.DTOs.Acl;

public class AssignPermissionRequest
{
    public string ResourceType { get; set; } = string.Empty;
    public int ResourceId { get; set; }
    public int SubjectType { get; set; }
    public int SubjectId { get; set; }
    public int PermissionId { get; set; }
}

