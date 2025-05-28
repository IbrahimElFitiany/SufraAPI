using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sufra.DTOs.CustomerDTOs
{
    public class CustomerRegisterDTO
    {
        private string _fname;
        private string _lname;
        private string _email;
        private string _phone;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2-50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters")]
        public string Fname
        {
            get => _fname;
            set => _fname = value?.Trim().Pascalize();
        }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2-50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters")]
        public string Lname
        {
            get => _lname;
            set => _lname = value?.Trim().Pascalize();
        }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(254 , MinimumLength = 5)]
        public string Email
        {
            get => _email;
            set => _email = value?.Trim().ToLower();
        }

        [Required(ErrorMessage = "Password is required")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must contain uppercase, lowercase, number, and special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Invalid phone number format")]
        public string Phone
        {
            get => _phone;
            set => _phone = Regex.Replace(value?.Trim() ?? "", @"[^\d]", "");
        }
    }
}
