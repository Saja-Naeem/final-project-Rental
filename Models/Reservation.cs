using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserID { get; set; }

        [Required]
        [ForeignKey("Car")]
        public int CarID { get; set; }

        public string? PickupLocation { get; set; }
        public string? DropoffLocation { get; set; }

        
        public DateTime? PickupDate { get; set; }

        
        public DateTime? DropoffDate { get; set; }

        
        public ReservationStatus? ReservationStatus { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public Car? Car { get; set; }
        public Payment? Payment { get; set; } 
    }
}
