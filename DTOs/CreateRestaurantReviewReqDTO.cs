using System.ComponentModel.DataAnnotations;

namespace sufra.Controllers
{
    public class CreateRestaurantReviewReqDTO
    {
        public int RestaurantId { get; set; }
        [Range(1,5)]
        public decimal Rating { get; set; }
        public string Review { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    }
}