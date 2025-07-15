namespace Sufra.DTOs.RestaurantDTOs.OpeningHoursDTOs
{
    public class RestaurantOpeningHoursResponseDTO
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
    }
}