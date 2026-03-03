using StreamingDVR.Models;
using StreamingDVR.Services;

namespace StreamingDVR.Forms
{
    public partial class EpgSourceManagerForm : Form
    {
        private readonly ConfigurationService _configService;
        private List<EpgSource> _epgSources = new();

        public EpgSourceManagerForm(ConfigurationService configService)
        {
            _configService = configService;
            InitializeComponent();
            LoadEpgSources();
        }

        private void LoadEpgSources()
        {
            var config = _configService.LoadConfiguration();
            _epgSources = config.EpgSources;
            RefreshList();
        }

        private void RefreshList()
        {
            lstEpgSources.Items.Clear();
            foreach (var source in _epgSources)
            {
                var item = new ListViewItem(source.Name);
                item.SubItems.Add(source.Url);
                item.SubItems.Add(source.IsActive ? "Active" : "Inactive");
                item.SubItems.Add(source.LastUpdated?.ToString("g") ?? "Never");
                item.Tag = source;
                lstEpgSources.Items.Add(item);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using var form = new EpgSourceEditorForm(null);
            if (form.ShowDialog() == DialogResult.OK && form.EpgSource != null)
            {
                _epgSources.Add(form.EpgSource);
                SaveAndRefresh();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (lstEpgSources.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an EPG source to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var source = (EpgSource)lstEpgSources.SelectedItems[0].Tag;
            using var form = new EpgSourceEditorForm(source);
            if (form.ShowDialog() == DialogResult.OK)
            {
                SaveAndRefresh();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (lstEpgSources.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an EPG source to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var source = (EpgSource)lstEpgSources.SelectedItems[0].Tag;
            var result = MessageBox.Show(
                $"Are you sure you want to delete the EPG source '{source.Name}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _epgSources.Remove(source);
                SaveAndRefresh();
            }
        }

        private void BtnToggleActive_Click(object sender, EventArgs e)
        {
            if (lstEpgSources.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an EPG source to toggle.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var source = (EpgSource)lstEpgSources.SelectedItems[0].Tag;
            source.IsActive = !source.IsActive;
            SaveAndRefresh();
        }

        private void SaveAndRefresh()
        {
            var config = _configService.LoadConfiguration();
            config.EpgSources = _epgSources;
            _configService.SaveConfiguration(config);
            RefreshList();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void LstEpgSources_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }
    }
}
