using Sufra.Models.Restaurants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sufra.Models.Orders
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        // Navigation Properties
        public virtual Order Order { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}
