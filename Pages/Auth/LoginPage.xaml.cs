using Foodshare.Services;
using Foodshare.Models;
using Foodshare.Pages.Needy;
using Foodshare.Pages.Restaurant;
using Foodshare.Pages.Ngo;
using Foodshare.Pages.Volunteer;

namespace Foodshare.Pages.Auth;

public partial class LoginPage : ContentPage
{
    readonly AuthService _auth;
    public LoginPage(AuthService auth)
    {
        InitializeComponent();
        _auth = auth;
    }

    protected override void OnAppearing()
    {
        EmailEntry.Text = "needy@saiynex.kz";
        PasswordEntry.Text = "123456";
    }

    async void Login_Clicked(object sender, EventArgs e)
    {
        var u = await _auth.Login(EmailEntry.Text?.Trim() ?? "", PasswordEntry.Text ?? "");
        if (u == null)
        {
            await DisplayAlert("Ошибка", "Неверный email или пароль", "OK");
            return;
        }
        App.CurrentCity = CityPicker.SelectedItem?.ToString() ?? "Атырау";
        await OpenRoleShell(u);
    }

    async Task OpenRoleShell(User u)
    {
        switch (u.Role)
        {
            case UserRole.Needy: await Navigation.PushAsync(new NeedyHomePage()); break;
            case UserRole.NGO: await Navigation.PushAsync(new NgoHomePage()); break;
            case UserRole.Restaurant: await Navigation.PushAsync(new RestaurantHomePage()); break;
            case UserRole.Volunteer: await Navigation.PushAsync(new VolunteerOrdersPage()); break;
        }
    }

    async void Register_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new RegisterRolePage());
    async void Forgot_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new ForgotPasswordPage());
    async void Lang_Clicked(object sender, EventArgs e)
    {
        // простое переключение между ru/kk
        var loc = Application.Current.Services.GetService<LocalizationService>()!;
        var lang = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ru" ? "kk" : "ru";
        loc.Init(lang);
        await DisplayAlert("OK", $"Язык: {lang}", "OK");
        await Navigation.PushAsync(new LoginPage(_auth));
        Navigation.RemovePage(this);
    }
}
