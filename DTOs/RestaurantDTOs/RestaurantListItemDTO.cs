namespace Sufra.DTOs.RestaurantDTOs
{
    public class RestaurantListItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public decimal Rating { get; set; }
        public string CuisineName { get; set; }
    }
}