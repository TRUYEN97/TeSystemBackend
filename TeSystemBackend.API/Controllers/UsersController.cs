using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.API.Extensions;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Users;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IIdentityRoleService _identityRoleService;
    private readonly IValidator<CreateUserRequest> _createValidator;
    private readonly IValidator<UpdateUserRequest> _updateValidator;
    private readonly IValidator<AssignRoleRequest> _assignRoleValidator;

    public UsersController(
        IUserService userService,
        IIdentityRoleService identityRoleService,
        IValidator<CreateUserRequest> createValidator,
        IValidator<UpdateUserRequest> updateValidator,
        IValidator<AssignRoleRequest> assignRoleValidator)
    {
        _userService = userService;
        _identityRoleService = identityRoleService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _assignRoleValidator = assignRoleValidator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<UserDto>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return ApiResponse<List<UserDto>>.Success(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResponse<UserDto>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        return ApiResponse<UserDto>.Success(user);
    }

    [HttpPost]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<ApiResponse<UserDto>> Create(CreateUserRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var user = await _userService.CreateAsync(request);
        return ApiResponse<UserDto>.Success(user);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<ApiResponse<UserDto>> Update(int id, UpdateUserRequest request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var user = await _userService.UpdateAsync(id, request);
        return ApiResponse<UserDto>.Success(user);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<ApiResponse<object>> Delete(int id)
    {
        await _userService.DeleteAsync(id);
        return ApiResponse<object>.Success(null!);
    }

    [HttpPost("assign-role")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<ApiResponse<object>> AssignRole(AssignRoleRequest request)
    {
        await _assignRoleValidator.ValidateAndThrowAsync(request);

        await _identityRoleService.AssignRoleToUserAsync(request.UserId, request.RoleName);
        return ApiResponse<object>.Success(null!, ErrorMessages.Success);
    }
}



