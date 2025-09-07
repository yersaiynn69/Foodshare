using Foodshare.Services;

namespace Foodshare.Pages.Auth
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            CityPicker.SelectedIndex = 0;

            LoginBtn.Clicked += async (_, __) =>
            {
                var email = EmailEntry.Text?.Trim() ?? "";
                var pass = PasswordEntry.Text ?? "";
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
                {
                    await DisplayAlert("Ошибка", "Заполните Email и пароль", "OK");
                    return;
                }
                var ok = await AuthService.I.LoginAsync(email, pass);
                if (!ok) { await DisplayAlert("Ошибка", "Неверные данные", "OK"); return; }
                await Shell.Current.GoToAsync("//home");
            };

            RegisterBtn.Clicked += async (_, __) => await Navigation.PushAsync(new RegisterRolePage());
            ForgotBtn.Clicked += async (_, __) => await Navigation.PushAsync(new ForgotPasswordPage());
            LangBtn.Clicked += async (_, __) => await LocalizationService.I.ToggleLanguageAsync();
        }
    }
}
