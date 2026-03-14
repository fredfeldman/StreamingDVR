namespace StreamingDVR.Forms
{
    public partial class StartupProgressForm : Form
    {
        public StartupProgressForm()
        {
            InitializeComponent();
        }

        public void ReportStep(string message)
        {
            if (InvokeRequired) { Invoke(() => ReportStep(message)); return; }
            lblStatus.Text = message;
            lstLog.Items.Add(message);
            if (lstLog.Items.Count > 0)
                lstLog.TopIndex = lstLog.Items.Count - 1;
            Application.DoEvents();
        }

        public void ReportSourceResult(string sourceName, bool success, int channels = 0)
        {
            if (InvokeRequired) { Invoke(() => ReportSourceResult(sourceName, success, channels)); return; }
            string text = success
                ? $"✓  {sourceName}  —  {channels:N0} channels"
                : $"✗  {sourceName}  —  failed";
            var item = new ListViewItem(text) { ForeColor = success ? Color.LightGreen : Color.Salmon };
            lstSources.Items.Add(item);
        }

        public void SetSourceCount(int total)
        {
            if (InvokeRequired) { Invoke(() => SetSourceCount(total)); return; }
            lblStatus.Text = $"Connecting to {total} source(s)…";
            pbProgress.Maximum = Math.Max(1, total);
            pbProgress.Value   = 0;
        }

        public void IncrementProgress()
        {
            if (InvokeRequired) { Invoke(IncrementProgress); return; }
            if (pbProgress.Value < pbProgress.Maximum)
                pbProgress.Value++;
        }

        public void MarkComplete()
        {
            if (InvokeRequired) { Invoke(MarkComplete); return; }
            pbProgress.Value  = pbProgress.Maximum;
            lblStatus.Text    = "Ready.";
        }
    }
}
