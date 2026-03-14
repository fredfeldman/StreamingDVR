using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using StreamingDVR.Models;

namespace StreamingDVR.Services
{
    /// <summary>A channel entry parsed from an XMLTV file or URL.</summary>
    public class XmltvChannel
    {
        public string Id          { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public override string ToString() => string.IsNullOrEmpty(DisplayName) ? Id : $"{DisplayName}  ({Id})";
    }

    public class EpgService
    {
        private readonly HttpClient _httpClient;
        private string _serverUrl = string.Empty;
        private string _username  = string.Empty;
        private string _password  = string.Empty;

        public EpgService()
        {
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        }

        public void SetCredentials(string serverUrl, string username, string password)
        {
            _serverUrl = serverUrl.TrimEnd('/');
            _username  = username;
            _password  = password;
        }

        /// <summary>
        /// Loads the channel list from an XMLTV EPG source (URL or local file).
        /// Supports plain XML and gzip-compressed XML.
        /// </summary>
        public async Task<(List<XmltvChannel> Channels, string? Error)> GetChannelsFromSourceAsync(EpgSource source)
        {
            try
            {
                Stream xmlStream;

                if (source.SourceType == EpgSourceType.File)
                {
                    if (!File.Exists(source.FilePath))
                        return (new(), $"File not found: {source.FilePath}");

                    xmlStream = OpenPossiblyGzipped(File.OpenRead(source.FilePath), source.FilePath);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(source.Url))
                        return (new(), "No URL configured for this source.");

                    var response = await _httpClient.GetAsync(source.Url, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    var raw = await response.Content.ReadAsStreamAsync();
                    xmlStream = OpenPossiblyGzipped(raw, source.Url);
                }

                var channels = ParseXmltvChannels(xmlStream);
                return (channels, null);
            }
            catch (Exception ex)
            {
                return (new(), ex.Message);
            }
        }

        private static Stream OpenPossiblyGzipped(Stream raw, string pathOrUrl)
        {
            string lower = pathOrUrl.ToLowerInvariant();
            if (lower.EndsWith(".gz") || lower.EndsWith(".xml.gz"))
                return new GZipStream(raw, CompressionMode.Decompress);
            return raw;
        }

        private static List<XmltvChannel> ParseXmltvChannels(Stream stream)
        {
            var channels = new List<XmltvChannel>();

            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                IgnoreWhitespace = true,
                Async = true
            };

            using var reader = XmlReader.Create(stream, settings);
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element || reader.Name != "channel")
                    continue;

                string id = reader.GetAttribute("id") ?? string.Empty;
                string displayName = string.Empty;

                using var subtree = reader.ReadSubtree();
                while (subtree.Read())
                {
                    if (subtree.NodeType == XmlNodeType.Element && subtree.Name == "display-name")
                    {
                        displayName = subtree.ReadElementContentAsString();
                        break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(id))
                    channels.Add(new XmltvChannel { Id = id, DisplayName = displayName });
            }

            return channels.OrderBy(c => c.DisplayName).ToList();
        }

        // ── XMLTV programme cache ─────────────────────────────────────────────
        // Key: EpgSource.Id  →  channelId  →  list of programmes
        private readonly Dictionary<Guid, Dictionary<string, List<EpgListing>>> _xmltvCache = new();

        // ── disk cache settings ───────────────────────────────────────────────
        private static readonly TimeSpan UrlCacheTtl = TimeSpan.FromHours(24);

