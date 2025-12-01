namespace TeSystemBackend.Domain.Entities;

public class Computer
{
    public int Id { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public string? Description { get; set; }

    public virtual Location Location { get; set; } = null!;
}

