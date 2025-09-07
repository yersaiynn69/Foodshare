using Foodshare.Services;
using Foodshare.Models;

namespace Foodshare.Pages.Restaurant;

public partial class ReservedOrdersPage : ContentPage
{
    readonly DbService _db;
    public ReservedOrdersPage(){ InitializeComponent(); _db=Application.Current.Services.GetService<DbService>()!; }

    protected override async void OnAppearing()
    {
        var me = Services.AuthService.CurrentUser!;
        var foods = await _db.Conn.Table<FoodItem>().Where(f=>f.RestaurantUserId==me.Id).ToListAsync();
        var bookings = await _db.Conn.Table<Booking>().Where(b=>b.Status==BookingStatus.Reserved || b.Status==BookingStatus.PickedUp).ToListAsync();
        var users = await _db.Conn.Table<User>().ToListAsync();

        var items = from b in bookings
                    join f in foods on b.FoodItemId equals f.Id
                    join u in users on b.BookerUserId equals u.Id
                    select new { Title=$"{f.Title} — {f.Kg:0.##} кг", Booker=$"Получатель: {u.FullName} ({u.Phone})" };

        List.ItemsSource = items.ToList();
    }
}
