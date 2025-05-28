using System.ComponentModel.DataAnnotations;

namespace Sufra.DTOs.CustomerDTOs
{
    public class CustomerLoginReqDTO
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
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters")]
        public string Password { get; set; }
    }
}
