using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeSystemBackend.Domain.Entities;
using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAdminAsync(
        UserManager<AppUser> userManager,
        ApplicationDbContext context)
    {
        const string adminUsername = "admin";
        const string adminEmail = "admin@system.com";
        const string adminPassword = "Admin1";

        var existingAdmin = await userManager.FindByNameAsync(adminUsername);
        if (existingAdmin != null)
        {
            return;
        }

        var admin = new AppUser
        {
            UserName = adminUsername,
            Email = adminEmail,
            Name = "System Administrator"
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        var adminSid = new AclSid
        {
            Principal = PrincipalType.User,
            SidName = adminUsername
        };

        await context.AclSids.AddAsync(adminSid);
        await context.SaveChangesAsync();

        var adminPermission = await context.Permissions
            .FirstOrDefaultAsync(p => p.Name == "ADMIN");

        if (adminPermission == null)
        {
            adminPermission = new Permission
            {
                Name = "ADMIN"
            };
            await context.Permissions.AddAsync(adminPermission);
            await context.SaveChangesAsync();
        }

        var computerClass = await context.AclClasses
            .FirstOrDefaultAsync(c => c.Name == "Computer");

        if (computerClass == null)
        {
            computerClass = new AclClass
            {
                Name = "Computer"
            };
            await context.AclClasses.AddAsync(computerClass);
            await context.SaveChangesAsync();
        }

        var locationClass = await context.AclClasses
            .FirstOrDefaultAsync(c => c.Name == "Location");

        if (locationClass == null)
        {
            locationClass = new AclClass
            {
                Name = "Location"
            };
            await context.AclClasses.AddAsync(locationClass);
            await context.SaveChangesAsync();
        }

        var teamClass = await context.AclClasses
            .FirstOrDefaultAsync(c => c.Name == "Team");

        if (teamClass == null)
        {
            teamClass = new AclClass
            {
                Name = "Team"
            };
            await context.AclClasses.AddAsync(teamClass);
            await context.SaveChangesAsync();
        }
    }
}

