using System.Collections.ObjectModel;
using Foodshare.Models;
using Foodshare.Services;

namespace Foodshare.Pages.Restaurant
{
    public partial class ReservedOrdersPage : ContentPage
    {
        public class Row
        {
            public string Title { get; set; } = "";
            public string Detail { get; set; } = "";
        }

        private readonly ObservableCollection<Row> _items = new();

        public ReservedOrdersPage()
        {
            InitializeComponent();
            List.ItemsSource = _items;
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
            var bookings = await DbService.I.GetRestaurantBookingsAsync(me.Id);
            foreach (var b in bookings.Where(x => x.Status != BookingStatus.Completed))
            {
                var food = await DbService.I.Conn.FindAsync<FoodItem>(b.FoodItemId);
                var who  = b.CreatedByNgo ? "НПО" : "Получатель";
                var recip = string.IsNullOrWhiteSpace(b.RecipientName) ? "" : $" • {b.RecipientName} {b.RecipientPhone}";
                _items.Add(new Row
                {
                    Title = food?.Title ?? "Заказ",
                    Detail = $"{who}{recip} • {b.Kg} кг • Статус: {b.Status}"
                });
            }
        }
    }
}
