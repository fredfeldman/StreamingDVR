using StreamingDVR.Models;
using StreamingDVR.Services;

namespace StreamingDVR.Forms
{
    public partial class AssignEpgForm : Form
    {
        private readonly ConfigurationService _configService;
        private readonly EpgService           _epgService;
        private readonly int                  _streamId;
        private readonly string               _channelName;

        private List<XmltvChannel> _allChannels = new();

        public AssignEpgForm(ConfigurationService configService, EpgService epgService,
                             int streamId, string channelName)
        {
            _configService = configService;
            _epgService    = epgService;
            _streamId      = streamId;
            _channelName   = channelName;

            InitializeComponent();
            Text = $"Configure EPG — {channelName}";

            LoadEpgSources();
            LoadExistingMapping();
            PositionCancelButton();
        }

        // ── Initialisation ────────────────────────────────────────────────────
        private void LoadEpgSources()
        {
            var config = _configService.LoadConfiguration();
            cboEpgSource.Items.Clear();
            cboEpgSource.Items.Add("(None)");

            foreach (var src in config.EpgSources.Where(s => s.IsActive))
                cboEpgSource.Items.Add(src);

            cboEpgSource.SelectedIndex = 0;
        }

        private void LoadExistingMapping()
        {
            var config  = _configService.LoadConfiguration();
            var mapping = config.ChannelEpgMappings.FirstOrDefault(m => m.StreamId == _streamId);
            if (mapping == null) return;

            if (mapping.EpgSourceId.HasValue)
            {
                var src = config.EpgSources.FirstOrDefault(s => s.Id == mapping.EpgSourceId.Value);
                if (src != null) cboEpgSource.SelectedItem = src;
            }

            txtEpgChannelId.Text = mapping.EpgChannelId ?? string.Empty;
            lblStatus.Text = "Existing mapping loaded.";
        }

        private void PositionCancelButton()
        {
            // Cancel sits to the right of Save; Clear sits between them
            btnClear.Location   = new System.Drawing.Point(btnSave.Right + 8, btnSave.Top);
            btnCancel.Location  = new System.Drawing.Point(btnClear.Right + 8, btnSave.Top);
        }

        // ── Source dropdown ───────────────────────────────────────────────────
        private void CboEpgSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstChannels.Items.Clear();
            _allChannels.Clear();
            txtSearch.Clear();
            lblStatus.Text = cboEpgSource.SelectedIndex > 0
                ? "Click 'Load Channels' to browse this source."
                : string.Empty;
        }

        // ── Load channels ─────────────────────────────────────────────────────
        private async void BtnLoadChannels_Click(object sender, EventArgs e)
        {
            if (cboEpgSource.SelectedItem is not EpgSource source)
            {
                lblStatus.Text = "Select an EPG source first.";
                return;
            }

            btnLoadChannels.Enabled = false;
            lstChannels.Items.Clear();
            lblStatus.Text = "Loading channel list…";

            var (channels, error) = await _epgService.GetChannelsFromSourceAsync(source);

            if (error != null)
            {
                lblStatus.ForeColor = System.Drawing.Color.Firebrick;
                lblStatus.Text = $"Error: {error}";
                btnLoadChannels.Enabled = true;
                return;
            }

            lblStatus.ForeColor = System.Drawing.SystemColors.GrayText;
            _allChannels = channels;
            ApplyFilter(txtSearch.Text);
            lblStatus.Text = $"{channels.Count} channels loaded. Select one or type an ID manually.";

            // Pre-select current mapping in the list
            if (!string.IsNullOrWhiteSpace(txtEpgChannelId.Text))
                PreselectChannel(txtEpgChannelId.Text);

            btnLoadChannels.Enabled = true;
        }

        // ── Search / filter ───────────────────────────────────────────────────
        private void TxtSearch_TextChanged(object sender, EventArgs e)
            => ApplyFilter(txtSearch.Text);

        private void ApplyFilter(string term)
        {
            lstChannels.BeginUpdate();
            lstChannels.Items.Clear();

            var filtered = string.IsNullOrWhiteSpace(term)
                ? _allChannels
                : _allChannels.Where(c =>
                    c.DisplayName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    c.Id.Contains(term, StringComparison.OrdinalIgnoreCase));

            foreach (var ch in filtered)
            {
                var item = new ListViewItem(ch.DisplayName);
                item.SubItems.Add(ch.Id);
                item.Tag = ch;
                lstChannels.Items.Add(item);
            }

            lstChannels.EndUpdate();
        }

        private void PreselectChannel(string channelId)
        {
            foreach (ListViewItem item in lstChannels.Items)
            {
                if (item.Tag is XmltvChannel ch && ch.Id == channelId)
                {
                    item.Selected = true;
                    item.EnsureVisible();
                    break;
                }
            }
        }

        // ── List selection ────────────────────────────────────────────────────
        private void LstChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstChannels.SelectedItems.Count > 0 &&
                lstChannels.SelectedItems[0].Tag is XmltvChannel ch)
            {
                txtEpgChannelId.Text = ch.Id;
            }
        }

        private void LstChannels_DoubleClick(object sender, EventArgs e)
        {
            if (lstChannels.SelectedItems.Count > 0)
                BtnSave_Click(sender, e);
        }

        // ── Save / Clear / Cancel ─────────────────────────────────────────────
        private void BtnSave_Click(object sender, EventArgs e)
        {
            var config = _configService.LoadConfiguration();
            config.ChannelEpgMappings.RemoveAll(m => m.StreamId == _streamId);

            if (cboEpgSource.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtEpgChannelId.Text))
            {
                var selectedSource = (EpgSource)cboEpgSource.SelectedItem!;
                config.ChannelEpgMappings.Add(new ChannelEpgMapping
                {
                    StreamId     = _streamId,
                    ChannelName  = _channelName,
                    EpgSourceId  = selectedSource.Id,
                    EpgChannelId = txtEpgChannelId.Text.Trim()
                });
            }

            _configService.SaveConfiguration(config);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            cboEpgSource.SelectedIndex = 0;
            txtEpgChannelId.Clear();
            lstChannels.Items.Clear();
            _allChannels.Clear();
            lblStatus.Text = "Mapping cleared. Click Save to remove the assignment.";
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
