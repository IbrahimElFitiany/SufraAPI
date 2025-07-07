using Sufra.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sufra.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; } = Guid.NewGuid().ToString();
        public int UserId { get; set; }
        public UserType UserType { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
