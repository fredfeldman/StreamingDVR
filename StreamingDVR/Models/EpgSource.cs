namespace StreamingDVR.Models
{
    public enum EpgSourceType
    {
        Url,
        File
    }

    public class EpgSource
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public EpgSourceType SourceType { get; set; } = EpgSourceType.Url;
        public string Url { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime? LastUpdated { get; set; }
        public string? LastError { get; set; }

        /// <summary>Returns the active URL or file path depending on SourceType.</summary>
        public string Location => SourceType == EpgSourceType.File ? FilePath : Url;

        public override string ToString() => Name;
    }
}
