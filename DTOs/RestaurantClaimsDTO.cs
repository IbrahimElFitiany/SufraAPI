using System.Security.Claims;
using Sufra.Infrastructure.Services;

namespace Sufra.DTOs
{
    public class RestaurantClaimsDTO : IJwtClaimsProvider
    {
        public int ManagerID { get; set; }
        public string ManagerName { get; set; }
        public string Email { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public bool IsApproved { get; set; }
        public string Role { get; set; }

        public IEnumerable<Claim> GetClaims()
        {
            return new[]
            {
                new Claim("managerId", ManagerID.ToString()),
                new Claim("managerName", ManagerName),
                new Claim("Email", Email),
                new Claim(ClaimTypes.Role, Role),
                new Claim("RestaurantId", RestaurantId.ToString()),
                new Claim("RestaurantName", RestaurantName),
                new Claim("Status", IsApproved.ToString())
            };
        }
    }
}
