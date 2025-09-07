using Foodshare.Models;
using Foodshare.Services;
using Foodshare.Pages.Needy;

namespace Foodshare.Pages.Auth;

public partial class RegisterNeedyPage : ContentPage
{
    readonly AuthService _auth;
    public RegisterNeedyPage(){ InitializeComponent(); _auth = Application.Current.Services.GetService<AuthService>()!; }

    async void Create_Clicked(object sender, EventArgs e)
    {
        if (await _auth.EmailExists(Email.Text!)) { await DisplayAlert("Ошибка","Email уже существует","OK"); return; }
        var u = new User{
            FullName = FullName.Text?.Trim() ?? "",
            Phone = Phone.Text?.Trim() ?? "",
            Email = Email.Text?.Trim() ?? "",
            Address = Address.Text?.Trim() ?? "",
            Role = UserRole.Needy,
            City = "Атырау"
        };
        await _auth.Register(u, Password.Text ?? "");
        await Navigation.PushAsync(new NeedyHomePage());
    }
}
