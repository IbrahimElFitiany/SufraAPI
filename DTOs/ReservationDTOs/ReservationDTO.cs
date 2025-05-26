namespace Sufra.DTOs.ReservationDTOs
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }

        public int TableId { get; set; }
        public string TabelLabel { get; set; }

        public DateTime ReservationDateTime { get; set; }
        public int PartySize { get; set; }

        public string reservationStatus{ get; set; }
    }
}