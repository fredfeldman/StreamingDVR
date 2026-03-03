namespace StreamingDVR.Forms
{
    partial class AssignEpgForm
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
            label1 = new Label();
            cboEpgSource = new ComboBox();
            label2 = new Label();
            txtEpgChannelId = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            label3 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 70);
            label1.Name = "label1";
            label1.Size = new Size(105, 25);
            label1.TabIndex = 0;
            label1.Text = "EPG Source:";
            // 
            // cboEpgSource
            // 
            cboEpgSource.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEpgSource.FormattingEnabled = true;
            cboEpgSource.Location = new Point(160, 67);
            cboEpgSource.Name = "cboEpgSource";
            cboEpgSource.Size = new Size(400, 33);
            cboEpgSource.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 115);
            label2.Name = "label2";
            label2.Size = new Size(134, 25);
            label2.TabIndex = 2;
            label2.Text = "EPG Channel ID:";
            // 
            // txtEpgChannelId
            // 
            txtEpgChannelId.Location = new Point(160, 112);
            txtEpgChannelId.Name = "txtEpgChannelId";
            txtEpgChannelId.PlaceholderText = "(Optional - leave blank to auto-match)";
            txtEpgChannelId.Size = new Size(400, 31);
            txtEpgChannelId.TabIndex = 3;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(360, 165);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 38);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(466, 165);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 38);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // label3
            // 
            label3.Location = new Point(20, 20);
            label3.Name = "label3";
            label3.Size = new Size(540, 35);
            label3.TabIndex = 6;
            label3.Text = "Assign an EPG source to this channel for program guide information.";
            // 
            // AssignEpgForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 223);
            Controls.Add(label3);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtEpgChannelId);
            Controls.Add(label2);
            Controls.Add(cboEpgSource);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AssignEpgForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Assign EPG to Channel";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label label1;
        private ComboBox cboEpgSource;
        private Label label2;
        private TextBox txtEpgChannelId;
        private Button btnSave;
        private Button btnCancel;
        private Label label3;
    }
}
