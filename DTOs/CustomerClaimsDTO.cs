using System.Security.Claims;
using Microsoft.Extensions.Options;
using SufraMVC.Infrastructure.Services;

namespace SufraMVC.DTOs
{
    public class CustomerClaimsDTO : IJwtClaimsProvider
    {
        public int userID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public IEnumerable<Claim> GetClaims()
        {
            return new[]
            {
                new Claim("UserID", userID.ToString()),
                new Claim("Name", Name),
                new Claim("Email", Email),
                new Claim(ClaimTypes.Role, Role)
            };
        }
    }
}
