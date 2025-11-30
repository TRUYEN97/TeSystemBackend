namespace TeSystemBackend.Application.DTOs.Acl;

public class ResourcePermissionDto
{
    public int EntryId { get; set; }
    public int SubjectType { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public int PermissionId { get; set; }
    public bool Granting { get; set; }
}

