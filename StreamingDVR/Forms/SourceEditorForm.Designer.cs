namespace StreamingDVR.Forms
{
    partial class SourceEditorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblName = new Label();
            txtName = new TextBox();
            lblSourceType = new Label();
            cmbSourceType = new ComboBox();
            chkActive = new CheckBox();
            groupBoxConnection = new GroupBox();
            panelXtreamEnigma = new Panel();
            lblServerUrl = new Label();
            txtServerUrl = new TextBox();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblPort = new Label();
            txtPort = new TextBox();
            panelM3U = new Panel();
            lblM3UUrl = new Label();
            txtM3UUrl = new TextBox();
            lblM3UFile = new Label();
            txtM3UFilePath = new TextBox();
            btnBrowseM3U = new Button();
            groupBoxEpg = new GroupBox();
            lblEpgUrl = new Label();
            txtEpgUrl = new TextBox();
            btnAddEpg = new Button();
            lstEpgUrls = new ListBox();
            btnRemoveEpg = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            groupBoxConnection.SuspendLayout();
            panelXtreamEnigma.SuspendLayout();
            panelM3U.SuspendLayout();
            groupBoxEpg.SuspendLayout();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(12, 15);
            lblName.Name = "lblName";
            lblName.Size = new Size(97, 20);
            lblName.TabIndex = 0;
            lblName.Text = "Source Name:";
            // 
            // txtName
            // 
            txtName.Location = new Point(115, 12);
            txtName.Name = "txtName";
            txtName.Size = new Size(350, 27);
            txtName.TabIndex = 1;
            // 
            // lblSourceType
            // 
            lblSourceType.AutoSize = true;
            lblSourceType.Location = new Point(12, 55);
            lblSourceType.Name = "lblSourceType";
            lblSourceType.Size = new Size(90, 20);
            lblSourceType.TabIndex = 2;
            lblSourceType.Text = "Source Type:";
            // 
            // cmbSourceType
            // 
            cmbSourceType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSourceType.FormattingEnabled = true;
            cmbSourceType.Location = new Point(115, 52);
            cmbSourceType.Name = "cmbSourceType";
            cmbSourceType.Size = new Size(250, 28);
            cmbSourceType.TabIndex = 3;
            cmbSourceType.SelectedIndexChanged += CmbSourceType_SelectedIndexChanged;
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Location = new Point(480, 15);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(72, 24);
            chkActive.TabIndex = 4;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            // 
            // groupBoxConnection
            // 
            groupBoxConnection.Controls.Add(panelXtreamEnigma);
            groupBoxConnection.Controls.Add(panelM3U);
            groupBoxConnection.Location = new Point(12, 95);
            groupBoxConnection.Name = "groupBoxConnection";
            groupBoxConnection.Size = new Size(560, 220);
            groupBoxConnection.TabIndex = 5;
            groupBoxConnection.TabStop = false;
            groupBoxConnection.Text = "Connection Settings";
            // 
            // panelXtreamEnigma
            // 
            panelXtreamEnigma.Controls.Add(lblServerUrl);
            panelXtreamEnigma.Controls.Add(txtServerUrl);
            panelXtreamEnigma.Controls.Add(lblUsername);
            panelXtreamEnigma.Controls.Add(txtUsername);
            panelXtreamEnigma.Controls.Add(lblPassword);
            panelXtreamEnigma.Controls.Add(txtPassword);
            panelXtreamEnigma.Controls.Add(lblPort);
            panelXtreamEnigma.Controls.Add(txtPort);
            panelXtreamEnigma.Dock = DockStyle.Fill;
            panelXtreamEnigma.Location = new Point(3, 23);
            panelXtreamEnigma.Name = "panelXtreamEnigma";
            panelXtreamEnigma.Size = new Size(554, 194);
            panelXtreamEnigma.TabIndex = 0;
            // 
            // lblServerUrl
            // 
            lblServerUrl.AutoSize = true;
            lblServerUrl.Location = new Point(15, 20);
            lblServerUrl.Name = "lblServerUrl";
            lblServerUrl.Size = new Size(85, 20);
            lblServerUrl.TabIndex = 0;
            lblServerUrl.Text = "Server URL:";
            // 
            // txtServerUrl
            // 
            txtServerUrl.Location = new Point(120, 17);
            txtServerUrl.Name = "txtServerUrl";
            txtServerUrl.PlaceholderText = "http://server:port";
            txtServerUrl.Size = new Size(420, 27);
            txtServerUrl.TabIndex = 1;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(15, 60);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(78, 20);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(180, 57);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(360, 27);
            txtUsername.TabIndex = 3;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(15, 100);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(73, 20);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(180, 97);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new Size(360, 27);
            txtPassword.TabIndex = 5;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(15, 140);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(108, 20);
            lblPort.TabIndex = 6;
            lblPort.Text = "Port (optional):";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(140, 137);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(100, 27);
            txtPort.TabIndex = 7;
            // 
            // panelM3U
            // 
            panelM3U.Controls.Add(lblM3UUrl);
            panelM3U.Controls.Add(txtM3UUrl);
            panelM3U.Controls.Add(lblM3UFile);
            panelM3U.Controls.Add(txtM3UFilePath);
            panelM3U.Controls.Add(btnBrowseM3U);
            panelM3U.Dock = DockStyle.Fill;
            panelM3U.Location = new Point(3, 23);
            panelM3U.Name = "panelM3U";
            panelM3U.Size = new Size(554, 194);
            panelM3U.TabIndex = 1;
            panelM3U.Visible = false;
            // 
            // lblM3UUrl
            // 
            lblM3UUrl.AutoSize = true;
            lblM3UUrl.Location = new Point(15, 20);
            lblM3UUrl.Name = "lblM3UUrl";
            lblM3UUrl.Size = new Size(74, 20);
            lblM3UUrl.TabIndex = 0;
            lblM3UUrl.Text = "M3U URL:";
            // 
            // txtM3UUrl
            // 
            txtM3UUrl.Location = new Point(120, 17);
            txtM3UUrl.Name = "txtM3UUrl";
            txtM3UUrl.PlaceholderText = "http://example.com/playlist.m3u";
            txtM3UUrl.Size = new Size(420, 27);
            txtM3UUrl.TabIndex = 1;
            // 
            // lblM3UFile
            // 
            lblM3UFile.AutoSize = true;
            lblM3UFile.Location = new Point(15, 60);
            lblM3UFile.Name = "lblM3UFile";
            lblM3UFile.Size = new Size(99, 20);
            lblM3UFile.TabIndex = 2;
            lblM3UFile.Text = "Or Local File:";
            // 
            // txtM3UFilePath
            // 
            txtM3UFilePath.Location = new Point(120, 57);
            txtM3UFilePath.Name = "txtM3UFilePath";
            txtM3UFilePath.ReadOnly = true;
            txtM3UFilePath.Size = new Size(330, 27);
            txtM3UFilePath.TabIndex = 3;
            // 
            // btnBrowseM3U
            // 
            btnBrowseM3U.Location = new Point(456, 55);
            btnBrowseM3U.Name = "btnBrowseM3U";
            btnBrowseM3U.Size = new Size(84, 30);
            btnBrowseM3U.TabIndex = 4;
            btnBrowseM3U.Text = "Browse...";
            btnBrowseM3U.UseVisualStyleBackColor = true;
            btnBrowseM3U.Click += BtnBrowseM3U_Click;
            // 
            // groupBoxEpg
            // 
            groupBoxEpg.Controls.Add(lblEpgUrl);
            groupBoxEpg.Controls.Add(txtEpgUrl);
            groupBoxEpg.Controls.Add(btnAddEpg);
            groupBoxEpg.Controls.Add(lstEpgUrls);
            groupBoxEpg.Controls.Add(btnRemoveEpg);
            groupBoxEpg.Location = new Point(12, 325);
            groupBoxEpg.Name = "groupBoxEpg";
            groupBoxEpg.Size = new Size(560, 200);
            groupBoxEpg.TabIndex = 6;
            groupBoxEpg.TabStop = false;
            groupBoxEpg.Text = "Additional EPG Sources";
            // 
            // lblEpgUrl
            // 
            lblEpgUrl.AutoSize = true;
            lblEpgUrl.Location = new Point(15, 30);
            lblEpgUrl.Name = "lblEpgUrl";
            lblEpgUrl.Size = new Size(69, 20);
            lblEpgUrl.TabIndex = 0;
            lblEpgUrl.Text = "EPG URL:";
            // 
            // txtEpgUrl
            // 
            txtEpgUrl.Location = new Point(90, 27);
            txtEpgUrl.Name = "txtEpgUrl";
            txtEpgUrl.PlaceholderText = "http://example.com/xmltv.xml";
            txtEpgUrl.Size = new Size(370, 27);
            txtEpgUrl.TabIndex = 1;
            // 
            // btnAddEpg
            // 
            btnAddEpg.Location = new Point(466, 25);
            btnAddEpg.Name = "btnAddEpg";
            btnAddEpg.Size = new Size(75, 30);
            btnAddEpg.TabIndex = 2;
            btnAddEpg.Text = "Add";
            btnAddEpg.UseVisualStyleBackColor = true;
            btnAddEpg.Click += BtnAddEpg_Click;
            // 
            // lstEpgUrls
            // 
            lstEpgUrls.FormattingEnabled = true;
            lstEpgUrls.ItemHeight = 20;
            lstEpgUrls.Location = new Point(15, 65);
            lstEpgUrls.Name = "lstEpgUrls";
            lstEpgUrls.Size = new Size(445, 104);
            lstEpgUrls.TabIndex = 3;
            lstEpgUrls.SelectedIndexChanged += LstEpgUrls_SelectedIndexChanged;
            // 
            // btnRemoveEpg
            // 
            btnRemoveEpg.Enabled = false;
            btnRemoveEpg.Location = new Point(466, 65);
            btnRemoveEpg.Name = "btnRemoveEpg";
            btnRemoveEpg.Size = new Size(75, 30);
            btnRemoveEpg.TabIndex = 4;
            btnRemoveEpg.Text = "Remove";
            btnRemoveEpg.UseVisualStyleBackColor = true;
            btnRemoveEpg.Click += BtnRemoveEpg_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(372, 540);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 35);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(478, 540);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 35);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // SourceEditorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 587);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(groupBoxEpg);
            Controls.Add(groupBoxConnection);
            Controls.Add(chkActive);
            Controls.Add(cmbSourceType);
            Controls.Add(lblSourceType);
            Controls.Add(txtName);
            Controls.Add(lblName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SourceEditorForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "IPTV Source Editor";
            Load += SourceEditorForm_Load;
            groupBoxConnection.ResumeLayout(false);
            panelXtreamEnigma.ResumeLayout(false);
            panelXtreamEnigma.PerformLayout();
            panelM3U.ResumeLayout(false);
            panelM3U.PerformLayout();
            groupBoxEpg.ResumeLayout(false);
            groupBoxEpg.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblName;
        private TextBox txtName;
        private Label lblSourceType;
        private ComboBox cmbSourceType;
        private CheckBox chkActive;
        private GroupBox groupBoxConnection;
        private Panel panelXtreamEnigma;
        private Label lblServerUrl;
        private TextBox txtServerUrl;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblPort;
        private TextBox txtPort;
        private Panel panelM3U;
        private Label lblM3UUrl;
        private TextBox txtM3UUrl;
        private Label lblM3UFile;
        private TextBox txtM3UFilePath;
        private Button btnBrowseM3U;
        private GroupBox groupBoxEpg;
        private Label lblEpgUrl;
        private TextBox txtEpgUrl;
        private Button btnAddEpg;
        private ListBox lstEpgUrls;
        private Button btnRemoveEpg;
        private Button btnSave;
        private Button btnCancel;
    }
}
