using Foodshare.Services;
using Foodshare.Models;

namespace Foodshare.Pages.Needy;

public partial class NpoDirectoryPage : ContentPage
{
    readonly DbService _db;
    public NpoDirectoryPage(){ InitializeComponent(); _db=Application.Current.Services.GetService<DbService>()!; }

    protected override async void OnAppearing()
    {
        var npos = await _db.Conn.Table<User>().Where(u=>u.Role==UserRole.NGO).ToListAsync();
        List.ItemsSource = npos.Select(n=> new { Title = $"{n.OrgName} — {n.Phone}", n.Address, n.Phone }).ToList();
    }

    async void Call_Clicked(object s, EventArgs e)
    {
        var phone = (string)((Button)s).CommandParameter;
        await Launcher.OpenAsync($"tel:{phone}");
    }
}
