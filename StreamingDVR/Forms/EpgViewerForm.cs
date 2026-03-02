using StreamingDVR.Services;

namespace StreamingDVR.Forms
{
    public class EpgViewerForm : Form
    {
        private ListView lstEpg;
        private ColumnHeader colTime;
        private ColumnHeader colTitle;
        private ColumnHeader colDescription;
        private Button btnClose;
        private Button btnRecordProgram;

        public EpgViewerForm(string channelName, List<EpgListing> epgListings)
        {
            InitializeComponent();
            this.Text = $"EPG - {channelName}";
            LoadEpgData(epgListings);
        }

        private void InitializeComponent()
        {
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            lstEpg = new ListView
            {
                Location = new Point(10, 10),
                Size = new Size(860, 500),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            colTime = new ColumnHeader { Text = "Time", Width = 200 };
            colTitle = new ColumnHeader { Text = "Title", Width = 300 };
            colDescription = new ColumnHeader { Text = "Description", Width = 340 };

            lstEpg.Columns.AddRange(new[] { colTime, colTitle, colDescription });

            btnRecordProgram = new Button
            {
                Text = "Record Selected Program",
                Location = new Point(600, 520),
                Size = new Size(180, 30)
            };
            btnRecordProgram.Click += BtnRecordProgram_Click;

            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(790, 520),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.AddRange(new Control[] { lstEpg, btnRecordProgram, btnClose });
            this.CancelButton = btnClose;
        }

        private void LoadEpgData(List<EpgListing> epgListings)
        {
            lstEpg.Items.Clear();

            foreach (var epg in epgListings.OrderBy(e => e.StartTime))
            {
                var timeRange = $"{epg.StartTime:HH:mm} - {epg.EndTime:HH:mm}";
                var item = new ListViewItem(timeRange);
                item.SubItems.Add(epg.Title);
                item.SubItems.Add(epg.Description ?? "");
                item.Tag = epg;

                if (epg.StartTime <= DateTime.Now && epg.EndTime >= DateTime.Now)
                {
                    item.BackColor = Color.LightGreen;
                }
                else if (epg.StartTime < DateTime.Now)
                {
                    item.ForeColor = Color.Gray;
                }

                lstEpg.Items.Add(item);
            }
        }

        private void BtnRecordProgram_Click(object? sender, EventArgs e)
        {
            if (lstEpg.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a program to record.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var epg = lstEpg.SelectedItems[0].Tag as EpgListing;
            if (epg == null) return;

            MessageBox.Show(
                $"Schedule recording for:\n\n{epg.Title}\n{epg.StartTime:yyyy-MM-dd HH:mm} - {epg.EndTime:HH:mm}\n\nThis feature will be available in the next update.",
                "Coming Soon",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
