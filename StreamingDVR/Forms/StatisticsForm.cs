using StreamingDVR.Services;

namespace StreamingDVR.Forms
{
    public class StatisticsForm : Form
    {
        private RecordingStatistics _stats;

        public StatisticsForm(RecordingStatistics stats)
        {
            _stats = stats;
            InitializeComponent();
            LoadStatistics();
        }

        private void InitializeComponent()
        {
            this.Text = "Recording Statistics";
            this.Size = new Size(600, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            var yPos = 20;

            AddStatLabel(panel, "📊 Recording Statistics", ref yPos, new Font("Segoe UI", 14, FontStyle.Bold));
            yPos += 10;

            AddStatLabel(panel, "General", ref yPos, new Font("Segoe UI", 11, FontStyle.Bold));
            AddStatItem(panel, "Total Recordings:", _stats.TotalRecordings.ToString(), ref yPos);
            AddStatItem(panel, "Completed:", _stats.CompletedRecordings.ToString(), ref yPos);
            AddStatItem(panel, "Active:", _stats.ActiveRecordings.ToString(), ref yPos);
            AddStatItem(panel, "Failed:", _stats.FailedRecordings.ToString(), ref yPos);
            yPos += 10;

            AddStatLabel(panel, "Storage", ref yPos, new Font("Segoe UI", 11, FontStyle.Bold));
            AddStatItem(panel, "Total Size:", _stats.TotalSizeFormatted, ref yPos);
            AddStatItem(panel, "Total Duration:", _stats.TotalDuration.ToString(@"dd\d\ hh\h\ mm\m"), ref yPos);
            yPos += 10;

            if (_stats.OldestRecording.HasValue)
            {
                AddStatLabel(panel, "Timeline", ref yPos, new Font("Segoe UI", 11, FontStyle.Bold));
                AddStatItem(panel, "Oldest Recording:", _stats.OldestRecording.Value.ToString("yyyy-MM-dd HH:mm"), ref yPos);
                AddStatItem(panel, "Newest Recording:", _stats.NewestRecording?.ToString("yyyy-MM-dd HH:mm") ?? "N/A", ref yPos);
                yPos += 10;
            }

            if (_stats.LargestRecording != null)
            {
                AddStatLabel(panel, "Records", ref yPos, new Font("Segoe UI", 11, FontStyle.Bold));
                AddStatItem(panel, "Largest Recording:", $"{_stats.LargestRecording.ChannelName} ({FormatSize(_stats.LargestRecording.FileSize)})", ref yPos);
            }

            if (_stats.LongestRecording != null && _stats.LongestRecording.EndTime.HasValue)
            {
                var duration = _stats.LongestRecording.EndTime.Value - _stats.LongestRecording.StartTime;
                AddStatItem(panel, "Longest Recording:", $"{_stats.LongestRecording.ChannelName} ({duration:hh\\:mm\\:ss})", ref yPos);
            }

            var btnClose = new Button
            {
                Text = "Close",
                Location = new Point(490, 420),
                Size = new Size(80, 30),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                DialogResult = DialogResult.OK
            };

            this.Controls.Add(panel);
            this.Controls.Add(btnClose);
            this.AcceptButton = btnClose;
        }

        private void AddStatLabel(Panel panel, string text, ref int yPos, Font font)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(20, yPos),
                AutoSize = true,
                Font = font
            };
            panel.Controls.Add(label);
            yPos += 35;
        }

        private void AddStatItem(Panel panel, string label, string value, ref int yPos)
        {
            var lblLabel = new Label
            {
                Text = label,
                Location = new Point(40, yPos),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 9)
            };

            var lblValue = new Label
            {
                Text = value,
                Location = new Point(250, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            panel.Controls.Add(lblLabel);
            panel.Controls.Add(lblValue);
            yPos += 30;
        }

        private void LoadStatistics()
        {
            // Statistics already loaded in constructor
        }

        private string FormatSize(long bytes)
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
    }
}
