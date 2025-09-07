using System.Collections.ObjectModel;
using Foodshare.Services;

namespace Foodshare.Pages.Common
{
    public partial class RatingsPage : ContentPage
    {
        public ObservableCollection<Row> Items { get; } = new();

        public class Row { public int Pos { get; set; } public string Name { get; set; } = ""; public string Score { get; set; } = ""; }

        public RatingsPage()
        {
            InitializeComponent();
            List.BindingContext = this;
            TypePicker.SelectedIndexChanged += async (_, __) => await LoadAsync();
            TypePicker.SelectedIndex = 0;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            Items.Clear();
            var idx = TypePicker.SelectedIndex;
            var data = idx switch
            {
                0 => (await DbService.I.GetRestaurantRatingAsync()).Select(x => (x.Name, Score: $"{x.Score:0.##} кг")).ToList(),
                1 => (await DbService.I.GetVolunteerRatingAsync()).Select(x => (x.Name, Score: $"{x.Score:0} рейсов")).ToList(),
                _ => (await DbService.I.GetNgoRatingAsync()).Select(x => (x.Name, Score: $"{x.Score:0} откликов")).ToList()
            };
            int pos = 1;
            foreach (var r in data)
                Items.Add(new Row { Pos = pos++, Name = r.Name, Score = r.Score });
            List.ItemsSource = Items;
        }
    }
}
