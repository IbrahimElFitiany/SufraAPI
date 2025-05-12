using System.Data;
using System.Security.Claims;
using Sufra_MVC.Infrastructure.Services;

namespace Sufra_MVC.DTOs
{
    public class AdminClaimsDTO : IJwtClaimsProvider
    {
        public int? userID { get; set; }   
        public string Name { get; set; }  
        public string Email { get; set; } 
        public string Role { get; set; } 

        public IEnumerable<Claim> GetClaims()
        {
            var claims = new List<Claim>();

            if (userID.HasValue)
                claims.Add(new Claim("UserID", userID.Value.ToString()));

            if (!string.IsNullOrEmpty(Name))
                claims.Add(new Claim("Name", Name));

            if (!string.IsNullOrEmpty(Email))
                claims.Add(new Claim("Email", Email));

            if (!string.IsNullOrEmpty(Role))
                claims.Add(new Claim(ClaimTypes.Role, Role));

            return claims;
        }
    }
}
