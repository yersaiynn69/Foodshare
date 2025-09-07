using System.Collections.ObjectModel;
using Foodshare.Models;
using Foodshare.Services;

namespace Foodshare.Pages.Ngo
{
    public partial class NgoHomePage : ContentPage
    {
        private readonly ObservableCollection<Row> _items = new();

        private class Row
        {
            public string Id { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public double Kg { get; set; }
            public Command<string> BookCmd { get; set; } = null!;
        }

        public NgoHomePage()
        {
            InitializeComponent();

            // нижние кнопки
            MapBtn.Clicked += async (_, __) => await Navigation.PushAsync(new Foodshare.Pages.Common.FoodMapPage());
            BookingsBtn.Clicked += async (_, __) => await Navigation.PushAsync(new Foodshare.Pages.Needy.NeedyBookingsPage()); // переиспользуем страницу бронирований

            // список доступной еды
            FoodList.BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            _items.Clear();
            var foods = await DbService.I.GetAvailableFoodAsync();
            foreach (var f in foods)
            {
                _items.Add(new Row
                {
                    Id = f.Id,
                    Title = f.Title,
                    Description = f.Description,
                    Kg = f.Kg,
                    BookCmd = new Command<string>(async id => await BookForWardAsync(id))
                });
            }
            FoodList.ItemsSource = _items;
        }

        private async Task BookForWardAsync(string foodId)
        {
            var me = await AuthService.I.GetCurrentAsync();
            if (me == null) return;

            // данные подопечного (без заглушек)
            var name = await DisplayPromptAsync("Получатель", "ФИО подопечного", "OK", "Отмена");
            if (string.IsNullOrWhiteSpace(name)) return;

            var phone = await DisplayPromptAsync("Получатель", "Телефон", "OK", "Отмена", keyboard: Keyboard.Telephone);
            if (string.IsNullOrWhiteSpace(phone)) return;

            var address = await DisplayPromptAsync("Получатель", "Адрес", "OK", "Отмена");
            if (string.IsNullOrWhiteSpace(address)) return;

            var kgStr = await DisplayPromptAsync("Бронь", "Количество кг", "OK", "Отмена", keyboard: Keyboard.Numeric);
            if (!double.TryParse(kgStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var kg))
                kg = 0;

            await DbService.I.ReserveFoodAsync(
                foodId: foodId,
                bookerUserId: me.Id,
                createdByNgo: true,
                kg: kg,
                recipientName: name,
                recipientPhone: phone,
                recipientAddress: address
            );

            await DisplayAlert("Готово", "Еда забронирована для подопечного", "OK");
            await LoadAsync();
        }
    }
}
