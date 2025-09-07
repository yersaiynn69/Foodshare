using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foodshare.Models;
using SQLite;

namespace Foodshare.Services
{
    public sealed class DbService
    {
        private static readonly Lazy<DbService> _lazy = new(() => new DbService());
        public static DbService I => _lazy.Value;

        private SQLiteAsyncConnection _db;

        private DbService() { }

        public async Task InitAsync(string dbPath)
        {
            if (_db != null) return;
            _db = new SQLiteAsyncConnection(dbPath);
            await _db.CreateTableAsync<User>();
            await _db.CreateTableAsync<FoodItem>();
            await _db.CreateTableAsync<Booking>();
            await _db.CreateTableAsync<NeedyPerson>();

            // сиды нуждающихся
            var cnt = await _db.Table<NeedyPerson>().CountAsync();
            if (cnt == 0)
            {
                await _db.InsertAsync(new NeedyPerson
                {
                    FullName = "Айгуль Т.",
                    Phone = "+7707*******",
                    Address = "Атырау, пр. Азаттык 10",
                    Notes = "Многодетная семья"
                });
                await _db.InsertAsync(new NeedyPerson
                {
                    FullName = "Даурен К.",
                    Phone = "+7707*******",
                    Address = "Атырау, ул. Сатыбалдиева 3",
                    Notes = "Пенсионер"
                });
            }
        }

        public SQLiteAsyncConnection Conn => _db;

        // ---------- Пользователи ----------
        public Task<User?> GetUserByEmailAsync(string email) =>
            _db.Table<User>().Where(x => x.Email == email).FirstOrDefaultAsync();

        public Task<User?> GetUserByIdAsync(string id) =>
            _db.Table<User>().Where(x => x.Id == id).FirstOrDefaultAsync();

        public Task<int> UpsertUserAsync(User u) => _db.InsertOrReplaceAsync(u);

        // ---------- Еда ----------
        public Task<List<FoodItem>> GetAvailableFoodAsync() =>
            _db.Table<FoodItem>().Where(x => x.IsAvailable).OrderByDescending(x => x.ExpiresAt).ToListAsync();

        public Task<List<FoodItem>> GetRestaurantHistoryAsync(string restaurantUserId) =>
            _db.Table<FoodItem>().Where(x => x.RestaurantUserId == restaurantUserId).OrderByDescending(x => x.ExpiresAt).ToListAsync();

        public Task<int> AddFoodAsync(FoodItem f) => _db.InsertAsync(f);

        public async Task ReserveFoodAsync(string foodId, string bookerUserId, bool createdByNgo, double kg,
            string? recipientName = null, string? recipientPhone = null, string? recipientAddress = null,
            double? recipientLat = null, double? recipientLng = null)
        {
            var food = await _db.Table<FoodItem>().Where(x => x.Id == foodId && x.IsAvailable).FirstOrDefaultAsync();
            if (food == null) throw new InvalidOperationException("Еда недоступна");

            food.IsAvailable = false;
            await _db.UpdateAsync(food);

            var b = new Booking
            {
                FoodItemId = food.Id,
                RestaurantUserId = food.RestaurantUserId,
                BookerUserId = bookerUserId,
                CreatedByNgo = createdByNgo,
                Kg = kg,
                Status = BookingStatus.Reserved,
                RecipientName = recipientName ?? string.Empty,
                RecipientPhone = recipientPhone ?? string.Empty,
                RecipientAddress = recipientAddress ?? string.Empty,
                RecipientLat = recipientLat ?? 0,
                RecipientLng = recipientLng ?? 0
            };
            await _db.InsertAsync(b);
        }

        public Task<List<Booking>> GetRestaurantBookingsAsync(string restaurantUserId) =>
            _db.Table<Booking>().Where(x => x.RestaurantUserId == restaurantUserId)
                .OrderByDescending(x => x.CreatedAt).ToListAsync();

        public Task<List<Booking>> GetVolunteerInboxAsync() =>
            _db.Table<Booking>().Where(x => x.Status == BookingStatus.Reserved || x.Status == BookingStatus.InDelivery)
                .OrderByDescending(x => x.CreatedAt).ToListAsync();

        public async Task AcceptByVolunteerAsync(string bookingId, string volunteerUserId)
        {
            var b = await _db.FindAsync<Booking>(bookingId);
            if (b == null) throw new InvalidOperationException("Бронь не найдена");
            b.VolunteerUserId = volunteerUserId;
            b.Status = BookingStatus.InDelivery;
            await _db.UpdateAsync(b);
        }

        public async Task CompleteDeliveryAsync(string bookingId)
        {
            var b = await _db.FindAsync<Booking>(bookingId);
            if (b == null) throw new InvalidOperationException("Бронь не найдена");

            var food = await _db.FindAsync<FoodItem>(b.FoodItemId);
            var rest = await _db.FindAsync<User>(b.RestaurantUserId);
            User? vol = null;
            User? ngo = null;

            if (!string.IsNullOrEmpty(b.VolunteerUserId))
                vol = await _db.FindAsync<User>(b.VolunteerUserId);

            if (b.CreatedByNgo && !string.IsNullOrEmpty(b.BookerUserId))
                ngo = await _db.FindAsync<User>(b.BookerUserId);

            if (rest != null) { rest.KgDonated += b.Kg > 0 ? b.Kg : (food?.Kg ?? 0); await _db.UpdateAsync(rest); }
            if (vol != null) { vol.DeliveriesDone += 1; await _db.UpdateAsync(vol); }
            if (ngo != null) { ngo.NgoResponses += 1; await _db.UpdateAsync(ngo); }

            b.Status = BookingStatus.Completed;
            b.CompletedAt = DateTime.UtcNow;
            await _db.UpdateAsync(b);
        }

        // ---------- Нуждающиеся ----------
        public Task<List<NeedyPerson>> GetPeopleAsync() =>
            _db.Table<NeedyPerson>().OrderByDescending(x => x.CreatedAt).ToListAsync();

        public Task<NeedyPerson?> GetPersonAsync(string id) =>
            _db.Table<NeedyPerson>().Where(x => x.Id == id).FirstOrDefaultAsync();

        public Task<int> UpsertPersonAsync(NeedyPerson p) => _db.InsertOrReplaceAsync(p);

        // ---------- Рейтинги ----------
        public async Task<List<(string Name, double Score)>> GetRestaurantRatingAsync()
        {
            var users = await _db.Table<User>().Where(u => u.Role == UserRole.Restaurant)
                .OrderByDescending(u => u.KgDonated).ToListAsync();
            return users.Select(u => (u.OrgName?.Length > 0 ? u.OrgName : u.FullName, u.KgDonated)).ToList();
        }

        public async Task<List<(string Name, double Score)>> GetVolunteerRatingAsync()
        {
            var users = await _db.Table<User>().Where(u => u.Role == UserRole.Volunteer)
                .OrderByDescending(u => u.DeliveriesDone).ToListAsync();
            return users.Select(u => (u.FullName, (double)u.DeliveriesDone)).ToList();
        }

        public async Task<List<(string Name, double Score)>> GetNgoRatingAsync()
        {
            var users = await _db.Table<User>().Where(u => u.Role == UserRole.Ngo)
                .OrderByDescending(u => u.NgoResponses).ToListAsync();
            return users.Select(u => (u.OrgName?.Length > 0 ? u.OrgName : u.FullName, (double)u.NgoResponses)).ToList();
        }
    }
}
