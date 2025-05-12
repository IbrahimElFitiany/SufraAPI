using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sufra_MVC.Models.Orders;
using Sufra_MVC.Models.RestaurantModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sufra_MVC.Models.Orders
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Cart")]
        public int CartId { get; set; }

        [Required]
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        // Navigation properties
        public virtual Cart Cart { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}
