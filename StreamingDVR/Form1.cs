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
        private Dictionary<string, RecordingPreviewForm> _previewForms = new();
        private List<IptvSource> _activeSources = new();

        public Form1()
        {
            InitializeComponent();
            _configService = new ConfigurationService();
            _epgService = new EpgService();
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 2000;
            _refreshTimer.Tick += RefreshTimer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LogDebug("========================================");
            LogDebug("=== Application Starting ===");
            LogDebug($"Version: {Application.ProductVersion}");
            LogDebug($"OS: {Environment.OSVersion}");
            LogDebug($".NET Version: {Environment.Version}");
            LogDebug("========================================");

            LogDebug("Checking FFmpeg availability...");
            CheckFFmpegAvailability();

            LogDebug("Loading configuration...");
            LoadConfiguration();

            if (string.IsNullOrEmpty(txtRecordingPath.Text))
            {
                var recordingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "IPTV Recordings");
                txtRecordingPath.Text = recordingsPath;
                LogDebug($"Set default recording path: {recordingsPath}");
            }
            else
            {
                LogDebug($"Recording path from config: {txtRecordingPath.Text}");
            }

            LogDebug("Initializing recording service...");
            InitializeRecordingService(txtRecordingPath.Text);

            LogDebug("Auto-connecting to active sources...");
            _ = ConnectToActiveSources();
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

                // Load Streamlink settings
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

            // Configure Streamlink settings
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
            lstChannels.Items.Clear();

            foreach (var channel in channels.OrderBy(c => c.Name))
            {
                var item = new ListViewItem(channel.Name);
                item.SubItems.Add(channel.StreamId.ToString());
                item.Tag = channel;
                lstChannels.Items.Add(item);
            }
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

        private void BtnManageEpgSources_Click(object? sender, EventArgs e)
        {
            using var epgManager = new EpgSourceManagerForm(_configService);
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

            using var assignEpgForm = new AssignEpgForm(_configService, channel.StreamId, channel.Name);
            if (assignEpgForm.ShowDialog() == DialogResult.OK)
            {
                UpdateStatus($"EPG assignment updated for {channel.Name}");
            }
        }

        private async Task ConnectToActiveSources()
        {
            LogDebug("=== ConnectToActiveSources: Starting ===");

            var config = _configService.LoadConfiguration();
            _activeSources = config.IptvSources.Where(s => s.IsActive).ToList();

            LogDebug($"Total sources in config: {config.IptvSources.Count}");
            LogDebug($"Active sources: {_activeSources.Count}");

            if (_activeSources.Count == 0)
            {
                LogDebug("No active sources configured");
                UpdateStatus("No active sources configured. Use Manage Sources to add sources.");
                return;
            }

            UpdateStatus($"Connecting to {_activeSources.Count} source(s)...");
            _allChannels.Clear();
            _categories.Clear();

            int successCount = 0;
            int attemptNumber = 0;

            foreach (var source in _activeSources)
            {
                attemptNumber++;
                LogDebug($"--- Source {attemptNumber}/{_activeSources.Count} ---");
                LogDebug($"Name: {source.Name}");
                LogDebug($"Type: {source.Type}");
                LogDebug($"ID: {source.Id}");

                try
                {
                    switch (source.Type)
                    {
                        case SourceType.XtreamCodes:
                            LogDebug($"Attempting Xtream Codes connection to {source.Name}");
                            LogDebug($"Server URL: {source.ServerUrl}");
                            LogDebug($"Username: {source.Username}");
                            LogDebug($"Port: {source.Port}");

                            if (await ConnectToXtreamSource(source))
                            {
                                successCount++;
                                LogDebug($"✓ Successfully connected to {source.Name}");
                            }
                            else
                            {
                                LogDebug($"✗ Failed to connect to {source.Name} (returned false)");
                            }
                            break;

                        case SourceType.Enigma2:
                            LogDebug($"Attempting Enigma2 connection to {source.Name}");
                            LogDebug($"Server URL: {source.ServerUrl}");
                            LogDebug($"Username: {source.Username}");
                            LogDebug($"Port: {source.Port}");

                            if (await ConnectToEnigma2Source(source))
                            {
                                successCount++;
                                LogDebug($"✓ Successfully connected to {source.Name}");
                            }
                            else
                            {
                                LogDebug($"✗ Failed to connect to {source.Name} (returned false)");
                            }
                            break;

                        case SourceType.M3U:
                            LogDebug($"M3U support coming soon: {source.Name}");
                            UpdateStatus($"M3U support coming soon: {source.Name}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    source.LastError = ex.Message;
                    LogDebug($"✗ Exception connecting to {source.Name}:");
                    LogDebug($"  Exception Type: {ex.GetType().Name}");
                    LogDebug($"  Message: {ex.Message}");
                    LogDebug($"  Stack Trace: {ex.StackTrace}");
                    UpdateStatus($"Failed to connect to {source.Name}: {ex.Message}");
                }
            }

            LogDebug($"=== Connection Summary ===");
            LogDebug($"Successful: {successCount}/{_activeSources.Count}");
            LogDebug($"Total channels loaded: {_allChannels.Count}");
            LogDebug($"Total categories loaded: {_categories.Count}");

            if (successCount > 0)
            {
                UpdateStatus($"Connected to {successCount} of {_activeSources.Count} source(s)");
                LoadChannelsAndCategoriesFromCache();
                _refreshTimer.Start();
                LogDebug("Timer started for periodic refresh");
            }
            else
            {
                LogDebug("No sources connected successfully");
                UpdateStatus("Failed to connect to any sources");
            }

            LogDebug("=== ConnectToActiveSources: Complete ===");
        }

        private async Task<bool> ConnectToEnigma2Source(IptvSource source)
        {
            LogDebug($"  >> ConnectToEnigma2Source: {source.Name}");

            // Validate server URL (credentials are optional for Enigma2)
            if (string.IsNullOrEmpty(source.ServerUrl))
            {
                LogDebug("  ✗ Missing server URL");
                throw new Exception("Server URL is required");
            }

            // Log credential status
            if (string.IsNullOrEmpty(source.Username))
            {
                LogDebug("  Anonymous access (no username/password)");
            }
            else
            {
                LogDebug("  Using authentication");
            }

            LogDebug("  Credentials validation passed");

            // Initialize service if needed
            if (_enigma2Service == null)
            {
                LogDebug("  Creating new Enigma2Service instance");
                _enigma2Service = new Enigma2Service();
            }
            else
            {
                LogDebug("  Reusing existing Enigma2Service instance");
            }

            // Attempt authentication
            LogDebug("  Calling AuthenticateAsync...");
            var success = await _enigma2Service.AuthenticateAsync(
                source.ServerUrl,
                source.Username,
                source.Password,
                source.Port);

            LogDebug($"  Authentication result: {success}");

            if (success)
            {
                source.LastConnected = DateTime.Now;
                source.LastError = null;

                LogDebug("  Loading bouquets (categories)...");
                var bouquets = await _enigma2Service.GetBouquetsAsync();
                LogDebug($"  Loaded {bouquets.Count} bouquets");

                LogDebug("  Loading channels from all bouquets...");
                var channels = await _enigma2Service.GetAllChannelsAsync();
                LogDebug($"  Loaded {channels.Count} channels");

                // Add source identifier to channels
                LogDebug("  Tagging channels with source ID");
                foreach (var channel in channels)
                {
                    channel.CategoryId = $"{source.Id}_{channel.CategoryId}";
                }

                _allChannels.AddRange(channels);
                _categories.AddRange(bouquets);

                LogDebug($"  << ConnectToEnigma2Source: Success (Total channels: {_allChannels.Count})");
                return true;
            }

            LogDebug("  << ConnectToEnigma2Source: Failed (authentication returned false)");
            return false;
        }

        private async Task<bool> ConnectToXtreamSource(IptvSource source)
        {
            LogDebug($"  >> ConnectToXtreamSource: {source.Name}");

            // Validate credentials
            if (string.IsNullOrEmpty(source.ServerUrl) || 
                string.IsNullOrEmpty(source.Username) || 
                string.IsNullOrEmpty(source.Password))
            {
                LogDebug("  ✗ Missing connection details");
                LogDebug($"    ServerUrl empty: {string.IsNullOrEmpty(source.ServerUrl)}");
                LogDebug($"    Username empty: {string.IsNullOrEmpty(source.Username)}");
                LogDebug($"    Password empty: {string.IsNullOrEmpty(source.Password)}");
                throw new Exception("Missing connection details");
            }

            LogDebug("  Credentials validation passed");

            // Initialize service if needed
            if (_xtreamService == null)
            {
                LogDebug("  Creating new XtreamCodesService instance");
                _xtreamService = new XtreamCodesService();
            }
            else
            {
                LogDebug("  Reusing existing XtreamCodesService instance");
            }

            // Attempt authentication
            LogDebug("  Calling AuthenticateAsync...");
            var success = await _xtreamService.AuthenticateAsync(
                source.ServerUrl,
                source.Username,
                source.Password);

            LogDebug($"  Authentication result: {success}");

            if (success)
            {
                LogDebug("  Setting EPG credentials");
                _epgService.SetCredentials(source.ServerUrl, source.Username, source.Password);
                source.LastConnected = DateTime.Now;
                source.LastError = null;

                LogDebug("  Loading channels...");
                var channels = await _xtreamService.GetLiveChannelsAsync();
                LogDebug($"  Loaded {channels.Count} channels");

                LogDebug("  Loading categories...");
                var categories = await _xtreamService.GetLiveCategoriesAsync();
                LogDebug($"  Loaded {categories.Count} categories");

                // Add source identifier to channels
                LogDebug("  Tagging channels with source ID");
                foreach (var channel in channels)
                {
                    channel.CategoryId = $"{source.Id}_{channel.CategoryId}";
                }

                _allChannels.AddRange(channels);
                _categories.AddRange(categories);

                LogDebug($"  << ConnectToXtreamSource: Success (Total channels: {_allChannels.Count})");
                return true;
            }

            LogDebug("  << ConnectToXtreamSource: Failed (authentication returned false)");
            return false;
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
            LogDebug($"UI updated with {lstCategories.Items.Count} categories");
            LogDebug("=== LoadChannelsAndCategoriesFromCache: Complete ===");
        }

        private void LogDebug(string message)
        {
            // Write to debug output
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {message}");

            // Also write to a log file for persistent debugging
            try
            {
                var logPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "IPTV_DVR",
                    "debug.log");

                var logDir = Path.GetDirectoryName(logPath);
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir!);

                File.AppendAllText(logPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}{Environment.NewLine}");
            }
            catch
            {
                // Silently fail if logging fails - don't interrupt the app
            }
        }
    }
}
