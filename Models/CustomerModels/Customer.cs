using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Models.Reservation;
using Models.Orders;

namespace Sufra_MVC.Models.CustomerModels
{
    [Index(nameof(Email), IsUnique = true)]
    public class Customer
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

        [Required, Phone]
        public string Phone { get; set;}
        [DefaultValue(typeof(DateTime), "CURRENT_DATE")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<RestaurantReview> RestaurantReviews { get; set; }

    }
}
