using SufraMVC.Models.Reservations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SufraMVC.Models.Restaurants
{
    public class Table
    {
        [Key]
        public int Id { get; private set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; private set; }

        [Required, Range(1, 100)]
        public int Capacity { get; private set; }

        [Required]
        public string TableLabel { get; private set; }

        // Navigation Property for Reservations
        public virtual ICollection<Reservation> Reservations { get; set; }

        public Table(int capacity, string tableLabel, int restaurantId)
        {
            TableLabel = tableLabel;
            Capacity = capacity;
            RestaurantId = restaurantId;
            Reservations = new List<Reservation>();  // Initialize the Reservations collection
        }

        public void UpdateLabel(string newLabel)
        {
            if (string.IsNullOrWhiteSpace(newLabel))
                throw new ArgumentException("Label cannot be empty");
            TableLabel = newLabel;
        }

        public void UpdateCapacity(int newCapacity)
        {
            if (newCapacity < 1 || newCapacity > 100)
                throw new ArgumentOutOfRangeException(nameof(newCapacity), "Capacity must be between 1 and 100");
            Capacity = newCapacity;
        }

        // Required by EF
        public Table() { }
    }
}
