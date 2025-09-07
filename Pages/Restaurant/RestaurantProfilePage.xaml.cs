using Foodshare.Services;

namespace Foodshare.Pages.Restaurant;

public partial class RestaurantProfilePage : ContentView
{
    public RestaurantProfilePage(){ InitializeComponent(); }
    protected override void OnParentSet()
    {
        base.OnParentSet();
        var u = Services.AuthService.CurrentUser!;
        Name.Text = $"{u.OrgName}";
        Address.Text = u.Address;
        Stats.Text = $"Отдано: {u.KgDonated:0.##} кг";
    }
    async void Logout_Clicked(object s, EventArgs e)
    {
        Application.Current.Services.GetService<AuthService>()!.Logout();
        await Shell.Current.Navigation.PopToRootAsync();
    }
}
