using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class Transmission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransmissionID { get; set; }

        [ MaxLength(100)]
        public string? TransmissionType { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public virtual ICollection<Car>? Cars { get; set; }
    }
}
