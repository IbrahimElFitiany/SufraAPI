using Models.Reservation;

namespace DTOs
{
    public class CreateReservationResDTO
    {
        public int ReservationId { get; set; }
        public string RestaurantName { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public int PartySize { get; set; }
        public string Status { get; set; }
    }
}