using Foodshare.Services;

namespace Foodshare.Pages.Ngo;

public partial class NgoProfilePage : ContentView
{
    public NgoProfilePage(){ InitializeComponent(); }
    protected override void OnParentSet()
    {
        base.OnParentSet();
        var u = AuthService.CurrentUser!;
        Name.Text = u.OrgName;
        Address.Text = u.Address;
    }
    async void Logout_Clicked(object s, EventArgs e)
    {
        Application.Current.Services.GetService<AuthService>()!.Logout();
        await Shell.Current.Navigation.PopToRootAsync();
    }
}
