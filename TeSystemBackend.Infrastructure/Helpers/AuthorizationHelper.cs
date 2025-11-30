using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Domain.Enums;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.Infrastructure.Helpers;

public static class AuthorizationHelper
{
    public static bool IsAdmin(AppUser user)
    {
        return user.UserName == "admin";
    }

    public static async Task<bool> HasPermissionAsync(
        ApplicationDbContext context,
        AppUser user,
        string resourceType,
        int resourceId,
        string permissionName)
    {
        if (IsAdmin(user))
        {
            return true;
        }

        var userSid = await context.AclSids
            .FirstOrDefaultAsync(s => s.Principal == PrincipalType.User && s.SidName == user.UserName);

        if (userSid == null)
        {
            return false;
        }

        var aclClass = await context.AclClasses
            .FirstOrDefaultAsync(c => c.Name == resourceType);

        if (aclClass == null)
        {
            return false;
        }

        var objectIdentity = await context.AclObjectIdentities
            .FirstOrDefaultAsync(oi => oi.ResourceTypeId == aclClass.Id && oi.ResourceId == resourceId);

        if (objectIdentity == null)
        {
            return false;
        }

        var permission = await context.Permissions
            .FirstOrDefaultAsync(p => p.Name == permissionName);

        if (permission == null)
        {
            return false;
        }

        var hasEntry = await context.AclEntries
            .AnyAsync(e => e.ObjectIdentityId == objectIdentity.Id
                && e.SidId == userSid.Id
                && e.PermissionId == permission.Id
                && e.Granting);

        if (hasEntry)
        {
            return true;
        }

        var userTeams = await context.UserTeams
            .Where(ut => ut.UserId == user.Id)
            .Select(ut => ut.TeamId)
            .ToListAsync();

        if (userTeams.Any())
        {
            var teamSidNames = userTeams.Select(t => t.ToString()).ToList();
            var teamSids = await context.AclSids
                .Where(s => s.Principal == PrincipalType.Team
                    && teamSidNames.Contains(s.SidName))
                .Select(s => s.Id)
                .ToListAsync();

            if (teamSids.Any())
            {
                hasEntry = await context.AclEntries
                    .AnyAsync(e => e.ObjectIdentityId == objectIdentity.Id
                        && teamSids.Contains(e.SidId)
                        && e.PermissionId == permission.Id
                        && e.Granting);

                if (hasEntry)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

