using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamingDVR.Services
{
    public class EpgService
    {
        private readonly HttpClient _httpClient;
        private string _serverUrl = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;

        public EpgService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public void SetCredentials(string serverUrl, string username, string password)
        {
            _serverUrl = serverUrl.TrimEnd('/');
            _username = username;
            _password = password;
        }

        public async Task<List<EpgListing>> GetEpgForChannelAsync(string epgChannelId, int? limit = null)
        {
            try
            {
                var url = $"{_serverUrl}/player_api.php?username={_username}&password={_password}&action=get_short_epg&stream_id={epgChannelId}";
                if (limit.HasValue)
                {
                    url += $"&limit={limit}";
                }

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<EpgResponse>(content);

                return result?.EpgListings ?? new List<EpgListing>();
            }
            catch
            {
                return new List<EpgListing>();
            }
        }

        public async Task<List<EpgListing>> GetAllEpgAsync()
        {
            try
            {
                var url = $"{_serverUrl}/player_api.php?username={_username}&password={_password}&action=get_simple_data_table&stream_id=";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Dictionary<string, List<EpgListing>>>(content);

                return result?.Values.SelectMany(x => x).ToList() ?? new List<EpgListing>();
            }
            catch
            {
                return new List<EpgListing>();
            }
        }
    }

    public class EpgResponse
    {
        [JsonPropertyName("epg_listings")]
        public List<EpgListing> EpgListings { get; set; } = new();
    }

    public class EpgListing
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("epg_id")]
        public string EpgId { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("lang")]
        public string? Lang { get; set; }

        [JsonPropertyName("start")]
        public string Start { get; set; } = string.Empty;

        [JsonPropertyName("end")]
        public string End { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("channel_id")]
        public string? ChannelId { get; set; }

        public DateTime StartTime => DateTimeOffset.FromUnixTimeSeconds(long.Parse(Start)).LocalDateTime;
        public DateTime EndTime => DateTimeOffset.FromUnixTimeSeconds(long.Parse(End)).LocalDateTime;
    }
}
