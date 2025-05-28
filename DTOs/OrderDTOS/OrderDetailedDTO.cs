using Sufra.Models.Orders;

namespace Sufra.DTOs.OrderDTOS
{
    public class OrderDetailedDTO
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}