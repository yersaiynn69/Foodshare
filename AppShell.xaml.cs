using Foodshare.Pages.Auth;
using Foodshare.Pages.Needy;
using Foodshare.Pages.Ngo;
using Foodshare.Pages.Restaurant;
using Foodshare.Pages.Volunteer;

namespace Foodshare;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(RegisterRolePage), typeof(RegisterRolePage));
        Routing.RegisterRoute(nameof(RegisterNeedyPage), typeof(RegisterNeedyPage));
        Routing.RegisterRoute(nameof(RegisterRestaurantPage), typeof(RegisterRestaurantPage));
        Routing.RegisterRoute(nameof(RegisterNgoPage), typeof(RegisterNgoPage));
        Routing.RegisterRoute(nameof(RegisterVolunteerPage), typeof(RegisterVolunteerPage));
        Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));

        Routing.RegisterRoute(nameof(NeedyHomePage), typeof(NeedyHomePage));
        Routing.RegisterRoute(nameof(FoodMapPage), typeof(FoodMapPage));
        Routing.RegisterRoute(nameof(NpoDirectoryPage), typeof(NpoDirectoryPage));
        Routing.RegisterRoute(nameof(NeedyBookingsPage), typeof(NeedyBookingsPage));
        Routing.RegisterRoute(nameof(NeedyProfilePage), typeof(NeedyProfilePage));

        Routing.RegisterRoute(nameof(NgoHomePage), typeof(NgoHomePage));
        Routing.RegisterRoute(nameof(NeedyPeopleListPage), typeof(NeedyPeopleListPage));
        Routing.RegisterRoute(nameof(NgoProfilePage), typeof(NgoProfilePage));

        Routing.RegisterRoute(nameof(RestaurantHomePage), typeof(RestaurantHomePage));
        Routing.RegisterRoute(nameof(AddFoodPage), typeof(AddFoodPage));
        Routing.RegisterRoute(nameof(MyDistributionsPage), typeof(MyDistributionsPage));
        Routing.RegisterRoute(nameof(ReservedOrdersPage), typeof(ReservedOrdersPage));
        Routing.RegisterRoute(nameof(RestaurantProfilePage), typeof(RestaurantProfilePage));

        Routing.RegisterRoute(nameof(VolunteerOrdersPage), typeof(VolunteerOrdersPage));
        Routing.RegisterRoute(nameof(DeliveryDetailPage), typeof(DeliveryDetailPage));
        Routing.RegisterRoute(nameof(VolunteerProfilePage), typeof(VolunteerProfilePage));

        Routing.RegisterRoute(nameof(RatingsPage), typeof(RatingsPage));
    }
}
