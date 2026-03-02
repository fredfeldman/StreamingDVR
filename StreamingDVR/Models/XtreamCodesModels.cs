using System.Text.Json.Serialization;

namespace StreamingDVR.Models
{
    public class XtreamAuthInfo
    {
        [JsonPropertyName("user_info")]
        public UserInfo? UserInfo { get; set; }

        [JsonPropertyName("server_info")]
        public ServerInfo? ServerInfo { get; set; }
    }

    public class UserInfo
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("auth")]
        public int Auth { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("exp_date")]
        public string? ExpDate { get; set; }

        [JsonPropertyName("is_trial")]
        public string? IsTrial { get; set; }

        [JsonPropertyName("active_cons")]
        public string? ActiveConnections { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("max_connections")]
        public string? MaxConnections { get; set; }
    }

    public class ServerInfo
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("port")]
        public string Port { get; set; } = string.Empty;

        [JsonPropertyName("https_port")]
        public string? HttpsPort { get; set; }

        [JsonPropertyName("server_protocol")]
        public string ServerProtocol { get; set; } = string.Empty;

        [JsonPropertyName("rtmp_port")]
        public string? RtmpPort { get; set; }

        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }

        [JsonPropertyName("timestamp_now")]
        public long TimestampNow { get; set; }

        [JsonPropertyName("time_now")]
        public string? TimeNow { get; set; }
    }

    public class LiveChannel
    {
        [JsonPropertyName("num")]
        public int Num { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("stream_type")]
        public string StreamType { get; set; } = string.Empty;

        [JsonPropertyName("stream_id")]
        public int StreamId { get; set; }

        [JsonPropertyName("stream_icon")]
        public string? StreamIcon { get; set; }

        [JsonPropertyName("epg_channel_id")]
        public string? EpgChannelId { get; set; }

        [JsonPropertyName("added")]
        public string? Added { get; set; }

        [JsonPropertyName("category_id")]
        public string? CategoryId { get; set; }

        [JsonPropertyName("custom_sid")]
        public string? CustomSid { get; set; }

        [JsonPropertyName("tv_archive")]
        public int TvArchive { get; set; }

        [JsonPropertyName("direct_source")]
        public string? DirectSource { get; set; }

        [JsonPropertyName("tv_archive_duration")]
        public int TvArchiveDuration { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("category_id")]
        public string CategoryId { get; set; } = string.Empty;

        [JsonPropertyName("category_name")]
        public string CategoryName { get; set; } = string.Empty;

        [JsonPropertyName("parent_id")]
        public int ParentId { get; set; }
    }

    public class Recording
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ChannelName { get; set; } = string.Empty;
        public int StreamId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public RecordingStatus Status { get; set; }
        public long FileSize { get; set; }
    }

    public enum RecordingStatus
    {
        Scheduled,
        Recording,
        Completed,
        Failed,
        Stopped
    }

    public class ScheduledRecording
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ChannelName { get; set; } = string.Empty;
        public int StreamId { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsRecurring { get; set; }
        public DayOfWeek[]? RecurringDays { get; set; }
    }
}
