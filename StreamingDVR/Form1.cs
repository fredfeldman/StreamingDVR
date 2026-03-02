using StreamingDVR.Models;
using StreamingDVR.Services;
using StreamingDVR.Forms;
using StreamingDVR.Utilities;
using System.Diagnostics;

namespace StreamingDVR
{
    public partial class Form1 : Form
    {
        private XtreamCodesService _xtreamService;
        private RecordingService? _recordingService;
        private ConfigurationService _configService;
        private EpgService _epgService;
        private List<LiveChannel> _allChannels = new();
        private List<Category> _categories = new();
        private List<LiveChannel> _currentCategoryChannels = new();
        private System.Windows.Forms.Timer _refreshTimer;
        private Dictionary<string, RecordingPreviewForm> _previewForms = new();

        public Form1()
        {
            InitializeComponent();
            _xtreamService = new XtreamCodesService();
            _configService = new ConfigurationService();
            _epgService = new EpgService();
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 2000;
            _refreshTimer.Tick += RefreshTimer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckFFmpegAvailability();
            LoadConfiguration();

            if (string.IsNullOrEmpty(txtRecordingPath.Text))
            {
                var recordingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "IPTV Recordings");
                txtRecordingPath.Text = recordingsPath;
            }

            InitializeRecordingService(txtRecordingPath.Text);
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
                var config = _configService.LoadConfiguration();
                txtServerUrl.Text = config.ServerUrl;
                txtRecordingPath.Text = config.RecordingPath;

                if (config.RememberCredentials)
                {
                    txtUsername.Text = config.Username;
                    txtPassword.Text = config.Password;
                }

                UpdateStatus("Configuration loaded");
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
                    RememberCredentials = true
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
                    if (!_xtreamService.IsAuthenticated) return;

                    var streamUrl = _xtreamService.GetStreamUrl(scheduled.StreamId);
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
            if (string.IsNullOrWhiteSpace(txtServerUrl.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill in all connection details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnConnect.Enabled = false;
            UpdateStatus("Connecting...");

            try
            {
                var success = await _xtreamService.AuthenticateAsync(
                    txtServerUrl.Text.Trim(),
                    txtUsername.Text.Trim(),
                    txtPassword.Text.Trim());

                if (success)
                {
                    _epgService.SetCredentials(txtServerUrl.Text.Trim(), txtUsername.Text.Trim(), txtPassword.Text.Trim());
                    SaveConfiguration();
                    UpdateStatus("Connected successfully");
                    MessageBox.Show("Connected successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadChannelsAndCategories();
                    _refreshTimer.Start();
                }
                else
                {
                    UpdateStatus("Connection failed");
                    MessageBox.Show("Failed to connect. Please check your credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Connection error");
                MessageBox.Show($"Error connecting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConnect.Enabled = true;
            }
        }

        private async Task LoadChannelsAndCategories()
        {
            try
            {
                UpdateStatus("Loading categories and channels...");

                _categories = await _xtreamService.GetLiveCategoriesAsync();
                _allChannels = await _xtreamService.GetLiveChannelsAsync();

                lstCategories.Items.Clear();
                lstCategories.Items.Add("All Channels");
                foreach (var category in _categories.OrderBy(c => c.CategoryName))
                {
                    lstCategories.Items.Add(category.CategoryName);
                }

                if (lstCategories.Items.Count > 0)
                {
                    lstCategories.SelectedIndex = 0;
                }

                UpdateStatus($"Loaded {_allChannels.Count} channels in {_categories.Count} categories");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading channels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error loading channels");
            }
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
                    _currentCategoryChannels = await _xtreamService.GetLiveChannelsByCategoryAsync(selectedCategory.CategoryId);
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
                var streamUrl = _xtreamService.GetStreamUrl(channel.StreamId);
                MessageBox.Show($"Stream URL:\n{streamUrl}\n\nUse VLC or another media player to open this URL.",
                    "Stream URL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
                var streamUrl = _xtreamService.GetStreamUrl(channel.StreamId);
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
            if (!_xtreamService.IsAuthenticated)
            {
                MessageBox.Show("Please connect to Xtream Codes first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await LoadChannelsAndCategories();
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
                var streamUrl = _xtreamService.GetStreamUrl(channel.StreamId);
                Clipboard.SetText(streamUrl);
                UpdateStatus("Stream URL copied to clipboard");
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
    }

    public class RecordingDurationForm : Form
    {
        private NumericUpDown numHours;
        private NumericUpDown numMinutes;
        private Button btnOk;
        private Button btnCancel;

        public TimeSpan Duration { get; private set; }

        public RecordingDurationForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Recording Duration";
            this.Size = new Size(300, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblHours = new Label { Text = "Hours:", Location = new Point(20, 20), AutoSize = true };
            numHours = new NumericUpDown
            {
                Location = new Point(100, 18),
                Width = 60,
                Minimum = 0,
                Maximum = 24,
                Value = 1
            };

            var lblMinutes = new Label { Text = "Minutes:", Location = new Point(20, 50), AutoSize = true };
            numMinutes = new NumericUpDown
            {
                Location = new Point(100, 48),
                Width = 60,
                Minimum = 0,
                Maximum = 59,
                Value = 0
            };

            btnOk = new Button
            {
                Text = "OK",
                Location = new Point(100, 80),
                DialogResult = DialogResult.OK
            };
            btnOk.Click += (s, e) =>
            {
                Duration = TimeSpan.FromHours((double)numHours.Value) + TimeSpan.FromMinutes((double)numMinutes.Value);
                if (Duration.TotalSeconds == 0)
                {
                    MessageBox.Show("Please specify a duration greater than 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                }
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(180, 80),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.AddRange(new Control[] { lblHours, numHours, lblMinutes, numMinutes, btnOk, btnCancel });
            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }
    }
}
