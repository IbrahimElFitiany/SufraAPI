using System.ComponentModel.DataAnnotations;

namespace Sufra_MVC.Models
{
    public class Gov
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        //navigation

        public virtual ICollection<District> Districts { get; set; }
    }
}
