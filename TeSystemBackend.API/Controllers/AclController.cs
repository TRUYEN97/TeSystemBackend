using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.API.Helpers;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Acl;
using TeSystemBackend.Application.Services;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/acl")]
[Authorize]
public class AclController : ControllerBase
{
    private readonly IAclService _aclService;
    private readonly IValidator<AssignPermissionRequest> _assignValidator;
    private readonly UserManager<AppUser> _userManager;

    public AclController(
        IAclService aclService,
        IValidator<AssignPermissionRequest> assignValidator,
        UserManager<AppUser> userManager)
    {
        _aclService = aclService;
        _assignValidator = assignValidator;
        _userManager = userManager;
    }

    [HttpPost("assign")]
    public async Task<ApiResponse<object>> AssignPermission(AssignPermissionRequest request)
    {
        await ControllerHelper.EnsureAdminAsync(User, _userManager);

        var validationResult = await _assignValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        await _aclService.AssignPermissionAsync(request);
        return ApiResponse<object>.Success(null!, "Đã gán quyền thành công");
    }

    [HttpDelete("revoke/{entryId:int}")]
    public async Task<ApiResponse<object>> RevokePermission(int entryId)
    {
        await ControllerHelper.EnsureAdminAsync(User, _userManager);

        await _aclService.RevokePermissionAsync(entryId);
        return ApiResponse<object>.Success(null!, "Đã thu hồi quyền");
    }

    [HttpGet("resources/{resourceType}/{resourceId:int}")]
    public async Task<ApiResponse<List<ResourcePermissionDto>>> GetResourcePermissions(string resourceType, int resourceId)
    {
        await ControllerHelper.EnsureAdminAsync(User, _userManager);

        var permissions = await _aclService.GetResourcePermissionsAsync(resourceType, resourceId);
        return ApiResponse<List<ResourcePermissionDto>>.Success(permissions);
    }
}

