using Foodshare.Services;

namespace Foodshare.Pages.Ngo
{
    public partial class NgoProfilePage : ContentPage
    {
        public NgoProfilePage()
        {
            InitializeComponent();
            SaveBtn.Clicked += OnSave;
            LogoutBtn.Clicked += async (_, __) => { await AuthService.I.LogoutAsync(); await Shell.Current.GoToAsync($"//{nameof(Foodshare.Pages.Auth.LoginPage)}"); };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var me = await AuthService.I.GetCurrentAsync();
            if (me == null) return;
            OrgName.Text = me.OrgName;
            Bin.Text = me.BinIin;
            Phone.Text = me.Phone;
            Address.Text = me.Address;
        }

        private async void OnSave(object? s, EventArgs e)
        {
            var me = await AuthService.I.GetCurrentAsync();
            if (me == null) return;
            me.OrgName = OrgName.Text?.Trim() ?? "";
            me.BinIin = Bin.Text?.Trim() ?? "";
            me.Phone = Phone.Text?.Trim() ?? "";
            me.Address = Address.Text?.Trim() ?? "";
            await DbService.I.UpsertUserAsync(me);
            await DisplayAlert("Готово", "Сохранено", "OK");
        }
    }
}
