using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Application.DTOs.Acl;
using TeSystemBackend.Application.Services;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Domain.Enums;
using TeSystemBackend.Infrastructure.Data;

namespace TeSystemBackend.Infrastructure.Services;

public class AclService : IAclService
{
    private readonly ApplicationDbContext _context;

    public AclService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AssignPermissionAsync(AssignPermissionRequest request)
    {
        var aclClass = await _context.AclClasses
            .FirstOrDefaultAsync(c => c.Name == request.ResourceType);

        if (aclClass == null)
        {
            throw new KeyNotFoundException($"Resource type '{request.ResourceType}' không tồn tại.");
        }

        var objectIdentity = await _context.AclObjectIdentities
            .FirstOrDefaultAsync(oi => oi.ResourceTypeId == aclClass.Id && oi.ResourceId == request.ResourceId);

        if (objectIdentity == null)
        {
            var adminSid = await _context.AclSids
                .FirstOrDefaultAsync(s => s.Principal == PrincipalType.User && s.SidName == "admin");

            if (adminSid == null)
            {
                throw new InvalidOperationException("Admin SID không tồn tại.");
            }

            objectIdentity = new AclObjectIdentity
            {
                ResourceTypeId = aclClass.Id,
                ResourceId = request.ResourceId,
                OwnerSid = adminSid.Id,
                EntriesInheriting = true
            };

            await _context.AclObjectIdentities.AddAsync(objectIdentity);
            await _context.SaveChangesAsync();
        }

        var subjectPrincipal = (PrincipalType)request.SubjectType;
        string subjectName;

        switch (subjectPrincipal)
        {
            case PrincipalType.User:
                var user = await _context.Users.FindAsync(request.SubjectId);
                if (user == null)
                {
                    throw new KeyNotFoundException("User không tồn tại.");
                }
                subjectName = user.UserName ?? string.Empty;
                break;

            case PrincipalType.Team:
                var team = await _context.Teams.FindAsync(request.SubjectId);
                if (team == null)
                {
                    throw new KeyNotFoundException("Team không tồn tại.");
                }
                subjectName = request.SubjectId.ToString();
                break;

            case PrincipalType.TeamRoleLocation:
            case PrincipalType.Role:
                throw new ArgumentException($"Subject type {subjectPrincipal} chưa được hỗ trợ.");

            default:
                throw new ArgumentException("Subject type không hợp lệ.");
        }

        var sid = await _context.AclSids
            .FirstOrDefaultAsync(s => s.Principal == subjectPrincipal && s.SidName == subjectName);

        if (sid == null)
        {
            sid = new AclSid
            {
                Principal = subjectPrincipal,
                SidName = subjectName
            };
            await _context.AclSids.AddAsync(sid);
            await _context.SaveChangesAsync();
        }

        var permission = await _context.Permissions.FindAsync(request.PermissionId);
        if (permission == null)
        {
            throw new KeyNotFoundException("Permission không tồn tại.");
        }

        var existingEntry = await _context.AclEntries
            .FirstOrDefaultAsync(e => e.ObjectIdentityId == objectIdentity.Id
                && e.SidId == sid.Id
                && e.PermissionId == permission.Id);

        if (existingEntry != null)
        {
            if (existingEntry.Granting)
            {
                throw new InvalidOperationException("Quyền đã được gán.");
            }
            existingEntry.Granting = true;
        }
        else
        {
            var entry = new AclEntry
            {
                ObjectIdentityId = objectIdentity.Id,
                SidId = sid.Id,
                PermissionId = permission.Id,
                Granting = true,
                AuditSuccess = false,
                AuditFailure = false
            };

            await _context.AclEntries.AddAsync(entry);
        }

        await _context.SaveChangesAsync();
    }

    public async Task RevokePermissionAsync(int entryId)
    {
        var entry = await _context.AclEntries.FindAsync(entryId);
        if (entry == null)
        {
            return;
        }

        _context.AclEntries.Remove(entry);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ResourcePermissionDto>> GetResourcePermissionsAsync(string resourceType, int resourceId)
    {
        var aclClass = await _context.AclClasses
            .FirstOrDefaultAsync(c => c.Name == resourceType);

        if (aclClass == null)
        {
            throw new KeyNotFoundException($"Resource type '{resourceType}' không tồn tại.");
        }

        var objectIdentity = await _context.AclObjectIdentities
            .FirstOrDefaultAsync(oi => oi.ResourceTypeId == aclClass.Id && oi.ResourceId == resourceId);

        if (objectIdentity == null)
        {
            return new List<ResourcePermissionDto>();
        }

        var entries = await _context.AclEntries
            .Include(e => e.Sid)
            .Include(e => e.Permission)
            .Where(e => e.ObjectIdentityId == objectIdentity.Id && e.Granting)
            .ToListAsync();

        var result = new List<ResourcePermissionDto>();

        foreach (var entry in entries)
        {
            string subjectName = entry.Sid.SidName;
            int subjectType = (int)entry.Sid.Principal;
            int subjectId = 0;

            switch (entry.Sid.Principal)
            {
                case PrincipalType.User:
                    var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.UserName == entry.Sid.SidName);
                    if (user != null)
                    {
                        subjectName = user.Name;
                        subjectId = user.Id;
                    }
                    break;

                case PrincipalType.Team:
                    if (int.TryParse(entry.Sid.SidName, out var teamId))
                    {
                        var team = await _context.Teams.FindAsync(teamId);
                        if (team != null)
                        {
                            subjectName = team.Name;
                            subjectId = teamId;
                        }
                    }
                    break;

                case PrincipalType.TeamRoleLocation:
                case PrincipalType.Role:
                    break;
            }

            result.Add(new ResourcePermissionDto
            {
                EntryId = entry.Id,
                SubjectType = subjectType,
                SubjectName = subjectName,
                SubjectId = subjectId,
                PermissionName = entry.Permission.Name,
                PermissionId = entry.Permission.Id,
                Granting = entry.Granting
            });
        }

        return result;
    }
}

