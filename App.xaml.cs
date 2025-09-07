using Foodshare.Pages.Auth;
using Foodshare.Services;

namespace Foodshare;

public partial class App : Application
{
    public static string CurrentCity = "Атырау";
    public App(LocalizationService loc)
    {
        InitializeComponent();
        loc.Init("ru");
        MainPage = new AppShell();
    }
}
