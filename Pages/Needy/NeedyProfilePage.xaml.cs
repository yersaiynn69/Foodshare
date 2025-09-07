using Foodshare.Services;

namespace Foodshare.Pages.Needy
{
    public partial class NeedyProfilePage : ContentPage
    {
        public NeedyProfilePage()
        {
            InitializeComponent();
            SaveBtn.Clicked += OnSave;
            LangBtn.Clicked += async (_, __) => await LocalizationService.I.ToggleLanguageAsync();
            LogoutBtn.Clicked += async (_, __) => { await AuthService.I.LogoutAsync(); await Shell.Current.GoToAsync($"//{nameof(Foodshare.Pages.Auth.LoginPage)}"); };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var me = await AuthService.I.GetCurrentAsync();
            if (me == null) return;
            FullName.Text = me.FullName;
            Phone.Text = me.Phone;
            Address.Text = me.Address;
        }

        private async void OnSave(object? s, EventArgs e)
        {
            var me = await AuthService.I.GetCurrentAsync();
            if (me == null) return;
            me.FullName = FullName.Text?.Trim() ?? "";
            me.Phone = Phone.Text?.Trim() ?? "";
            me.Address = Address.Text?.Trim() ?? "";
            await DbService.I.UpsertUserAsync(me);
            await DisplayAlert("Готово", "Сохранено", "OK");
        }
    }
}
