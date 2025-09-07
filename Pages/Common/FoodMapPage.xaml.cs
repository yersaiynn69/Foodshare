using System.Text.Json;
using Foodshare.Services;

namespace Foodshare.Pages.Common
{
    public partial class FoodMapPage : ContentPage
    {
        public FoodMapPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // грузим локальную карту
            var html = await FileSystem.OpenAppPackageFileAsync("Resources/Raw/map.html");
            using var sr = new StreamReader(html);
            var text = await sr.ReadToEndAsync();
            MapView.Source = new HtmlWebViewSource { Html = text };

            // даём карте маркеры
            var foods = await DbService.I.GetAvailableFoodAsync();
            var payload = foods.Select(f => new
            {
                id = f.Id,
                title = f.Title,
                desc = f.Description,
                kg = f.Kg,
                lat = f.Latitude,
                lng = f.Longitude,
                addr = f.Address
            }).ToList();
            var json = JsonSerializer.Serialize(payload);

            // задержка чтобы WebView успел инициализироваться
            await Task.Delay(500);
            try
            {
                await MapView.EvaluateJavaScriptAsync($"window.updateMarkers && window.updateMarkers({json});");
            }
            catch { /* игнор */ }
        }
    }
}
