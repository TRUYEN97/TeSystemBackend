namespace TeSystemBackend.Application.Services;

public class LocationResourceProviderFactory : ILocationResourceProviderFactory
{
    private readonly Dictionary<string, IResourceCounter> _counters;
    private readonly Dictionary<string, object> _providers;

    public LocationResourceProviderFactory(
        ComputerLocationResourceProvider computerProvider)
    {
        _counters = new Dictionary<string, IResourceCounter>
        {
            { computerProvider.ResourceType, computerProvider }
        };

        _providers = new Dictionary<string, object>
        {
            { computerProvider.ResourceType, computerProvider }
        };
    }

    public IResourceCounter? GetCounter(string resourceType)
    {
        return _counters.TryGetValue(resourceType, out var counter) ? counter : null;
    }

    public ILocationResourceProvider<TDto>? GetProvider<TDto>(string resourceType) where TDto : class
    {
        if (_providers.TryGetValue(resourceType, out var provider) &&
            provider is ILocationResourceProvider<TDto> typedProvider)
        {
            return typedProvider;
        }
        return null;
    }

    public List<IResourceCounter> GetAllCounters()
    {
        return _counters.Values.ToList();
    }

    public List<string> GetAvailableResourceTypes()
    {
        return _counters.Keys.ToList();
    }
}

