using Sufra.Models.Reservations;

namespace Sufra.DTOs.ReservationDTOs
{
    public class ReservationQueryDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public ReservationStatus? Status{ get; set; }

        //will implement other filtering soon
    }
}
