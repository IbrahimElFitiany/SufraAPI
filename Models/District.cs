using Sufra_MVC.Models;
using Sufra_MVC.Models.RestaurantModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sufra_MVC.Models
{
    public class District
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("Gov")]
        public int GovId { get; set; }


        // Navigation Properties
        public virtual Gov Gov { get; set; }
        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
