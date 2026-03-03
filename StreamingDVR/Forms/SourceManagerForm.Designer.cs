namespace StreamingDVR.Forms
{
    partial class SourceManagerForm
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
            lstSources = new ListView();
            colName = new ColumnHeader();
            colType = new ColumnHeader();
            colStatus = new ColumnHeader();
            colLastConnected = new ColumnHeader();
            panel1 = new Panel();
            btnClose = new Button();
            btnTestConnection = new Button();
            btnMoveDown = new Button();
            btnMoveUp = new Button();
            btnDelete = new Button();
            btnEdit = new Button();
            btnAdd = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lstSources
            // 
            lstSources.Columns.AddRange(new ColumnHeader[] { colName, colType, colStatus, colLastConnected });
            lstSources.Dock = DockStyle.Fill;
            lstSources.FullRowSelect = true;
            lstSources.Location = new Point(0, 0);
            lstSources.MultiSelect = false;
            lstSources.Name = "lstSources";
            lstSources.Size = new Size(884, 461);
            lstSources.TabIndex = 0;
            lstSources.UseCompatibleStateImageBehavior = false;
            lstSources.View = View.Details;
            lstSources.SelectedIndexChanged += LstSources_SelectedIndexChanged;
            // 
            // colName
            // 
            colName.Text = "Name";
            colName.Width = 250;
            // 
            // colType
            // 
            colType.Text = "Type";
            colType.Width = 150;
            // 
            // colStatus
            // 
            colStatus.Text = "Status";
            colStatus.Width = 100;
            // 
            // colLastConnected
            // 
            colLastConnected.Text = "Last Connected";
            colLastConnected.Width = 180;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnClose);
            panel1.Controls.Add(btnTestConnection);
            panel1.Controls.Add(btnMoveDown);
            panel1.Controls.Add(btnMoveUp);
            panel1.Controls.Add(btnDelete);
            panel1.Controls.Add(btnEdit);
            panel1.Controls.Add(btnAdd);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 411);
            panel1.Name = "panel1";
            panel1.Size = new Size(884, 50);
            panel1.TabIndex = 1;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 10);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 30);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add Source";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += BtnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Enabled = false;
            btnEdit.Location = new Point(118, 10);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 30);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += BtnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Enabled = false;
            btnDelete.Location = new Point(224, 10);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 30);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnMoveUp
            // 
            btnMoveUp.Enabled = false;
            btnMoveUp.Location = new Point(350, 10);
            btnMoveUp.Name = "btnMoveUp";
            btnMoveUp.Size = new Size(80, 30);
            btnMoveUp.TabIndex = 3;
            btnMoveUp.Text = "Move Up";
            btnMoveUp.UseVisualStyleBackColor = true;
            btnMoveUp.Click += BtnMoveUp_Click;
            // 
            // btnMoveDown
            // 
            btnMoveDown.Enabled = false;
            btnMoveDown.Location = new Point(436, 10);
            btnMoveDown.Name = "btnMoveDown";
            btnMoveDown.Size = new Size(100, 30);
            btnMoveDown.TabIndex = 4;
            btnMoveDown.Text = "Move Down";
            btnMoveDown.UseVisualStyleBackColor = true;
            btnMoveDown.Click += BtnMoveDown_Click;
            // 
            // btnTestConnection
            // 
            btnTestConnection.Enabled = false;
            btnTestConnection.Location = new Point(562, 10);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(130, 30);
            btnTestConnection.TabIndex = 5;
            btnTestConnection.Text = "Test Connection";
            btnTestConnection.UseVisualStyleBackColor = true;
            btnTestConnection.Click += BtnTestConnection_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(772, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 6;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;
            // 
            // SourceManagerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 461);
            Controls.Add(panel1);
            Controls.Add(lstSources);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SourceManagerForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "IPTV Source Manager";
            Load += SourceManagerForm_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private ListView lstSources;
        private ColumnHeader colName;
        private ColumnHeader colType;
        private ColumnHeader colStatus;
        private ColumnHeader colLastConnected;
        private Panel panel1;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnMoveUp;
        private Button btnMoveDown;
        private Button btnTestConnection;
        private Button btnClose;
    }
}
