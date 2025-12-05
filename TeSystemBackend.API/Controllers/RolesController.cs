using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Roles;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    private readonly IIdentityRoleService _identityRoleService;

    public RolesController(
        IUnitOfWork unitOfWork, 
        IPermissionService permissionService,
        IIdentityRoleService identityRoleService)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
        _identityRoleService = identityRoleService;
    }

    [HttpGet]
    public async Task<ApiResponse<List<RoleDto>>> GetAll()
    {
        var roles = await _unitOfWork.Roles.GetAllAsync();

        var roleDtos = new List<RoleDto>();
        foreach (var role in roles)
        {
            var permissions = await _permissionService.GetRolePermissionsAsync(role.Id);
            var userCount = await _identityRoleService.GetUserCountByRoleAsync(role.Name);
            roleDtos.Add(new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = permissions,
                UserCount = userCount
            });
        }

        return ApiResponse<List<RoleDto>>.Success(roleDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResponse<RoleDto>> GetById(int id)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(id);

        if (role == null)
        {
            throw new KeyNotFoundException("Role not found");
        }

        var permissions = await _permissionService.GetRolePermissionsAsync(role.Id);
        var userCount = await _identityRoleService.GetUserCountByRoleAsync(role.Name);
        var roleDto = new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Permissions = permissions,
            UserCount = userCount
        };

        return ApiResponse<RoleDto>.Success(roleDto);
    }
}
