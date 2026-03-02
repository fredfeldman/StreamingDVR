using System.Diagnostics;
using StreamingDVR.Models;

namespace StreamingDVR.Services
{
    public class RecordingService
    {
        private readonly Dictionary<string, Process> _activeRecordings = new();
        private readonly List<Recording> _recordings = new();
        private readonly List<ScheduledRecording> _scheduledRecordings = new();
        private readonly string _recordingsPath;
        private readonly System.Threading.Timer _schedulerTimer;
        private readonly RecordingPersistenceService _persistenceService;
        private Func<int, string, TimeSpan, Task<Recording>>? _scheduledRecordingCallback;

        public event EventHandler<Recording>? RecordingStarted;
        public event EventHandler<Recording>? RecordingStopped;
        public event EventHandler<Recording>? RecordingFailed;
        public event EventHandler<ScheduledRecording>? ScheduledRecordingTriggered;

        public RecordingService(string recordingsPath)
        {
            _recordingsPath = recordingsPath;
            _persistenceService = new RecordingPersistenceService();

            if (!Directory.Exists(_recordingsPath))
            {
                Directory.CreateDirectory(_recordingsPath);
            }

            LoadPersistedData();
            _schedulerTimer = new System.Threading.Timer(CheckScheduledRecordings, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private void LoadPersistedData()
        {
            try
            {
                var savedRecordings = _persistenceService.LoadRecordings();
                foreach (var recording in savedRecordings)
                {
                    if (File.Exists(recording.FilePath))
                    {
                        _recordings.Add(recording);
                    }
                }

                var savedScheduled = _persistenceService.LoadScheduledRecordings();
                foreach (var scheduled in savedScheduled)
                {
                    if (!scheduled.IsRecurring && scheduled.StartTime < DateTime.Now)
                    {
                        continue;
                    }
                    _scheduledRecordings.Add(scheduled);
                }
            }
            catch
            {
                // Continue with empty lists if loading fails
            }
        }

        private void SavePersistedData()
        {
            try
            {
                _persistenceService.SaveRecordings(_recordings);
                _persistenceService.SaveScheduledRecordings(_scheduledRecordings);
            }
            catch
            {
                // Silently fail
            }
        }

        public void SetScheduledRecordingCallback(Func<int, string, TimeSpan, Task<Recording>> callback)
        {
            _scheduledRecordingCallback = callback;
        }

        public async Task<Recording> StartRecordingAsync(string channelName, int streamId, string streamUrl, TimeSpan? duration = null)
        {
            var recording = new Recording
            {
                ChannelName = channelName,
                StreamId = streamId,
                StartTime = DateTime.Now,
                Duration = duration,
                Status = RecordingStatus.Recording
            };

            var sanitizedName = SanitizeFileName(channelName);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"{sanitizedName}_{timestamp}.mp4";
            recording.FilePath = Path.Combine(_recordingsPath, fileName);

            try
            {
                var process = await StartFFmpegRecordingAsync(streamUrl, recording.FilePath, duration);
                _activeRecordings[recording.Id] = process;
                _recordings.Add(recording);

                RecordingStarted?.Invoke(this, recording);

                _ = Task.Run(async () =>
                {
                    await process.WaitForExitAsync();
                    _activeRecordings.Remove(recording.Id);

                    if (File.Exists(recording.FilePath))
                    {
                        recording.FileSize = new FileInfo(recording.FilePath).Length;
                        recording.EndTime = DateTime.Now;
                        recording.Status = RecordingStatus.Completed;
                        RecordingStopped?.Invoke(this, recording);
                    }
                    else
                    {
                        recording.Status = RecordingStatus.Failed;
                        RecordingFailed?.Invoke(this, recording);
                    }
                });

                return recording;
            }
            catch (Exception)
            {
                recording.Status = RecordingStatus.Failed;
                RecordingFailed?.Invoke(this, recording);
                throw;
            }
        }

        public void StopRecording(string recordingId)
        {
            if (_activeRecordings.TryGetValue(recordingId, out var process))
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.StandardInput.WriteLine("q");
                        process.StandardInput.Flush();
                        
                        if (!process.WaitForExit(5000))
                        {
                            process.Kill();
                        }
                    }

                    var recording = _recordings.FirstOrDefault(r => r.Id == recordingId);
                    if (recording != null)
                    {
                        recording.Status = RecordingStatus.Stopped;
                        recording.EndTime = DateTime.Now;
                        if (File.Exists(recording.FilePath))
                        {
                            recording.FileSize = new FileInfo(recording.FilePath).Length;
                        }
                        RecordingStopped?.Invoke(this, recording);
                    }
                }
                catch
                {
                    try
                    {
                        process.Kill();
                    }
                    catch { }
                }
                finally
                {
                    _activeRecordings.Remove(recordingId);
                }
            }
        }

