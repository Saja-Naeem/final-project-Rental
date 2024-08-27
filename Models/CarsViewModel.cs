namespace Rental.Models
{
    public class CarsViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Car> Cars { get; set; }
    }
}
