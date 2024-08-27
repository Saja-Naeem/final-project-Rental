using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarID { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        [ForeignKey("Transmission")]
        public int? TransmissionID { get; set; }

        public string? Name { get; set; }
        public string? Model { get; set; }
        public double? Price { get; set; }
        public int? NumberOfSeats { get; set; }
        public string? AvailabilityStatus { get; set; }
        public string? FuelType { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public bool? IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Category? Category { get; set; }
        public virtual Transmission? Transmission { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
    }
}

