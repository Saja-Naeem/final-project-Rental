using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }

        [Required]
        [ForeignKey("Reservation")]
        public int? ReservationID { get; set; }

        [Required, StringLength(50)]
        public string PaymentMethod { get; set; }

        [Required, StringLength(4)]
        public string CVV { get; set; }

        [Required, StringLength(16)]
        public string CardNumber { get; set; }

        [Required]
        public DateTime? ExpiryDate { get; set; }

        // Navigation properties
        public virtual Reservation? Reservation { get; set; }
    }
}
