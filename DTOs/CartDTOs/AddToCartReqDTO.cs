using System.ComponentModel.DataAnnotations;

namespace Sufra.DTOs.CartDTOs
{
    public class AddToCartReqDTO
    {
        [Required]
        public int MenuItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int  Quantity { get; set; } = 1;
    }
}