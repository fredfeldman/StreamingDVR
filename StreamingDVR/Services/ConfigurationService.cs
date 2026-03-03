using System.Text.Json;
using StreamingDVR.Models;

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
                    var config = JsonSerializer.Deserialize<AppConfiguration>(json) ?? new AppConfiguration();

                    // Migrate old config to new format
                    if (!string.IsNullOrEmpty(config.ServerUrl) && (config.IptvSources == null || config.IptvSources.Count == 0))
                    {
                        config.IptvSources = new List<IptvSource>
                        {
                            new IptvSource
                            {
                                Name = "Default Xtream Codes",
                                Type = SourceType.XtreamCodes,
                                ServerUrl = config.ServerUrl,
                                Username = config.Username,
                                Password = config.Password,
                                IsActive = true
                            }
                        };
                    }

                    return config;
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
        // Legacy single-source properties (for backwards compatibility)
        public string ServerUrl { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // New multi-source support
        public List<IptvSource> IptvSources { get; set; } = new();

        // EPG management
        public List<EpgSource> EpgSources { get; set; } = new();
        public List<ChannelEpgMapping> ChannelEpgMappings { get; set; } = new();

        // Recording settings
        public string RecordingPath { get; set; } = string.Empty;
        public bool RememberCredentials { get; set; } = false;
        public int DefaultRecordingDurationMinutes { get; set; } = 60;

        // Streamlink settings
        public bool UseStreamlink { get; set; } = false;
        public string StreamlinkQuality { get; set; } = "best";
        public bool StreamlinkRetryOpen { get; set; } = true;
        public int StreamlinkRetryStreams { get; set; } = 3;
        public string StreamlinkOptions { get; set; } = string.Empty;

        // UI settings
        public bool ShowWelcomeScreen { get; set; } = true;
        public bool FirstRun { get; set; } = true;
    }
}
