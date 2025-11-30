using TeSystemBackend.Application.DTOs.Acl;

namespace TeSystemBackend.Application.Services;

public interface IAclService
{
    Task AssignPermissionAsync(AssignPermissionRequest request);
    Task RevokePermissionAsync(int entryId);
    Task<List<ResourcePermissionDto>> GetResourcePermissionsAsync(string resourceType, int resourceId);
}

