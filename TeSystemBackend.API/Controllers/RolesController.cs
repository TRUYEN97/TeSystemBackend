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

    public RolesController(IUnitOfWork unitOfWork, IPermissionService permissionService)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
    }

    [HttpGet]
    public async Task<ApiResponse<List<RoleDto>>> GetAll()
    {
        var roles = await _unitOfWork.Roles.GetAllAsync();

        var roleDtos = new List<RoleDto>();
        foreach (var role in roles)
        {
            var permissions = await _permissionService.GetRolePermissionsAsync(role.Id);
            roleDtos.Add(new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = permissions
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
        var roleDto = new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Permissions = permissions
        };

        return ApiResponse<RoleDto>.Success(roleDto);
    }
}
