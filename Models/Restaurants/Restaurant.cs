
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sufra.Models.Orders;
using Sufra.Models.Reservations;
using Sufra.Exceptions;

namespace Sufra.Models.Restaurants
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImgUrl { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        public string Description { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
        public string Address { get; set; }
        public bool IsApproved { get; set; }

        [Range(1, 5)]
        [DefaultValue(0)]
        public decimal Rating { get; set; }


        // Foreign Keys
        [ForeignKey("Cuisine")]
        public int CuisineId { get; set; }

        [ForeignKey("Manager")]
        public int ManagerId { get; set; }

        [ForeignKey("District")]
        public int DistrictId { get; set; }

        // Navigation Properties
        public virtual Cuisine Cuisine { get; set; }
        public virtual RestaurantManager Manager { get; set; }
        public virtual District District { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<MenuSection> MenuSections { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Order> Orders { get; set; }



        // Private Backing Fields for Collections
        private readonly List<RestaurantOpeningHours> _openingHours = new();
        private readonly List<Table> _tables = new();
        private readonly List<RestaurantReview> _restaurantReviews = new();

        // Public Methods for Controlled Access
        public virtual IReadOnlyCollection<RestaurantOpeningHours> OpeningHours => _openingHours.AsReadOnly();
        public virtual IReadOnlyCollection<Table> Tables => _tables.AsReadOnly();
        public virtual IReadOnlyCollection<RestaurantReview> RestaurantReviews => _restaurantReviews.AsReadOnly();




        //-----------------------------------------------------------------------

        // Table Behavior
        public void AddTable(int capacity, string label)
        {
            Table table = new Table(capacity,label, Id);
            _tables.Add(table);
        }
        public IReadOnlyCollection<Table> GetTables()
        {
            return Tables;
        }
        public void RemoveTable(int tableId)
        {
            Table table = Tables.FirstOrDefault(t => t.Id == tableId);
            if (table == null)
            {
                throw new TableNotFoundException("Table Not Found");
            }
            _tables.Remove(table);
        }

        //-----------------------------------------------------------------------


        // OpeningHours Behavior 
        public void AddOpeningHour(DayOfWeek day, TimeSpan openTime, TimeSpan closeTime)
        {
            if (openTime == closeTime)
                throw new ArgumentException("Open and close time cannot be the same.");

            bool openingHoursExists = OpeningHours.Any(h =>
                h.DayOfWeek == day
            );

            if (openingHoursExists)
            {
                throw new OpeningHoursExistsException("Opening hours for this day already exist. Use update method to modify them.");
            }

            _openingHours.Add(new RestaurantOpeningHours(day,openTime,closeTime,Id));
        }
        public IReadOnlyCollection<RestaurantOpeningHours> GetOpeningHours()
        {
            return OpeningHours;
        }
        public void UpdateOpeningHour(DayOfWeek day, TimeSpan openTime, TimeSpan closeTime)
        {
            if (openTime >= closeTime)
                throw new ArgumentException("Start time must be before end time.");

            var existingOpeningHour = OpeningHours.FirstOrDefault(h => h.DayOfWeek == day);

            if (existingOpeningHour == null)
            {
                throw new InvalidOperationException("No opening hour found for the specified day.");
            }
            if(openTime == existingOpeningHour.OpenTime && closeTime == existingOpeningHour.CloseTime)
            {
                throw new Exception("Same Value");
            }
            existingOpeningHour.OpenTime = openTime;
            existingOpeningHour.CloseTime = closeTime;
        }
        public void DeleteOpeningHour(DayOfWeek day)
        {
            var existingOpeningHour = OpeningHours.FirstOrDefault(h => h.DayOfWeek == day);

            if (existingOpeningHour == null)
            {
                throw new InvalidOperationException("No opening hour found for the specified day.");
            }
            _openingHours.Remove(existingOpeningHour);
        }
        public bool IsInOpeningHour(DateTime reservationTime)
        {
            DayOfWeek day = reservationTime.DayOfWeek;
            TimeSpan reservationTimeOfDay = reservationTime.TimeOfDay;

            RestaurantOpeningHours openingHour = OpeningHours.FirstOrDefault(h => h.DayOfWeek == day);

            if (openingHour == null) return false;

            if (openingHour.OpenTime < openingHour.CloseTime)
            {
                return reservationTimeOfDay >= openingHour.OpenTime && reservationTimeOfDay <= openingHour.CloseTime;
            }
            else
            {
                return reservationTimeOfDay >= openingHour.OpenTime || reservationTimeOfDay <= openingHour.CloseTime;
            }
        }

        //-----------------------------------------------------------------------


        // Review Behavior 
        public void AddReview(RestaurantReview restaurantReview)
        {
            _restaurantReviews.Add(restaurantReview);
        }
    }
}
