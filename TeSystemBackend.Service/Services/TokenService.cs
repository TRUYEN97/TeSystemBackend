//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using TeSystemBackend.Data;
//using TeSystemBackend.Data.Entities;

//namespace TeSystemBackend.Service.Services
//{
//    public class TokenService
//    {
//        private readonly IConfiguration _config;
//        private readonly AppDbContext _db;

//        public TokenService(IConfiguration config, AppDbContext db)
//        {
//            _config = config;
//            _db = db;
//        }

//        public string CreateAccessToken(AppUserEntity user)
//        {
//            var secretKey = _config["Jwt:SecretKey"];
//            if (string.IsNullOrEmpty(secretKey))
//                throw new Exception("JWT SecretKey not configured!");

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var claims = new List<Claim>()
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
//                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
//                new Claim("username", user.UserName ?? "")
//            };

//            var expires = DateTime.UtcNow.AddMinutes(
//                Convert.ToDouble(_config["Jwt:AccessTokenExpirationMinutes"])
//            );

//            var token = new JwtSecurityToken(
//                issuer: _config["Jwt:Issuer"],
//                audience: _config["Jwt:Audience"],
//                claims: claims,
//                expires: expires,
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//        public async Task<RefreshTokenEntity> CreateRefreshToken(long userId, string ipAddress)
//        {
//            var refreshToken = new RefreshTokenEntity
//            {
//                UserId = userId,
//                Token = Guid.NewGuid().ToString("N"),
//                ExpiresAt = DateTime.UtcNow.AddDays(7),
//                CreatedByIp = ipAddress
//            };

//            _db.RefreshTokens.Add(refreshToken);
//            await _db.SaveChangesAsync();

//            return refreshToken;
//        }

//        public async Task<RefreshTokenEntity?> GetValidRefreshToken(string token)
//        {
//            return await Task.FromResult(
//                _db.RefreshTokens.FirstOrDefault(x =>
//                    x.Token == token &&
//                    !x.IsRevoked &&
//                    x.ExpiresAt > DateTime.UtcNow
//                )
//            );
//        }

//        public async Task RevokeRefreshToken(RefreshTokenEntity token, string ipAddress)
//        {
//            token.IsRevoked = true;
//            token.RevokedAt = DateTime.UtcNow;
//            token.RevokedByIp = ipAddress;

//            _db.RefreshTokens.Update(token);
//            await _db.SaveChangesAsync();
//        }
//    }
//}
