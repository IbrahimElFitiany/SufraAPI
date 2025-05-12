namespace DTOs.MenuSectionDTOs
{
    public class CreateMenuItemReqDTO
    {
        public int MenuSectionId { get; set; }
        public string Name { get; set; }
        public string MenuItemImg { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Availability { get; set; }
    }
}
