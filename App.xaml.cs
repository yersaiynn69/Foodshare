using Foodshare.Services;

namespace Foodshare
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "foodshare.db3");
            await DbService.I.InitAsync(dbPath);
            await LocalizationService.I.InitAsync();
        }
    }
}
