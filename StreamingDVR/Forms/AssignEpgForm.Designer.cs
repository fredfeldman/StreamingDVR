namespace StreamingDVR.Forms
{
    partial class AssignEpgForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle         = new Label();
            lblSource        = new Label();
            cboEpgSource     = new ComboBox();
            btnLoadChannels  = new Button();
            lblChannelSearch = new Label();
            txtSearch        = new TextBox();
            lstChannels      = new ListView();
            colDisplayName   = new ColumnHeader();
            colChannelId     = new ColumnHeader();
            lblManual        = new Label();
            txtEpgChannelId  = new TextBox();
            lblStatus        = new Label();
            btnSave          = new Button();
            btnClear         = new Button();
            btnCancel        = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(16, 14);
            lblTitle.Name     = "lblTitle";
            lblTitle.Size     = new Size(668, 36);
            lblTitle.Text     = "Assign an EPG source to this channel. Select a source, load its channel list, then pick a match.";
            // 
            // lblSource
            // 
            lblSource.AutoSize = true;
            lblSource.Location = new Point(16, 62);
            lblSource.Name     = "lblSource";
            lblSource.Text     = "EPG Source:";
            // 
            // cboEpgSource
            // 
            cboEpgSource.DropDownStyle     = ComboBoxStyle.DropDownList;
            cboEpgSource.FormattingEnabled = true;
            cboEpgSource.Location          = new Point(130, 59);
            cboEpgSource.Name              = "cboEpgSource";
            cboEpgSource.Size              = new Size(400, 33);
            cboEpgSource.TabIndex          = 0;
            cboEpgSource.SelectedIndexChanged += CboEpgSource_SelectedIndexChanged;
            // 
            // btnLoadChannels
            // 
            btnLoadChannels.Location = new Point(540, 59);
            btnLoadChannels.Name     = "btnLoadChannels";
            btnLoadChannels.Size     = new Size(144, 33);
            btnLoadChannels.TabIndex = 1;
            btnLoadChannels.Text     = "Load Channels";
            btnLoadChannels.UseVisualStyleBackColor = true;
            btnLoadChannels.Click   += BtnLoadChannels_Click;
            // 
            // lblChannelSearch
            // 
            lblChannelSearch.AutoSize = true;
            lblChannelSearch.Location = new Point(16, 106);
            lblChannelSearch.Name     = "lblChannelSearch";
            lblChannelSearch.Text     = "Search:";
            // 
            // txtSearch
            // 
            txtSearch.Location        = new Point(130, 103);
            txtSearch.Name            = "txtSearch";
            txtSearch.PlaceholderText = "Filter channels…";
            txtSearch.Size            = new Size(554, 31);
            txtSearch.TabIndex        = 2;
            txtSearch.TextChanged    += TxtSearch_TextChanged;
            // 
            // lstChannels
            // 
            lstChannels.Columns.AddRange(new ColumnHeader[] { colDisplayName, colChannelId });
            lstChannels.FullRowSelect  = true;
            lstChannels.HideSelection  = false;
            lstChannels.Location       = new Point(16, 144);
            lstChannels.MultiSelect    = false;
            lstChannels.Name           = "lstChannels";
            lstChannels.Size           = new Size(668, 220);
            lstChannels.TabIndex       = 3;
            lstChannels.View           = View.Details;
            lstChannels.SelectedIndexChanged += LstChannels_SelectedIndexChanged;
            lstChannels.DoubleClick   += LstChannels_DoubleClick;
            // 
            // colDisplayName
            // 
            colDisplayName.Text  = "Channel Name";
            colDisplayName.Width = 360;
            // 
            // colChannelId
            // 
            colChannelId.Text  = "EPG Channel ID";
            colChannelId.Width = 300;
            // 
            // lblManual
            // 
            lblManual.AutoSize = true;
            lblManual.Location = new Point(16, 380);
            lblManual.Name     = "lblManual";
            lblManual.Text     = "Channel ID:";
            // 
            // txtEpgChannelId
            // 
            txtEpgChannelId.Location        = new Point(130, 377);
            txtEpgChannelId.Name            = "txtEpgChannelId";
            txtEpgChannelId.PlaceholderText = "Select from list above or type an ID manually";
            txtEpgChannelId.Size            = new Size(554, 31);
            txtEpgChannelId.TabIndex        = 4;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = false;
            lblStatus.Location = new Point(16, 420);
            lblStatus.Name     = "lblStatus";
            lblStatus.Size     = new Size(500, 26);
            lblStatus.ForeColor = System.Drawing.SystemColors.GrayText;
            lblStatus.Text     = "";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(426, 460);
            btnSave.Name     = "btnSave";
            btnSave.Size     = new Size(120, 38);
            btnSave.TabIndex = 5;
            btnSave.Text     = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click   += BtnSave_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(554, 460);
            btnClear.Name     = "btnClear";
            btnClear.Size     = new Size(120, 38);
            btnClear.TabIndex = 6;
            btnClear.Text     = "Clear Mapping";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click   += BtnClear_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(556, 460);
            btnCancel.Name     = "btnCancel";
            btnCancel.Size     = new Size(120, 38);
            btnCancel.TabIndex = 7;
            btnCancel.Text     = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click   += BtnCancel_Click;
            // 
            // AssignEpgForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode       = AutoScaleMode.Font;
            ClientSize          = new Size(700, 516);
            Controls.Add(lblTitle);
            Controls.Add(lblSource);
            Controls.Add(cboEpgSource);
            Controls.Add(btnLoadChannels);
            Controls.Add(lblChannelSearch);
            Controls.Add(txtSearch);
            Controls.Add(lstChannels);
            Controls.Add(lblManual);
            Controls.Add(txtEpgChannelId);
            Controls.Add(lblStatus);
            Controls.Add(btnSave);
            Controls.Add(btnClear);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            Name            = "AssignEpgForm";
            StartPosition   = FormStartPosition.CenterParent;
            Text            = "Configure EPG for Channel";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label       lblTitle;
        private Label       lblSource;
        private ComboBox    cboEpgSource;
        private Button      btnLoadChannels;
        private Label       lblChannelSearch;
        private TextBox     txtSearch;
        private ListView    lstChannels;
        private ColumnHeader colDisplayName;
        private ColumnHeader colChannelId;
        private Label       lblManual;
        private TextBox     txtEpgChannelId;
        private Label       lblStatus;
        private Button      btnSave;
        private Button      btnClear;
        private Button      btnCancel;
    }
}
