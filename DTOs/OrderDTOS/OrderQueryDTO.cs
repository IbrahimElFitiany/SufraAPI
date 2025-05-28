using Sufra.Models.Orders;
using System.ComponentModel.DataAnnotations;

namespace Sufra.DTOs.OrderDTOS
{
    public class OrderQueryDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1.")]
        public int page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int pageSize { get; set; } = 2;

        public OrderStatus? Status{ get; set; }
    }
}
