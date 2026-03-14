namespace StreamingDVR.Forms
{
    partial class EpgSourceManagerForm
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
            lstEpgSources = new ListView();
            colName = new ColumnHeader();
            colType = new ColumnHeader();
            colUrl = new ColumnHeader();
            colStatus = new ColumnHeader();
            colLastUpdated = new ColumnHeader();
            colCacheInfo = new ColumnHeader();
            panel1 = new Panel();
            btnClose = new Button();
            btnToggleActive = new Button();
            btnDelete = new Button();
            btnEdit = new Button();
            btnAdd = new Button();
            btnRefreshCache = new Button();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lstEpgSources
            // 
            lstEpgSources.Columns.AddRange(new ColumnHeader[] { colName, colType, colUrl, colStatus, colLastUpdated, colCacheInfo });
            lstEpgSources.Dock = DockStyle.Fill;
            lstEpgSources.FullRowSelect = true;
            lstEpgSources.Location = new Point(0, 60);
            lstEpgSources.Name = "lstEpgSources";
            lstEpgSources.Size = new Size(1000, 490);
            lstEpgSources.TabIndex = 0;
            lstEpgSources.UseCompatibleStateImageBehavior = false;
            lstEpgSources.View = View.Details;
            lstEpgSources.DoubleClick += LstEpgSources_DoubleClick;
            // 
            // colName
            // 
            colName.Text  = "Name";
            colName.Width = 180;
            // 
            // colType
            // 
            colType.Text  = "Type";
            colType.Width = 60;
            // 
            // colUrl
            // 
            colUrl.Text  = "Location";
            colUrl.Width = 410;
            // 
            // colStatus
            // 
            colStatus.Text = "Status";
            colStatus.Width = 100;
            // 
            // colLastUpdated
            // 
            colLastUpdated.Text  = "Last Updated";
            colLastUpdated.Width = 160;
            // 
            // colCacheInfo
            // 
            colCacheInfo.Text  = "Cache";
            colCacheInfo.Width = 200;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnClose);
            panel1.Controls.Add(btnToggleActive);
            panel1.Controls.Add(btnDelete);
            panel1.Controls.Add(btnEdit);
            panel1.Controls.Add(btnAdd);
            panel1.Controls.Add(btnRefreshCache);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 550);
            panel1.Name = "panel1";
            panel1.Size = new Size(1000, 60);
            panel1.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(880, 11);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 38);
            btnClose.TabIndex = 4;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;
            // 
            // btnToggleActive
            // 
            btnToggleActive.Location = new Point(332, 11);
            btnToggleActive.Name = "btnToggleActive";
            btnToggleActive.Size = new Size(150, 38);
            btnToggleActive.TabIndex = 3;
            btnToggleActive.Text = "Toggle Active";
            btnToggleActive.UseVisualStyleBackColor = true;
            btnToggleActive.Click += BtnToggleActive_Click;
            // 
            // btnRefreshCache
            // 
            btnRefreshCache.Location = new Point(492, 11);
            btnRefreshCache.Name = "btnRefreshCache";
            btnRefreshCache.Size = new Size(150, 38);
            btnRefreshCache.TabIndex = 5;
            btnRefreshCache.Text = "Clear Cache";
            btnRefreshCache.UseVisualStyleBackColor = true;
            btnRefreshCache.Click += BtnRefreshCache_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(222, 11);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 38);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(116, 11);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 38);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += BtnEdit_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(10, 11);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 38);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += BtnAdd_Click;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(10);
            label1.Size = new Size(1000, 60);
            label1.TabIndex = 2;
            label1.Text = "Manage EPG (Electronic Program Guide) sources. These sources provide program information for your channels.";
            // 
            // EpgSourceManagerForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 610);
            Controls.Add(lstEpgSources);
            Controls.Add(panel1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EpgSourceManagerForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "EPG Source Manager";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private ListView lstEpgSources;
        private ColumnHeader colName;
        private ColumnHeader colType;
        private ColumnHeader colUrl;
        private ColumnHeader colStatus;
        private ColumnHeader colLastUpdated;
        private ColumnHeader colCacheInfo;
        private Panel panel1;
        private Button btnClose;
        private Button btnToggleActive;
        private Button btnDelete;
        private Button btnEdit;
        private Button btnAdd;
        private Button btnRefreshCache;
        private Label label1;
    }
}
