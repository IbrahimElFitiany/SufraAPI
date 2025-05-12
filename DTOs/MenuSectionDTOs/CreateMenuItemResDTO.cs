using Sufra_MVC.DTOs;

namespace DTOs.MenuSectionDTOs
{
    public class CreateMenuItemResDTO
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public MenuItemDTO MenuItemDTO { get; set; }
    }
}
