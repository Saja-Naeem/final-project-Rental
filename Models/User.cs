using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserID { get; set; }

        [Required, MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(15)]
        [Phone]
        public string? PhoneNumber { get; set; }

        [ MaxLength(200)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        // Navigation properties

        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Testimonial>? Testimonials { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();


    }
}
