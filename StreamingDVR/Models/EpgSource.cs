namespace StreamingDVR.Models
{
    public class EpgSource
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime? LastUpdated { get; set; }
        public string? LastError { get; set; }
        
        public override string ToString() => Name;
    }
}
