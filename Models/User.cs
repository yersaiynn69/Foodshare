using SQLite;

namespace Foodshare.Models;

public class User
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    [Indexed, Unique] public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public UserRole Role { get; set; }
    public string City { get; set; } = "Атырау";

    // Org for Restaurant/NGO
    public string OrgName { get; set; } = "";
    public string BinIin { get; set; } = ""; // БИН/ИИН
    public string Address { get; set; } = "";
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // Volunteer vehicle
    public string VehicleInfo { get; set; } = "";

    // Ratings counters
    public double KgDonated { get; set; } = 0;
    public int DeliveriesDone { get; set; } = 0;
    public int NgoResponses { get; set; } = 0;
}
