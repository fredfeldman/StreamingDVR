namespace StreamingDVR.Models
{
    public class ChannelEpgMapping
    {
        public int StreamId { get; set; }
        public string ChannelName { get; set; } = string.Empty;
        public Guid? EpgSourceId { get; set; }
        public string? EpgChannelId { get; set; }
    }
}
