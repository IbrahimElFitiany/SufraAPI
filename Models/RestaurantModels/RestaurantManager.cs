using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sufra_MVC.Models.RestaurantModels
{
    public class RestaurantManager
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Fname { get; set; }

        [Required, StringLength(255)]
        public string Lname { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DefaultValue(typeof(DateTime), "CURRENT_DATE")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Navigation Property (one-to-one)
        public virtual Restaurant Restaurant { get; set; }
    }
}
