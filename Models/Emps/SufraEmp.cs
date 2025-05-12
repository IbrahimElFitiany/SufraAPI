using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sufra_MVC.Models.Emps
{
    public class SufraEmp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Fname { get; set; }

        [Required]
        public string Lname { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        [DefaultValue(typeof(DateTime), "CURRENT_DATE")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
