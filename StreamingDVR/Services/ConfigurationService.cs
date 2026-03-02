using System.Text.Json;

namespace StreamingDVR.Services
{
    public class ConfigurationService
    {
        private readonly string _configPath;

        public ConfigurationService()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "IPTV_DVR");
            
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            _configPath = Path.Combine(appDataPath, "config.json");
        }

        public AppConfiguration LoadConfiguration()
        {
            try
            {
                if (File.Exists(_configPath))
                {
                    var json = File.ReadAllText(_configPath);
                    return JsonSerializer.Deserialize<AppConfiguration>(json) ?? new AppConfiguration();
                }
            }
            catch
            {
                // If loading fails, return default config
            }

            return new AppConfiguration();
        }

        public void SaveConfiguration(AppConfiguration config)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(_configPath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save configuration: {ex.Message}");
            }
        }
    }

    public class AppConfiguration
    {
        public string ServerUrl { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RecordingPath { get; set; } = string.Empty;
        public bool RememberCredentials { get; set; } = false;
        public int DefaultRecordingDurationMinutes { get; set; } = 60;
        public bool ShowWelcomeScreen { get; set; } = true;
        public bool FirstRun { get; set; } = true;
    }
}
