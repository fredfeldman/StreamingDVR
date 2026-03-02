using StreamingDVR.Models;
using System.Text.Json;

namespace StreamingDVR.Services
{
    public class RecordingPersistenceService
    {
        private readonly string _dataPath;
        private readonly string _recordingsFile;
        private readonly string _scheduledFile;

        public RecordingPersistenceService()
        {
            _dataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "IPTV_DVR");

            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }

            _recordingsFile = Path.Combine(_dataPath, "recordings.json");
            _scheduledFile = Path.Combine(_dataPath, "scheduled.json");
        }

        public List<Recording> LoadRecordings()
        {
            try
            {
                if (File.Exists(_recordingsFile))
                {
                    var json = File.ReadAllText(_recordingsFile);
                    return JsonSerializer.Deserialize<List<Recording>>(json) ?? new List<Recording>();
                }
            }
            catch
            {
                // Return empty list on error
            }

            return new List<Recording>();
        }

        public void SaveRecordings(List<Recording> recordings)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(recordings, options);
                File.WriteAllText(_recordingsFile, json);
            }
            catch
            {
                // Silently fail
            }
        }

        public List<ScheduledRecording> LoadScheduledRecordings()
        {
            try
            {
                if (File.Exists(_scheduledFile))
                {
                    var json = File.ReadAllText(_scheduledFile);
                    return JsonSerializer.Deserialize<List<ScheduledRecording>>(json) ?? new List<ScheduledRecording>();
                }
            }
            catch
            {
                // Return empty list on error
            }

            return new List<ScheduledRecording>();
        }

        public void SaveScheduledRecordings(List<ScheduledRecording> scheduled)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(scheduled, options);
                File.WriteAllText(_scheduledFile, json);
            }
            catch
            {
                // Silently fail
            }
        }

        public void ExportRecordingsToFile(string filePath, List<Recording> recordings)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(recordings, options);
            File.WriteAllText(filePath, json);
        }

        public List<Recording> ImportRecordingsFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Recording>>(json) ?? new List<Recording>();
        }
    }
}
