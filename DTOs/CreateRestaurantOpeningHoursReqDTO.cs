
using System.ComponentModel.DataAnnotations;

namespace SufraMVC.DTOs
{
    public class CreateRestaurantOpeningHoursReqDTO
    {
        [Range(0, 6, ErrorMessage = "DayOfWeek must be between 1 and 7.")]
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }

    }
}