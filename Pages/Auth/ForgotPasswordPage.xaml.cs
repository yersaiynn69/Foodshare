namespace Foodshare.Pages.Auth;

public partial class ForgotPasswordPage : ContentPage
{
    public ForgotPasswordPage(){ InitializeComponent(); }
    async void Reset_Clicked(object s, EventArgs e)
    {
        await DisplayAlert("Готово","Если email существует, инструкция отправлена.","OK");
        await Navigation.PopAsync();
    }
}