        private static string CacheDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "IPTV_DVR", "epg_cache");

        private static string GetCachePath(Guid sourceId)
            => Path.Combine(CacheDirectory, $"{sourceId}.json.gz");

        private static readonly JsonSerializerOptions _cacheJsonOptions = new()
        {
            WriteIndented            = false,
            DefaultIgnoreCondition   = JsonIgnoreCondition.WhenWritingNull
        };

        // ── public API ────────────────────────────────────────────────────────

        /// <summary>
        /// Loads all programmes from an XMLTV source.
        /// Returns from disk cache if data is still fresh; otherwise parses the
        /// source and writes a new cache file.
        /// </summary>
        /// <param name="forceRefresh">Bypass disk cache and re-download/re-parse.</param>
        /// <returns>(Error message or null, true if served from disk cache)</returns>
        public async Task<(string? Error, bool FromCache)> LoadXmltvSourceAsync(
            EpgSource source, bool forceRefresh = false)
        {
            if (!forceRefresh && await TryLoadFromDiskCacheAsync(source))
                return (null, true);

            try
            {
                Stream xmlStream;
                if (source.SourceType == EpgSourceType.File)
                {
                    if (!File.Exists(source.FilePath))
                        return ($"File not found: {source.FilePath}", false);
                    xmlStream = OpenPossiblyGzipped(File.OpenRead(source.FilePath), source.FilePath);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(source.Url))
                        return ("No URL configured for this source.", false);
                    var response = await _httpClient.GetAsync(source.Url, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    var raw = await response.Content.ReadAsStreamAsync();
                    xmlStream = OpenPossiblyGzipped(raw, source.Url);
                }

                var byChannel = await ParseXmltvProgrammesAsync(xmlStream);
                _xmltvCache[source.Id] = byChannel;

                // Write to disk cache in the background — don't block the caller.
                _ = SaveToDiskCacheAsync(source.Id);

                return (null, false);
            }
            catch (Exception ex)
            {
                return (ex.Message, false);
            }
        }

        /// <summary>Returns true if this source has been loaded into the in-memory cache.</summary>
        public bool IsSourceCached(Guid sourceId) => _xmltvCache.ContainsKey(sourceId);

        /// <summary>
        /// Removes the in-memory and disk cache for a source, forcing a full
        /// re-download on the next call to LoadXmltvSourceAsync.
        /// </summary>
        public void InvalidateCache(Guid sourceId)
        {
            _xmltvCache.Remove(sourceId);
            try { File.Delete(GetCachePath(sourceId)); } catch { }
        }

        /// <summary>
        /// Returns the disk-cache write time and programme count for a source,
        /// or null if no disk cache exists.
        /// </summary>
        public (DateTime CachedAt, int TotalProgrammes)? GetCacheInfo(Guid sourceId)
        {
            string path = GetCachePath(sourceId);
            if (!File.Exists(path)) return null;

            int programmes = _xmltvCache.TryGetValue(sourceId, out var d)
                ? d.Values.Sum(l => l.Count)
                : 0;

            return (File.GetLastWriteTime(path), programmes);
        }

        // ── disk cache helpers ────────────────────────────────────────────────

        private async Task<bool> TryLoadFromDiskCacheAsync(EpgSource source)
        {
            try
            {
                string path = GetCachePath(source.Id);
                if (!File.Exists(path)) return false;

                if (source.SourceType == EpgSourceType.Url)
                {
                    // Expire URL caches after the configured TTL
                    var age = DateTime.UtcNow - File.GetLastWriteTimeUtc(path);
                    if (age > UrlCacheTtl) return false;
                }
                else
                {
                    // Expire file caches when the source file is newer than the cache
                    if (!File.Exists(source.FilePath)) return false;
                    if (File.GetLastWriteTimeUtc(source.FilePath) > File.GetLastWriteTimeUtc(path))
                        return false;
                }

                using var fs   = File.OpenRead(path);
                using var gz   = new GZipStream(fs, CompressionMode.Decompress);
                var cached = await JsonSerializer.DeserializeAsync<
                    Dictionary<string, List<EpgListing>>>(gz, _cacheJsonOptions);

                if (cached == null) return false;
                _xmltvCache[source.Id] = cached;
                return true;
            }
            catch
            {
                return false;   // corrupt / unreadable cache → fall through to fresh parse
            }
        }

        private async Task SaveToDiskCacheAsync(Guid sourceId)
        {
            if (!_xmltvCache.TryGetValue(sourceId, out var data)) return;
            try
            {
                Directory.CreateDirectory(CacheDirectory);
                string path    = GetCachePath(sourceId);
                string tmpPath = path + ".tmp";

                using (var fs = File.Create(tmpPath))
                using (var gz = new GZipStream(fs, CompressionLevel.Optimal))
                    await JsonSerializer.SerializeAsync(gz, data, _cacheJsonOptions);

                File.Move(tmpPath, path, overwrite: true);
            }
            catch { /* saving failed — app still works without disk cache */ }
        }


        public List<EpgListing> GetProgrammesFromCache(Guid sourceId, string channelId,
                                                        DateTime? from = null, DateTime? to = null)
        {
            if (!_xmltvCache.TryGetValue(sourceId, out var byChannel))
                return new();
            if (!byChannel.TryGetValue(channelId, out var programmes))
                return new();

            var result = programmes.AsEnumerable();
            if (from.HasValue)  result = result.Where(p => p.EndTime   >= from.Value);
            if (to.HasValue)    result = result.Where(p => p.StartTime <= to.Value);
            return result.OrderBy(p => p.StartTime).ToList();
        }

        /// <summary>Returns all channel IDs present in a cached XMLTV source.</summary>
        public IReadOnlyCollection<string> GetCachedChannelIds(Guid sourceId) =>
            _xmltvCache.TryGetValue(sourceId, out var d) ? d.Keys : Array.Empty<string>();

        private static async Task<Dictionary<string, List<EpgListing>>> ParseXmltvProgrammesAsync(Stream stream)
        {
            var result   = new Dictionary<string, List<EpgListing>>(StringComparer.OrdinalIgnoreCase);
            var settings = new XmlReaderSettings
            {
                DtdProcessing    = DtdProcessing.Ignore,
                IgnoreWhitespace = true,
                Async            = true
            };

            using var reader = XmlReader.Create(stream, settings);
            while (await reader.ReadAsync())
            {
                if (reader.NodeType != XmlNodeType.Element || reader.Name != "programme")
                    continue;

                string channelId = reader.GetAttribute("channel") ?? string.Empty;
                string startRaw  = reader.GetAttribute("start")   ?? string.Empty;
                string stopRaw   = reader.GetAttribute("stop")    ?? string.Empty;

                if (string.IsNullOrEmpty(channelId)) continue;

                string title = string.Empty, desc = string.Empty;

                using var sub = reader.ReadSubtree();
                while (await sub.ReadAsync())
                {
                    if (sub.NodeType != XmlNodeType.Element) continue;
                    if (sub.Name == "title" && string.IsNullOrEmpty(title))
                        title = await sub.ReadElementContentAsStringAsync();
                    else if (sub.Name == "desc" && string.IsNullOrEmpty(desc))
                        desc = await sub.ReadElementContentAsStringAsync();
                }

                var start = ParseXmltvTime(startRaw);
                var stop  = ParseXmltvTime(stopRaw);
                if (start == null || stop == null) continue;

                var listing = new EpgListing
                {
                    Title       = title,
                    Description = desc,
                    ChannelId   = channelId,
                    // Store as Unix seconds strings so the existing StartTime/EndTime properties work
                    Start = new DateTimeOffset(start.Value).ToUnixTimeSeconds().ToString(),
                    End   = new DateTimeOffset(stop.Value).ToUnixTimeSeconds().ToString()
                };

                if (!result.TryGetValue(channelId, out var list))
                    result[channelId] = list = new List<EpgListing>();
                list.Add(listing);
            }

            return result;
        }

        private static readonly string[] XmltvTimeFormats =
        {
            "yyyyMMddHHmmss zzz",
            "yyyyMMddHHmmss zzzz",
            "yyyyMMddHHmmss",
            "yyyyMMddHHmmss +0000",
        };

        private static DateTime? ParseXmltvTime(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            // Normalise timezone: "+0000" → "+00:00"
            var s = System.Text.RegularExpressions.Regex.Replace(
                raw.Trim(), @"([+-])(\d{2})(\d{2})$", "$1$2:$3");
            if (DateTimeOffset.TryParse(s, out var dto))
                return dto.LocalDateTime;
            if (DateTime.TryParseExact(raw.Trim().Split(' ')[0], "yyyyMMddHHmmss",
                    null, System.Globalization.DateTimeStyles.AssumeUniversal, out var dt))
                return dt.ToLocalTime();
            return null;
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
