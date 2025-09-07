namespace Foodshare.Pages.Needy
{
    public partial class NpoDirectoryPage : ContentPage
    {
        public class Row
        {
            public string Name { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Email { get; set; } = "";
            public Command<string> CallCmd { get; set; } = null!;
            public Command<string> MailCmd { get; set; } = null!;
        }

        public NpoDirectoryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var items = new List<Row>
            {
                new Row { Name="БлагФонд Атырау", Phone="+7 701 000 0001", Email="help@atyrau.kz" },
                new Row { Name="Поддержка семьи", Phone="+7 701 000 0002", Email="family@atyrau.kz" }
            };
            foreach (var it in items)
            {
                it.CallCmd = new Command<string>(p => Launcher.OpenAsync(new Uri($"tel:{p}")));
                it.MailCmd = new Command<string>(m => Launcher.OpenAsync(new Uri($"mailto:{m}?subject=Заявка на помощь")));
            }
            List.ItemsSource = items;
        }
    }
}
