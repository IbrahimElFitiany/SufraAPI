using System.Data;
using System.Security.Claims;
using Sufra.Common.Constants;
using Sufra.Infrastructure.Services;

namespace Sufra.DTOs.SufraEmpDTOs
{
    public class AdminClaimsDTO : IJwtClaimsProvider
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
                new Claim(ClaimTypes.Role, RoleNames.Admin)
            };
        }
    }
}
