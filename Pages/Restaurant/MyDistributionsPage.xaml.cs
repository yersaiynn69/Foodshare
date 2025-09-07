using Foodshare.Services;
using Foodshare.Models;

namespace Foodshare.Pages.Restaurant;

public partial class MyDistributionsPage : ContentPage
{
    readonly DbService _db;
    public MyDistributionsPage(){ InitializeComponent(); _db=Application.Current.Services.GetService<DbService>()!; }

    protected override async void OnAppearing()
    {
        var me = Services.AuthService.CurrentUser!;
        var foods = await _db.Conn.Table<FoodItem>().Where(f=>f.RestaurantUserId==me.Id && !f.IsAvailable).ToListAsync();
        List.ItemsSource = foods.Select(f=> new Label{ Text=$"{f.Title} — {f.Kg:0.##} кг (отдано)" }).ToList();
    }
}
