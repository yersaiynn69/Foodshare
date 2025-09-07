using Foodshare.Models;
using Foodshare.Services;

namespace Foodshare.Pages.Volunteer
{
    public partial class DeliveryDetailPage : ContentPage
    {
        private readonly string _bookingId;
        private Booking? _booking;
        private FoodItem? _food;
        private User? _rest;

        public DeliveryDetailPage(string bookingId)
        {
            InitializeComponent();
            _bookingId = bookingId;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _booking = await DbService.I.Conn.FindAsync<Booking>(_bookingId);
            if (_booking == null) { await DisplayAlert("Ошибка", "Бронирование не найдено", "OK"); await Navigation.PopAsync(); return; }

            _food = await DbService.I.Conn.FindAsync<FoodItem>(_booking.FoodItemId);
            _rest = await DbService.I.Conn.FindAsync<User>(_booking.RestaurantUserId);

            TitleLbl.Text = _food?.Title ?? "Доставка";
            FoodLbl.Text  = $"Еда: {_food?.Title} • {_food?.Kg} кг";
            RestLbl.Text  = $"Ресторан: {_rest?.OrgName ?? _rest?.FullName}";
            RecipLbl.Text = string.IsNullOrWhiteSpace(_booking.RecipientAddress)
                ? "Получатель: самовывоз"
                : $"Получатель: {_booking.RecipientName}, {_booking.RecipientPhone}, {_booking.RecipientAddress}";

            CallBtn.IsVisible = !string.IsNullOrWhiteSpace(_booking.RecipientPhone);
            CallBtn.Clicked += (_, __) => Launcher.OpenAsync(new Uri($"tel:{_booking!.RecipientPhone}"));

            DoneBtn.Clicked += async (_, __) =>
            {
                await DbService.I.CompleteDeliveryAsync(_bookingId);
                await DisplayAlert("Готово", "Доставка завершена", "OK");
                await Navigation.PopAsync();
            };
        }
    }
}
