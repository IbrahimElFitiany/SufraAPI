using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Sufra.Models.Customers;

namespace Sufra.Models.Restaurants
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }


        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string ReviewText { get; set; }

        [DefaultValue(typeof(DateTime), "CURRENT_DATE")]
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}
