using SQLite;
using Foodshare.Models;

namespace Foodshare.Services;

public class DbService
{
    readonly SQLiteAsyncConnection _db;
    public DbService()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, "foodshare.db3");
        _db = new SQLiteAsyncConnection(path);
        _db.CreateTableAsync<User>().Wait();
        _db.CreateTableAsync<FoodItem>().Wait();
        _db.CreateTableAsync<Booking>().Wait();
        _db.CreateTableAsync<NeedyPerson>().Wait();
        Seed().Wait();
    }

    async Task Seed()
    {
        // demo data once
        if (await _db.Table<User>().CountAsync() == 0)
        {
            var rest = new User
            {
                Email = "rest@saiynex.kz",
                PasswordHash = Hash("123456"),
                FullName = "Ресторан 'Добро'",
                Role = UserRole.Restaurant,
                OrgName = "ТОО ДоброФуд",
                BinIin = "123456789012",
                Address = "Атырау, Сатыбалдиева 10",
                City = "Атырау",
                Latitude = 47.0945,
                Longitude = 51.9234
            };
            await _db.InsertAsync(rest);

            await _db.InsertAsync(new FoodItem
            {
                RestaurantUserId = rest.Id,
                Title = "Плов и хлеб",
                Description = "Остатки после обеда, 3 порции",
                Kg = 1.5,
                ExpiresAt = DateTime.UtcNow.AddHours(4),
                Address = rest.Address,
                Latitude = rest.Latitude,
                Longitude = rest.Longitude,
                IsAvailable = true
            });

            await _db.InsertAsync(new User
            {
                Email = "npo@saiynex.kz",
                PasswordHash = Hash("123456"),
                FullName = "НПО 'Поддержка'",
                Role = UserRole.NGO,
                OrgName = "ОФ Поддержка",
                BinIin = "990099009900",
                Address = "Атырау, Азаттык 20",
                City = "Атырау"
            });

            await _db.InsertAsync(new User
            {
                Email = "needy@saiynex.kz",
                PasswordHash = Hash("123456"),
                FullName = "Айдарбек Нуждающийся",
                Role = UserRole.Needy,
                Phone = "+77010000000",
                Address = "Атырау, Сатпаева 5",
                City = "Атырау"
            });

            await _db.InsertAsync(new User
            {
                Email = "vol@saiynex.kz",
                PasswordHash = Hash("123456"),
                FullName = "Ерлан Волонтёр",
                Role = UserRole.Volunteer,
                Phone = "+77020000000",
                VehicleInfo = "Седан"
            });

            await _db.InsertAllAsync(new[]
            {
                new NeedyPerson{ FullName="Семья Ибраевы", Phone="+77030000001", Address="Атырау, Махамбет 12", Note="3 детей"},
                new NeedyPerson{ FullName="Пенсионерка Майра", Phone="+77030000002", Address="Атырау, Байдулла 7", Note="одинокая"},
            });
        }
    }

    public static string Hash(string s) => Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(s)));

    public SQLiteAsyncConnection Conn => _db;
}
