namespace Foodshare.Pages.Auth;

public partial class RegisterRolePage : ContentPage
{
    public RegisterRolePage(){ InitializeComponent(); }
    async void Needy_Clicked(object s, EventArgs e)=> await Navigation.PushAsync(new RegisterNeedyPage());
    async void Rest_Clicked(object s, EventArgs e)=> await Navigation.PushAsync(new RegisterRestaurantPage());
    async void Ngo_Clicked(object s, EventArgs e)=> await Navigation.PushAsync(new RegisterNgoPage());
    async void Vol_Clicked(object s, EventArgs e)=> await Navigation.PushAsync(new RegisterVolunteerPage());
}
