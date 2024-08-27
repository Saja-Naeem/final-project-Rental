using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class Testimonial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestimonialID { get; set; }

        [Required]
        public string UserID { get; set; }

        public string? Message { get; set; }
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public bool? IsDeleted { get; set; } = false;
        public TestimonialStatus? TestimonialStatus { get; set; }

        // Navigation property
        public virtual User? User { get; set; }
    }
}
