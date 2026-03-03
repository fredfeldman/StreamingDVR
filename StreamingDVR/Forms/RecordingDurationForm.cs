using System;
using System.Drawing;
using System.Windows.Forms;

namespace StreamingDVR.Forms
{
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
