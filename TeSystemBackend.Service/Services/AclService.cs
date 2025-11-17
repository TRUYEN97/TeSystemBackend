using TeSystemBackend.Core.Entities;
using TeSystemBackend.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Service.Interfaces;

namespace TeSystemBackend.Service
{
    public class AclService : IAclService
    {
        private readonly IAppDbContext _dbContext;

        public AclService(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<AclEntry> GrantPermissionAsync(long userId, long resourceId, long permissionId)
        {
            var existing = await _dbContext.AclEntries
                .FirstOrDefaultAsync(a => a.UserId == userId && a.ResourceId == resourceId && a.PermissionId == permissionId);

            if (existing != null)
            {
                existing.IsAllowed = true;
                _dbContext.AclEntries.Update(existing);
                await _dbContext.SaveChangesAsync();
                return existing;
            }

            var entry = new AclEntry
            {
                UserId = userId,
                ResourceId = resourceId,
                PermissionId = permissionId,
                IsAllowed = true
            };

            _dbContext.AclEntries.Add(entry);
            await _dbContext.SaveChangesAsync();
            return entry;
        }

        public async Task RevokePermissionAsync(long userId, long resourceId, long permissionId)
        {
            var entry = await _dbContext.AclEntries
                .FirstOrDefaultAsync(a => a.UserId == userId && a.ResourceId == resourceId && a.PermissionId == permissionId);
            if (entry != null)
            {
                entry.IsAllowed = false;
                _dbContext.AclEntries.Update(entry);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
