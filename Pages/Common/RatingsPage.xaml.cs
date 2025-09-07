using Foodshare.Services;
using Foodshare.Models;

namespace Foodshare.Pages.Common;

public partial class RatingsPage : ContentPage
{
    readonly DbService _db;
    enum Mode { Restaurants, Volunteers, NGOs }
    Mode _mode = Mode.Restaurants;
    public RatingsPage(){ InitializeComponent(); _db = Application.Current.Services.GetService<DbService>()!; }

    protected override async void OnAppearing(){ await Load(); }

    async Task Load()
    {
        if (_mode == Mode.Restaurants)
        {
            var q = await _db.Conn.Table<User>().Where(u=>u.Role==UserRole.Restaurant).OrderByDescending(u=>u.KgDonated).ToListAsync();
            List.ItemsSource = q.Select(u=> new { Title = $"{u.OrgName} — {u.KgDonated:0.##} кг", Subtitle=$"{u.Address}" });
        }
        else if (_mode == Mode.Volunteers)
        {
            var q = await _db.Conn.Table<User>().Where(u=>u.Role==UserRole.Volunteer).OrderByDescending(u=>u.DeliveriesDone).ToListAsync();
            List.ItemsSource = q.Select(u=> new { Title = $"{u.FullName} — доставок: {u.DeliveriesDone}", Subtitle=$"{u.Phone}" });
        }
        else
        {
            var q = await _db.Conn.Table<User>().Where(u=>u.Role==UserRole.NGO).OrderByDescending(u=>u.NgoResponses).ToListAsync();
            List.ItemsSource = q.Select(u=> new { Title = $"{u.OrgName} — откликов: {u.NgoResponses}", Subtitle=$"{u.Address}" });
        }
    }

    async void Vol_Clicked(object s, EventArgs e){ _mode=Mode.Volunteers; await Load(); }
    async void Ngo_Clicked(object s, EventArgs e){ _mode=Mode.NGOs; await Load(); }
}
