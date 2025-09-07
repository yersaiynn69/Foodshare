using Foodshare.Services;
using Foodshare.Models;

namespace Foodshare.Pages.Volunteer;

public partial class VolunteerOrdersPage : TabbedPage
{
    readonly DbService _db;
    public VolunteerOrdersPage(){ InitializeComponent(); _db=Application.Current.Services.GetService<DbService>()!; }

    protected override async void OnAppearing()
    {
        var bookings = await _db.Conn.Table<Booking>().Where(b=>b.Status==BookingStatus.Reserved).ToListAsync();
        var foods = await _db.Conn.Table<FoodItem>().ToListAsync();
        var users = await _db.Conn.Table<User>().ToListAsync();

        var items = from b in bookings
                    join f in foods on b.FoodItemId equals f.Id
                    join r in users on f.RestaurantUserId equals r.Id
                    join u in users on b.BookerUserId equals u.Id
                    select new { BookingId=b.Id, Title=$"{f.Title} — {f.Kg:0.##} кг", Sub=$"Забрать: {r.Address} → Доставить: {u.Address}" };
        List.ItemsSource = items.ToList();
    }

    async void Accept_Clicked(object s, EventArgs e)
    {
        var id = (int)((Button)s).CommandParameter;
        var b = await _db.Conn.FindAsync<Booking>(id);
        if(b==null) return;
        b.VolunteerUserId = Services.AuthService.CurrentUser!.Id;
        b.Status = BookingStatus.PickedUp;
        await _db.Conn.UpdateAsync(b);
        await Navigation.PushAsync(new DeliveryDetailPage(id));
    }
}
