using Sufra.Models.Orders;

namespace Sufra.DTOs.OrderDTOS
{
    public class OrderQueryDTO
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 2;
        public OrderStatus? Status{ get; set; }
    }
}
