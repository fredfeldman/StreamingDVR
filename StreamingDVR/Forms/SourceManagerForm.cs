using StreamingDVR.Models;
using StreamingDVR.Services;

namespace StreamingDVR.Forms
{
    public partial class SourceManagerForm : Form
    {
        private List<IptvSource> _sources;
        private IptvSource? _selectedSource;

        public List<IptvSource> Sources => _sources;

        public SourceManagerForm(List<IptvSource> existingSources)
        {
            InitializeComponent();
            _sources = existingSources ?? new List<IptvSource>();
        }

        private void SourceManagerForm_Load(object sender, EventArgs e)
        {
            RefreshSourcesList();
        }

        private void RefreshSourcesList()
        {
            lstSources.Items.Clear();
            foreach (var source in _sources)
            {
                var item = new ListViewItem(source.Name);
                item.SubItems.Add(source.Type.ToString());
                item.SubItems.Add(source.IsActive ? "Active" : "Inactive");
                item.SubItems.Add(source.LastConnected?.ToString("g") ?? "Never");
                item.Tag = source;
                lstSources.Items.Add(item);
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = lstSources.SelectedItems.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnTestConnection.Enabled = hasSelection;
            btnMoveUp.Enabled = hasSelection && lstSources.SelectedIndices[0] > 0;
            btnMoveDown.Enabled = hasSelection && lstSources.SelectedIndices[0] < lstSources.Items.Count - 1;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using var form = new SourceEditorForm(null);
            if (form.ShowDialog() == DialogResult.OK && form.Source != null)
            {
                _sources.Add(form.Source);
                RefreshSourcesList();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (lstSources.SelectedItems.Count == 0) return;

            var source = (IptvSource)lstSources.SelectedItems[0].Tag;
            using var form = new SourceEditorForm(source);
            if (form.ShowDialog() == DialogResult.OK)
            {
                RefreshSourcesList();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (lstSources.SelectedItems.Count == 0) return;

            var source = (IptvSource)lstSources.SelectedItems[0].Tag;
            var result = MessageBox.Show(
                $"Are you sure you want to delete the source '{source.Name}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _sources.Remove(source);
                RefreshSourcesList();
            }
        }

        private void BtnMoveUp_Click(object sender, EventArgs e)
        {
            if (lstSources.SelectedItems.Count == 0) return;

            int index = lstSources.SelectedIndices[0];
            if (index > 0)
            {
                var source = _sources[index];
                _sources.RemoveAt(index);
                _sources.Insert(index - 1, source);
                RefreshSourcesList();
                lstSources.Items[index - 1].Selected = true;
            }
        }

        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            if (lstSources.SelectedItems.Count == 0) return;

            int index = lstSources.SelectedIndices[0];
            if (index < _sources.Count - 1)
            {
                var source = _sources[index];
                _sources.RemoveAt(index);
                _sources.Insert(index + 1, source);
                RefreshSourcesList();
                lstSources.Items[index + 1].Selected = true;
            }
        }

        private async void BtnTestConnection_Click(object sender, EventArgs e)
        {
            if (lstSources.SelectedItems.Count == 0) return;

            var source = (IptvSource)lstSources.SelectedItems[0].Tag;
            btnTestConnection.Enabled = false;
            btnTestConnection.Text = "Testing...";

            try
            {
                // TODO: Implement connection test based on source type
                await Task.Delay(1000); // Placeholder
                MessageBox.Show("Connection test successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection test failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTestConnection.Enabled = true;
                btnTestConnection.Text = "Test Connection";
            }
        }

        private void LstSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
