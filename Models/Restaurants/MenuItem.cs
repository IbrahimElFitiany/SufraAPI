using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SufraMVC.Models.Restaurants
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        [ForeignKey("MenuSection")]
        public int MenuSectionId { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required, StringLength(255)]
        public string MenuItemImg { get; set; }

        public string Description { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [DefaultValue(true)]
        public bool Availability { get; set; } = true;

        // Navigation Properties
        public virtual Restaurant Restaurant { get; set; }
        public virtual MenuSection MenuSection { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
