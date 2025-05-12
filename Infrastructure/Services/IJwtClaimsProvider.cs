using System.Security.Claims;

namespace Sufra_MVC.Infrastructure.Services
{
    public interface IJwtClaimsProvider
    {
        IEnumerable<Claim> GetClaims();

    }
}