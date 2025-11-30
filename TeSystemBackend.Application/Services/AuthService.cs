using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TeSystemBackend.Application.DTOs.Auth;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Domain.Entities;

namespace TeSystemBackend.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingEmail != null)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var existingUserName = await _userRepository.GetByUserNameAsync(request.Username);
        if (existingUserName != null)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var user = new AppUser
        {
            UserName = request.Username,
            Email = request.Email,
            Name = request.Name
        };

        var created = await _userRepository.CreateAsync(user, request.Password);

        var token = GenerateJwtToken(created);
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
            throw new UnauthorizedAccessException("Sai username");
        }

        var valid = await _userRepository.CheckPasswordAsync(user, request.Password);
        if (!valid)
        {
            throw new UnauthorizedAccessException("Sai password");
        }

        var token = GenerateJwtToken(user);
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
        var existingToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (existingToken == null || !existingToken.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        var user = existingToken.User;

        existingToken.RevokedAt = DateTime.UtcNow;
        var newRefreshToken = await GenerateAndStoreRefreshTokenAsync(user);
        existingToken.ReplacedByToken = newRefreshToken.Token;
        await _refreshTokenRepository.UpdateAsync(existingToken);

        var newAccessToken = GenerateJwtToken(user);

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
        var existingToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (existingToken == null || !existingToken.IsActive)
        {
            return;
        }

        existingToken.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(existingToken);
    }

    private string GenerateJwtToken(AppUser user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var key = Environment.GetEnvironmentVariable("JWT_KEY")
                  ?? jwtSection["Key"];
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException("JWT key is not configured.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty)
        };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<RefreshToken> GenerateAndStoreRefreshTokenAsync(AppUser user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var refreshTokenDaysConfig = jwtSection["RefreshTokenExpiryDays"];

        var refreshTokenDays = 7;
        if (int.TryParse(refreshTokenDaysConfig, out var configDays) && configDays > 0)
        {
            refreshTokenDays = configDays;
        }

        var randomNumber = new byte[64];
        RandomNumberGenerator.Fill(randomNumber);
        var token = Convert.ToBase64String(randomNumber);

        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays)
        };

        await _refreshTokenRepository.AddAsync(refreshToken);

        return refreshToken;
    }
}



