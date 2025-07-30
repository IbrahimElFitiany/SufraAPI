using System.Security.Claims;
using Sufra.Common.Constants;
using Sufra.Infrastructure.Services;

namespace Sufra.DTOs.RestaurantDTOs
{
    public class RestaurantClaimsDTO : IJwtClaimsProvider
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public bool IsApproved { get; set; }
        public string Role { get; set; }

        public IEnumerable<Claim> GetClaims()
        {
            return new[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.Role, RoleNames.RestaurantManager),
                new Claim("RestaurantId", RestaurantId.ToString()),
                new Claim("RestaurantName", RestaurantName),
                new Claim("IsApproved", IsApproved.ToString())
            };
        }
    }
}
