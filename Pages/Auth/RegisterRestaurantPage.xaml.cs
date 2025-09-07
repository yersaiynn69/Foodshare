using Foodshare.Models;
using Foodshare.Services;
using Foodshare.Pages.Restaurant;
using Microsoft.Maui.Devices.Sensors;

namespace Foodshare.Pages.Auth;

public partial class RegisterRestaurantPage : ContentPage
{
    readonly AuthService _auth; readonly DbService _db;
    public RegisterRestaurantPage(){ InitializeComponent(); _auth = Application.Current.Services.GetService<AuthService>()!; _db = Application.Current.Services.GetService<DbService>()!; }

    async void Create_Clicked(object sender, EventArgs e)
    {
        if (await _auth.EmailExists(Email.Text!)) { await DisplayAlert("Ошибка","Email уже существует","OK"); return; }
        double? lat=null,lng=null;
        try{
            var locations = await Geocoding.GetLocationsAsync(Address.Text);
            var loc = locations?.FirstOrDefault();
            if(loc!=null){ lat=loc.Latitude; lng=loc.Longitude; }
        }catch{}
        var u = new User{
            OrgName = OrgName.Text?.Trim() ?? "", BinIin = Bin.Text?.Trim() ?? "",
            FullName = FullName.Text?.Trim() ?? "", Phone = Phone.Text?.Trim() ?? "",
            Email = Email.Text?.Trim() ?? "", Address = Address.Text?.Trim() ?? "",
            Role = UserRole.Restaurant, City="Атырау", Latitude=lat, Longitude=lng
        };
        await _auth.Register(u, Password.Text ?? "");
        await Navigation.PushAsync(new RestaurantHomePage());
    }
}
