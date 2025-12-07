namespace TeSystemBackend.Application.DTOs.Locations;

public class LocationStatisticsDto
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public Dictionary<string, int> ResourceCounts { get; set; } = new();
    public bool IncludeChildren { get; set; }
}

