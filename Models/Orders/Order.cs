using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Sufra_MVC.Models.CustomerModels;

namespace Models.Orders
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [DefaultValue(OrderStatus.Pending)]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required, Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        [DefaultValue(typeof(DateTime), "CURRENT_TIMESTAMP")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}
