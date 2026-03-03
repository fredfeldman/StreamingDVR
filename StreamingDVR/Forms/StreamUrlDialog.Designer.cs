namespace StreamingDVR.Forms
{
    partial class StreamUrlDialog
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
            lblInfo = new Label();
            txtStreamUrl = new TextBox();
            btnCopy = new Button();
            btnOpenVLC = new Button();
            btnClose = new Button();
            lblStatus = new Label();
            SuspendLayout();
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(12, 15);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(447, 20);
            lblInfo.TabIndex = 0;
            lblInfo.Text = "Copy the stream URL below to use with VLC or another media player:";
            // 
            // txtStreamUrl
            // 
            txtStreamUrl.BackColor = System.Drawing.SystemColors.Window;
            txtStreamUrl.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtStreamUrl.Location = new Point(12, 45);
            txtStreamUrl.Multiline = true;
            txtStreamUrl.Name = "txtStreamUrl";
            txtStreamUrl.ReadOnly = true;
            txtStreamUrl.ScrollBars = ScrollBars.Vertical;
            txtStreamUrl.Size = new Size(660, 80);
            txtStreamUrl.TabIndex = 1;
            // 
            // btnCopy
            // 
            btnCopy.Image = null;
            btnCopy.Location = new Point(12, 145);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(150, 40);
            btnCopy.TabIndex = 2;
            btnCopy.Text = "📋 Copy URL";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += BtnCopy_Click;
            // 
            // btnOpenVLC
            // 
            btnOpenVLC.Location = new Point(180, 145);
            btnOpenVLC.Name = "btnOpenVLC";
            btnOpenVLC.Size = new Size(150, 40);
            btnOpenVLC.TabIndex = 3;
            btnOpenVLC.Text = "▶ Open in Player";
            btnOpenVLC.UseVisualStyleBackColor = true;
            btnOpenVLC.Click += BtnOpenVLC_Click;
            // 
            // btnClose
            // 
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(522, 145);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(150, 40);
            btnClose.TabIndex = 4;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblStatus.Location = new Point(12, 198);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 20);
            lblStatus.TabIndex = 5;
            lblStatus.Visible = false;
            // 
            // StreamUrlDialog
            // 
            AcceptButton = btnCopy;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(684, 228);
            Controls.Add(lblStatus);
            Controls.Add(btnClose);
            Controls.Add(btnOpenVLC);
            Controls.Add(btnCopy);
            Controls.Add(txtStreamUrl);
            Controls.Add(lblInfo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "StreamUrlDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Stream URL";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblInfo;
        private TextBox txtStreamUrl;
        private Button btnCopy;
        private Button btnOpenVLC;
        private Button btnClose;
        private Label lblStatus;
    }
}
