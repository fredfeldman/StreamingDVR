namespace StreamingDVR
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip = new MenuStrip();
            menuFile = new ToolStripMenuItem();
            menuOpenRecordingsFolder = new ToolStripMenuItem();
            menuSeparatorFile = new ToolStripSeparator();
            menuViewStatistics = new ToolStripMenuItem();
            menuSeparatorFile2 = new ToolStripSeparator();
            menuExit = new ToolStripMenuItem();
            menuTools = new ToolStripMenuItem();
            menuCheckFFmpeg = new ToolStripMenuItem();
            menuSettings = new ToolStripMenuItem();
            menuHelp = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            tabControl = new TabControl();
            tabChannels = new TabPage();
            splitContainer1 = new SplitContainer();
            lstCategories = new ListBox();
            txtSearch = new TextBox();
            lstChannels = new ListView();
            colChannelName = new ColumnHeader();
            colStreamId = new ColumnHeader();
            contextMenuChannels = new ContextMenuStrip(components);
            menuRecordNow = new ToolStripMenuItem();
            menuScheduleRecording = new ToolStripMenuItem();
            menuViewEpg = new ToolStripMenuItem();
            menuSeparator1 = new ToolStripSeparator();
            menuCopyStreamUrl = new ToolStripMenuItem();
            panel1 = new Panel();
            btnViewEpg = new Button();
            btnRefresh = new Button();
            btnStopPreview = new Button();
            btnRecord = new Button();
            btnScheduleRecord = new Button();
            tabRecordings = new TabPage();
            lstRecordings = new ListView();
            colRecName = new ColumnHeader();
            colRecStatus = new ColumnHeader();
            colRecStart = new ColumnHeader();
            colRecDuration = new ColumnHeader();
            colRecSize = new ColumnHeader();
            contextMenuRecordings = new ContextMenuStrip(components);
            menuPlayRecording = new ToolStripMenuItem();
            menuStopRecording = new ToolStripMenuItem();
            menuSeparator2 = new ToolStripSeparator();
            menuOpenFolder = new ToolStripMenuItem();
            menuDeleteRecording = new ToolStripMenuItem();
            panel2 = new Panel();
            btnOpenRecordingFolder = new Button();
            btnStopRecording = new Button();
            btnPlayRecording = new Button();
            btnDeleteRecording = new Button();
            tabScheduled = new TabPage();
            lstScheduled = new ListView();
            colSchedChannel = new ColumnHeader();
            colSchedStart = new ColumnHeader();
            colSchedDuration = new ColumnHeader();
            panel3 = new Panel();
            btnAddSchedule = new Button();
            btnRemoveSchedule = new Button();
            btnRefreshScheduled = new Button();
            tabSettings = new TabPage();
            groupBox1 = new GroupBox();
            btnConnect = new Button();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            txtServerUrl = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            btnBrowseRecordingPath = new Button();
            txtRecordingPath = new TextBox();
            label4 = new Label();
            lblStatus = new Label();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            menuStrip.SuspendLayout();
            tabControl.SuspendLayout();
            tabChannels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuChannels.SuspendLayout();
            panel1.SuspendLayout();
            tabRecordings.SuspendLayout();
            contextMenuRecordings.SuspendLayout();
            panel2.SuspendLayout();
            tabScheduled.SuspendLayout();
            panel3.SuspendLayout();
            tabSettings.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabChannels);
            tabControl.Controls.Add(tabRecordings);
            tabControl.Controls.Add(tabScheduled);
            tabControl.Controls.Add(tabSettings);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1200, 650);
            tabControl.TabIndex = 0;
            // 
            // tabChannels
            // 
            tabChannels.Controls.Add(splitContainer1);
            tabChannels.Controls.Add(panel1);
            tabChannels.Location = new Point(4, 29);
            tabChannels.Name = "tabChannels";
            tabChannels.Padding = new Padding(3);
            tabChannels.Size = new Size(1192, 617);
            tabChannels.TabIndex = 0;
            tabChannels.Text = "Channels";
            tabChannels.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(lstCategories);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(lstChannels);
            splitContainer1.Panel2.Controls.Add(txtSearch);
            splitContainer1.Size = new Size(1186, 561);
            splitContainer1.SplitterDistance = 250;
            splitContainer1.TabIndex = 0;
            // 
            // lstCategories
            // 
            lstCategories.Dock = DockStyle.Fill;
            lstCategories.FormattingEnabled = true;
            lstCategories.ItemHeight = 20;
            lstCategories.Location = new Point(0, 0);
            lstCategories.Name = "lstCategories";
            lstCategories.Size = new Size(250, 561);
            lstCategories.TabIndex = 0;
            lstCategories.SelectedIndexChanged += LstCategories_SelectedIndexChanged;
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Top;
            txtSearch.Location = new Point(0, 0);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search channels...";
            txtSearch.Size = new Size(932, 27);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // lstChannels
            // 
            lstChannels.Columns.AddRange(new ColumnHeader[] { colChannelName, colStreamId });
            lstChannels.ContextMenuStrip = contextMenuChannels;
            lstChannels.Dock = DockStyle.Fill;
            lstChannels.FullRowSelect = true;
            lstChannels.Location = new Point(0, 27);
            lstChannels.Name = "lstChannels";
            lstChannels.Size = new Size(932, 534);
            lstChannels.TabIndex = 0;
            lstChannels.UseCompatibleStateImageBehavior = false;
            lstChannels.View = View.Details;
            lstChannels.DoubleClick += LstChannels_DoubleClick;
            // 
            // colChannelName
            // 
            colChannelName.Text = "Channel Name";
            colChannelName.Width = 600;
            // 
            // colStreamId
            // 
            colStreamId.Text = "Stream ID";
            colStreamId.Width = 150;
            // 
            // contextMenuChannels
            // 
            contextMenuChannels.Items.AddRange(new ToolStripItem[] { menuRecordNow, menuScheduleRecording, menuViewEpg, menuSeparator1, menuCopyStreamUrl });
            contextMenuChannels.Name = "contextMenuChannels";
            contextMenuChannels.Size = new Size(200, 120);
            // 
            // menuRecordNow
            // 
            menuRecordNow.Name = "menuRecordNow";
            menuRecordNow.Size = new Size(199, 24);
            menuRecordNow.Text = "Record Now";
            menuRecordNow.Click += BtnRecord_Click;
            // 
            // menuScheduleRecording
            // 
            menuScheduleRecording.Name = "menuScheduleRecording";
            menuScheduleRecording.Size = new Size(199, 24);
            menuScheduleRecording.Text = "Schedule Recording";
            menuScheduleRecording.Click += BtnScheduleRecord_Click;
            // 
            // menuViewEpg
            // 
            menuViewEpg.Name = "menuViewEpg";
            menuViewEpg.Size = new Size(199, 24);
            menuViewEpg.Text = "View EPG";
            menuViewEpg.Click += MenuViewEpg_Click;
            // 
            // menuSeparator1
            // 
            menuSeparator1.Name = "menuSeparator1";
            menuSeparator1.Size = new Size(196, 6);
            // 
            // menuCopyStreamUrl
            // 
            menuCopyStreamUrl.Name = "menuCopyStreamUrl";
            menuCopyStreamUrl.Size = new Size(199, 24);
            menuCopyStreamUrl.Text = "Copy Stream URL";
            menuCopyStreamUrl.Click += MenuCopyStreamUrl_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnScheduleRecord);
            panel1.Controls.Add(btnRecord);
            panel1.Controls.Add(btnStopPreview);
            panel1.Controls.Add(btnRefresh);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, 564);
            panel1.Name = "panel1";
            panel1.Size = new Size(1186, 50);
            panel1.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(10, 10);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // btnStopPreview
            // 
            btnStopPreview.Location = new Point(120, 10);
            btnStopPreview.Name = "btnStopPreview";
            btnStopPreview.Size = new Size(120, 30);
            btnStopPreview.TabIndex = 1;
            btnStopPreview.Text = "Stop Preview";
            btnStopPreview.UseVisualStyleBackColor = true;
            btnStopPreview.Visible = false;
            // 
            // btnRecord
            // 
            btnRecord.Location = new Point(250, 10);
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new Size(120, 30);
            btnRecord.TabIndex = 2;
            btnRecord.Text = "Record Now";
            btnRecord.UseVisualStyleBackColor = true;
            btnRecord.Click += BtnRecord_Click;
            // 
            // btnScheduleRecord
            // 
            btnScheduleRecord.Location = new Point(380, 10);
            btnScheduleRecord.Name = "btnScheduleRecord";
            btnScheduleRecord.Size = new Size(150, 30);
            btnScheduleRecord.TabIndex = 3;
            btnScheduleRecord.Text = "Schedule Recording";
            btnScheduleRecord.UseVisualStyleBackColor = true;
            btnScheduleRecord.Click += BtnScheduleRecord_Click;
            // 
            // tabRecordings
            // 
            tabRecordings.Controls.Add(lstRecordings);
            tabRecordings.Controls.Add(panel2);
            tabRecordings.Location = new Point(4, 29);
            tabRecordings.Name = "tabRecordings";
            tabRecordings.Padding = new Padding(3);
            tabRecordings.Size = new Size(1192, 617);
            tabRecordings.TabIndex = 1;
            tabRecordings.Text = "Recordings";
            tabRecordings.UseVisualStyleBackColor = true;
            // 
            // lstRecordings
            // 
            lstRecordings.Columns.AddRange(new ColumnHeader[] { colRecName, colRecStatus, colRecStart, colRecDuration, colRecSize });
            lstRecordings.ContextMenuStrip = contextMenuRecordings;
            lstRecordings.Dock = DockStyle.Fill;
            lstRecordings.FullRowSelect = true;
            lstRecordings.Location = new Point(3, 3);
            lstRecordings.Name = "lstRecordings";
            lstRecordings.Size = new Size(1186, 561);
            lstRecordings.TabIndex = 0;
            lstRecordings.UseCompatibleStateImageBehavior = false;
            lstRecordings.View = View.Details;
            // 
            // colRecName
            // 
            colRecName.Text = "Channel Name";
            colRecName.Width = 400;
            // 
            // colRecStatus
            // 
            colRecStatus.Text = "Status";
            colRecStatus.Width = 120;
            // 
            // colRecStart
            // 
            colRecStart.Text = "Start Time";
            colRecStart.Width = 180;
            // 
            // colRecDuration
            // 
            colRecDuration.Text = "Duration";
            colRecDuration.Width = 120;
            // 
            // colRecSize
            // 
            colRecSize.Text = "Size";
            colRecSize.Width = 120;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnDeleteRecording);
            panel2.Controls.Add(btnPlayRecording);
            panel2.Controls.Add(btnStopRecording);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(3, 564);
            panel2.Name = "panel2";
            panel2.Size = new Size(1186, 50);
            panel2.TabIndex = 1;
            // 
            // btnStopRecording
            // 
            btnStopRecording.Location = new Point(10, 10);
            btnStopRecording.Name = "btnStopRecording";
            btnStopRecording.Size = new Size(120, 30);
            btnStopRecording.TabIndex = 0;
            btnStopRecording.Text = "Stop";
            btnStopRecording.UseVisualStyleBackColor = true;
            btnStopRecording.Click += BtnStopRecording_Click;
            // 
            // btnPlayRecording
            // 
            btnPlayRecording.Location = new Point(140, 10);
            btnPlayRecording.Name = "btnPlayRecording";
            btnPlayRecording.Size = new Size(120, 30);
            btnPlayRecording.TabIndex = 1;
            btnPlayRecording.Text = "Play";
            btnPlayRecording.UseVisualStyleBackColor = true;
            btnPlayRecording.Click += BtnPlayRecording_Click;
            // 
            // btnDeleteRecording
            // 
            btnDeleteRecording.Location = new Point(270, 10);
            btnDeleteRecording.Name = "btnDeleteRecording";
            btnDeleteRecording.Size = new Size(120, 30);
            btnDeleteRecording.TabIndex = 2;
            btnDeleteRecording.Text = "Delete";
            btnDeleteRecording.UseVisualStyleBackColor = true;
            btnDeleteRecording.Click += BtnDeleteRecording_Click;
            // 
            // tabScheduled
            // 
            tabScheduled.Controls.Add(lstScheduled);
            tabScheduled.Controls.Add(panel3);
            tabScheduled.Location = new Point(4, 29);
            tabScheduled.Name = "tabScheduled";
            tabScheduled.Size = new Size(1192, 617);
            tabScheduled.TabIndex = 2;
            tabScheduled.Text = "Scheduled";
            tabScheduled.UseVisualStyleBackColor = true;
            // 
            // lstScheduled
            // 
            lstScheduled.Columns.AddRange(new ColumnHeader[] { colSchedChannel, colSchedStart, colSchedDuration });
            lstScheduled.Dock = DockStyle.Fill;
            lstScheduled.FullRowSelect = true;
            lstScheduled.Location = new Point(0, 0);
            lstScheduled.Name = "lstScheduled";
            lstScheduled.Size = new Size(1192, 567);
            lstScheduled.TabIndex = 0;
            lstScheduled.UseCompatibleStateImageBehavior = false;
            lstScheduled.View = View.Details;
            // 
            // colSchedChannel
            // 
            colSchedChannel.Text = "Channel Name";
            colSchedChannel.Width = 400;
            // 
            // colSchedStart
            // 
            colSchedStart.Text = "Start Time";
            colSchedStart.Width = 200;
            // 
            // colSchedDuration
            // 
            colSchedDuration.Text = "Duration";
            colSchedDuration.Width = 150;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnRefreshScheduled);
            panel3.Controls.Add(btnRemoveSchedule);
            panel3.Controls.Add(btnAddSchedule);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 567);
            panel3.Name = "panel3";
            panel3.Size = new Size(1192, 50);
            panel3.TabIndex = 1;
            // 
            // btnAddSchedule
            // 
            btnAddSchedule.Location = new Point(10, 10);
            btnAddSchedule.Name = "btnAddSchedule";
            btnAddSchedule.Size = new Size(120, 30);
            btnAddSchedule.TabIndex = 0;
            btnAddSchedule.Text = "Add";
            btnAddSchedule.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSchedule
            // 
            btnRemoveSchedule.Location = new Point(140, 10);
            btnRemoveSchedule.Name = "btnRemoveSchedule";
            btnRemoveSchedule.Size = new Size(120, 30);
            btnRemoveSchedule.TabIndex = 1;
            btnRemoveSchedule.Text = "Remove";
            btnRemoveSchedule.UseVisualStyleBackColor = true;
            btnRemoveSchedule.Click += BtnRemoveSchedule_Click;
            // 
            // btnRefreshScheduled
            // 
            btnRefreshScheduled.Location = new Point(270, 10);
            btnRefreshScheduled.Name = "btnRefreshScheduled";
            btnRefreshScheduled.Size = new Size(120, 30);
            btnRefreshScheduled.TabIndex = 2;
            btnRefreshScheduled.Text = "Refresh";
            btnRefreshScheduled.UseVisualStyleBackColor = true;
            btnRefreshScheduled.Click += BtnRefreshScheduled_Click;
            // 
            // tabSettings
            // 
            tabSettings.Controls.Add(groupBox2);
            tabSettings.Controls.Add(groupBox1);
            tabSettings.Location = new Point(4, 29);
            tabSettings.Name = "tabSettings";
            tabSettings.Size = new Size(1192, 617);
            tabSettings.TabIndex = 3;
            tabSettings.Text = "Settings";
            tabSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtServerUrl);
            groupBox1.Controls.Add(txtUsername);
            groupBox1.Controls.Add(txtPassword);
            groupBox1.Controls.Add(btnConnect);
            groupBox1.Location = new Point(20, 20);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(600, 200);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Xtream Codes Connection";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 40);
            label1.Name = "label1";
            label1.Size = new Size(85, 20);
            label1.TabIndex = 0;
            label1.Text = "Server URL:";
            // 
            // txtServerUrl
            // 
            txtServerUrl.Location = new Point(120, 37);
            txtServerUrl.Name = "txtServerUrl";
            txtServerUrl.Size = new Size(450, 27);
            txtServerUrl.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 80);
            label2.Name = "label2";
            label2.Size = new Size(78, 20);
            label2.TabIndex = 2;
            label2.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(120, 77);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(450, 27);
            txtUsername.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 120);
            label3.Name = "label3";
            label3.Size = new Size(73, 20);
            label3.TabIndex = 4;
            label3.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(120, 117);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(450, 27);
            txtPassword.TabIndex = 5;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(470, 155);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(100, 30);
            btnConnect.TabIndex = 6;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += BtnConnect_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(txtRecordingPath);
            groupBox2.Controls.Add(btnBrowseRecordingPath);
            groupBox2.Location = new Point(20, 240);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(600, 100);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Recording Settings";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 40);
            label4.Name = "label4";
            label4.Size = new Size(115, 20);
            label4.TabIndex = 0;
            label4.Text = "Recording Path:";
            // 
            // txtRecordingPath
            // 
            txtRecordingPath.Location = new Point(145, 37);
            txtRecordingPath.Name = "txtRecordingPath";
            txtRecordingPath.Size = new Size(340, 27);
            txtRecordingPath.TabIndex = 1;
            // 
            // btnBrowseRecordingPath
            // 
            btnBrowseRecordingPath.Location = new Point(495, 35);
            btnBrowseRecordingPath.Name = "btnBrowseRecordingPath";
            btnBrowseRecordingPath.Size = new Size(75, 30);
            btnBrowseRecordingPath.TabIndex = 2;
            btnBrowseRecordingPath.Text = "Browse";
            btnBrowseRecordingPath.UseVisualStyleBackColor = true;
            btnBrowseRecordingPath.Click += BtnBrowseRecordingPath_Click;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 650);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1200, 26);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(92, 20);
            toolStripStatusLabel.Text = "Not connected";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 676);
            Controls.Add(tabControl);
            Controls.Add(statusStrip);
            Name = "Form1";
            Text = "IPTV DVR - Xtream Codes";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            tabControl.ResumeLayout(false);
            tabChannels.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuChannels.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tabRecordings.ResumeLayout(false);
            contextMenuRecordings.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tabScheduled.ResumeLayout(false);
            panel3.ResumeLayout(false);
            tabSettings.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuOpenRecordingsFolder;
        private ToolStripSeparator menuSeparatorFile;
        private ToolStripMenuItem menuViewStatistics;
        private ToolStripSeparator menuSeparatorFile2;
        private ToolStripMenuItem menuExit;
        private ToolStripMenuItem menuTools;
        private ToolStripMenuItem menuCheckFFmpeg;
        private ToolStripMenuItem menuSettings;
        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuAbout;
        private TabControl tabControl;
        private TabPage tabChannels;
        private TabPage tabRecordings;
        private TabPage tabScheduled;
        private TabPage tabSettings;
        private SplitContainer splitContainer1;
        private ListBox lstCategories;
        private TextBox txtSearch;
        private ListView lstChannels;
        private ColumnHeader colChannelName;
        private ColumnHeader colStreamId;
        private ContextMenuStrip contextMenuChannels;
        private ToolStripMenuItem menuRecordNow;
        private ToolStripMenuItem menuScheduleRecording;
        private ToolStripMenuItem menuViewEpg;
        private ToolStripSeparator menuSeparator1;
        private ToolStripMenuItem menuCopyStreamUrl;
        private Panel panel1;
        private Button btnViewEpg;
        private Button btnRefresh;
        private Button btnStopPreview;
        private Button btnRecord;
        private Button btnScheduleRecord;
        private ListView lstRecordings;
        private ColumnHeader colRecName;
        private ColumnHeader colRecStatus;
        private ColumnHeader colRecStart;
        private ColumnHeader colRecDuration;
        private ColumnHeader colRecSize;
        private ContextMenuStrip contextMenuRecordings;
        private ToolStripMenuItem menuPlayRecording;
        private ToolStripMenuItem menuStopRecording;
        private ToolStripSeparator menuSeparator2;
        private ToolStripMenuItem menuOpenFolder;
        private ToolStripMenuItem menuDeleteRecording;
        private Panel panel2;
        private Button btnOpenRecordingFolder;
        private Button btnStopRecording;
        private Button btnPlayRecording;
        private Button btnDeleteRecording;
        private ListView lstScheduled;
        private ColumnHeader colSchedChannel;
        private ColumnHeader colSchedStart;
        private ColumnHeader colSchedDuration;
        private Panel panel3;
        private Button btnRefreshScheduled;
        private Button btnAddSchedule;
        private Button btnRemoveSchedule;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtServerUrl;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnConnect;
        private GroupBox groupBox2;
        private Label label4;
        private TextBox txtRecordingPath;
        private Button btnBrowseRecordingPath;
        private Label lblStatus;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
    }
}
