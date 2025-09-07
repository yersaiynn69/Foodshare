using Foodshare.Services;

namespace Foodshare.Pages.Needy;

public partial class NeedyProfilePage : ContentView
{
    public NeedyProfilePage(){ InitializeComponent(); }
    protected override void OnParentSet()
    {
        base.OnParentSet();
        var u = AuthService.CurrentUser!;
        Name.Text = u.FullName;
        Phone.Text = u.Phone;
    }
    async void Logout_Clicked(object s, EventArgs e)
    {
        Application.Current.Services.GetService<AuthService>()!.Logout();
        await Shell.Current.Navigation.PopToRootAsync();
    }
}
