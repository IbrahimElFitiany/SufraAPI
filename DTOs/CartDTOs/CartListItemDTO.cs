using Sufra.DTOs.MenuDTOs;

namespace Sufra.DTOs.CartDTOs
{
    public class CartListItemDTO
    {
        public int CartItemId { get; set; }
        public string Name { get; set; }
        public string MenuItemImg { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal PriceTotal => Price * Quantity;
    }
}