        public void AddScheduledRecording(ScheduledRecording scheduledRecording)
        {
            _scheduledRecordings.Add(scheduledRecording);
            SavePersistedData();
        }

        public void RemoveScheduledRecording(string id)
        {
            var scheduled = _scheduledRecordings.FirstOrDefault(s => s.Id == id);
            if (scheduled != null)
            {
                _scheduledRecordings.Remove(scheduled);
                SavePersistedData();
            }
        }

        public List<Recording> GetRecordings() => new List<Recording>(_recordings);

        public List<ScheduledRecording> GetScheduledRecordings() => new List<ScheduledRecording>(_scheduledRecordings);

        public Dictionary<string, Recording> GetActiveRecordings()
        {
            return _recordings
                .Where(r => r.Status == RecordingStatus.Recording && _activeRecordings.ContainsKey(r.Id))
                .ToDictionary(r => r.Id, r => r);
        }

        public void DeleteRecording(string recordingId)
        {
            var recording = _recordings.FirstOrDefault(r => r.Id == recordingId);
            if (recording != null)
            {
                if (recording.Status == RecordingStatus.Recording)
                {
                    StopRecording(recordingId);
                }

                if (File.Exists(recording.FilePath))
                {
                    try
                    {
                        File.Delete(recording.FilePath);
                    }
                    catch { }
                }

                _recordings.Remove(recording);
            }
        }

        private async Task<Process> StartFFmpegRecordingAsync(string streamUrl, string outputPath, TimeSpan? duration)
        {
            var ffmpegArgs = $"-i \"{streamUrl}\" -c copy -bsf:a aac_adtstoasc";
            
            if (duration.HasValue)
            {
                ffmpegArgs += $" -t {(int)duration.Value.TotalSeconds}";
            }
            
            ffmpegArgs += $" \"{outputPath}\"";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = processStartInfo };
            process.Start();

            await Task.Delay(1000);

            if (process.HasExited)
            {
                throw new Exception("FFmpeg failed to start recording");
            }

            return process;
        }

        private void CheckScheduledRecordings(object? state)
        {
            var now = DateTime.Now;
            var recordingsToStart = new List<ScheduledRecording>();

            foreach (var scheduled in _scheduledRecordings.ToList())
            {
                var shouldRecord = false;

                if (scheduled.IsRecurring && scheduled.RecurringDays != null)
                {
                    if (scheduled.RecurringDays.Contains(now.DayOfWeek))
                    {
                        var scheduledTime = new DateTime(now.Year, now.Month, now.Day, 
                            scheduled.StartTime.Hour, scheduled.StartTime.Minute, scheduled.StartTime.Second);
                        
                        if (now >= scheduledTime && now < scheduledTime.AddMinutes(1))
                        {
                            shouldRecord = true;
                        }
                    }
                }
                else
                {
                    if (now >= scheduled.StartTime && now < scheduled.StartTime.AddMinutes(1))
                    {
                        shouldRecord = true;
                        recordingsToStart.Add(scheduled);
                    }
                }

                if (shouldRecord)
                {
                }
            }

            foreach (var scheduled in recordingsToStart)
            {
                if (!scheduled.IsRecurring)
                {
                    _scheduledRecordings.Remove(scheduled);
                }
            }
        }

        private string SanitizeFileName(string fileName)
        {
            var invalid = Path.GetInvalidFileNameChars();
            return string.Join("_", fileName.Split(invalid, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }

        public void Dispose()
        {
            _schedulerTimer?.Dispose();
            
            foreach (var recordingId in _activeRecordings.Keys.ToList())
            {
                StopRecording(recordingId);
            }
        }
    }
}
