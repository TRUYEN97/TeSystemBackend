namespace TeSystemBackend.Application.DTOs.Computers;

public class CreateComputerDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public string? Description { get; set; }
}




