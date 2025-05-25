using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sufra.Models.Restaurants
{
    public class MenuSection
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        // Navigation Properties
        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
