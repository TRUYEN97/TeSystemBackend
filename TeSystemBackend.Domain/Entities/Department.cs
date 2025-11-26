namespace TeSystemBackend.Domain.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual List<Team> Teams { get; set; } = new();
}


