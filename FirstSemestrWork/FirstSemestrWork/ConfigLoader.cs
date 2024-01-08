using System.Text.Json;
using FirstSemestrWork.Configuration;
namespace FirstSemestrWork;

public static class ConfigLoader
{
    public static AppSettings LoadConfig(string filePath)
    {
        try
        {
            using (var file = File.OpenRead(filePath))
            {
                var config = JsonSerializer.Deserialize<AppSettings>(file) ?? throw new Exception("Failed to deserialize configuration.");
                CreateFoldersAlongTheWay(config.StaticFilesPath);
                return config;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading configuration: {ex.Message}");
            throw;
        }
    }

    private static void CreateFoldersAlongTheWay(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}