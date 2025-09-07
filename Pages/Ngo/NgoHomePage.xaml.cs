using Foodshare.Services;
using Foodshare.Models;

namespace Foodshare.Pages.Ngo;

public partial class NgoHomePage : TabbedPage
{
    readonly DbService _db;
    public NgoHomePage(){ InitializeComponent(); _db=Application.Current.Services.GetService<DbService>()!; }

    protected override async void OnAppearing()
    {
        var list = await _db.Conn.Table<FoodItem>().Where(f=>f.IsAvailable).ToListAsync();
        CatalogList.ItemsSource = list.Select(f=> new { f.Id, f.Title, f.Description, Kg=$"{f.Kg:0.##}", f.Address });
        People.ItemsSource = await _db.Conn.Table<NeedyPerson>().ToListAsync();
    }

    async void Map_Clicked(object s, EventArgs e)=> await Navigation.PushAsync(new Pages.Common.FoodMapPage());
    async void Bookings_Clicked(object s, EventArgs e)=> await Navigation.PushAsync(new Pages.Needy.NeedyBookingsPage());
    async void Call_Clicked(object s, EventArgs e){ var p=(string)((Button)s).CommandParameter; await Launcher.OpenAsync($"tel:{p}"); }
}
