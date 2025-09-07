using System.Collections.ObjectModel;
using Foodshare.Models;
using Foodshare.Services;

namespace Foodshare.Pages.Needy
{
    public partial class NeedyHomePage : ContentPage
    {
        private readonly ObservableCollection<Row> _items = new();

        private class Row
        {
            public string Id { get; set; } = "";
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public double Kg { get; set; }
            public Command<string> BookCmd { get; set; } = null!;
        }

        public NeedyHomePage()
        {
            InitializeComponent();
            // кнопки
            var mapBtn   = this.FindByName<Button>("MapBtn");
            var bookBtn  = this.FindByName<Button>("BookingsBtn");
            mapBtn.Clicked += async (_, __) => await Navigation.PushAsync(new Foodshare.Pages.Common.FoodMapPage());
            bookBtn.Clicked += async (_, __) => await Navigation.PushAsync(new NeedyBookingsPage());

            // список
            var list = this.FindByName<CollectionView>("FoodList");
            list.BindingContext = this;
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
                    BookCmd = new Command<string>(async id => await BookAsync(id))
                });
            }
            var list = this.FindByName<CollectionView>("FoodList");
            list.ItemsSource = _items;
        }

        private async Task BookAsync(string foodId)
        {
            var me = await AuthService.I.GetCurrentAsync();
            if (me == null) return;

            var kgStr = await DisplayPromptAsync("Бронь", "Количество кг", "OK", "Отмена", keyboard: Keyboard.Numeric);
            if (!double.TryParse(kgStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var kg))
                kg = 0;

            await DbService.I.ReserveFoodAsync(foodId, me.Id, createdByNgo: false, kg: kg);
            await DisplayAlert("Готово", "Еда забронирована", "OK");
            await LoadAsync();
        }
    }
}
