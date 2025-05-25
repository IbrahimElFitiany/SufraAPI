using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Sufra.Models.Restaurants;
using Sufra.Models.Customers;

namespace Sufra.Models.Orders
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        [DefaultValue(OrderStatus.Pending)]
        [JsonConverter(typeof(JsonStringEnumConverter))] // Serialize enum as string
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required, Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        [DefaultValue(typeof(DateTime), "CURRENT_TIMESTAMP")]
        public DateTime OrderDate { get; set; } = DateTime.Now;



        // Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual Restaurant Restaurant { get; set; }



        // Private collection for OrderItems
        private ICollection<OrderItem> _orderItems = new List<OrderItem>();
        public virtual IEnumerable<OrderItem> OrderItems => _orderItems;


        public void AddOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));
            _orderItems.Add(orderItem);
        }




    }
}
