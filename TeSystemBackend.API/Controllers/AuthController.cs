using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TeSystemBackend.Application.DTOs;
using TeSystemBackend.Application.DTOs.Auth;
using TeSystemBackend.Application.Services;

namespace TeSystemBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        var validationResult = await _registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var result = await _authService.RegisterAsync(request);
        return ApiResponse<AuthResponse>.Success(result);
    }

    [HttpPost("login")]
    public async Task<ApiResponse<AuthResponse>> Login(LoginRequest request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var result = await _authService.LoginAsync(request);
        return ApiResponse<AuthResponse>.Success(result);
    }

    [HttpPost("refresh-token")]
    public async Task<ApiResponse<AuthResponse>> RefreshToken(RefreshTokenRequest request)
    {
        var validationResult = await _refreshTokenValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var result = await _authService.RefreshTokenAsync(request.RefreshToken);
        return ApiResponse<AuthResponse>.Success(result);
    }

    [HttpPost("logout")]
    public async Task<ApiResponse<object>> Logout(RefreshTokenRequest request)
    {
        var validationResult = await _refreshTokenValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
        return ApiResponse<object>.Success(null!, "Logged out");
    }
}
