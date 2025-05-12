using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Sufra_MVC.Models.RestaurantModels
{
    public class RestaurantOpeningHours
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        [Required]
        [Range(0, 6, ErrorMessage = "DayOfWeek must be between 0 and 6.")]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan OpenTime { get; set; }

        [Required]
        public TimeSpan CloseTime { get; set; }


        public RestaurantOpeningHours(DayOfWeek day, TimeSpan openTime, TimeSpan closeTime, int restaurantId)
        {
            DayOfWeek = day;
            OpenTime = openTime;
            CloseTime = closeTime;
            RestaurantId = restaurantId;
        }

        // Required by EF
        public RestaurantOpeningHours() { }
    }
}
