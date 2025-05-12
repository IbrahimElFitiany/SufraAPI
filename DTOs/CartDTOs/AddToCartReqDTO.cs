namespace DTOs.CartDTOs
{
    public class AddToCartReqDTO
    {
        public int CustomerId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
