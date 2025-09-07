using System.Collections.ObjectModel;
using Foodshare.Models;
using Foodshare.Services;

namespace Foodshare.Pages.Needy
{
    public partial class NeedyBookingsPage : ContentPage
    {
        public class Row { public string Title { get; set; } = ""; public string Detail { get; set; } = ""; }
        private readonly ObservableCollection<Row> _items = new();

        public NeedyBookingsPage() { InitializeComponent(); List.ItemsSource = _items; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _items.Clear();
            var me = await AuthService.I.GetCurrentAsync();
            if (me == null) return;
            var bookings = await DbService.I.Conn.Table<Booking>().Where(x => x.BookerUserId == me.Id)
                .OrderByDescending(x => x.CreatedAt).ToListAsync();
            foreach (var b in bookings)
            {
                var food = await DbService.I.Conn.FindAsync<FoodItem>(b.FoodItemId);
                _items.Add(new Row
                {
                    Title = food?.Title ?? "Заказ",
                    Detail = $"{b.Kg} кг • Статус: {b.Status}"
                });
            }
        }
    }
}
