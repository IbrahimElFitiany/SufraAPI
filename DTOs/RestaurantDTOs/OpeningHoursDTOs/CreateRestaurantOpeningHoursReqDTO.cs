using System.ComponentModel.DataAnnotations;

namespace Sufra.DTOs.RestaurantDTOs.OpeningHoursDTOs
{
    public class CreateRestaurantOpeningHoursReqDTO
    {
        [Range(0, 6, ErrorMessage = "DayOfWeek must be between 0 and 6.")]
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }

    }
}