namespace TeSystemBackend.Application.Services;

public interface ILocationResourceProvider<TDto> where TDto : class
{
    string ResourceType { get; }
    
    Task<int> CountByLocationIdAsync(int locationId);
    Task<Dictionary<int, int>> CountByLocationIdsAsync(List<int> locationIds);
    
    Task<List<TDto>> GetByLocationIdAsync(int locationId);
    Task<Dictionary<int, List<TDto>>> GetByLocationIdsAsync(List<int> locationIds);
    
    Task<List<TDto>> GetByLocationIdWithAuthAsync(int locationId, int userId, IAppAuthorizationService authService);
}

