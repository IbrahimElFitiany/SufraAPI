namespace Sufra.DTOs.ReservationDTOs
{
    public class ReservationQueryDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        //will implement other filtering soon
    }
}
