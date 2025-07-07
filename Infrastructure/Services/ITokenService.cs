namespace Sufra.Infrastructure.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IJwtClaimsProvider user);
    }
}