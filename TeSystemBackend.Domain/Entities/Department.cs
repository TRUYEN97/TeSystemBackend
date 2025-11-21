namespace TeSystemBackend.Domain.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentDepartmentId { get; set; }
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual Department? ParentDepartment { get; set; }
    public virtual List<Department> ChildDepartments { get; set; } = new List<Department>();
    public virtual List<Team> Teams { get; set; } = new List<Team>();
    public virtual List<AclEntry> AclEntries { get; set; } = new List<AclEntry>();
}

