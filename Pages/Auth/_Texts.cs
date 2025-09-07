namespace Foodshare.Pages.Auth;
public static class Texts
{
    public static string AppTitle => Loc("AppTitle");
    public static string Email => Loc("Email");
    public static string Password => Loc("Password");
    public static string City => Loc("City");
    public static string Login => Loc("Login");
    public static string NoAccount => Loc("NoAccount");
    public static string Register => Loc("Register");
    public static string ForgotPassword => Loc("ForgotPassword");
    public static string Language => Loc("Language");
    static string Loc(string k) => Application.Current.Services.GetService<Foodshare.Services.LocalizationService>()!.T(k);
}
