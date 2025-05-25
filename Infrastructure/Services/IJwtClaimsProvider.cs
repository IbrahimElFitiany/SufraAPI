using System.Security.Claims;

namespace SufraMVC.Infrastructure.Services
{
    public interface IJwtClaimsProvider
    {
        IEnumerable<Claim> GetClaims();

    }
}