using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.Repositories;

namespace TeSystemBackend.Application.Services;

public class AppAuthorizationService : IAppAuthorizationService
{
    private readonly IIdentityRoleService _identityRoleService;
    private readonly IUserTeamService _userTeamService;
    private readonly IUnitOfWork _unitOfWork;

    public AppAuthorizationService(
        IIdentityRoleService identityRoleService,
        IUserTeamService userTeamService,
        IUnitOfWork unitOfWork)
    {
        _identityRoleService = identityRoleService;
        _userTeamService = userTeamService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> IsAdminAsync(int userId)
    {
        return await _identityRoleService.UserHasRoleAsync(userId, Roles.Admin);
    }

    public async Task<bool> HasPermissionAsync(int userId, string permission, int? locationId = null)
    {
        if (await IsAdminAsync(userId))
            return true;

        var userTeamIds = await _userTeamService.GetUserTeamIdsAsync(userId);
        if (!userTeamIds.Any())
            return false;

        return await _unitOfWork.TeamRoleLocations.TeamsHavePermissionAsync(
            userTeamIds, permission, locationId);
    }

    public async Task<bool> HasPermissionOnResourceAsync(
        int userId, 
        string permission, 
        int resourceId, 
        string resourceType)
    {
        if (await IsAdminAsync(userId))
            return true;

        int? locationId = resourceType switch
        {
            "Computer" => (await _unitOfWork.Computers.GetByIdAsync(resourceId))?.LocationId,
            _ => null
        };

        if (!locationId.HasValue)
            return false;

        return await HasPermissionAsync(userId, permission, locationId.Value);
    }

    public async Task<bool> CanViewComputerAsync(int userId, int computerId)
    {
        return await HasPermissionOnResourceAsync(
            userId, 
            Permissions.ComputerView, 
            computerId, 
            "Computer");
    }

    public async Task<bool> CanCreateComputerAsync(int userId, int locationId)
    {
        return await HasPermissionAsync(userId, Permissions.ComputerCreate, locationId);
    }

    public async Task<bool> CanUpdateComputerAsync(int userId, int computerId)
    {
        return await HasPermissionOnResourceAsync(
            userId, 
            Permissions.ComputerUpdate, 
            computerId, 
            "Computer");
    }

    public async Task<bool> CanDeleteComputerAsync(int userId, int computerId)
    {
        return await HasPermissionOnResourceAsync(
            userId, 
            Permissions.ComputerDelete, 
            computerId, 
            "Computer");
    }

    public async Task<bool> CanManageTeamsAsync(int userId)
    {
        return await IsAdminAsync(userId);
    }

    public async Task<bool> CanManageDepartmentsAsync(int userId)
    {
        return await IsAdminAsync(userId);
    }
}
