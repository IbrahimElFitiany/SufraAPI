using System.Data;
using System.Security.Claims;
using Sufra.Common.Enums;
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
                new Claim("UserID", UserId.ToString()),
                new Claim("Name", Name),
                new Claim("Email", Email),
                new Claim(ClaimTypes.Role, Role)
            };
        }
    }
}
