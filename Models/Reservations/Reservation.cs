using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Sufra.Models.Customers;
using Sufra.Models.Restaurants;

namespace Sufra.Models.Reservations
{
    public enum ReservationStatus
    {
        Pending,
        Approved,
        Rejected,
        Canceled
    }
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        [ForeignKey("Table")]
        public int TableId { get; set; }


        [Required]
        public int PartySize { get; set; }

        [Required]
        public DateTime ReservationDateTime { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))] // Serialize enum as string
        public ReservationStatus Status { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual Table Table { get; set; }
    }
}
