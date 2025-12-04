using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeSystemBackend.API.Extensions;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Auth;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<RefreshTokenRequest> _refreshTokenValidator;

    public AuthController(
        IAuthService authService,
        IValidator<RegisterRequest> registerValidator,
        IValidator<LoginRequest> loginValidator,
        IValidator<RefreshTokenRequest> refreshTokenValidator)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _refreshTokenValidator = refreshTokenValidator;
    }

    [HttpPost("register")]
    public async Task<ApiResponse<AuthResponse>> Register(RegisterRequest request)
    {
        await _registerValidator.ValidateAndThrowAsync(request);

        var result = await _authService.RegisterAsync(request);
        return ApiResponse<AuthResponse>.Success(result);
    }

    [HttpPost("login")]
    public async Task<ApiResponse<AuthResponse>> Login(LoginRequest request)
    {
        await _loginValidator.ValidateAndThrowAsync(request);

        var result = await _authService.LoginAsync(request);
        return ApiResponse<AuthResponse>.Success(result);
    }

    [HttpPost("refresh-token")]
    public async Task<ApiResponse<AuthResponse>> RefreshToken(RefreshTokenRequest request)
    {
        await _refreshTokenValidator.ValidateAndThrowAsync(request);

        var result = await _authService.RefreshTokenAsync(request.RefreshToken);
        return ApiResponse<AuthResponse>.Success(result);
    }

    [HttpPost("logout")]
    public async Task<ApiResponse<object>> Logout(RefreshTokenRequest request)
    {
        await _refreshTokenValidator.ValidateAndThrowAsync(request);

        await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
        return ApiResponse<object>.Success(null!, ErrorMessages.LoggedOut);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<ApiResponse<object>> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        [FromServices] IValidator<ChangePasswordRequest> validator)
    {
        await validator.ValidateAndThrowAsync(request);

        var userIdClaim = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)
                         ?? HttpContext.User?.FindFirst("sub");
        
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedAccess);
        }

        await _authService.ChangePasswordAsync(userId, request);
        return ApiResponse<object>.Success(null!, "Password changed successfully");
    }
}
