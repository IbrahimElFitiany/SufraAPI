namespace Sufra_MVC.DTOs
{
    public class MenuItemDTO
    {
        public int MenuItemId { get; set; }
        public int RestaurantId { get; set; }
        public int MenuSectionId { get; set; }
        public string Name { get; set; }
        public string MenuItemImg { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Availability { get; set; }
    }
}
