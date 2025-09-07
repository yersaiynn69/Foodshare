using System;
using SQLite;

namespace Foodshare.Models
{
    public class FoodItem
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string RestaurantUserId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public double Kg { get; set; } = 0;

        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(12);

        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsAvailable { get; set; } = true; // скрываем из каталога/карты после брони
    }
}
