using Sufra.DTOs;

namespace Sufra.DTOs.MenuSectionDTOs
{
    public class MenuSectionDTO
    {
        public int RestaurantId { get; set; }
        public int MenuSectionId { get; set; }
        public string MenuSectionName { get; set; }
        public List<MenuItemDTO> Items { get; set; }
    }
}
