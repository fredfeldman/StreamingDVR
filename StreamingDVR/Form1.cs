using StreamingDVR.Models;
using StreamingDVR.Services;
using StreamingDVR.Forms;
using StreamingDVR.Utilities;
using System.Diagnostics;

namespace StreamingDVR
{
    public partial class Form1 : Form
    {
        private XtreamCodesService? _xtreamService;
        private Enigma2Service? _enigma2Service;
        private RecordingService? _recordingService;
        private ConfigurationService _configService;
        private EpgService _epgService;
        private List<LiveChannel> _allChannels = new();
        private List<Category> _categories = new();
        private List<LiveChannel> _currentCategoryChannels = new();
        private System.Windows.Forms.Timer _refreshTimer;
        private System.Windows.Forms.Timer _nowNextTimer;
        private Dictionary<string, RecordingPreviewForm> _previewForms = new();
        private List<IptvSource> _activeSources = new();

        // ── async debug log ──────────────────────────────────────────────────
        private readonly System.Collections.Concurrent.ConcurrentQueue<string> _logQueue = new();
        private readonly System.Threading.SemaphoreSlim _logSignal = new(0);
        private Task? _logWriterTask;

        public Form1()
        {
            InitializeComponent();
            _configService = new ConfigurationService();
            _epgService = new EpgService();
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 2000;
            _refreshTimer.Tick += RefreshTimer_Tick;

            _nowNextTimer = new System.Windows.Forms.Timer();
            _nowNextTimer.Interval = 60_000;   // refresh Now/Next every minute
            _nowNextTimer.Tick += async (_, __) => await RefreshNowNextAsync();

            _logWriterTask = Task.Run(BackgroundLogWriterAsync);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LogDebug("=== Application Starting ===");

            LoadConfiguration();

            if (string.IsNullOrEmpty(txtRecordingPath.Text))
            {
                txtRecordingPath.Text = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                    "IPTV Recordings");
            }

            InitializeRecordingService(txtRecordingPath.Text);

            // Run network-bound startup on a progress dialog so the form
            // appears immediately and the user sees what's happening.
            _ = RunStartupAsync();
        }

