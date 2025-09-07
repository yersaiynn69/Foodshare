using Foodshare.Services;
using Foodshare.Models;

namespace Foodshare.Pages.Restaurant;

public partial class RestaurantHomePage : TabbedPage
{
    readonly DbService _db;
    public RestaurantHomePage(){ InitializeComponent(); _db=Application.Current.Services.GetService<DbService>()!; }

    protected override async void OnAppearing()
    {
        var me = Services.AuthService.CurrentUser!;
        var foods = await _db.Conn.Table<FoodItem>().Where(f=>f.RestaurantUserId==me.Id).ToListAsync();
        History.ItemsSource = foods.Select(f=> new Label{ Text=$"{f.Title} — {f.Kg:0.##} кг — {(f.IsAvailable?"доступно":"отдано")}" }).ToList();
    }

    async void Add_Clicked(object s, EventArgs e)=> await Navigation.PushAsync(new AddFoodPage());
    async void My_Clicked(object s, EventArgs e)=> await Navigation.PushAsync(new MyDistributionsPage());
    async void Reserved_Clicked(object s, EventArgs e)=> await Navigation.PushAsync(new ReservedOrdersPage());
}
