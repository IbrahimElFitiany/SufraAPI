using Sufra.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sufra.DTOs
{
    public class LoginReqDTO
    {
        private string _email;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(254, MinimumLength = 5)]
        public string Email
        {
            get => _email;
            set => _email = value?.Trim().ToLower();
        }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(3, ErrorMessage = "Password must be at least 6 characters")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "UserType is required")]
        public UserType UserType { get; set; }
    }
}