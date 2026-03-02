using StreamingDVR.Models;

namespace StreamingDVR.Services
{
    public class RecordingStatistics
    {
        public int TotalRecordings { get; set; }
        public int CompletedRecordings { get; set; }
        public int FailedRecordings { get; set; }
        public int ActiveRecordings { get; set; }
        public long TotalSizeBytes { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public DateTime? OldestRecording { get; set; }
        public DateTime? NewestRecording { get; set; }
        public Recording? LargestRecording { get; set; }
        public Recording? LongestRecording { get; set; }

        public string TotalSizeFormatted
        {
            get
            {
                string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                double len = TotalSizeBytes;
                int order = 0;
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }
                return $"{len:0.##} {sizes[order]}";
            }
        }

        public static RecordingStatistics Calculate(List<Recording> recordings)
        {
            var stats = new RecordingStatistics
            {
                TotalRecordings = recordings.Count,
                CompletedRecordings = recordings.Count(r => r.Status == RecordingStatus.Completed),
                FailedRecordings = recordings.Count(r => r.Status == RecordingStatus.Failed),
                ActiveRecordings = recordings.Count(r => r.Status == RecordingStatus.Recording),
                TotalSizeBytes = recordings.Sum(r => r.FileSize)
            };

            var completedRecordings = recordings.Where(r => r.EndTime.HasValue && r.Status == RecordingStatus.Completed).ToList();

            if (completedRecordings.Any())
            {
                stats.TotalDuration = TimeSpan.FromTicks(
                    completedRecordings.Sum(r => (r.EndTime!.Value - r.StartTime).Ticks));

                stats.OldestRecording = completedRecordings.Min(r => r.StartTime);
                stats.NewestRecording = completedRecordings.Max(r => r.StartTime);
                stats.LargestRecording = completedRecordings.OrderByDescending(r => r.FileSize).FirstOrDefault();
                stats.LongestRecording = completedRecordings.OrderByDescending(r => (r.EndTime!.Value - r.StartTime).Ticks).FirstOrDefault();
            }

            return stats;
        }
    }
}
