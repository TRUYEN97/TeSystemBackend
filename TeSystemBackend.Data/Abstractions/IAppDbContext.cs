using Microsoft.EntityFrameworkCore;
using System.Threading;
using TeSystemBackend.Core.Entities;
using TeSystemBackend.Data.Entities;

namespace TeSystemBackend.Data.Abstractions
{
    public interface IAppDbContext
    {
        DbSet<GroupUser> GroupUsers { get; }
        DbSet<UserMixGroupUser> UserMixGroupUsers { get; }
        DbSet<Model> Models { get; }
        DbSet<Role> AppRoles { get; }
        DbSet<Permission> AppPermissions { get; }
        DbSet<UserModelRole> UserModelRoles { get; }
        DbSet<RoleMixPermission> RoleMixPermissions { get; }
        DbSet<AclEntry> AclEntries { get; }
        DbSet<RefreshTokenEntity> RefreshTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

