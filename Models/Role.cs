using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Rolename { get; set; }

        public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();
    }
}
