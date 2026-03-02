using StreamingDVR.Models;

namespace StreamingDVR.Forms
{
    public class ScheduleRecordingForm : Form
    {
        private Label lblChannel;
        private Label lblChannelName;
        private DateTimePicker dtpDate;
        private DateTimePicker dtpTime;
        private NumericUpDown numHours;
        private NumericUpDown numMinutes;
        private CheckBox chkRecurring;
        private CheckedListBox clbDays;
        private Button btnOk;
        private Button btnCancel;
        private GroupBox grpRecurring;

        public ScheduledRecording? ScheduledRecording { get; private set; }

        public ScheduleRecordingForm(string channelName, int streamId)
        {
            InitializeComponent();
            lblChannelName.Text = channelName;
            ScheduledRecording = new ScheduledRecording
            {
                ChannelName = channelName,
                StreamId = streamId
            };
        }

        private void InitializeComponent()
        {
            this.Text = "Schedule Recording";
            this.Size = new Size(450, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            lblChannel = new Label
            {
                Text = "Channel:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(Font, FontStyle.Bold)
            };

            lblChannelName = new Label
            {
                Text = "",
                Location = new Point(100, 20),
                AutoSize = true,
                MaximumSize = new Size(320, 0)
            };

            var lblDate = new Label
            {
                Text = "Date:",
                Location = new Point(20, 60),
                AutoSize = true
            };

            dtpDate = new DateTimePicker
            {
                Location = new Point(120, 57),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Today
            };

            var lblTime = new Label
            {
                Text = "Time:",
                Location = new Point(20, 95),
                AutoSize = true
            };

            dtpTime = new DateTimePicker
            {
                Location = new Point(120, 92),
                Width = 200,
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true
            };

            var lblDuration = new Label
            {
                Text = "Duration:",
                Location = new Point(20, 130),
                AutoSize = true
            };

            var lblHours = new Label
            {
                Text = "Hours:",
                Location = new Point(120, 130),
                AutoSize = true
            };

            numHours = new NumericUpDown
            {
                Location = new Point(180, 128),
                Width = 60,
                Minimum = 0,
                Maximum = 24,
                Value = 1
            };

            var lblMinutes = new Label
            {
                Text = "Minutes:",
                Location = new Point(250, 130),
                AutoSize = true
            };

            numMinutes = new NumericUpDown
            {
                Location = new Point(320, 128),
                Width = 60,
                Minimum = 0,
                Maximum = 59,
                Value = 0
            };

            chkRecurring = new CheckBox
            {
                Text = "Recurring Recording",
                Location = new Point(20, 170),
                AutoSize = true
            };
            chkRecurring.CheckedChanged += ChkRecurring_CheckedChanged;

            grpRecurring = new GroupBox
            {
                Text = "Repeat On",
                Location = new Point(20, 200),
                Size = new Size(400, 150),
                Enabled = false
            };

            clbDays = new CheckedListBox
            {
                Location = new Point(10, 25),
                Size = new Size(380, 110)
            };
            clbDays.Items.AddRange(new object[]
            {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday"
            });

            grpRecurring.Controls.Add(clbDays);

            btnOk = new Button
            {
                Text = "Schedule",
                Location = new Point(240, 370),
                Size = new Size(90, 30),
                DialogResult = DialogResult.OK
            };
            btnOk.Click += BtnOk_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(340, 370),
                Size = new Size(90, 30),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.AddRange(new Control[]
            {
                lblChannel, lblChannelName, lblDate, dtpDate,
                lblTime, dtpTime, lblDuration, lblHours, numHours,
                lblMinutes, numMinutes, chkRecurring, grpRecurring,
                btnOk, btnCancel
            });

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void ChkRecurring_CheckedChanged(object? sender, EventArgs e)
        {
            grpRecurring.Enabled = chkRecurring.Checked;
            dtpDate.Enabled = !chkRecurring.Checked;
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            var duration = TimeSpan.FromHours((double)numHours.Value) + TimeSpan.FromMinutes((double)numMinutes.Value);
            
            if (duration.TotalSeconds == 0)
            {
                MessageBox.Show("Please specify a duration greater than 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (ScheduledRecording != null)
            {
                ScheduledRecording.Duration = duration;
                ScheduledRecording.IsRecurring = chkRecurring.Checked;

                if (chkRecurring.Checked)
                {
                    var selectedDays = new List<DayOfWeek>();
                    for (int i = 0; i < clbDays.CheckedIndices.Count; i++)
                    {
                        selectedDays.Add((DayOfWeek)((clbDays.CheckedIndices[i] + 1) % 7));
                    }

                    if (selectedDays.Count == 0)
                    {
                        MessageBox.Show("Please select at least one day for recurring recording.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.DialogResult = DialogResult.None;
                        return;
                    }

                    ScheduledRecording.RecurringDays = selectedDays.ToArray();
                    ScheduledRecording.StartTime = new DateTime(
                        DateTime.Today.Year,
                        DateTime.Today.Month,
                        DateTime.Today.Day,
                        dtpTime.Value.Hour,
                        dtpTime.Value.Minute,
                        dtpTime.Value.Second);
                }
                else
                {
                    var scheduledDateTime = new DateTime(
                        dtpDate.Value.Year,
                        dtpDate.Value.Month,
                        dtpDate.Value.Day,
                        dtpTime.Value.Hour,
                        dtpTime.Value.Minute,
                        dtpTime.Value.Second);

                    if (scheduledDateTime <= DateTime.Now)
                    {
                        MessageBox.Show("Scheduled time must be in the future.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.DialogResult = DialogResult.None;
                        return;
                    }

                    ScheduledRecording.StartTime = scheduledDateTime;
                }
            }
        }
    }
}
