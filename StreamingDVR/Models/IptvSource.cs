namespace StreamingDVR.Models
{
    public enum SourceType
    {
        XtreamCodes,
        Enigma2,
        M3U
    }

    public class IptvSource
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public SourceType Type { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Xtream Codes / Enigma2 properties
        public string? ServerUrl { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? Port { get; set; }
        
        // M3U properties
        public string? M3UUrl { get; set; }
        public string? M3UFilePath { get; set; }
        
        // Additional EPG sources
        public List<string> EpgUrls { get; set; } = new();
        
        // Last connection info
        public DateTime? LastConnected { get; set; }
        public string? LastError { get; set; }
    }
}
