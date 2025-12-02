namespace TeSystemBackend.Application.Services;

public interface IAppAuthorizationService
{
    Task<bool> IsAdminAsync(int userId);
    Task<bool> HasPermissionAsync(int userId, string permission, int? locationId = null);
    Task<bool> HasPermissionOnResourceAsync(int userId, string permission, int resourceId, string resourceType);
    
    Task<bool> CanViewComputerAsync(int userId, int computerId);
    Task<bool> CanCreateComputerAsync(int userId, int locationId);
    Task<bool> CanUpdateComputerAsync(int userId, int computerId);
    Task<bool> CanDeleteComputerAsync(int userId, int computerId);
    
    Task<bool> CanManageTeamsAsync(int userId);
    Task<bool> CanManageDepartmentsAsync(int userId);
}
