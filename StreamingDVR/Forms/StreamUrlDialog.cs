using System;
using System.Windows.Forms;

namespace StreamingDVR.Forms
{
    public partial class StreamUrlDialog : Form
    {
        private readonly string _streamUrl;

        public StreamUrlDialog(string streamUrl, string channelName)
        {
            InitializeComponent();
            _streamUrl = streamUrl;
            Text = $"Stream URL - {channelName}";
            txtStreamUrl.Text = streamUrl;
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(_streamUrl);
                lblStatus.Text = "✓ URL copied to clipboard!";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Visible = true;

                // Hide status after 2 seconds
                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 2000;
                timer.Tick += (s, args) =>
                {
                    lblStatus.Visible = false;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Failed to copy: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOpenVLC_Click(object sender, EventArgs e)
        {
            try
            {
                // Try to open with VLC
                var vlcPaths = new[]
                {
                    @"C:\Program Files\VideoLAN\VLC\vlc.exe",
                    @"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe"
                };

                string vlcPath = null;
                foreach (var path in vlcPaths)
                {
                    if (System.IO.File.Exists(path))
                    {
                        vlcPath = path;
                        break;
                    }
                }

                if (vlcPath != null)
                {
                    System.Diagnostics.Process.Start(vlcPath, $"\"{_streamUrl}\"");
                    lblStatus.Text = "✓ Opening in VLC...";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Visible = true;
                }
                else
                {
                    // Try default handler
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = _streamUrl,
                        UseShellExecute = true
                    });
                    lblStatus.Text = "✓ Opening with default player...";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open stream: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
