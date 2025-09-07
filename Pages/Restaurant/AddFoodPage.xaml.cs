using Foodshare.Services;
using Foodshare.Models;
using Microsoft.Maui.Devices.Sensors;

namespace Foodshare.Pages.Restaurant;

public partial class AddFoodPage : ContentPage
{
    readonly DbService _db;
    string _photoPath = "";
    public AddFoodPage(){ InitializeComponent(); _db=Application.Current.Services.GetService<DbService>()!; }

    async void Photo_Clicked(object s, EventArgs e)
    {
        var file = await FilePicker.PickAsync(new PickOptions{ FileTypes = FilePickerFileType.Images });
        if (file!=null){ _photoPath = file.FullPath; Photo.Source = _photoPath; }
    }

    async void Add_Clicked(object s, EventArgs e)
    {
        var me = Services.AuthService.CurrentUser!;
        double.TryParse(KgEntry.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var kg);
        double? lat=null,lng=null;
        try{ var loc = (await Geocoding.GetLocationsAsync(AddressEntry.Text)).FirstOrDefault(); if(loc!=null){ lat=loc.Latitude; lng=loc.Longitude; } }catch{}
        var item = new FoodItem{
            RestaurantUserId = me.Id,
            Title = TitleEntry.Text ?? "",
            Description = DescEntry.Text ?? "",
            Kg = kg,
            ExpiresAt = DateTime.UtcNow.AddHours(8),
            Address = AddressEntry.Text ?? me.Address,
            Latitude = lat, Longitude = lng,
            IsAvailable = true,
            PhotoPath = _photoPath
        };
        await _db.Conn.InsertAsync(item);
        await DisplayAlert("Готово","Еда добавлена и доступна на карте/в каталоге","OK");
        await Navigation.PopAsync();
    }
}
