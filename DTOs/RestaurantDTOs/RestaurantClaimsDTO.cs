using System.Security.Claims;
using Sufra.Common.Enums;
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
        public UserType Role { get; set; }

        public IEnumerable<Claim> GetClaims()
        {
            return new[]
            {
                new Claim("managerId", UserId.ToString()),
                new Claim("managerName", Name),
                new Claim("Email", Email),
                new Claim(ClaimTypes.Role, Role.ToString()),
                new Claim("RestaurantId", RestaurantId.ToString()),
                new Claim("RestaurantName", RestaurantName),
                new Claim("IsApproved", IsApproved.ToString())
            };
        }
    }
}
