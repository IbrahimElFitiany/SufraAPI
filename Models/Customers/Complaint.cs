using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SufraMVC.Models.Customers
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required]
        public string ComplaintText { get; set; }

        [DefaultValue(typeof(DateTime), "CURRENT_TIMESTAMP")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Customer Customer { get; set; }
    }
}
