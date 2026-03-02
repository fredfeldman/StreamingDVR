namespace StreamingDVR.Forms
{
    public class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "About IPTV DVR";
            this.Size = new Size(500, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblTitle = new Label
            {
                Text = "IPTV DVR - Xtream Codes Edition",
                Location = new Point(20, 20),
                Size = new Size(450, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };

            var lblVersion = new Label
            {
                Text = "Version 1.0.0",
                Location = new Point(20, 55),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            var lblDescription = new Label
            {
                Text = "A Windows desktop application for recording IPTV streams using Xtream Codes API.\n\n" +
                       "Features:\n" +
                       "• Connect to Xtream Codes IPTV services\n" +
                       "• Browse channels by category\n" +
                       "• Record live streams with FFmpeg\n" +
                       "• Schedule recordings\n" +
                       "• EPG (Electronic Program Guide) support\n" +
                       "• Manage and play recordings",
                Location = new Point(20, 90),
                Size = new Size(450, 150),
                Font = new Font("Segoe UI", 9)
            };

            var lblTech = new Label
            {
                Text = "Built with .NET 8 and Windows Forms\nRecording powered by FFmpeg",
                Location = new Point(20, 250),
                Size = new Size(450, 40),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };

            var btnClose = new Button
            {
                Text = "Close",
                Location = new Point(390, 270),
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };

            this.Controls.AddRange(new Control[] { lblTitle, lblVersion, lblDescription, lblTech, btnClose });
            this.AcceptButton = btnClose;
        }
    }
}
