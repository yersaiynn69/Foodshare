using SQLite;

namespace Foodshare.Models;

public class NeedyPerson
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string Note { get; set; } = "";
}
