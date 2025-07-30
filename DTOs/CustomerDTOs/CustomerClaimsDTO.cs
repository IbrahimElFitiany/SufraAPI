using System.Security.Claims;
using Sufra.Common.Constants;
using Sufra.Infrastructure.Services;

namespace Sufra.DTOs.CustomerDTOs
{
    public class CustomerClaimsDTO : IJwtClaimsProvider
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public IEnumerable<Claim> GetClaims()
        {
            return new[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.Role, RoleNames.Customer)
            };
        }
    }
}
