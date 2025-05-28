using Sufra.Models.Orders;

namespace Sufra.DTOs.OrderDTOS
{
    public class OrderItemListDTO
    {
        public int OrderId { get; set; }                          
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; } 
        public decimal TotalPrice { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantImageUrl { get; set; }
    }
}