        private async Task RunStartupAsync()
        {
            using var dlg = new StartupProgressForm();
            dlg.Show(this);

            await ConnectToActiveSources(dlg);

            dlg.MarkComplete();
            await Task.Delay(400);   // brief pause so "Ready" is visible
            dlg.Close();

            // FFmpeg check is deferred until after the main window is usable
            _ = Task.Run(() =>
            {
                bool ok = FFmpegValidator.IsFFmpegAvailable();
                if (!ok)
                    BeginInvoke(() =>
                    {
                        var r = MessageBox.Show(
                            "FFmpeg is not detected on your system.\n\n" +
                            "Recording will not work without FFmpeg.\n\n" +
                            "Would you like to see installation instructions?",
                            "FFmpeg Not Found",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (r == DialogResult.Yes)
                            FFmpegValidator.ShowFFmpegInstallationInstructions();
                    });
            });
        }

        private void CheckFFmpegAvailability()
        {
            if (!FFmpegValidator.IsFFmpegAvailable())
            {
                var result = MessageBox.Show(
                    "FFmpeg is not detected on your system.\n\n" +
                    "FFmpeg is required for recording streams. The application will work for browsing channels, " +
                    "but you won't be able to record without FFmpeg installed.\n\n" +
                    "Would you like to see installation instructions?",
                    "FFmpeg Not Found",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    FFmpegValidator.ShowFFmpegInstallationInstructions();
                }
            }
        }

        private void LoadConfiguration()
        {
            try
            {
                LogDebug("  Loading configuration from file...");
                var config = _configService.LoadConfiguration();

                LogDebug($"  Legacy ServerUrl: {config.ServerUrl}");
                LogDebug($"  Recording Path: {config.RecordingPath}");
                LogDebug($"  Remember Credentials: {config.RememberCredentials}");
                LogDebug($"  IPTV Sources count: {config.IptvSources.Count}");
                LogDebug($"  Use Streamlink: {config.UseStreamlink}");
                LogDebug($"  Streamlink Quality: {config.StreamlinkQuality}");

                foreach (var source in config.IptvSources)
                {
                    LogDebug($"    - {source.Name} ({source.Type}) - Active: {source.IsActive}");
                }

                txtServerUrl.Text = config.ServerUrl;
                txtRecordingPath.Text = config.RecordingPath;

                // TODO: Load Streamlink settings when UI controls are added
                chkUseStreamlink.Checked = config.UseStreamlink;
                cboStreamlinkQuality.SelectedItem = config.StreamlinkQuality;
                if (cboStreamlinkQuality.SelectedIndex == -1)
                {
                    cboStreamlinkQuality.SelectedIndex = 0; // Default to "best"
                }
                txtStreamlinkOptions.Text = config.StreamlinkOptions;

                if (config.RememberCredentials)
                {
                    txtUsername.Text = config.Username;
                    txtPassword.Text = config.Password;
                    LogDebug("  Loaded saved credentials (legacy)");
                }

                UpdateStatus("Configuration loaded");
                LogDebug("  Configuration loaded successfully");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Failed to load configuration: {ex.Message}");
            }
        }

        private void SaveConfiguration()
        {
            try
            {
                var config = new AppConfiguration
                {
                    ServerUrl = txtServerUrl.Text,
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    RecordingPath = txtRecordingPath.Text,
                    RememberCredentials = true,
                    // TODO: Save Streamlink settings when UI controls are added
                    UseStreamlink = chkUseStreamlink.Checked,
                    StreamlinkQuality = cboStreamlinkQuality.SelectedItem?.ToString() ?? "best",
                    StreamlinkOptions = txtStreamlinkOptions.Text
                };

                _configService.SaveConfiguration(config);
                UpdateStatus("Configuration saved");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Failed to save configuration: {ex.Message}");
            }
        }

        private void InitializeRecordingService(string path)
        {
            _recordingService?.Dispose();
            _recordingService = new RecordingService(path);

            // TODO: Configure Streamlink settings when UI controls are added
            var config = _configService.LoadConfiguration();
            _recordingService.ConfigureStreamlink(
                config.UseStreamlink,
                config.StreamlinkQuality,
                config.StreamlinkRetryOpen,
                config.StreamlinkRetryStreams,
                config.StreamlinkOptions
            );

            _recordingService.RecordingStarted += (s, rec) => BeginInvoke(() =>
            {
                UpdateStatus($"Recording started: {rec.ChannelName}");
                RefreshRecordingsList();
            });
            _recordingService.RecordingStopped += (s, rec) => BeginInvoke(() =>
            {
                UpdateStatus($"Recording stopped: {rec.ChannelName}");
                RefreshRecordingsList();
            });
            _recordingService.RecordingFailed += (s, rec) => BeginInvoke(() =>
            {
                UpdateStatus($"Recording failed: {rec.ChannelName}");
                RefreshRecordingsList();
            });
            _recordingService.ScheduledRecordingTriggered += async (s, scheduled) =>
            {
                try
                {
                    // Try to find the channel in our cached channels to get stream URL
                    var channel = _allChannels.FirstOrDefault(c => c.StreamId == scheduled.StreamId);
                    if (channel == null)
                    {
                        BeginInvoke(() =>
                        {
                            UpdateStatus($"Cannot start scheduled recording: Channel not found");
                        });
                        return;
                    }

                    var streamUrl = GetStreamUrlForChannel(channel);
                    if (string.IsNullOrEmpty(streamUrl))
                    {
                        BeginInvoke(() =>
                        {
                            UpdateStatus($"Cannot start scheduled recording: No active source");
                        });
                        return;
                    }

                    await _recordingService.StartRecordingAsync(scheduled.ChannelName, scheduled.StreamId, streamUrl, scheduled.Duration);

                    BeginInvoke(() =>
                    {
                        UpdateStatus($"Scheduled recording started: {scheduled.ChannelName}");
                        RefreshScheduledList();
                    });
                }
                catch (Exception ex)
                {
                    BeginInvoke(() =>
                    {
                        UpdateStatus($"Failed to start scheduled recording: {ex.Message}");
                    });
                }
            };
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            // This is the legacy connection button - redirect to manage sources
            MessageBox.Show(
                "Please use 'Manage Sources' in the Settings tab to configure your IPTV connections.\n\n" +
                "The new multi-source manager supports:\n" +
                "• Xtream Codes\n" +
                "• Enigma2 Boxes\n" +
                "• M3U Playlists",
                "Use Manage Sources",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            tabControl.SelectedTab = tabSettings;
        }

        private async Task LoadChannelsAndCategories()
        {
            // This method is deprecated - use ConnectToActiveSources instead
            await ConnectToActiveSources();
        }

        private async void LstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCategories.SelectedIndex < 0) return;

            lstChannels.Items.Clear();
            txtSearch.Clear();
            UpdateStatus("Loading channels...");

            try
            {
                if (lstCategories.SelectedIndex == 0)
                {
                    _currentCategoryChannels = _allChannels;
                }
                else
                {
                    var selectedCategory = _categories[lstCategories.SelectedIndex - 1];

                    // Filter channels by category from cached channels
                    _currentCategoryChannels = _allChannels
                        .Where(c => c.CategoryId == selectedCategory.CategoryId)
                        .ToList();
                }

                DisplayChannels(_currentCategoryChannels);
                UpdateStatus($"Showing {_currentCategoryChannels.Count} channels");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading channels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayChannels(List<LiveChannel> channels)
        {
            lstChannels.BeginUpdate();
            lstChannels.Items.Clear();

            foreach (var channel in channels.OrderBy(c => c.Name))
            {
                var item = new ListViewItem(channel.Name);
                item.SubItems.Add(channel.StreamId.ToString());
                item.SubItems.Add("");   // Now
                item.SubItems.Add("");   // Next
                item.Tag = channel;
                lstChannels.Items.Add(item);
            }

            lstChannels.EndUpdate();

            _ = RefreshNowNextAsync();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            var searchText = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                DisplayChannels(_currentCategoryChannels);
            }
            else
            {
                var filtered = _currentCategoryChannels
                    .Where(c => c.Name.ToLower().Contains(searchText))
                    .ToList();
                DisplayChannels(filtered);
            }
        }

        private void LstChannels_DoubleClick(object sender, EventArgs e)
        {
            if (lstChannels.SelectedItems.Count == 0) return;

            var channel = lstChannels.SelectedItems[0].Tag as LiveChannel;
            if (channel != null)
            {
                var streamUrl = GetStreamUrlForChannel(channel);
                if (!string.IsNullOrEmpty(streamUrl))
                {
                    using var dialog = new StreamUrlDialog(streamUrl, channel.Name);
                    dialog.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show("Unable to generate stream URL. Source not configured.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetStreamUrlForChannel(LiveChannel channel)
        {
            // Check stream type to determine which service to use
            if (channel.StreamType == "enigma2" && _enigma2Service != null && _enigma2Service.IsAuthenticated)
            {
                return _enigma2Service.GetStreamUrl(channel);
            }
            else if (_xtreamService != null && _xtreamService.IsAuthenticated)
            {
                return _xtreamService.GetStreamUrl(channel.StreamId);
            }

            // TODO: Add support for M3U stream URLs
            return string.Empty;
        }

        private async void BtnRecord_Click(object sender, EventArgs e)
        {
            if (lstChannels.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a channel to record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_recordingService == null)
            {
                MessageBox.Show("Recording service not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var channel = lstChannels.SelectedItems[0].Tag as LiveChannel;
            if (channel == null) return;

            var result = MessageBox.Show(
                "Do you want to specify a duration?\n\nYes - Record for specific duration\nNo - Record until manually stopped",
                "Recording Duration",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            TimeSpan? duration = null;
            if (result == DialogResult.Yes)
            {
                using var durationForm = new RecordingDurationForm();
                if (durationForm.ShowDialog() == DialogResult.OK)
                {
                    duration = durationForm.Duration;
                }
                else
                {
                    return;
                }
            }

            try
            {
                var streamUrl = GetStreamUrlForChannel(channel);
                if (string.IsNullOrEmpty(streamUrl))
                {
                    MessageBox.Show("Unable to get stream URL. Please check your source configuration.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                await _recordingService.StartRecordingAsync(channel.Name, channel.StreamId, streamUrl, duration);
                MessageBox.Show($"Recording started for {channel.Name}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControl.SelectedTab = tabRecordings;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting recording: {ex.Message}\n\nMake sure FFmpeg is installed and available in PATH.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnScheduleRecord_Click(object sender, EventArgs e)
        {
            if (lstChannels.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a channel to schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var channel = lstChannels.SelectedItems[0].Tag as LiveChannel;
            if (channel == null) return;

            MessageBox.Show("Schedule recording feature coming soon!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            LogDebug("=== Manual Refresh Clicked ===");

            if (_activeSources.Count == 0)
            {
                LogDebug("No active sources - showing warning");
                MessageBox.Show("No active sources configured. Please use 'Manage Sources' to add IPTV sources.", 
                    "No Sources", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabControl.SelectedTab = tabSettings;
                return;
            }

            LogDebug($"Refreshing {_activeSources.Count} active sources");
            await ConnectToActiveSources();
        }

        private void RefreshTimer_Tick(object? sender, EventArgs e)
        {
            RefreshRecordingsList();
        }

        private void RefreshRecordingsList()
        {
            if (_recordingService == null) return;

            lstRecordings.Items.Clear();
            var recordings = _recordingService.GetRecordings().OrderByDescending(r => r.StartTime);

            foreach (var recording in recordings)
            {
                var item = new ListViewItem(recording.ChannelName);
                item.SubItems.Add(recording.Status.ToString());
                item.SubItems.Add(recording.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));

                var duration = recording.EndTime.HasValue 
                    ? (recording.EndTime.Value - recording.StartTime).ToString(@"hh\:mm\:ss")
                    : recording.Status == RecordingStatus.Recording 
                        ? (DateTime.Now - recording.StartTime).ToString(@"hh\:mm\:ss")
                        : "-";
                item.SubItems.Add(duration);

                var size = recording.FileSize > 0 
                    ? FormatFileSize(recording.FileSize) 
                    : "-";
                item.SubItems.Add(size);

                item.Tag = recording;
                lstRecordings.Items.Add(item);
            }
        }

        private void BtnStopRecording_Click(object sender, EventArgs e)
        {
            if (lstRecordings.SelectedItems.Count == 0) return;
            if (_recordingService == null) return;

            var recording = lstRecordings.SelectedItems[0].Tag as Recording;
            if (recording == null) return;

            if (recording.Status != RecordingStatus.Recording)
            {
                MessageBox.Show("This recording is not active.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _recordingService.StopRecording(recording.Id);
            UpdateStatus($"Stopped recording: {recording.ChannelName}");
        }

        private void BtnPlayRecording_Click(object sender, EventArgs e)
        {
            if (lstRecordings.SelectedItems.Count == 0) return;

            var recording = lstRecordings.SelectedItems[0].Tag as Recording;
            if (recording == null) return;

            if (recording.Status == RecordingStatus.Recording)
            {
                MessageBox.Show("Cannot play a recording that is still in progress.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!File.Exists(recording.FilePath))
            {
                MessageBox.Show("Recording file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = recording.FilePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing recording: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeleteRecording_Click(object sender, EventArgs e)
        {
            if (lstRecordings.SelectedItems.Count == 0) return;
            if (_recordingService == null) return;

            var recording = lstRecordings.SelectedItems[0].Tag as Recording;
            if (recording == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete this recording?\n\n{recording.ChannelName}\n{recording.StartTime}",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _recordingService.DeleteRecording(recording.Id);
                RefreshRecordingsList();
                UpdateStatus($"Deleted recording: {recording.ChannelName}");
            }
        }

        private void BtnRemoveSchedule_Click(object sender, EventArgs e)
        {
            if (lstScheduled.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a scheduled recording to remove.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_recordingService == null) return;

            var scheduled = lstScheduled.SelectedItems[0].Tag as ScheduledRecording;
            if (scheduled == null) return;

            var result = MessageBox.Show(
                $"Remove scheduled recording?\n\n{scheduled.ChannelName}\n{scheduled.StartTime}",
                "Confirm Remove",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _recordingService.RemoveScheduledRecording(scheduled.Id);
                RefreshScheduledList();
                UpdateStatus("Scheduled recording removed");
            }
        }

        private void RefreshScheduledList()
        {
            if (_recordingService == null) return;

            lstScheduled.Items.Clear();
            var scheduled = _recordingService.GetScheduledRecordings().OrderBy(s => s.StartTime);

            foreach (var schedule in scheduled)
            {
                var item = new ListViewItem(schedule.ChannelName);

                var timeInfo = schedule.IsRecurring
                    ? $"Recurring - {schedule.StartTime:HH:mm} ({string.Join(", ", schedule.RecurringDays?.Select(d => d.ToString().Substring(0, 3)) ?? Array.Empty<string>())})"
                    : schedule.StartTime.ToString("yyyy-MM-dd HH:mm");

                item.SubItems.Add(timeInfo);
                item.SubItems.Add(schedule.Duration.ToString(@"hh\:mm\:ss"));
                item.Tag = schedule;
                lstScheduled.Items.Add(item);
            }
        }

        private void BtnRefreshScheduled_Click(object sender, EventArgs e)
        {
            RefreshScheduledList();
            UpdateStatus("Scheduled recordings refreshed");
        }

        private void BtnViewEpg_Click(object sender, EventArgs e)
        {
            ShowEpgForSelectedChannel();
        }

        private void MenuViewEpg_Click(object? sender, EventArgs e)
        {
            ShowEpgForSelectedChannel();
        }

        private void MenuCopyStreamUrl_Click(object? sender, EventArgs e)
        {
            if (lstChannels.SelectedItems.Count == 0) return;

            var channel = lstChannels.SelectedItems[0].Tag as LiveChannel;
            if (channel != null)
            {
                var streamUrl = GetStreamUrlForChannel(channel);
                if (!string.IsNullOrEmpty(streamUrl))
                {
                    Clipboard.SetText(streamUrl);
                    UpdateStatus("Stream URL copied to clipboard");
                }
                else
                {
                    MessageBox.Show("Unable to get stream URL. Source not configured.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnOpenRecordingFolder_Click(object sender, EventArgs e)
        {
            OpenRecordingFolder();
        }

        private async void ShowEpgForSelectedChannel()
        {
            if (lstChannels.SelectedItems.Count == 0) return;

            var channel = lstChannels.SelectedItems[0].Tag as LiveChannel;
            if (channel == null || string.IsNullOrEmpty(channel.EpgChannelId)) 
            {
                MessageBox.Show("No EPG data available for this channel.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UpdateStatus("Loading EPG data...");

            try
            {
                var epgListings = await _epgService.GetEpgForChannelAsync(channel.EpgChannelId, 50);

                if (epgListings.Count == 0)
                {
                    MessageBox.Show("No EPG data available for this channel.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus("No EPG data available");
                    return;
                }

                using var epgForm = new EpgViewerForm(channel.Name, epgListings);
                epgForm.ShowDialog();
                UpdateStatus("EPG viewer closed");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading EPG: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("EPG load failed");
            }
        }

        private void OpenRecordingFolder()
        {
            try
            {
                if (Directory.Exists(txtRecordingPath.Text))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = txtRecordingPath.Text,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Recording folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBrowseRecordingPath_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "Select folder for recordings";
            dialog.SelectedPath = txtRecordingPath.Text;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtRecordingPath.Text = dialog.SelectedPath;
                InitializeRecordingService(dialog.SelectedPath);
                SaveConfiguration();
                UpdateStatus($"Recording path updated: {dialog.SelectedPath}");
            }
        }

        private void UpdateStatus(string message)
        {
            toolStripStatusLabel.Text = message;
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var activeRecordings = _recordingService?.GetActiveRecordings();
            if (activeRecordings != null && activeRecordings.Count > 0)
            {
                var result = MessageBox.Show(
                    $"There are {activeRecordings.Count} active recording(s).\n\nDo you want to stop all recordings and exit?",
                    "Active Recordings",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            _refreshTimer?.Stop();
            _nowNextTimer?.Stop();
            _recordingService?.Dispose();

            foreach (var form in _previewForms.Values)
            {
                form.Close();
            }
        }

        private void MenuExit_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void MenuSettings_Click(object? sender, EventArgs e)
        {
            tabControl.SelectedTab = tabSettings;
        }

        private void MenuCheckFFmpeg_Click(object? sender, EventArgs e)
        {
            if (FFmpegValidator.IsFFmpegAvailable())
            {
                var version = FFmpegValidator.GetFFmpegVersion();
                MessageBox.Show($"FFmpeg is installed and available!\n\n{version}", 
                    "FFmpeg Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FFmpegValidator.ShowFFmpegInstallationInstructions();
            }
        }

        private void MenuAbout_Click(object? sender, EventArgs e)
        {
            using var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void MenuViewStatistics_Click(object? sender, EventArgs e)
        {
            if (_recordingService == null) return;

            var recordings = _recordingService.GetRecordings();
            var stats = RecordingStatistics.Calculate(recordings);

            using var statsForm = new StatisticsForm(stats);
            statsForm.ShowDialog();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F5:
                    if (tabControl.SelectedTab == tabChannels)
                    {
                        BtnRefresh_Click(this, EventArgs.Empty);
                        return true;
                    }
                    else if (tabControl.SelectedTab == tabRecordings)
                    {
                        RefreshRecordingsList();
                        return true;
                    }
                    else if (tabControl.SelectedTab == tabScheduled)
                    {
                        RefreshScheduledList();
                        return true;
                    }
                    break;

                case Keys.Control | Keys.R:
                    if (tabControl.SelectedTab == tabChannels && lstChannels.SelectedItems.Count > 0)
                    {
                        BtnRecord_Click(this, EventArgs.Empty);
                        return true;
                    }
                    break;

                case Keys.Delete:
                    if (tabControl.SelectedTab == tabRecordings && lstRecordings.SelectedItems.Count > 0)
                    {
                        BtnDeleteRecording_Click(this, EventArgs.Empty);
                        return true;
                    }
                    else if (tabControl.SelectedTab == tabScheduled && lstScheduled.SelectedItems.Count > 0)
                    {
                        BtnRemoveSchedule_Click(this, EventArgs.Empty);
                        return true;
                    }
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnManageSources_Click(object? sender, EventArgs e)
        {
            var config = _configService.LoadConfiguration();

            using var sourceManager = new SourceManagerForm(config.IptvSources);
            if (sourceManager.ShowDialog() == DialogResult.OK)
            {
                config.IptvSources = sourceManager.Sources;

                try
                {
                    _configService.SaveConfiguration(config);
                    UpdateStatus($"Saved {config.IptvSources.Count} source(s)");

                    // Reconnect to active sources
                    _ = ConnectToActiveSources();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving configuration: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnManageEpgSources_Click(object sender, EventArgs e)
        {
            using var epgManager = new EpgSourceManagerForm(_configService, _epgService);
            if (epgManager.ShowDialog() == DialogResult.OK)
            {
                UpdateStatus("EPG sources updated");
            }
        }

        private void MenuAssignEpg_Click(object? sender, EventArgs e)
        {
            if (lstChannels.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a channel.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var channel = lstChannels.SelectedItems[0].Tag as LiveChannel;
            if (channel == null) return;

            using var assignEpgForm = new AssignEpgForm(_configService, _epgService, channel.StreamId, channel.Name);
            if (assignEpgForm.ShowDialog() == DialogResult.OK)
            {
                UpdateStatus($"EPG assignment updated for {channel.Name}");
            }
        }

        private async Task ConnectToActiveSources(StartupProgressForm? progress = null)
        {
            LogDebug("=== ConnectToActiveSources: Starting ===");

            var config = _configService.LoadConfiguration();
            _activeSources = config.IptvSources.Where(s => s.IsActive).ToList();

            if (_activeSources.Count == 0)
            {
                UpdateStatus("No active sources configured. Use Manage Sources to add sources.");
                progress?.ReportStep("No active sources configured.");
                return;
            }

            progress?.SetSourceCount(_activeSources.Count);
            progress?.ReportStep($"Connecting to {_activeSources.Count} source(s) in parallel…");
            UpdateStatus($"Connecting to {_activeSources.Count} source(s)…");

            _allChannels.Clear();
            _categories.Clear();

            // Collect results in a thread-safe way since connections run in parallel
            var channelBag  = new System.Collections.Concurrent.ConcurrentBag<LiveChannel>();
            var categoryBag = new System.Collections.Concurrent.ConcurrentBag<Category>();
            int successCount = 0;

            var tasks = _activeSources.Select(async source =>
            {
                progress?.ReportStep($"Connecting to {source.Name}…");
                try
                {
                    bool ok = false;
                    switch (source.Type)
                    {
                        case SourceType.XtreamCodes:
                            ok = await ConnectToXtreamSource(source, channelBag, categoryBag);
                            break;
                        case SourceType.Enigma2:
                            ok = await ConnectToEnigma2Source(source, channelBag, categoryBag);
                            break;
                        case SourceType.M3U:
                            UpdateStatus($"M3U support coming soon: {source.Name}");
                            break;
                    }

                    int count = channelBag.Count;
                    if (ok) System.Threading.Interlocked.Increment(ref successCount);
                    progress?.ReportSourceResult(source.Name, ok, ok ? count : 0);
                }
                catch (Exception ex)
                {
                    source.LastError = ex.Message;
                    LogDebug($"✗ Exception connecting to {source.Name}: {ex.Message}");
                    progress?.ReportSourceResult(source.Name, false);
                }
                finally
                {
                    progress?.IncrementProgress();
                }
            });

            await Task.WhenAll(tasks);

            // Merge thread-safe bags back into the shared lists
            _allChannels.AddRange(channelBag);
            _categories.AddRange(categoryBag);

            if (successCount > 0)
            {
                LoadChannelsAndCategoriesFromCache();
                _refreshTimer.Start();
                _nowNextTimer.Start();
                _ = RefreshNowNextAsync();
                LogDebug("Timer started for periodic refresh");
            }
            else
            {
                UpdateStatus("Could not connect to any sources. Check Settings.");
            }

            LogDebug($"=== ConnectToActiveSources: Complete — {successCount}/{_activeSources.Count} succeeded ===");
        }

        private async Task<bool> ConnectToEnigma2Source(
            IptvSource source,
            System.Collections.Concurrent.ConcurrentBag<LiveChannel> channelBag,
            System.Collections.Concurrent.ConcurrentBag<Category> categoryBag)
        {
            LogDebug($"  >> ConnectToEnigma2Source: {source.Name}");

            if (string.IsNullOrEmpty(source.ServerUrl))
                throw new Exception("Server URL is required");

            if (_enigma2Service == null)
                _enigma2Service = new Enigma2Service();

            bool success = await _enigma2Service.AuthenticateAsync(
                source.ServerUrl, source.Username, source.Password, source.Port);

            if (!success)
            {
                LogDebug($"  << ConnectToEnigma2Source: Failed");
                return false;
            }

            source.LastConnected = DateTime.Now;
            source.LastError     = null;

            var bouquets = await _enigma2Service.GetBouquetsAsync();
            var channels = await _enigma2Service.GetAllChannelsAsync();

            foreach (var ch in channels)
            {
                ch.CategoryId = $"{source.Id}_{ch.CategoryId}";
                channelBag.Add(ch);
            }
            foreach (var b in bouquets)
                categoryBag.Add(b);

            LogDebug($"  << ConnectToEnigma2Source: {channels.Count} channels, {bouquets.Count} bouquets");
            return true;
        }

        private async Task<bool> ConnectToXtreamSource(
            IptvSource source,
            System.Collections.Concurrent.ConcurrentBag<LiveChannel> channelBag,
            System.Collections.Concurrent.ConcurrentBag<Category> categoryBag)
        {
            LogDebug($"  >> ConnectToXtreamSource: {source.Name}");

            if (string.IsNullOrEmpty(source.ServerUrl) ||
                string.IsNullOrEmpty(source.Username)  ||
                string.IsNullOrEmpty(source.Password))
            {
                throw new Exception("Missing connection details");
            }

            if (_xtreamService == null)
                _xtreamService = new XtreamCodesService();

            bool success = await _xtreamService.AuthenticateAsync(
                source.ServerUrl, source.Username, source.Password);

            if (!success)
            {
                LogDebug($"  << ConnectToXtreamSource: Failed");
                return false;
            }

            _epgService.SetCredentials(source.ServerUrl, source.Username, source.Password);
            source.LastConnected = DateTime.Now;
            source.LastError     = null;

            var channels   = await _xtreamService.GetLiveChannelsAsync();
            var categories = await _xtreamService.GetLiveCategoriesAsync();

            foreach (var ch in channels)
            {
                ch.CategoryId = $"{source.Id}_{ch.CategoryId}";
                channelBag.Add(ch);
            }
            foreach (var cat in categories)
                categoryBag.Add(cat);

            LogDebug($"  << ConnectToXtreamSource: {channels.Count} channels, {categories.Count} categories");
            return true;
        }

        private void LoadChannelsAndCategoriesFromCache()
        {
            LogDebug("=== LoadChannelsAndCategoriesFromCache ===");
            LogDebug($"Total channels in cache: {_allChannels.Count}");
            LogDebug($"Total categories in cache: {_categories.Count}");

            // Load categories
            lstCategories.Items.Clear();
            lstCategories.Items.Add("All Channels");

            var uniqueCategories = _categories
                .GroupBy(c => c.CategoryName)
                .Select(g => g.First())
                .OrderBy(c => c.CategoryName)
                .ToList();

            LogDebug($"Unique categories: {uniqueCategories.Count}");

            foreach (var category in uniqueCategories)
            {
                lstCategories.Items.Add(category);
            }

            if (lstCategories.Items.Count > 0)
            {
                lstCategories.SelectedIndex = 0;
                LogDebug("Selected 'All Channels' category");
            }

            UpdateStatus($"Loaded {_allChannels.Count} channels from {_activeSources.Count} source(s)");
            PopulateEpgCategoryCombo();
            LogDebug($"UI updated with {lstCategories.Items.Count} categories");
            LogDebug("=== LoadChannelsAndCategoriesFromCache: Complete ===");
        }

        private void LogDebug(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {message}");
            _logQueue.Enqueue($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}");
            _logSignal.Release();
        }

        private async Task BackgroundLogWriterAsync()
        {
            var logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "IPTV_DVR", "debug.log");
            try
            {
                var dir = Path.GetDirectoryName(logPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            }
            catch { return; }

            while (true)
            {
                await _logSignal.WaitAsync();
                var lines = new System.Text.StringBuilder();
                while (_logQueue.TryDequeue(out var line))
                    lines.AppendLine(line);
                try { await File.AppendAllTextAsync(logPath, lines.ToString()); }
                catch { /* silently ignore log errors */ }
            }
        }

        // Streamlink Event Handlers

        private void ChkUseStreamlink_CheckedChanged(object sender, EventArgs e)
        {
            // Enable/disable Streamlink quality controls based on checkbox
            cboStreamlinkQuality.Enabled = chkUseStreamlink.Checked;
            btnCheckStreamlink.Enabled = chkUseStreamlink.Checked;

            // Save configuration when changed
            SaveConfiguration();

            // Reconfigure recording service
            var config = _configService.LoadConfiguration();
            _recordingService?.ConfigureStreamlink(
                config.UseStreamlink,
                config.StreamlinkQuality,
                config.StreamlinkRetryOpen,
                config.StreamlinkRetryStreams,
                config.StreamlinkOptions
            );

            UpdateStatus(chkUseStreamlink.Checked
                ? "Streamlink enabled for recordings"
                : "Using FFmpeg for recordings");
        }

        private void BtnCheckStreamlink_Click(object sender, EventArgs e)
        {
            CheckStreamlinkAvailability();
        }

        private void CheckStreamlinkAvailability()
        {
            if (StreamlinkValidator.IsStreamlinkAvailable())
            {
                var info = StreamlinkValidator.GetStreamlinkInfo();
                MessageBox.Show(
                    info ?? "Streamlink is installed and available!",
                    "Streamlink Status",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                StreamlinkValidator.ShowStreamlinkInstallationInstructions();
            }
        }

        // ── TV Guide Tab ─────────────────────────────────────────────────────

        private void PopulateEpgCategoryCombo()
        {
            cboEpgCategory.Items.Clear();
            cboEpgCategory.Items.Add("All Categories");

            foreach (var cat in _categories.OrderBy(c => c.CategoryName))
                cboEpgCategory.Items.Add(cat);

            if (cboEpgCategory.Items.Count > 0)
                cboEpgCategory.SelectedIndex = 0;
        }

        // ── Now/Next EPG on Channels tab ─────────────────────────────────────

        private async Task RefreshNowNextAsync()
        {
            if (lstChannels.Items.Count == 0) return;

            var config = _configService.LoadConfiguration();
            var now    = DateTime.Now;

            // Pre-load any XMLTV sources not yet cached
            var neededSourceIds = lstChannels.Items.Cast<ListViewItem>()
                .Select(i => i.Tag as LiveChannel)
                .Where(ch => ch != null)
                .Select(ch => config.ChannelEpgMappings.FirstOrDefault(m => m.StreamId == ch!.StreamId))
                .Where(m => m?.EpgSourceId != null)
                .Select(m => m!.EpgSourceId!.Value)
                .Distinct()
                .Where(id => !_epgService.IsSourceCached(id))
                .ToList();

            foreach (var sourceId in neededSourceIds)
            {
                var src = config.EpgSources.FirstOrDefault(s => s.Id == sourceId);
                if (src != null)
                    await _epgService.LoadXmltvSourceAsync(src);
            }

            lstChannels.BeginUpdate();
            foreach (ListViewItem item in lstChannels.Items)
            {
                if (item.Tag is not LiveChannel ch) continue;
                if (item.SubItems.Count < 4) continue;

                List<EpgListing> listings;
                var mapping = config.ChannelEpgMappings.FirstOrDefault(m => m.StreamId == ch.StreamId);

                if (mapping?.EpgSourceId != null && !string.IsNullOrEmpty(mapping.EpgChannelId) &&
                    _epgService.IsSourceCached(mapping.EpgSourceId.Value))
                {
                    listings = _epgService.GetProgrammesFromCache(
                        mapping.EpgSourceId.Value, mapping.EpgChannelId,
                        from: now.AddMinutes(-1), to: now.AddHours(4));
                }
                else
                {
                    // Skip Xtream Codes per-channel API call here — too many calls;
                    // users should use AssignEpg with XMLTV for Now/Next data.
                    listings = new List<EpgListing>();
                }

                var current = listings.FirstOrDefault(p => p.StartTime <= now && p.EndTime > now);
                var next    = listings.FirstOrDefault(p => p.StartTime > now);

                item.SubItems[2].Text = current != null
                    ? $"{current.Title}  ({current.StartTime:HH:mm}–{current.EndTime:HH:mm})"
                    : "";
                item.SubItems[3].Text = next != null
                    ? $"{next.Title}  ({next.StartTime:HH:mm})"
                    : "";
            }
            lstChannels.EndUpdate();
        }

        private async void BtnEpgLoad_Click(object sender, EventArgs e)
        {
            if (_allChannels.Count == 0)
            {
                lblEpgStatus.Text = "No channels loaded — connect a source first.";
                return;
            }

            btnEpgLoad.Enabled = false;
            lblEpgStatus.Text  = "Loading EPG data…";

            try
            {
                var config = _configService.LoadConfiguration();

                // Determine which channels to show
                IEnumerable<LiveChannel> channels = _allChannels;
                if (cboEpgCategory.SelectedItem is Category selectedCat)
                {
                    channels = _allChannels.Where(ch =>
                        ch.CategoryId?.EndsWith(selectedCat.CategoryId) == true ||
                        ch.CategoryId == selectedCat.CategoryId);
                }

                var channelList = channels.Take(200).ToList();

                // Pre-load any XMLTV sources that are referenced by mappings for channels
                // in this batch — load each source only once.
                var neededSourceIds = channelList
                    .Select(ch => config.ChannelEpgMappings.FirstOrDefault(m => m.StreamId == ch.StreamId))
                    .Where(m => m?.EpgSourceId != null)
                    .Select(m => m!.EpgSourceId!.Value)
                    .Distinct()
                    .ToList();

                foreach (var sourceId in neededSourceIds)
                {
                    if (_epgService.IsSourceCached(sourceId)) continue;

                    var src = config.EpgSources.FirstOrDefault(s => s.Id == sourceId);
                    if (src == null) continue;

                    lblEpgStatus.Text = $"Loading EPG source: {src.Name}…";
                    var (err, fromCache) = await _epgService.LoadXmltvSourceAsync(src);
                    if (err != null)
                        LogDebug($"[EPG] Failed to load source '{src.Name}': {err}");
                    else
                        lblEpgStatus.Text = fromCache
                            ? $"EPG: {src.Name} loaded from cache"
                            : $"EPG: {src.Name} downloaded and cached";
                }

                // Build the grid rows
                var rows   = new List<EpgRow>();
                var window = TimeSpan.FromHours(12);
                var from   = DateTime.Now.AddHours(-1);
                var to     = DateTime.Now.Add(window);
                int loaded = 0;

                foreach (var ch in channelList)
                {
                    List<EpgListing> listings;

                    // Try configured XMLTV mapping first
                    var mapping = config.ChannelEpgMappings.FirstOrDefault(m => m.StreamId == ch.StreamId);
                    if (mapping?.EpgSourceId != null && !string.IsNullOrEmpty(mapping.EpgChannelId) &&
                        _epgService.IsSourceCached(mapping.EpgSourceId.Value))
                    {
                        listings = _epgService.GetProgrammesFromCache(
                            mapping.EpgSourceId.Value, mapping.EpgChannelId, from, to);
                    }
                    else
                    {
                        // Fall back to Xtream Codes server EPG
                        var epgId = ch.EpgChannelId ?? ch.StreamId.ToString();
                        listings  = await _epgService.GetEpgForChannelAsync(epgId, limit: 10);
                    }

                    var programs = listings
                        .Where(l => l.EndTime > from)
                        .Select(l => new EpgProgram
                        {
                            Title       = l.Title,
                            Description = l.Description,
                            Start       = l.StartTime,
                            End         = l.EndTime
                        })
                        .OrderBy(p => p.Start)
                        .ToList();

                    rows.Add(new EpgRow
                    {
                        ChannelName = ch.Name,
                        StreamId    = ch.StreamId,
                        Programs    = programs
                    });

                    loaded++;
                    if (loaded % 10 == 0)
                        lblEpgStatus.Text = $"Loading… {loaded}/{channelList.Count} channels";
                }

                epgGrid.ViewStart = DateTime.Now.Date.AddHours(DateTime.Now.Hour);
                epgGrid.LoadData(rows);

                int totalProg = rows.Sum(r => r.Programs.Count);
                int withData  = rows.Count(r => r.Programs.Count > 0);
                lblEpgStatus.Text = $"{rows.Count} channels, {totalProg} programmes ({withData} channels have EPG data)";

                if (totalProg == 0)
                    lblEpgStatus.Text += " — check EPG source assignments in Settings";
            }
            catch (Exception ex)
            {
                lblEpgStatus.Text = $"Error: {ex.Message}";
            }
            finally
            {
                btnEpgLoad.Enabled = true;
            }
        }


        private void BtnEpgNow_Click(object sender, EventArgs e)
        {
            epgGrid.ViewStart = DateTime.Now.Date.AddHours(DateTime.Now.Hour);
        }
    }
}
