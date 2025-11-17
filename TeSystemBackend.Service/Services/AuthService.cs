using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TeSystemBackend.Core.Entities;
using TeSystemBackend.Data.Abstractions;
using TeSystemBackend.Data.Entities;
using TeSystemBackend.Service.Exceptions;
using TeSystemBackend.Service.Interfaces;

namespace TeSystemBackend.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly IAppDbContext _dbContext;
        private readonly IConfigurationSection _jwtSettings;

        public AuthService(
            UserManager<AppUserEntity> userManager,
            IAppDbContext dbContext,
            IConfiguration config)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _jwtSettings = config.GetSection("Jwt");
        }

        public async Task<(string accessToken, string refreshToken)> LoginAsync(
            string username,
            string password,
            string ipAddress)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                throw new NotFoundException("Người dùng không tồn tại");

            var validPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!validPassword)
                throw new UnauthorizedException("Sai mật khẩu");

            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = await GenerateRefreshToken(user.Id, ipAddress);

            return (accessToken, refreshToken);
        }

        private async Task<List<string>> GetUserRolesAsync(long userId)
        {
            var userRoles = await _dbContext.UserModelRoles
                .Where(umr => umr.UserId == userId)
                .Include(umr => umr.Role)
                .Where(umr => !umr.IsTemporary || (umr.ExpireAt.HasValue && umr.ExpireAt.Value > DateTime.UtcNow))
                .Select(umr => umr.Role.Name)
                .Distinct()
                .ToListAsync();

            return userRoles;
        }

        private async Task<List<string>> GetUserPermissionsAsync(long userId)
        {
            var permissions = new HashSet<string>();

            var rolePermissions = await _dbContext.UserModelRoles
                .Where(umr => umr.UserId == userId)
                .Where(umr => !umr.IsTemporary || (umr.ExpireAt.HasValue && umr.ExpireAt.Value > DateTime.UtcNow))
                .Include(umr => umr.Role)
                    .ThenInclude(r => r.RoleMixPermissions)
                        .ThenInclude(rmp => rmp.Permission)
                .SelectMany(umr => umr.Role.RoleMixPermissions.Select(rmp => rmp.Permission.Name))
                .Distinct()
                .ToListAsync();

            foreach (var perm in rolePermissions)
            {
                permissions.Add(perm);
            }

            var aclPermissions = await _dbContext.AclEntries
                .Where(a => a.UserId == userId && a.IsAllowed)
                .Include(a => a.Permission)
                .Select(a => a.Permission.Name)
                .Distinct()
                .ToListAsync();

            foreach (var perm in aclPermissions)
            {
                permissions.Add(perm);
            }

            return permissions.ToList();
        }

        private async Task<string> GenerateJwtTokenAsync(AppUserEntity user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.GetValue<string>("Key")!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            var roles = await GetUserRolesAsync(user.Id);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var permissions = await GetUserPermissionsAsync(user.Id);
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission));
            }

            var accessTokenMinutes = _jwtSettings.GetValue<double?>("AccessTokenExpirationMinutes") ?? 60;

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.GetValue<string>("Issuer"),
                audience: _jwtSettings.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GenerateRefreshToken(long userId, string ipAddress)
        {
            var tokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var hashedToken = HashToken(tokenValue);
            var refreshDays = _jwtSettings.GetValue<int?>("RefreshTokenExpirationDays") ?? 7;
            var expiresAt = DateTime.UtcNow.AddDays(refreshDays);

            var refreshToken = new RefreshTokenEntity
            {
                UserId = userId,
                Token = hashedToken,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress,
            };

            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();

            return tokenValue;
        }

        private static string HashToken(string token)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }

        private async Task<RefreshTokenEntity?> GetValidRefreshTokenAsync(string refreshToken)
        {
            var hashed = HashToken(refreshToken);

            return await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x =>
                    x.Token == hashed &&
                    !x.IsRevoked &&
                    x.ExpiresAt > DateTime.UtcNow);
        }

        public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string oldRefreshToken, string ipAddress)
        {
            var existingToken = await GetValidRefreshTokenAsync(oldRefreshToken);
            if (existingToken == null)
                throw new UnauthorizedException("Refresh token không hợp lệ");

            existingToken.IsRevoked = true;
            existingToken.RevokedAt = DateTime.UtcNow;
            existingToken.RevokedByIp = ipAddress;

            await _dbContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(existingToken.UserId.ToString());
            if (user == null)
                throw new NotFoundException("Người dùng không tồn tại");

            var newAccessToken = await GenerateJwtTokenAsync(user);
            var newRefreshToken = await GenerateRefreshToken(user.Id, ipAddress);

            return (newAccessToken, newRefreshToken);
        }
    }
}
