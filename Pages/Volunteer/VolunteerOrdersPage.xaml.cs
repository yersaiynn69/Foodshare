using System.Collections.ObjectModel;
using Foodshare.Models;
using Foodshare.Services;

namespace Foodshare.Pages.Volunteer
{
    public partial class VolunteerOrdersPage : ContentPage
    {
        public ObservableCollection<Row> Items { get; } = new();

        public Command<string> AcceptCmd { get; }
        public Command<string> OpenCmd { get; }

        public class Row
        {
            public string Id { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string Detail { get; set; } = string.Empty;
        }

        public VolunteerOrdersPage()
        {
            InitializeComponent();
            List.BindingContext = this;

            AcceptCmd = new Command<string>(async id =>
            {
                // текущий пользователь — волонтёр
                var me = await AuthService.I.GetCurrentAsync();
                if (me == null) return;
                await DbService.I.AcceptByVolunteerAsync(id, me.Id);
                await LoadAsync();
            });

            OpenCmd = new Command<string>(async id =>
            {
                await Navigation.PushAsync(new DeliveryDetailPage(id));
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            Items.Clear();
            var list = await DbService.I.GetVolunteerInboxAsync();
            foreach (var b in list)
            {
                var food = await DbService.I.Conn.FindAsync<FoodItem>(b.FoodItemId);
                var title = food != null ? food.Title : "Заказ";
                var route = $"Ресторан: {b.RestaurantUserId} → Получатель: {(string.IsNullOrEmpty(b.RecipientAddress) ? "самовывоз" : b.RecipientAddress)}";
                Items.Add(new Row
                {
                    Id = b.Id,
                    Title = title,
                    Detail = $"{route} • {b.Kg} кг • Статус: {b.Status}"
                });
            }
            List.ItemsSource = Items;
        }
    }
}
