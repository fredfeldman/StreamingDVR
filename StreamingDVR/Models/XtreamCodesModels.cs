using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamingDVR.Models
{
    /// <summary>
    /// Handles Xtream Codes servers that return numeric fields as either strings or numbers.
    /// e.g. "port": 9692  vs  "port": "9692"
    /// </summary>
    public class StringOrNumberConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.GetInt64().ToString(),
                JsonTokenType.True   => "true",
                JsonTokenType.False  => "false",
                JsonTokenType.Null   => null,
                _ => reader.GetString()
            };
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value);
        }
    }

    /// <summary>
    /// Handles Xtream Codes servers that return numeric timestamp fields as either
    /// a JSON number or a JSON string. e.g. 1234567890 vs "1234567890"
    /// </summary>
    public class NumberOrStringToLongConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetInt64(),
                JsonTokenType.String => long.TryParse(reader.GetString(), out var val) ? val : 0,
                _ => 0
            };
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value);
    }

    /// <summary>
    /// Handles Xtream Codes servers that return integer fields as either
    /// a JSON number or a JSON string. e.g. 1 vs "1"
    /// </summary>
    public class NumberOrStringToIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetInt32(),
                JsonTokenType.String => int.TryParse(reader.GetString(), out var val) ? val : 0,
                _ => 0
            };
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value);
    }

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
        [JsonConverter(typeof(NumberOrStringToIntConverter))]
        public int Auth { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("exp_date")]
        public string? ExpDate { get; set; }

        [JsonPropertyName("is_trial")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string? IsTrial { get; set; }

        [JsonPropertyName("active_cons")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string? ActiveConnections { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("max_connections")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string? MaxConnections { get; set; }
    }

    public class ServerInfo
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("port")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string Port { get; set; } = string.Empty;

        [JsonPropertyName("https_port")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string? HttpsPort { get; set; }

        [JsonPropertyName("server_protocol")]
        public string ServerProtocol { get; set; } = string.Empty;

        [JsonPropertyName("rtmp_port")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string? RtmpPort { get; set; }

        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }

        [JsonPropertyName("timestamp_now")]
        [JsonConverter(typeof(NumberOrStringToLongConverter))]
        public long TimestampNow { get; set; }

        [JsonPropertyName("time_now")]
        public string? TimeNow { get; set; }
    }

    public class LiveChannel
    {
        [JsonPropertyName("num")]
        [JsonConverter(typeof(NumberOrStringToIntConverter))]
        public int Num { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("stream_type")]
        public string StreamType { get; set; } = string.Empty;

        [JsonPropertyName("stream_id")]
        [JsonConverter(typeof(NumberOrStringToIntConverter))]
        public int StreamId { get; set; }

        [JsonPropertyName("stream_icon")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string? StreamIcon { get; set; }

        [JsonPropertyName("epg_channel_id")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string? EpgChannelId { get; set; }

        [JsonPropertyName("added")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string? Added { get; set; }

        [JsonPropertyName("category_id")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string? CategoryId { get; set; }

        [JsonPropertyName("custom_sid")]
        [JsonConverter(typeof(StringOrNumberConverter))]
        public string? CustomSid { get; set; }

        [JsonPropertyName("tv_archive")]
        [JsonConverter(typeof(NumberOrStringToIntConverter))]
        public int TvArchive { get; set; }

        [JsonPropertyName("direct_source")]
        public string? DirectSource { get; set; }

        [JsonPropertyName("tv_archive_duration")]
        [JsonConverter(typeof(NumberOrStringToIntConverter))]
        public int TvArchiveDuration { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("category_id")]
        public string CategoryId { get; set; } = string.Empty;

        [JsonPropertyName("category_name")]
        public string CategoryName { get; set; } = string.Empty;

        [JsonPropertyName("parent_id")]
        [JsonConverter(typeof(NumberOrStringToIntConverter))]
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
