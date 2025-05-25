using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Sufra.Models.Customers;


namespace Sufra.Models.Restaurants
{
    public class RestaurantReview
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        [Required, Range(1, 5)]
        public decimal Rating { get; set; }

        [Required]
        public string ReviewText { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.UtcNow; // Use UtcNow

        // Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
