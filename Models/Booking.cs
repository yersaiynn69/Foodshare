using System;
using SQLite;

namespace Foodshare.Models
{
    public class Booking
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Еда
        public string FoodItemId { get; set; } = string.Empty;
        public string RestaurantUserId { get; set; } = string.Empty;

        // Кто бронировал (нуждающийся или НПО)
        public string BookerUserId { get; set; } = string.Empty;
        public bool CreatedByNgo { get; set; } = false;

        // Кому доставляем (если бронировала НПО)
        public string RecipientName { get; set; } = string.Empty;
        public string RecipientPhone { get; set; } = string.Empty;
        public string RecipientAddress { get; set; } = string.Empty;
        public double RecipientLat { get; set; } = 0;
        public double RecipientLng { get; set; } = 0;

        // Волонтёр
        public string VolunteerUserId { get; set; } = string.Empty;

        // Кол-во
        public double Kg { get; set; } = 0;

        // Статус
        public BookingStatus Status { get; set; } = BookingStatus.Reserved;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
    }
}
