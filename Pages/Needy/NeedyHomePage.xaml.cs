using Foodshare.Services;
using Foodshare.Models;
using Microsoft.Maui.ApplicationModel.Communication;

namespace Foodshare.Pages.Needy;

public partial class NeedyHomePage : TabbedPage
{
    readonly DbService _db; readonly AuthService _auth;
    public NeedyHomePage(){ InitializeComponent(); _db=Application.Current.Services.GetService<DbService>()!; _auth=Application.Current.Services.GetService<AuthService>()!; }

    protected override async void OnAppearing()
    {
        await LoadCatalog();
        await LoadNpo();
    }

    async Task LoadCatalog()
    {
        var list = await _db.Conn.Table<FoodItem>().Where(f=>f.IsAvailable).ToListAsync();
        CatalogList.ItemsSource = list.Select(f=> new { f.Id, f.Title, f.Description, Kg=$"{f.Kg:0.##}", f.Address });
    }

    async Task LoadNpo()
    {
        var npos = await _db.Conn.Table<User>().Where(u=>u.Role==UserRole.NGO).ToListAsync();
        NpoList.ItemsSource = npos.Select(n=> new Label
        {
            Text = $"{n.OrgName} — {n.Phone}\n{n.Address}",
            GestureRecognizers = { new TapGestureRecognizer{ Command = new Command(async ()=> await Launcher.OpenAsync($"tel:{n.Phone}")) } }
        });
    }

    async void Map_Clicked(object s, EventArgs e) => await Navigation.PushAsync(new Pages.Common.FoodMapPage());
    async void Bookings_Clicked(object s, EventArgs e) => await Navigation.PushAsync(new NeedyBookingsPage());

    async void Reserve_Clicked(object s, EventArgs e)
    {
        var id = (int)((Button)s).CommandParameter;
        var food = await _db.Conn.FindAsync<FoodItem>(id);
        if (food==null || !food.IsAvailable){ await DisplayAlert("Уведомление","Недоступно","OK"); return; }
        food.IsAvailable=false;
        await _db.Conn.UpdateAsync(food);
        var b = new Booking{ FoodItemId=id, BookerUserId=AuthService.CurrentUser!.Id, Status=BookingStatus.Reserved };
        await _db.Conn.InsertAsync(b);
        await DisplayAlert("Готово","Забронировано. Волонтёры увидят заказ для доставки.","OK");
        await LoadCatalog();
    }
}
