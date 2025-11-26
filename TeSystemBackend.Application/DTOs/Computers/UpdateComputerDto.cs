namespace TeSystemBackend.Application.DTOs.Computers;

public class UpdateComputerDto
{
    public string Name { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public string? Description { get; set; }
}


