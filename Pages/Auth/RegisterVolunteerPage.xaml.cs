using Foodshare.Models;
using Foodshare.Services;
using Foodshare.Pages.Volunteer;

namespace Foodshare.Pages.Auth;

public partial class RegisterVolunteerPage : ContentPage
{
    readonly AuthService _auth;
    public RegisterVolunteerPage(){ InitializeComponent(); _auth = Application.Current.Services.GetService<AuthService>()!; }

    async void Create_Clicked(object sender, EventArgs e)
    {
        if (await _auth.EmailExists(Email.Text!)) { await DisplayAlert("Ошибка","Email уже существует","OK"); return; }
        var u = new User{
            FullName = FullName.Text?.Trim() ?? "", Phone = Phone.Text?.Trim() ?? "",
            VehicleInfo = Vehicle.Text?.Trim() ?? "", Email = Email.Text?.Trim() ?? "",
            Role = UserRole.Volunteer, City="Атырау"
        };
        await _auth.Register(u, Password.Text ?? "");
        await Navigation.PushAsync(new VolunteerOrdersPage());
    }
}
