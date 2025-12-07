namespace TeSystemBackend.Application.DTOs.Locations;

public class LocationResourcesDto
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public Dictionary<string, int> ResourceCounts { get; set; } = new();
    public Dictionary<string, object> Resources { get; set; } = new();
    public bool IncludeChildren { get; set; }
}

