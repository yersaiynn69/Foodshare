using Foodshare.Services;
using Foodshare.Models;

namespace Foodshare.Pages.Needy;

public partial class NeedyBookingsPage : ContentPage
{
    readonly DbService _db;
    public NeedyBookingsPage(){ InitializeComponent(); _db=Application.Current.Services.GetService<DbService>()!; }

    protected override async void OnAppearing()
    {
        var me = Services.AuthService.CurrentUser!;
        var bookings = await _db.Conn.Table<Booking>().Where(b=>b.BookerUserId==me.Id).ToListAsync();
        var foods = await _db.Conn.Table<FoodItem>().ToListAsync();
        var users = await _db.Conn.Table<User>().ToListAsync();
        var items = from b in bookings
                    join f in foods on b.FoodItemId equals f.Id
                    join r in users on f.RestaurantUserId equals r.Id
                    select new { Title = $"{f.Title} — {f.Kg:0.##} кг", Status = b.Status.ToString(), RestPhone = r.Phone };
        List.ItemsSource = items.ToList();
    }

    async void Call_Clicked(object s, EventArgs e)
    {
        var phone = (string)((Button)s).CommandParameter;
        if(!string.IsNullOrWhiteSpace(phone)) await Launcher.OpenAsync($"tel:{phone}");
    }
}
