using Foodshare.Models;
using Foodshare.Services;

namespace Foodshare.Pages.Restaurant
{
    public partial class AddFoodPage : ContentPage
    {
        public AddFoodPage()
        {
            InitializeComponent();
            SaveBtn.Clicked += async (_, __) =>
            {
                var me = await AuthService.I.GetCurrentAsync();
                if (me == null) return;

                var f = new FoodItem
                {
                    RestaurantUserId = me.Id,
                    Title = TitleEntry.Text?.Trim() ?? "",
                    Description = DescEditor.Text?.Trim() ?? "",
                    Kg = double.TryParse(KgEntry.Text, out var kg) ? kg : 0,
                    Address = AddressEntry.Text?.Trim() ?? "",
                    Latitude = double.TryParse(LatEntry.Text, out var lat) ? lat : 0,
                    Longitude = double.TryParse(LngEntry.Text, out var lng) ? lng : 0,
                    IsAvailable = true
                };
                await DbService.I.AddFoodAsync(f);
                await DisplayAlert("Готово", "Еда добавлена", "OK");
                await Navigation.PopAsync();
            };
        }
    }
}
