using System.Diagnostics;

namespace StreamingDVR.Forms
{
    public class RecordingPreviewForm : Form
    {
        private Label lblInfo;
        private ProgressBar progressBar;
        private Label lblStatus;
        private Label lblFileSize;
        private Label lblDuration;
        private Button btnStop;
        private Button btnOpenFolder;
        private System.Windows.Forms.Timer updateTimer;
        private string _filePath;
        private DateTime _startTime;

        public RecordingPreviewForm(string channelName, string filePath)
        {
            _filePath = filePath;
            _startTime = DateTime.Now;
            InitializeComponent();
            lblInfo.Text = $"Recording: {channelName}";
            StartMonitoring();
        }

        private void InitializeComponent()
        {
            this.Text = "Recording in Progress";
            this.Size = new Size(500, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            lblInfo = new Label
            {
                Location = new Point(20, 20),
                Size = new Size(450, 30),
                Font = new Font(Font.FontFamily, 12, FontStyle.Bold)
            };

            progressBar = new ProgressBar
            {
                Location = new Point(20, 60),
                Size = new Size(450, 30),
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30
            };

            lblStatus = new Label
            {
                Text = "Status: Recording...",
                Location = new Point(20, 100),
                AutoSize = true
            };

            lblDuration = new Label
            {
                Text = "Duration: 00:00:00",
                Location = new Point(20, 130),
                AutoSize = true
            };

            lblFileSize = new Label
            {
                Text = "File Size: 0 MB",
                Location = new Point(20, 160),
                AutoSize = true
            };

            btnStop = new Button
            {
                Text = "Stop Recording",
                Location = new Point(250, 210),
                Size = new Size(120, 35),
                DialogResult = DialogResult.OK
            };

            btnOpenFolder = new Button
            {
                Text = "Open Folder",
                Location = new Point(120, 210),
                Size = new Size(120, 35)
            };
            btnOpenFolder.Click += BtnOpenFolder_Click;

            this.Controls.AddRange(new Control[]
            {
                lblInfo, progressBar, lblStatus, lblDuration,
                lblFileSize, btnStop, btnOpenFolder
            });
        }

        private void StartMonitoring()
        {
            updateTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            var duration = DateTime.Now - _startTime;
            lblDuration.Text = $"Duration: {duration:hh\\:mm\\:ss}";

            if (File.Exists(_filePath))
            {
                try
                {
                    var fileInfo = new FileInfo(_filePath);
                    var sizeMB = fileInfo.Length / (1024.0 * 1024.0);
                    lblFileSize.Text = $"File Size: {sizeMB:F2} MB";
                }
                catch
                {
                    // Ignore if file is locked
                }
            }
        }

        private void BtnOpenFolder_Click(object? sender, EventArgs e)
        {
            try
            {
                var folder = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = folder,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            updateTimer?.Stop();
            updateTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
