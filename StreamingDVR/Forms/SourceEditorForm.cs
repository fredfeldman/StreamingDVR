using StreamingDVR.Models;

namespace StreamingDVR.Forms
{
    public partial class SourceEditorForm : Form
    {
        private IptvSource? _source;
        private List<string> _epgUrls = new();

        public IptvSource? Source => _source;

        public SourceEditorForm(IptvSource? source)
        {
            InitializeComponent();
            _source = source ?? new IptvSource();
            _epgUrls = new List<string>(_source.EpgUrls);
        }

        private void SourceEditorForm_Load(object sender, EventArgs e)
        {
            // Populate source types
            cmbSourceType.Items.Clear();
            cmbSourceType.Items.Add("Xtream Codes");
            cmbSourceType.Items.Add("Enigma2 Box");
            cmbSourceType.Items.Add("M3U Playlist");

            if (_source != null && _source.Name != string.Empty)
            {
                // Edit mode
                txtName.Text = _source.Name;
                cmbSourceType.SelectedIndex = (int)_source.Type;
                chkActive.Checked = _source.IsActive;

                // Load type-specific fields
                LoadSourceSpecificFields();
            }
            else
            {
                // New source
                cmbSourceType.SelectedIndex = 0;
                chkActive.Checked = true;
            }

            RefreshEpgList();
        }

        private void LoadSourceSpecificFields()
        {
            if (_source == null) return;

            switch (_source.Type)
            {
                case SourceType.XtreamCodes:
                case SourceType.Enigma2:
                    txtServerUrl.Text = _source.ServerUrl ?? string.Empty;
                    txtUsername.Text = _source.Username ?? string.Empty;
                    txtPassword.Text = _source.Password ?? string.Empty;
                    if (_source.Port.HasValue)
                        txtPort.Text = _source.Port.Value.ToString();
                    break;

                case SourceType.M3U:
                    txtM3UUrl.Text = _source.M3UUrl ?? string.Empty;
                    txtM3UFilePath.Text = _source.M3UFilePath ?? string.Empty;
                    break;
            }
        }

        private void CmbSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFieldsVisibility();
        }

        private void UpdateFieldsVisibility()
        {
            int selectedType = cmbSourceType.SelectedIndex;

            // Show/hide panels based on source type
            panelXtreamEnigma.Visible = selectedType == 0 || selectedType == 1; // Xtream or Enigma2
            panelM3U.Visible = selectedType == 2; // M3U

            // Update labels based on source type
            if (selectedType == 1) // Enigma2
            {
                lblPort.Text = "Port (typically 80):";
                lblUsername.Text = "Username (optional):";
                lblPassword.Text = "Password (optional):";
                txtUsername.PlaceholderText = "Leave empty if no auth";
                txtPassword.PlaceholderText = "Leave empty if no auth";
            }
            else // Xtream Codes or default
            {
                lblPort.Text = "Port (optional):";
                lblUsername.Text = "Username:";
                lblPassword.Text = "Password:";
                txtUsername.PlaceholderText = "";
                txtPassword.PlaceholderText = "";
            }
        }

        private void RefreshEpgList()
        {
            lstEpgUrls.Items.Clear();
            foreach (var url in _epgUrls)
            {
                lstEpgUrls.Items.Add(url);
            }
            btnRemoveEpg.Enabled = lstEpgUrls.SelectedItems.Count > 0;
        }

        private void BtnAddEpg_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEpgUrl.Text))
            {
                _epgUrls.Add(txtEpgUrl.Text.Trim());
                txtEpgUrl.Clear();
                RefreshEpgList();
            }
        }

        private void BtnRemoveEpg_Click(object sender, EventArgs e)
        {
            if (lstEpgUrls.SelectedItems.Count > 0)
            {
                var selectedIndex = lstEpgUrls.SelectedIndex;
                _epgUrls.RemoveAt(selectedIndex);
                RefreshEpgList();
            }
        }

        private void LstEpgUrls_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveEpg.Enabled = lstEpgUrls.SelectedItems.Count > 0;
        }

        private void BtnBrowseM3U_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "M3U Playlist Files (*.m3u;*.m3u8)|*.m3u;*.m3u8|All Files (*.*)|*.*",
                Title = "Select M3U Playlist File"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtM3UFilePath.Text = dialog.FileName;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            if (_source == null)
                _source = new IptvSource();

            _source.Name = txtName.Text.Trim();
            _source.Type = (SourceType)cmbSourceType.SelectedIndex;
            _source.IsActive = chkActive.Checked;
            _source.EpgUrls = new List<string>(_epgUrls);

            switch (_source.Type)
            {
                case SourceType.XtreamCodes:
                case SourceType.Enigma2:
                    _source.ServerUrl = txtServerUrl.Text.Trim();
                    _source.Username = txtUsername.Text.Trim();
                    _source.Password = txtPassword.Text;
                    if (int.TryParse(txtPort.Text, out int port))
                        _source.Port = port;
                    break;

                case SourceType.M3U:
                    _source.M3UUrl = txtM3UUrl.Text.Trim();
                    _source.M3UFilePath = txtM3UFilePath.Text.Trim();
                    break;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name for this source.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            int sourceType = cmbSourceType.SelectedIndex;

            if (sourceType == 0 || sourceType == 1) // Xtream or Enigma2
            {
                if (string.IsNullOrWhiteSpace(txtServerUrl.Text))
                {
                    MessageBox.Show("Please enter a server URL.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtServerUrl.Focus();
                    return false;
                }

                // Username is required for Xtream Codes, but optional for Enigma2
                if (sourceType == 0 && string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Please enter a username for Xtream Codes.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return false;
                }
                // For Enigma2, credentials are optional (some boxes don't require authentication)
            }
            else if (sourceType == 2) // M3U
            {
                if (string.IsNullOrWhiteSpace(txtM3UUrl.Text) && string.IsNullOrWhiteSpace(txtM3UFilePath.Text))
                {
                    MessageBox.Show("Please enter either an M3U URL or select a local M3U file.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
