using Foodshare.Services;
using System.Text.Json;

namespace Foodshare.Pages.Common;

public partial class FoodMapPage : ContentPage
{
    readonly DbService _db;
    public FoodMapPage(){ InitializeComponent(); _db = Application.Current.Services.GetService<DbService>()!; }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var html = await FileSystem.OpenAppPackageFileAsync("Resources/Raw/map.html");
        using var sr = new StreamReader(html);
        MapWeb.Source = new HtmlWebViewSource { Html = sr.ReadToEnd() };
        MapWeb.Navigated += async (_, __) => await PushMarkers();
    }

    async Task PushMarkers()
    {
        var foods = await _db.Conn.Table<Models.FoodItem>().Where(f=>f.IsAvailable==true).ToListAsync();
        var payload = new
        {
            center = new { lat = 47.0945, lng = 51.9234 },
            items = foods.Where(f=>f.Latitude.HasValue && f.Longitude.HasValue).Select(f => new {
                title = f.Title,
                desc = $"{f.Description} • {f.Kg:0.##} кг",
                address = f.Address,
                lat = f.Latitude!.Value,
                lng = f.Longitude!.Value
            })
        };
        var json = JsonSerializer.Serialize(payload);
#if ANDROID
        await MapWeb.EvaluateJavaScriptAsync($"window.updateMarkers({JsonEscape(json)});");
#else
        await MapWeb.EvaluateJavaScriptAsync($"window.updateMarkers({json});");
#endif
    }

    static string JsonEscape(string s) => "\""+s.Replace("\\","\\\\").Replace("\"","\\\"").Replace("\n","").Replace("\r","")+"\"";
}
