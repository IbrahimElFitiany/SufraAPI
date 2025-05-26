using NuGet.Packaging.Signing;
using Sufra.Models.Orders;

namespace Sufra.DTOs.OrderDTOS
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}