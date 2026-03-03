using System.Net.Http;
using System.Text.Json;
using System.Xml.Linq;
using StreamingDVR.Models;

namespace StreamingDVR.Services
{
    public class Enigma2Service
    {
        private string _baseUrl = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private HttpClient _httpClient;
        private bool _isAuthenticated = false;

        public bool IsAuthenticated => _isAuthenticated;
        public string BaseUrl => _baseUrl;

        public Enigma2Service()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public async Task<bool> AuthenticateAsync(string serverUrl, string username, string password, int? port = null)
        {
            try
            {
                // Clean up URL
                _baseUrl = serverUrl.TrimEnd('/');
                if (!_baseUrl.StartsWith("http://") && !_baseUrl.StartsWith("https://"))
                {
                    _baseUrl = "http://" + _baseUrl;
                }

                // Add port if specified
                if (port.HasValue && port.Value > 0)
                {
                    var uri = new Uri(_baseUrl);
                    _baseUrl = $"{uri.Scheme}://{uri.Host}:{port.Value}";
                }

                _username = username;
                _password = password;

                // Set up authentication (optional for some Enigma2 boxes)
                _httpClient.DefaultRequestHeaders.Authorization = null;
                if (!string.IsNullOrEmpty(_username))
                {
                    var authValue = Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes($"{_username}:{_password ?? string.Empty}"));
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authValue);
                    System.Diagnostics.Debug.WriteLine("[Enigma2Service] Using authentication");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[Enigma2Service] No authentication (anonymous access)");
                }

                // Test connection by getting box info
                var aboutUrl = $"{_baseUrl}/web/about";
                System.Diagnostics.Debug.WriteLine($"[Enigma2Service] Testing connection to: {aboutUrl}");

                var response = await _httpClient.GetAsync(aboutUrl);

                System.Diagnostics.Debug.WriteLine($"[Enigma2Service] HTTP Status: {(int)response.StatusCode} {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"[Enigma2Service] Response length: {content.Length} bytes");

                    // Verify it's actually Enigma2 by checking for expected XML structure
                    if (content.Contains("<e2about>") || content.Contains("<e2enigmaversion>"))
                    {
                        System.Diagnostics.Debug.WriteLine("[Enigma2Service] Valid Enigma2 response detected");
                        _isAuthenticated = true;
                        return true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("[Enigma2Service] Response doesn't contain Enigma2 XML markers");
                        System.Diagnostics.Debug.WriteLine($"[Enigma2Service] First 500 chars: {content.Substring(0, Math.Min(500, content.Length))}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    System.Diagnostics.Debug.WriteLine("[Enigma2Service] Authentication failed - 401 Unauthorized");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[Enigma2Service] Request failed with status: {response.StatusCode}");
                }

                _isAuthenticated = false;
                return false;
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Enigma2Service] HTTP Request failed: {ex.Message}");
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine($"[Enigma2Service] Inner exception: {ex.InnerException.Message}");
                _isAuthenticated = false;
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Enigma2Service] Unexpected error: {ex.GetType().Name} - {ex.Message}");
                _isAuthenticated = false;
                return false;
            }
        }

        public async Task<List<Category>> GetBouquetsAsync()
        {
            if (!_isAuthenticated)
                throw new InvalidOperationException("Not authenticated");

            var categories = new List<Category>();

            try
            {
                var url = $"{_baseUrl}/web/getservices";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var xml = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(xml);

                var services = doc.Descendants("e2service");
                foreach (var service in services)
                {
                    var name = service.Element("e2servicename")?.Value;
                    var reference = service.Element("e2servicereference")?.Value;

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(reference))
                    {
                        categories.Add(new Category
                        {
                            CategoryId = reference,
                            CategoryName = name,
                            ParentId = 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get bouquets: {ex.Message}", ex);
            }

            return categories;
        }

        public async Task<List<LiveChannel>> GetChannelsAsync(string bouquetReference)
        {
            if (!_isAuthenticated)
                throw new InvalidOperationException("Not authenticated");

            var channels = new List<LiveChannel>();

            try
            {
                var encodedRef = Uri.EscapeDataString(bouquetReference);
                var url = $"{_baseUrl}/web/getservices?sRef={encodedRef}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var xml = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(xml);

                var services = doc.Descendants("e2service");
                int num = 1;

                foreach (var service in services)
                {
                    var name = service.Element("e2servicename")?.Value;
                    var reference = service.Element("e2servicereference")?.Value;

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(reference))
                    {
                        // Skip markers (these are separators in the bouquet)
                        if (reference.Contains(":0:0:0:0:0:0:0:0:0:"))
                            continue;

                        channels.Add(new LiveChannel
                        {
                            Num = num++,
                            Name = name,
                            StreamId = reference.GetHashCode(), // Use hash of reference as stream ID
                            CategoryId = bouquetReference,
                            EpgChannelId = reference,
                            StreamType = "enigma2"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get channels: {ex.Message}", ex);
            }

            return channels;
        }

        public async Task<List<LiveChannel>> GetAllChannelsAsync()
        {
            var allChannels = new List<LiveChannel>();
            var bouquets = await GetBouquetsAsync();

            foreach (var bouquet in bouquets)
            {
                try
                {
                    var channels = await GetChannelsAsync(bouquet.CategoryId);
                    allChannels.AddRange(channels);
                }
                catch
                {
                    // Continue with other bouquets if one fails
                }
            }

            return allChannels;
        }

        public string GetStreamUrl(LiveChannel channel)
        {
            if (!_isAuthenticated || string.IsNullOrEmpty(channel.EpgChannelId))
                return string.Empty;

            var encodedRef = Uri.EscapeDataString(channel.EpgChannelId);
            return $"{_baseUrl}/web/stream.m3u?ref={encodedRef}";
        }

        public string GetStreamUrl(string serviceReference)
        {
            if (!_isAuthenticated || string.IsNullOrEmpty(serviceReference))
                return string.Empty;

            var encodedRef = Uri.EscapeDataString(serviceReference);
            return $"{_baseUrl}/web/stream.m3u?ref={encodedRef}";
        }

        public async Task<string> GetBoxInfoAsync()
        {
            if (!_isAuthenticated)
                return "Not connected";

            try
            {
                var url = $"{_baseUrl}/web/about";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var xml = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(xml);

                var model = doc.Descendants("e2model").FirstOrDefault()?.Value ?? "Unknown";
                var version = doc.Descendants("e2enigmaversion").FirstOrDefault()?.Value ?? "Unknown";

                return $"{model} - {version}";
            }
            catch
            {
                return "Connected";
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
