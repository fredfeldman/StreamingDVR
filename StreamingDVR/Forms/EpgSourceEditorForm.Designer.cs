namespace StreamingDVR.Forms
{
    partial class EpgSourceEditorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblDescription = new Label();
            lblName        = new Label();
            txtName        = new TextBox();
            grpSourceType  = new GroupBox();
            rdoUrl         = new RadioButton();
            rdoFile        = new RadioButton();
            lblSource      = new Label();
            txtUrl         = new TextBox();
            txtFilePath    = new TextBox();
            btnBrowse      = new Button();
            chkActive      = new CheckBox();
            btnSave        = new Button();
            btnCancel      = new Button();
            grpSourceType.SuspendLayout();
            SuspendLayout();
            // 
            // lblDescription
            // 
            lblDescription.Location  = new Point(16, 16);
            lblDescription.Name      = "lblDescription";
            lblDescription.Size      = new Size(628, 36);
            lblDescription.Text      = "Configure an XMLTV EPG source. Choose a URL to fetch from the internet or a local file on disk.";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(16, 68);
            lblName.Name     = "lblName";
            lblName.Text     = "Name:";
            // 
            // txtName
            // 
            txtName.Location        = new Point(130, 65);
            txtName.Name            = "txtName";
            txtName.Size            = new Size(510, 31);
            txtName.PlaceholderText = "e.g. UK Freeview EPG";
            txtName.TabIndex        = 0;
            // 
            // grpSourceType
            // 
            grpSourceType.Controls.Add(rdoUrl);
            grpSourceType.Controls.Add(rdoFile);
            grpSourceType.Location = new Point(16, 112);
            grpSourceType.Name     = "grpSourceType";
            grpSourceType.Size     = new Size(628, 55);
            grpSourceType.Text     = "Source Type";
            grpSourceType.TabIndex = 1;
            // 
            // rdoUrl
            // 
            rdoUrl.AutoSize  = true;
            rdoUrl.Checked   = true;
            rdoUrl.Location  = new Point(16, 20);
            rdoUrl.Name      = "rdoUrl";
            rdoUrl.TabIndex  = 0;
            rdoUrl.Text      = "URL  (fetched over HTTP/HTTPS)";
            rdoUrl.CheckedChanged += RdoSourceType_CheckedChanged;
            // 
            // rdoFile
            // 
            rdoFile.AutoSize = true;
            rdoFile.Location = new Point(310, 20);
            rdoFile.Name     = "rdoFile";
            rdoFile.TabIndex = 1;
            rdoFile.Text     = "Local File  (XMLTV .xml / .gz)";
            // 
            // lblSource
            // 
            lblSource.AutoSize = true;
            lblSource.Location = new Point(16, 186);
            lblSource.Name     = "lblSource";
            lblSource.Text     = "URL:";
            // 
            // txtUrl
            // 
            txtUrl.Location        = new Point(130, 183);
            txtUrl.Name            = "txtUrl";
            txtUrl.PlaceholderText = "https://example.com/epg.xml.gz";
            txtUrl.Size            = new Size(514, 31);
            txtUrl.TabIndex        = 2;
            // 
            // txtFilePath
            // 
            txtFilePath.Location        = new Point(130, 183);
            txtFilePath.Name            = "txtFilePath";
            txtFilePath.PlaceholderText = "C:\\EPG\\guide.xml";
            txtFilePath.Size            = new Size(418, 31);
            txtFilePath.TabIndex        = 2;
            txtFilePath.Visible         = false;
            // 
            // btnBrowse
            // 
            btnBrowse.Location        = new Point(556, 183);
            btnBrowse.Name            = "btnBrowse";
            btnBrowse.Size            = new Size(88, 31);
            btnBrowse.TabIndex        = 3;
            btnBrowse.Text            = "Browse…";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Visible         = false;
            btnBrowse.Click           += BtnBrowse_Click;
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Checked  = true;
            chkActive.Location = new Point(130, 232);
            chkActive.Name     = "chkActive";
            chkActive.TabIndex = 4;
            chkActive.Text     = "Active";
            chkActive.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(440, 280);
            btnSave.Name     = "btnSave";
            btnSave.Size     = new Size(100, 38);
            btnSave.TabIndex = 5;
            btnSave.Text     = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click   += BtnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(548, 280);
            btnCancel.Name     = "btnCancel";
            btnCancel.Size     = new Size(100, 38);
            btnCancel.TabIndex = 6;
            btnCancel.Text     = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click   += BtnCancel_Click;
            // 
            // EpgSourceEditorForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode       = AutoScaleMode.Font;
            ClientSize          = new Size(664, 336);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(chkActive);
            Controls.Add(btnBrowse);
            Controls.Add(txtFilePath);
            Controls.Add(txtUrl);
            Controls.Add(lblSource);
            Controls.Add(grpSourceType);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Controls.Add(lblDescription);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            Name            = "EpgSourceEditorForm";
            StartPosition   = FormStartPosition.CenterParent;
            Text            = "EPG Source";
            grpSourceType.ResumeLayout(false);
            grpSourceType.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label       lblDescription;
        private Label       lblName;
        private TextBox     txtName;
        private GroupBox    grpSourceType;
        private RadioButton rdoUrl;
        private RadioButton rdoFile;
        private Label       lblSource;
        private TextBox     txtUrl;
        private TextBox     txtFilePath;
        private Button      btnBrowse;
        private CheckBox    chkActive;
        private Button      btnSave;
        private Button      btnCancel;
    }
}
