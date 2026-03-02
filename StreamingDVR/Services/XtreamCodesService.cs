using System.Text.Json;
using StreamingDVR.Models;

namespace StreamingDVR.Services
{
    public class XtreamCodesService
    {
        private readonly HttpClient _httpClient;
        private string _serverUrl = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private XtreamAuthInfo? _authInfo;

        public XtreamCodesService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public bool IsAuthenticated => _authInfo?.UserInfo?.Auth == 1;

        public async Task<bool> AuthenticateAsync(string serverUrl, string username, string password)
        {
            try
            {
                _serverUrl = serverUrl.TrimEnd('/');
                _username = username;
                _password = password;

                var authUrl = $"{_serverUrl}/player_api.php?username={_username}&password={_password}";
                var response = await _httpClient.GetAsync(authUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _authInfo = JsonSerializer.Deserialize<XtreamAuthInfo>(content);

                return IsAuthenticated;
            }
            catch
            {
                _authInfo = null;
                return false;
            }
        }

        public async Task<List<LiveChannel>> GetLiveChannelsAsync()
        {
            if (!IsAuthenticated)
                throw new InvalidOperationException("Not authenticated");

            try
            {
                var url = $"{_serverUrl}/player_api.php?username={_username}&password={_password}&action=get_live_streams";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var channels = JsonSerializer.Deserialize<List<LiveChannel>>(content);

                return channels ?? new List<LiveChannel>();
            }
            catch
            {
                return new List<LiveChannel>();
            }
        }

        public async Task<List<Category>> GetLiveCategoriesAsync()
        {
            if (!IsAuthenticated)
                throw new InvalidOperationException("Not authenticated");

            try
            {
                var url = $"{_serverUrl}/player_api.php?username={_username}&password={_password}&action=get_live_categories";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<Category>>(content);

                return categories ?? new List<Category>();
            }
            catch
            {
                return new List<Category>();
            }
        }

        public async Task<List<LiveChannel>> GetLiveChannelsByCategoryAsync(string categoryId)
        {
            if (!IsAuthenticated)
                throw new InvalidOperationException("Not authenticated");

            try
            {
                var url = $"{_serverUrl}/player_api.php?username={_username}&password={_password}&action=get_live_streams&category_id={categoryId}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var channels = JsonSerializer.Deserialize<List<LiveChannel>>(content);

                return channels ?? new List<LiveChannel>();
            }
            catch
            {
                return new List<LiveChannel>();
            }
        }

        public string GetStreamUrl(int streamId, string extension = "ts")
        {
            if (!IsAuthenticated)
                throw new InvalidOperationException("Not authenticated");

            return $"{_serverUrl}/live/{_username}/{_password}/{streamId}.{extension}";
        }

        public XtreamAuthInfo? GetAuthInfo() => _authInfo;
    }
}
