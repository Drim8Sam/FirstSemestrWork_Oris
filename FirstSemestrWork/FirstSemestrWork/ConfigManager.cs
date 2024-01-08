using FirstSemestrWork.Configuration;

namespace FirstSemestrWork;

public static class ConfigManager
{
    private static AppSettings _config;

    public static AppSettings GetConfig()
    {
        if (_config == null)
            _config = ConfigLoader.LoadConfig(Path.Combine(Directory.GetCurrentDirectory(), "Configuration", "appsettings.json"));

        return _config;
    }
}