using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Users;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserRequest> _createValidator;
    private readonly IValidator<UpdateUserRequest> _updateValidator;

    public UsersController(
        IUserService userService,
        IValidator<CreateUserRequest> createValidator,
        IValidator<UpdateUserRequest> updateValidator)
    {
        _userService = userService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
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
    public async Task<ApiResponse<UserDto>> Create(CreateUserRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var user = await _userService.CreateAsync(request);
        return ApiResponse<UserDto>.Success(user);
    }

    [HttpPut("{id:int}")]
    public async Task<ApiResponse<UserDto>> Update(int id, UpdateUserRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var user = await _userService.UpdateAsync(id, request);
        return ApiResponse<UserDto>.Success(user);
    }

    [HttpDelete("{id:int}")]
    public async Task<ApiResponse<object>> Delete(int id)
    {
        await _userService.DeleteAsync(id);
        return ApiResponse<object>.Success(null!);
    }
}



