using Foodshare.Models;
using Foodshare.Services;
using Foodshare.Pages.Ngo;

namespace Foodshare.Pages.Auth;

public partial class RegisterNgoPage : ContentPage
{
    readonly AuthService _auth;
    public RegisterNgoPage(){ InitializeComponent(); _auth = Application.Current.Services.GetService<AuthService>()!; }

    async void Create_Clicked(object sender, EventArgs e)
    {
        if (await _auth.EmailExists(Email.Text!)) { await DisplayAlert("Ошибка","Email уже существует","OK"); return; }
        var u = new User{
            OrgName = OrgName.Text?.Trim() ?? "", BinIin = Bin.Text?.Trim() ?? "",
            FullName = FullName.Text?.Trim() ?? "", Phone = Phone.Text?.Trim() ?? "",
            Email = Email.Text?.Trim() ?? "", Address = Address.Text?.Trim() ?? "",
            Role = UserRole.NGO, City="Атырау"
        };
        await _auth.Register(u, Password.Text ?? "");
        await Navigation.PushAsync(new NgoHomePage());
    }
}
