
namespace DTOs
{
    public class CreateReservationReqDTO
    {
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime DateTime{ get; set; }
        public int PartySize { get; set; }

    }
}