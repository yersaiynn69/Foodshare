using Foodshare.Services;

namespace Foodshare.Pages.Auth
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
            Title = "Сброс пароля";

            var email = new Entry { Placeholder = "Email" };
            var pass = new Entry { Placeholder = "Новый пароль", IsPassword = true };
            var btn  = new Button { Text = "Сменить" };

            btn.Clicked += async (_, __) =>
            {
                var ok = await AuthService.I.ResetPasswordAsync(email.Text?.Trim() ?? "", pass.Text ?? "");
                await DisplayAlert(ok ? "Готово" : "Ошибка", ok ? "Пароль обновлён" : "Email не найден", "OK");
                if (ok) await Navigation.PopAsync();
            };

            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 16,
                    Spacing = 10,
                    Children = { email, pass, btn }
                }
            };
        }
    }
}
