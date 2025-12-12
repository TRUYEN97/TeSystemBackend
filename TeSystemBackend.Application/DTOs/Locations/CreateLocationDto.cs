namespace TeSystemBackend.Application.DTOs.Locations;

public class CreateLocationDto
{
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
}

