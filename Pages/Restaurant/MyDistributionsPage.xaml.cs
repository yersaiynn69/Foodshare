using Foodshare.Services;

namespace Foodshare.Pages.Restaurant
{
    public partial class MyDistributionsPage : ContentPage
    {
        public MyDistributionsPage() { InitializeComponent(); }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var me = await AuthService.I.GetCurrentAsync();
            if (me == null) return;
            var list = await DbService.I.GetRestaurantHistoryAsync(me.Id);
            List.ItemsSource = list;
        }
    }
}
