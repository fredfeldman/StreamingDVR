namespace StreamingDVR.Forms
{
    public class WelcomeForm : Form
    {
        private CheckBox chkDontShowAgain;

        public bool DontShowAgain => chkDontShowAgain.Checked;

        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Welcome to IPTV DVR";
            this.Size = new Size(700, 550);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            var lblTitle = new Label
            {
                Text = "Welcome to IPTV DVR! 📺",
                Location = new Point(20, 20),
                Size = new Size(650, 40),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblIntro = new Label
            {
                Text = "Your complete solution for recording IPTV streams using Xtream Codes.",
                Location = new Point(20, 70),
                Size = new Size(650, 30),
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var grpGettingStarted = new GroupBox
            {
                Text = "🚀 Getting Started",
                Location = new Point(20, 110),
                Size = new Size(650, 200),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lblSteps = new Label
            {
                Text = "1. Install FFmpeg (Required for Recording)\n" +
                       "   • Using winget: winget install FFmpeg\n" +
                       "   • Or download from: ffmpeg.org\n\n" +
                       "2. Connect to Your IPTV Service\n" +
                       "   • Go to Settings tab\n" +
                       "   • Enter server URL, username, and password\n" +
                       "   • Click Connect\n\n" +
                       "3. Start Recording\n" +
                       "   • Browse channels in Channels tab\n" +
                       "   • Select a channel and click 'Record Now'\n" +
                       "   • Manage recordings in Recordings tab",
                Location = new Point(15, 30),
                Size = new Size(620, 160),
                Font = new Font("Segoe UI", 9)
            };

            grpGettingStarted.Controls.Add(lblSteps);

            var grpFeatures = new GroupBox
            {
                Text = "✨ Key Features",
                Location = new Point(20, 320),
                Size = new Size(650, 110),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lblFeatures = new Label
            {
                Text = "• Browse channels by category with real-time search\n" +
                       "• Record instantly or schedule for later (one-time or recurring)\n" +
                       "• View EPG (program guide) for supported channels\n" +
                       "• Manage recordings: play, stop, delete with full statistics",
                Location = new Point(15, 30),
                Size = new Size(620, 70),
                Font = new Font("Segoe UI", 9)
            };

            grpFeatures.Controls.Add(lblFeatures);

            chkDontShowAgain = new CheckBox
            {
                Text = "Don't show this message again",
                Location = new Point(20, 445),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };

            var btnGetStarted = new Button
            {
                Text = "Get Started",
                Location = new Point(470, 470),
                Size = new Size(120, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                DialogResult = DialogResult.OK
            };

            var btnClose = new Button
            {
                Text = "Close",
                Location = new Point(600, 470),
                Size = new Size(80, 35),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.AddRange(new Control[]
            {
                lblTitle, lblIntro, grpGettingStarted, grpFeatures,
                chkDontShowAgain, btnGetStarted, btnClose
            });

            this.AcceptButton = btnGetStarted;
            this.CancelButton = btnClose;
        }
    }
}
