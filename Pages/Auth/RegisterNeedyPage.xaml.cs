using Foodshare.Services;

namespace Foodshare.Pages.Auth
{
    public partial class RegisterNeedyPage : ContentPage
    {
        public RegisterNeedyPage() { InitializeComponent(); SaveBtn.Clicked += OnSave; }

        private async void OnSave(object? s, EventArgs e)
        {
            var ok = await AuthService.I.RegisterNeedyAsync(
                fullName: FullName.Text?.Trim() ?? "",
                phone: Phone.Text?.Trim() ?? "",
                email: Email.Text?.Trim() ?? "",
                city: "Атырау",
                addr: Address.Text?.Trim() ?? "",
                pass: Password.Text ?? ""
            );
            if (!ok) { await DisplayAlert("Ошибка", "Проверьте данные", "OK"); return; }
            await Shell.Current.GoToAsync("//home");
        }
    }
}
