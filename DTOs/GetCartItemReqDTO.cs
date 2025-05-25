namespace Sufra.DTOs
{
    public class GetCartItemReqDTO
    {
        public int CartItemId { get; set; }
        public MenuItemDTO menuItemDTO { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}