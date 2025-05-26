using Sufra.DTOs.MenuDTOs;

namespace Sufra.DTOs.CartDTOs
{
    public class CartItemResponseDTO
    {
        public int CartItemId { get; set; }
        public MenuItemDTO menuItemDTO { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}