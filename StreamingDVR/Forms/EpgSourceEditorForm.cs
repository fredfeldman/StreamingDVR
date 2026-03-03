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
            EpgSource = epgSource ?? new EpgSource();
            
            if (_isEditMode)
            {
                Text = "Edit EPG Source";
                txtName.Text = EpgSource.Name;
                txtUrl.Text = EpgSource.Url;
                chkActive.Checked = EpgSource.IsActive;
            }
            else
            {
                Text = "Add EPG Source";
                chkActive.Checked = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name for the EPG source.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                MessageBox.Show("Please enter a URL for the EPG source.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUrl.Focus();
                return;
            }

            if (!Uri.TryCreate(txtUrl.Text, UriKind.Absolute, out _))
            {
                MessageBox.Show("Please enter a valid URL.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUrl.Focus();
                return;
            }

            if (EpgSource != null)
            {
                EpgSource.Name = txtName.Text.Trim();
                EpgSource.Url = txtUrl.Text.Trim();
                EpgSource.IsActive = chkActive.Checked;
            }

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
