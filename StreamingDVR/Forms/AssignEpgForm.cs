using StreamingDVR.Models;
using StreamingDVR.Services;

namespace StreamingDVR.Forms
{
    public partial class AssignEpgForm : Form
    {
        public Guid? SelectedEpgSourceId { get; private set; }
        public string? EpgChannelId { get; private set; }
        private readonly ConfigurationService _configService;
        private readonly int _streamId;
        private readonly string _channelName;

        public AssignEpgForm(ConfigurationService configService, int streamId, string channelName)
        {
            _configService = configService;
            _streamId = streamId;
            _channelName = channelName;
            InitializeComponent();
            LoadEpgSources();
            LoadExistingMapping();
        }

        private void LoadEpgSources()
        {
            var config = _configService.LoadConfiguration();
            cboEpgSource.Items.Clear();
            cboEpgSource.Items.Add("(None)");
            
            foreach (var source in config.EpgSources.Where(s => s.IsActive))
            {
                cboEpgSource.Items.Add(source);
            }
            
            cboEpgSource.SelectedIndex = 0;
        }

        private void LoadExistingMapping()
        {
            var config = _configService.LoadConfiguration();
            var mapping = config.ChannelEpgMappings.FirstOrDefault(m => m.StreamId == _streamId);
            
            if (mapping != null && mapping.EpgSourceId.HasValue)
            {
                var source = config.EpgSources.FirstOrDefault(s => s.Id == mapping.EpgSourceId.Value);
                if (source != null)
                {
                    cboEpgSource.SelectedItem = source;
                }
                txtEpgChannelId.Text = mapping.EpgChannelId ?? "";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var config = _configService.LoadConfiguration();
            
            // Remove existing mapping
            config.ChannelEpgMappings.RemoveAll(m => m.StreamId == _streamId);
            
            // Add new mapping if an EPG source is selected
            if (cboEpgSource.SelectedIndex > 0)
            {
                var selectedSource = (EpgSource)cboEpgSource.SelectedItem;
                SelectedEpgSourceId = selectedSource.Id;
                EpgChannelId = string.IsNullOrWhiteSpace(txtEpgChannelId.Text) ? null : txtEpgChannelId.Text.Trim();
                
                config.ChannelEpgMappings.Add(new ChannelEpgMapping
                {
                    StreamId = _streamId,
                    ChannelName = _channelName,
                    EpgSourceId = SelectedEpgSourceId,
                    EpgChannelId = EpgChannelId
                });
            }
            else
            {
                SelectedEpgSourceId = null;
                EpgChannelId = null;
            }
            
            _configService.SaveConfiguration(config);
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
