namespace Sufra_MVC.DTOs.CartDTOs
{
    public class CartItemDTO
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}
