using StreamingDVR.Models;

namespace StreamingDVR.Forms
{
    public partial class EpgSourceEditorForm : Form
    {
        public EpgSource? EpgSource { get; private set; }
        private readonly bool _isEditMode;

        public EpgSourceEditorForm(EpgSource? epgSource)
        {
            InitializeComponent();
            _isEditMode = epgSource != null;
            EpgSource   = epgSource ?? new EpgSource();

            if (_isEditMode)
            {
                Text = "Edit EPG Source";
                txtName.Text   = EpgSource.Name;
                chkActive.Checked = EpgSource.IsActive;

                if (EpgSource.SourceType == EpgSourceType.File)
                {
                    rdoFile.Checked    = true;
                    txtFilePath.Text   = EpgSource.FilePath;
                }
                else
                {
                    rdoUrl.Checked = true;
                    txtUrl.Text    = EpgSource.Url;
                }
            }
            else
            {
                Text = "Add EPG Source";
                chkActive.Checked = true;
            }

            UpdateSourceTypeUI();
        }

        // ── UI toggle ────────────────────────────────────────────────────────
        private void RdoSourceType_CheckedChanged(object sender, EventArgs e)
            => UpdateSourceTypeUI();

        private void UpdateSourceTypeUI()
        {
            bool isFile = rdoFile.Checked;

            lblSource.Text       = isFile ? "File:" : "URL:";
            txtUrl.Visible       = !isFile;
            txtFilePath.Visible  = isFile;
            btnBrowse.Visible    = isFile;
        }

        // ── Browse ───────────────────────────────────────────────────────────
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title       = "Select XMLTV EPG File",
                Filter      = "XMLTV files (*.xml;*.xml.gz;*.gz)|*.xml;*.xml.gz;*.gz|All files (*.*)|*.*",
                FilterIndex = 1
            };

            if (!string.IsNullOrWhiteSpace(txtFilePath.Text) &&
                Directory.Exists(Path.GetDirectoryName(txtFilePath.Text)))
            {
                dlg.InitialDirectory = Path.GetDirectoryName(txtFilePath.Text)!;
            }

            if (dlg.ShowDialog() == DialogResult.OK)
                txtFilePath.Text = dlg.FileName;
        }

        // ── Save ─────────────────────────────────────────────────────────────
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name for the EPG source.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (rdoUrl.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtUrl.Text))
                {
                    MessageBox.Show("Please enter a URL for the EPG source.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUrl.Focus();
                    return;
                }

                if (!Uri.TryCreate(txtUrl.Text.Trim(), UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    MessageBox.Show("Please enter a valid HTTP or HTTPS URL.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUrl.Focus();
                    return;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtFilePath.Text))
                {
                    MessageBox.Show("Please select a file path for the EPG source.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnBrowse.Focus();
                    return;
                }

                if (!File.Exists(txtFilePath.Text.Trim()))
                {
                    MessageBox.Show("The specified file does not exist.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnBrowse.Focus();
                    return;
                }
            }

            EpgSource!.Name       = txtName.Text.Trim();
            EpgSource.IsActive    = chkActive.Checked;
            EpgSource.SourceType  = rdoFile.Checked ? EpgSourceType.File : EpgSourceType.Url;
            EpgSource.Url         = rdoUrl.Checked  ? txtUrl.Text.Trim()      : string.Empty;
            EpgSource.FilePath    = rdoFile.Checked ? txtFilePath.Text.Trim() : string.Empty;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
