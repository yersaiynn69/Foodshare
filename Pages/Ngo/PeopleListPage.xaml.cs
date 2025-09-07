using System.Collections.ObjectModel;
using Foodshare.Models;
using Foodshare.Services;

namespace Foodshare.Pages.Ngo
{
    public partial class PeopleListPage : ContentPage
    {
        public ObservableCollection<Row> Items { get; } = new();

        public class Row
        {
            public string Id { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public Command<string> OpenCmd { get; set; } = null!;
        }

        public PeopleListPage()
        {
            InitializeComponent();
            List.BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Items.Clear();
            var people = await DbService.I.GetPeopleAsync();
            foreach (var p in people)
            {
                Items.Add(new Row
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    Address = p.Address,
                    OpenCmd = new Command<string>(async id => await Navigation.PushAsync(new PeopleDetailPage(id)))
                });
            }
            List.ItemsSource = Items;
        }
    }
}
