namespace Foodshare.Models
{
    public enum UserRole
    {
        Needy = 0,
        Restaurant = 1,
        Ngo = 2,
        Volunteer = 3
    }

    public enum BookingStatus
    {
        Pending = 0,      // создано
        Reserved = 1,     // забронировано
        InDelivery = 2,   // волонтёр принял
        Completed = 3,    // доставлено
        Cancelled = 4
    }
}
