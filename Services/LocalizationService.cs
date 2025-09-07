using System.Globalization;
using System.Resources;

namespace Foodshare.Services;

public class LocalizationService
{
    ResourceManager? _rm;
    public void Init(string lang)
    {
        var culture = new CultureInfo(lang);
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        _rm = new ResourceManager("Foodshare.Localization.Strings", typeof(LocalizationService).Assembly);
    }
    public string T(string key)
    {
        return _rm?.GetString(key) ?? key;
    }
}
