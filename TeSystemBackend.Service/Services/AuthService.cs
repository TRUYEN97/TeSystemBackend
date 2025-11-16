using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeSystemBackend.Core.Entities;
using TeSystemBackend.Data;
using TeSystemBackend.Data.Entities;

namespace TeSystemBackend.Service
{
    public class AuthService
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;

        public AuthService(
            UserManager<AppUserEntity> userManager,
            AppDbContext dbContext,
            IConfiguration config)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _config = config;
        }

        public async Task<(string accessToken, string refreshToken)> LoginAsync(
            string username,
            string password,
            string ipAddress)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                throw new Exception("User not found");

            var validPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!validPassword)
                throw new Exception("Invalid password");

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
            var jwtSettings = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("Key")!)
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

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetValue<string>("Issuer"),
                audience: jwtSettings.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<double>("AccessTokenExpirationMinutes")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GenerateRefreshToken(long userId, string ipAddress)
        {
            var refreshToken = new RefreshTokenEntity
            {
                UserId = userId,
                Token = Guid.NewGuid().ToString("N"),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress,
            };

            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<AppUserEntity?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var token = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken && !x.IsRevoked);

            if (token == null || token.ExpiresAt < DateTime.UtcNow)
                return null;

            var user = await _userManager.FindByIdAsync(token.UserId.ToString());
            return user;
        }

        public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string oldRefreshToken, string ipAddress)
        {
            var user = await ValidateRefreshTokenAsync(oldRefreshToken);
            if (user == null)
                throw new Exception("Invalid refresh token");

            var newAccessToken = await GenerateJwtTokenAsync(user);
            var newRefreshToken = await GenerateRefreshToken(user.Id, ipAddress);

            return (newAccessToken, newRefreshToken);
        }
    }
}
