using System.Collections.ObjectModel;
using Foodshare.Models;
using Foodshare.Services;

namespace Foodshare.Pages.Restaurant
{
    public partial class RestaurantHomePage : ContentPage
    {
        private readonly ObservableCollection<FoodItem> _items = new();

        public RestaurantHomePage()
        {
            InitializeComponent();
            AddBtn.Clicked += async (_, __) => await Navigation.PushAsync(new AddFoodPage());
            ReservedBtn.Clicked += async (_, __) => await Navigation.PushAsync(new ReservedOrdersPage());
            HistoryBtn.Clicked += async (_, __) => await Navigation.PushAsync(new MyDistributionsPage());
            FoodList.ItemsSource = _items;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            _items.Clear();
            var me = await AuthService.I.GetCurrentAsync();
            if (me == null) return;
            var list = await DbService.I.GetRestaurantHistoryAsync(me.Id);
            foreach (var f in list.Where(x => x.IsAvailable))
                _items.Add(f);
        }
    }
}
