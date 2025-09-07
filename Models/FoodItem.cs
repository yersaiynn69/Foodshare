using SQLite;

namespace Foodshare.Models;

public class FoodItem
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public int RestaurantUserId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public double Kg { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string Address { get; set; } = "";
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string PhotoPath { get; set; } = "";
}
