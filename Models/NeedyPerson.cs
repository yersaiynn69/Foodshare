using System;
using SQLite;

namespace Foodshare.Models
{
    public class NeedyPerson
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; } = 0;
        public double Longitude { get; set; } = 0;

        public string Notes { get; set; } = string.Empty; // краткая инфо
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
