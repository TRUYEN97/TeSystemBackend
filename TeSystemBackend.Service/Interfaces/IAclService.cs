using TeSystemBackend.Core.Entities;

namespace TeSystemBackend.Service.Interfaces
{
    public interface IAclService
    {
        Task<AclEntry> GrantPermissionAsync(long userId, long resourceId, long permissionId);
        Task RevokePermissionAsync(long userId, long resourceId, long permissionId);
    }
}

