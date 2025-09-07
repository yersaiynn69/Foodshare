using Foodshare.Services;
using Foodshare.Models;

namespace Foodshare.Pages.Volunteer;

public partial class DeliveryDetailPage : ContentPage
{
    readonly DbService _db; readonly int _bookingId;
    User rest = null!, rec = null!;
    FoodItem food = null!;
    public DeliveryDetailPage(int bookingId){ InitializeComponent(); _db=Application.Current.Services.GetService<DbService>()!; _bookingId=bookingId; }

    protected override async void OnAppearing()
    {
        var b = await _db.Conn.FindAsync<Booking>(_bookingId);
        food = await _db.Conn.FindAsync<FoodItem>(b.FoodItemId);
        rest = await _db.Conn.FindAsync<User>(food.RestaurantUserId);
        rec = await _db.Conn.FindAsync<User>(b.BookerUserId);

        Rest.Text = $"Ресторан: {rest.OrgName}\nАдрес: {rest.Address}\nЕда: {food.Title} ({food
