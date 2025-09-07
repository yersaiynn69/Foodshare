using Foodshare.Services;

namespace Foodshare.Pages.Auth
{
    public partial class RegisterVolunteerPage : ContentPage
    {
        public RegisterVolunteerPage() { InitializeComponent(); SaveBtn.Clicked += OnSave; }

        private async void OnSave(object? s, EventArgs e)
        {
            var ok = await AuthService.I.RegisterVolunteerAsync(
                fullName: FullName.Text?.Trim() ?? "",
                phone: Phone.Text?.Trim() ?? "",
                email: Email.Text?.Trim() ?? "",
                city: "Атырау",
                transport: Transport.Text?.Trim() ?? "",
                pass: Password.Text ?? ""
            );
            if (!ok) { await DisplayAlert("Ошибка", "Проверьте данные", "OK"); return; }
            await Shell.Current.GoToAsync("//home");
        }
    }
}
