namespace StreamingDVR.Forms
{
    partial class EpgSourceEditorForm
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
            txtName = new TextBox();
            label2 = new Label();
            txtUrl = new TextBox();
            chkActive = new CheckBox();
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
            label1.Size = new Size(63, 25);
            label1.TabIndex = 0;
            label1.Text = "Name:";
            // 
            // txtName
            // 
            txtName.Location = new Point(120, 67);
            txtName.Name = "txtName";
            txtName.Size = new Size(450, 31);
            txtName.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 115);
            label2.Name = "label2";
            label2.Size = new Size(46, 25);
            label2.TabIndex = 2;
            label2.Text = "URL:";
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(120, 112);
            txtUrl.Name = "txtUrl";
            txtUrl.PlaceholderText = "http://example.com/epg.xml.gz";
            txtUrl.Size = new Size(450, 31);
            txtUrl.TabIndex = 3;
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Location = new Point(120, 157);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(85, 29);
            chkActive.TabIndex = 4;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(370, 205);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 38);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(476, 205);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 38);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // label3
            // 
            label3.Location = new Point(20, 20);
            label3.Name = "label3";
            label3.Size = new Size(550, 35);
            label3.TabIndex = 7;
            label3.Text = "Enter the details for the EPG source (typically an XML or XMLTV file URL).";
            // 
            // EpgSourceEditorForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(594, 263);
            Controls.Add(label3);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(chkActive);
            Controls.Add(txtUrl);
            Controls.Add(label2);
            Controls.Add(txtName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EpgSourceEditorForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "EPG Source";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label label1;
        private TextBox txtName;
        private Label label2;
        private TextBox txtUrl;
        private CheckBox chkActive;
        private Button btnSave;
        private Button btnCancel;
        private Label label3;
    }
}
