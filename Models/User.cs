using SQLite;

namespace Foodshare.Models
{
    public class User
    {
        [PrimaryKey]
        public string Id { get; set; } = System.Guid.NewGuid().ToString();

        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        // Для юр лиц
        public string OrgName { get; set; } = string.Empty;
        public string BinIin { get; set; } = string.Empty;

        // Контакты и адрес
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = "Атырау";
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public UserRole Role { get; set; }

        // Рейтинги/метрики
        public double KgDonated { get; set; } = 0;     // для ресторанов
        public int DeliveriesDone { get; set; } = 0;   // для волонтёров
        public int NgoResponses { get; set; } = 0;     // для НПО
    }
}
