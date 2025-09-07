using SQLite;

namespace Foodshare.Models;

public class Booking
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public int FoodItemId { get; set; }
    public int BookerUserId { get; set; } // Needy or NGO user id
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public BookingStatus Status { get; set; } = BookingStatus.Reserved;
    public int? VolunteerUserId { get; set; }
}
