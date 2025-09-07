using Foodshare.Models;
using Foodshare.Pages.Auth;
using Foodshare.Pages.Common;
using Foodshare.Pages.Needy;
using Foodshare.Pages.Ngo;
using Foodshare.Pages.Restaurant;
using Foodshare.Pages.Volunteer;
using Foodshare.Services;

namespace Foodshare
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            Loaded += async (_, __) => await BuildTabsForCurrentAsync();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));

            Routing.RegisterRoute(nameof(NeedyHomePage), typeof(NeedyHomePage));
            Routing.RegisterRoute(nameof(NeedyBookingsPage), typeof(NeedyBookingsPage));
            Routing.RegisterRoute(nameof(NpoDirectoryPage), typeof(NpoDirectoryPage));
            Routing.RegisterRoute(nameof(NgoHomePage), typeof(NgoHomePage));
            Routing.RegisterRoute(nameof(PeopleListPage), typeof(PeopleListPage));
            Routing.RegisterRoute(nameof(PeopleDetailPage), typeof(PeopleDetailPage));

            Routing.RegisterRoute(nameof(RestaurantHomePage), typeof(RestaurantHomePage));
            Routing.RegisterRoute(nameof(AddFoodPage), typeof(AddFoodPage));
            Routing.RegisterRoute(nameof(MyDistributionsPage), typeof(MyDistributionsPage));
            Routing.RegisterRoute(nameof(ReservedOrdersPage), typeof(ReservedOrdersPage));

            Routing.RegisterRoute(nameof(VolunteerOrdersPage), typeof(VolunteerOrdersPage));
            Routing.RegisterRoute(nameof(DeliveryDetailPage), typeof(DeliveryDetailPage));

            Routing.RegisterRoute(nameof(RatingsPage), typeof(RatingsPage));
            Routing.RegisterRoute(nameof(FoodMapPage), typeof(FoodMapPage));
            Routing.RegisterRoute(nameof(NgoProfilePage), typeof(NgoProfilePage));
            Routing.RegisterRoute(nameof(NeedyProfilePage), typeof(NeedyProfilePage));
            Routing.RegisterRoute(nameof(RestaurantProfilePage), typeof(RestaurantProfilePage));
        }

        public async Task BuildTabsForCurrentAsync()
        {
            var me = await AuthService.I.GetCurrentAsync();
            if (me == null)
            {
                await GoToAsync($"//{nameof(LoginPage)}");
                return;
            }
            await BuildTabsForAsync(me.Role);
        }

        public Task BuildTabsForAsync(UserRole role)
        {
            Items.Clear();

            var tabs = new TabBar { Route = "home" };

            void AddTab(string title, string route, Page page)
            {
                var shellContent = new ShellContent
                {
                    Title = title,
                    Route = route,
                    ContentTemplate = new DataTemplate(() => page)
                };
                tabs.Items.Add(shellContent);
            }

            switch (role)
            {
                case UserRole.Needy:
                    AddTab("Главная", "needy-home", new NeedyHomePage());
                    AddTab("Карта", "map", new FoodMapPage());
                    AddTab("НПО", "npo-dir", new NpoDirectoryPage());
                    AddTab("Брони", "bookings", new NeedyBookingsPage());
                    AddTab("Рейтинг", "ratings", new RatingsPage());
                    AddTab("Профиль", "profile", new NeedyProfilePage());
                    break;

                case UserRole.Ngo:
                    AddTab("Главная", "ngo-home", new NgoHomePage());
                    AddTab("Карта", "map", new FoodMapPage());
                    AddTab("Нуждающиеся", "people", new PeopleListPage());
                    AddTab("Брони", "bookings", new NeedyBookingsPage());
                    AddTab("Рейтинг", "ratings", new RatingsPage());
                    AddTab("Профиль", "profile", new NgoProfilePage());
                    break;

                case UserRole.Restaurant:
                    AddTab("Домой", "rest-home", new RestaurantHomePage());
                    AddTab("Добавить", "add-food", new AddFoodPage());
                    AddTab("Забронир.", "reserved", new ReservedOrdersPage());
                    AddTab("История", "history", new MyDistributionsPage());
                    AddTab("Рейтинг", "ratings", new RatingsPage());
                    AddTab("Профиль", "profile", new RestaurantProfilePage());
                    break;

                case UserRole.Volunteer:
                    AddTab("Заказы", "vol-orders", new VolunteerOrdersPage());
                    AddTab("Рейтинг", "ratings", new RatingsPage());
                    AddTab("Профиль", "profile", new NeedyProfilePage());
                    break;
            }

            Items.Add(tabs);
            CurrentItem = tabs;
            return Task.CompletedTask;
        }
    }
}
