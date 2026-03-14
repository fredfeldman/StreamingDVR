namespace StreamingDVR.Forms
{
    partial class StartupProgressForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle    = new Label();
            lblStatus   = new Label();
            pbProgress  = new ProgressBar();
            lstSources  = new ListView();
            colResult   = new ColumnHeader();
            lstLog      = new ListBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize  = false;
            lblTitle.Dock      = DockStyle.Top;
            lblTitle.Height    = 44;
            lblTitle.Font      = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.FromArgb(32, 42, 72);
            lblTitle.Text      = "  StreamingDVR — Starting Up";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Name      = "lblTitle";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize  = true;
            lblStatus.Location  = new Point(16, 58);
            lblStatus.Name      = "lblStatus";
            lblStatus.Size      = new Size(460, 25);
            lblStatus.Text      = "Initialising…";
            // 
            // pbProgress
            // 
            pbProgress.Location = new Point(16, 86);
            pbProgress.Name     = "pbProgress";
            pbProgress.Size     = new Size(460, 22);
            pbProgress.Style    = ProgressBarStyle.Continuous;
            // 
            // lstSources
            // 
            lstSources.Columns.AddRange(new ColumnHeader[] { colResult });
            lstSources.HeaderStyle   = ColumnHeaderStyle.None;
            lstSources.Location      = new Point(16, 120);
            lstSources.Name          = "lstSources";
            lstSources.Size          = new Size(460, 120);
            lstSources.View          = View.Details;
            lstSources.BackColor     = Color.FromArgb(20, 20, 30);
            lstSources.ForeColor     = Color.White;
            lstSources.FullRowSelect = true;
            // 
            // colResult
            // 
            colResult.Width = 456;
            // 
            // lstLog
            // 
            lstLog.Location      = new Point(16, 252);
            lstLog.Name          = "lstLog";
            lstLog.Size          = new Size(460, 80);
            lstLog.BackColor     = Color.FromArgb(15, 15, 22);
            lstLog.ForeColor     = Color.FromArgb(140, 150, 170);
            lstLog.Font          = new Font("Consolas", 7.5f);
            lstLog.BorderStyle   = BorderStyle.FixedSingle;
            // 
            // StartupProgressForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode       = AutoScaleMode.Font;
            BackColor           = Color.FromArgb(28, 28, 38);
            ForeColor           = Color.White;
            ClientSize          = new Size(492, 348);
            Controls.Add(lblTitle);
            Controls.Add(lblStatus);
            Controls.Add(pbProgress);
            Controls.Add(lstSources);
            Controls.Add(lstLog);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            Name            = "StartupProgressForm";
            StartPosition   = FormStartPosition.CenterScreen;
            Text            = "Starting StreamingDVR…";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label      lblTitle;
        private Label      lblStatus;
        private ProgressBar pbProgress;
        private ListView   lstSources;
        private ColumnHeader colResult;
        private ListBox    lstLog;
    }
}
