using System.Security.Claims;

namespace Sufra.Infrastructure.Services
{
    public interface IJwtClaimsProvider
    {
        IEnumerable<Claim> GetClaims();

    }
}