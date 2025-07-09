using Sufra.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sufra.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Token { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public int UserId { get; set; }
        [Required]
        public UserType UserType { get; set; }
        [Required]
        public DateTime ExpiresAt { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UserAgent { get; set; }

        public string? IpAddress { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTime? RevokedAt { get; set; }

        public string? ReplacedByToken { get; set; }

    }
}
