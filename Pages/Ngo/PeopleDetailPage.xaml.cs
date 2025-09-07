using Foodshare.Services;

namespace Foodshare.Pages.Ngo
{
    public partial class PeopleDetailPage : ContentPage
    {
        private readonly string _id;
        private Foodshare.Models.NeedyPerson? _p;

        public PeopleDetailPage(string id)
        {
            InitializeComponent();
            _id = id;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _p = await DbService.I.GetPersonAsync(_id);
            if (_p == null) { await DisplayAlert("Ошибка", "Профиль не найден", "OK"); await Navigation.PopAsync(); return; }

            Title = _p.FullName;
            NameLbl.Text = _p.FullName;
            AddrLbl.Text = $"Адрес: {_p.Address}";
            PhoneLbl.Text = $"Телефон: {_p.Phone}";
            NotesLbl.Text = _p.Notes;

            CallBtn.IsVisible = !string.IsNullOrWhiteSpace(_p.Phone);
            CallBtn.Clicked += (_, __) => Launcher.OpenAsync(new Uri($"tel:{_p.Phone}"));
        }
    }
}
