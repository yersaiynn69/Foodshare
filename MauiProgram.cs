using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Foodshare.Services;
using Foodshare.Pages.Auth;
using Foodshare.Pages.Needy;
using Foodshare.Pages.Restaurant;
using Foodshare.Pages.Ngo;
using Foodshare.Pages.Volunteer;
using Foodshare.Pages.Common;

namespace Foodshare;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
            });

        builder.Services.AddSingleton<DbService>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<LocalizationService>();

        // Auth
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterRolePage>();
        builder.Services.AddTransient<RegisterNeedyPage>();
        builder.Services.AddTransient<RegisterRestaurantPage>();
        builder.Services.AddTransient<RegisterNgoPage>();
        builder.Services.AddTransient<RegisterVolunteerPage>();
        builder.Services.AddTransient<ForgotPasswordPage>();

        // Needy
        builder.Services.AddTransient<NeedyHomePage>();
        builder.Services.AddTransient<FoodMapPage>();
        builder.Services.AddTransient<NpoDirectoryPage>();
        builder.Services.AddTransient<NeedyBookingsPage>();
        builder.Services.AddTransient<NeedyProfilePage>();

        // NGO
        builder.Services.AddTransient<NgoHomePage>();
        builder.Services.AddTransient<NeedyPeopleListPage>();
        builder.Services.AddTransient<NgoProfilePage>();

        // Restaurant
        builder.Services.AddTransient<RestaurantHomePage>();
        builder.Services.AddTransient<AddFoodPage>();
        builder.Services.AddTransient<MyDistributionsPage>();
        builder.Services.AddTransient<ReservedOrdersPage>();
        builder.Services.AddTransient<RestaurantProfilePage>();

        // Volunteer
        builder.Services.AddTransient<VolunteerOrdersPage>();
        builder.Services.AddTransient<DeliveryDetailPage>();
        builder.Services.AddTransient<VolunteerProfilePage>();

        // Common
        builder.Services.AddTransient<RatingsPage>();

        return builder.Build();
    }
}
