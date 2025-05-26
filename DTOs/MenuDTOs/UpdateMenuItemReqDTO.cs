namespace Sufra.DTOs.MenuDTOs
{
    public class UpdateMenuItemReqDTO
    {
        public int? MenuSectionId { get; set; }
        public string? Name { get; set; }
        public string? MenuItemImg { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public bool? Availability { get; set; }    
    }

}
