namespace TeSystemBackend.Application.Services;

public interface ILocationResourceProviderFactory
{
    IResourceCounter? GetCounter(string resourceType);
    ILocationResourceProvider<TDto>? GetProvider<TDto>(string resourceType) where TDto : class;
    List<IResourceCounter> GetAllCounters();
    List<string> GetAvailableResourceTypes();
}

public interface IResourceCounter
{
    string ResourceType { get; }
    Task<int> CountByLocationIdAsync(int locationId);
    Task<Dictionary<int, int>> CountByLocationIdsAsync(List<int> locationIds);
}

