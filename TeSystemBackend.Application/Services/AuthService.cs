using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TeSystemBackend.Application.Constants;
using TeSystemBackend.Application.DTOs.Auth;
using TeSystemBackend.Application.Helpers;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtConfiguration _jwtConfig;
    private readonly IIdentityRoleService _identityRoleService;
    private readonly UserManager<AppUser> _userManager;

    public AuthService(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork, 
        JwtConfiguration jwtConfig,
        IIdentityRoleService identityRoleService,
        UserManager<AppUser> userManager)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtConfig = jwtConfig;
        _identityRoleService = identityRoleService;
        _userManager = userManager;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingEmail != null)
        {
            throw new InvalidOperationException(ErrorMessages.EmailAlreadyExists);
        }

        var existingUserName = await _userRepository.GetByUserNameAsync(request.Username);
        if (existingUserName != null)
        {
            throw new InvalidOperationException(ErrorMessages.UsernameAlreadyExists);
        }

        var user = new AppUser
        {
            UserName = request.Username,
            Email = request.Email,
            Name = request.Name
        };

        var created = await _userRepository.CreateAsync(user, request.Password);

        var token = await GenerateJwtTokenAsync(created);
        var refreshToken = await GenerateAndStoreRefreshTokenAsync(created);

        return new AuthResponse
        {
            AccessToken = token,
            UserId = created.Id,
            Email = created.Email ?? string.Empty,
            Name = created.Name,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUserNameAsync(request.Username);
        if (user == null)
        {
            throw new UnauthorizedAccessException(ErrorMessages.InvalidUsername);
        }

        var valid = await _userRepository.CheckPasswordAsync(user, request.Password);
        if (!valid)
        {
            throw new UnauthorizedAccessException(ErrorMessages.InvalidPassword);
        }

        var token = await GenerateJwtTokenAsync(user);
        var refreshToken = await GenerateAndStoreRefreshTokenAsync(user);

        return new AuthResponse
        {
            AccessToken = token,
            UserId = user.Id,
            Email = user.Email ?? string.Empty,
            Name = user.Name,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var existingToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
        if (existingToken == null || !existingToken.IsActive)
        {
            throw new UnauthorizedAccessException(ErrorMessages.InvalidOrExpiredRefreshToken);
        }

        var user = existingToken.User;

        existingToken.RevokedAt = DateTime.UtcNow;
        var newRefreshToken = await GenerateAndStoreRefreshTokenAsync(user);
        existingToken.ReplacedByToken = newRefreshToken.Token;
        await _unitOfWork.RefreshTokens.UpdateAsync(existingToken);
        await _unitOfWork.SaveChangesAsync();

        var newAccessToken = await GenerateJwtTokenAsync(user);

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            UserId = user.Id,
            Email = user.Email ?? string.Empty,
            Name = user.Name,
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpiresAt = newRefreshToken.ExpiresAt
        };
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var existingToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
        if (existingToken == null || !existingToken.IsActive)
        {
            return;
        }

        existingToken.RevokedAt = DateTime.UtcNow;
        await _unitOfWork.RefreshTokens.UpdateAsync(existingToken);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<string> GenerateJwtTokenAsync(AppUser user)
    {
        _jwtConfig.ValidateKey();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty)
        };

        var roles = await _identityRoleService.GetUserRolesAsync(user.Id);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            _jwtConfig.Issuer,
            _jwtConfig.Audience,
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<RefreshToken> GenerateAndStoreRefreshTokenAsync(AppUser user)
    {
        var randomNumber = new byte[64];
        RandomNumberGenerator.Fill(randomNumber);
        var token = Convert.ToBase64String(randomNumber);

        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryDays)
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        return refreshToken;
    }
}



