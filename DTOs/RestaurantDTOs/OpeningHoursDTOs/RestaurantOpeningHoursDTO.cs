namespace Sufra.DTOs.RestaurantDTOs.OpeningHoursDTOs
{
    public class RestaurantOpeningHoursDTO
    {
        public int RestaurantId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }

    }
}