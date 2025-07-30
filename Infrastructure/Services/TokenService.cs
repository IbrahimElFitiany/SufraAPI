using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sufra.Configuration;
using Sufra.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sufra.Infrastructure.Services
{
    public class TokenService:ITokenService
    {
        private readonly TokenSettings _tokenSettings;

        public TokenService(IOptions<TokenSettings> tokenSettings)
        {
            _tokenSettings = tokenSettings.Value;
        }

        public string GenerateAccessToken (IJwtClaimsProvider user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = user.GetClaims().ToList();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var token = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_tokenSettings.AccessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public RefreshToken GenerateRefreshToken(int userId, string userType, string? ipAddress = null, string? userAgent = null)
        {
            return new RefreshToken
            {
                UserId = userId,
                UserType = userType,
                ExpiresAt = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                IsRevoked = false
            };
        }
    }
}
