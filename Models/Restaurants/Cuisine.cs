using System.ComponentModel.DataAnnotations;

namespace Sufra.Models.Restaurants
{
    public class Cuisine
    {
        [Key]
        public int Id { get; set; }

        private string _name;

        [Required, StringLength(255)]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim().ToLowerInvariant();
        }

       //navigation
        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
