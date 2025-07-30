using Sufra.Models;

namespace Sufra.Infrastructure.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IJwtClaimsProvider user);
        RefreshToken GenerateRefreshToken(int userId, string userType, string? ipAddress = null, string? userAgent = null);
    }
}