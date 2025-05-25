namespace Sufra.DTOs.CartDTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
        public decimal Total { get; set; }
    }

}
