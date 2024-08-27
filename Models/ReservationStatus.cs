namespace Rental.Models
{
    public enum ReservationStatus
    {
        Pending,    // Indicates that a reservation is awaiting approval or confirmation
        Approved,   // Indicates that a reservation has been approved and the car is officially reserved
        Cancelled,  // Indicates that a reservation has been cancelled and the car is available again
        Rejected    // Indicates that the reservation request was denied
    }
}

