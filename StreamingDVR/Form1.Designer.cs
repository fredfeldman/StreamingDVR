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
            lstChannels = new ListView();
            colChannelName = new ColumnHeader();
            colStreamId = new ColumnHeader();
            contextMenuChannels = new ContextMenuStrip(components);
            menuRecordNow = new ToolStripMenuItem();
            menuScheduleRecording = new ToolStripMenuItem();
            menuViewEpg = new ToolStripMenuItem();
            menuSeparator1 = new ToolStripSeparator();
            menuCopyStreamUrl = new ToolStripMenuItem();
            txtSearch = new TextBox();
            panel1 = new Panel();
            btnScheduleRecord = new Button();
            btnRecord = new Button();
            btnStopPreview = new Button();
            btnRefresh = new Button();
            tabRecordings = new TabPage();
            lstRecordings = new ListView();
            colRecName = new ColumnHeader();
            colRecStatus = new ColumnHeader();
            colRecStart = new ColumnHeader();
            colRecDuration = new ColumnHeader();
            colRecSize = new ColumnHeader();
            contextMenuRecordings = new ContextMenuStrip(components);
            panel2 = new Panel();
            btnOpenRecordingFolder = new Button();
            btnDeleteRecording = new Button();
            btnPlayRecording = new Button();
            btnStopRecording = new Button();
            tabScheduled = new TabPage();
            lstScheduled = new ListView();
            colSchedChannel = new ColumnHeader();
            colSchedStart = new ColumnHeader();
            colSchedDuration = new ColumnHeader();
            panel3 = new Panel();
            btnRefreshScheduled = new Button();
            btnRemoveSchedule = new Button();
            btnAddSchedule = new Button();
            tabSettings = new TabPage();
            groupBox2 = new GroupBox();
            label4 = new Label();
            txtRecordingPath = new TextBox();
            btnBrowseRecordingPath = new Button();
            groupBoxSources = new GroupBox();
            lblSourcesInfo = new Label();
            btnManageSources = new Button();
            menuAssignEpg = new ToolStripMenuItem();
            btnViewEpg = new Button();
            menuPlayRecording = new ToolStripMenuItem();
            menuStopRecording = new ToolStripMenuItem();
            menuSeparator2 = new ToolStripSeparator();
            menuOpenFolder = new ToolStripMenuItem();
            menuDeleteRecording = new ToolStripMenuItem();
            groupBox1 = new GroupBox();
            btnConnect = new Button();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            txtServerUrl = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            lblStatus = new Label();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            groupBoxStreamlink = new GroupBox();
            lblStreamlinkInfo = new Label();
            chkUseStreamlink = new CheckBox();
            lblStreamlinkQuality = new Label();
            cboStreamlinkQuality = new ComboBox();
            btnCheckStreamlink = new Button();
            lblStreamlinkOptions = new Label();
            txtStreamlinkOptions = new TextBox();
            tabControl.SuspendLayout();
            tabChannels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuChannels.SuspendLayout();
            panel1.SuspendLayout();
            tabRecordings.SuspendLayout();
            panel2.SuspendLayout();
            tabScheduled.SuspendLayout();
            panel3.SuspendLayout();
            tabSettings.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBoxSources.SuspendLayout();
            statusStrip.SuspendLayout();
            groupBoxStreamlink.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(24, 24);
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(8, 2, 0, 2);
            menuStrip.Size = new Size(1500, 24);
            menuStrip.TabIndex = 3;
            // 
            // menuFile
            // 
            menuFile.Name = "menuFile";
            menuFile.Size = new Size(32, 19);
            // 
            // menuOpenRecordingsFolder
            // 
            menuOpenRecordingsFolder.Name = "menuOpenRecordingsFolder";
            menuOpenRecordingsFolder.Size = new Size(32, 19);
            // 
            // menuSeparatorFile
            // 
            menuSeparatorFile.Name = "menuSeparatorFile";
            menuSeparatorFile.Size = new Size(6, 6);
            // 
            // menuViewStatistics
            // 
            menuViewStatistics.Name = "menuViewStatistics";
            menuViewStatistics.Size = new Size(32, 19);
            // 
            // menuSeparatorFile2
            // 
            menuSeparatorFile2.Name = "menuSeparatorFile2";
            menuSeparatorFile2.Size = new Size(6, 6);
            // 
            // menuExit
            // 
            menuExit.Name = "menuExit";
            menuExit.Size = new Size(32, 19);
            // 
            // menuTools
            // 
            menuTools.Name = "menuTools";
            menuTools.Size = new Size(32, 19);
            // 
            // menuCheckFFmpeg
            // 
            menuCheckFFmpeg.Name = "menuCheckFFmpeg";
            menuCheckFFmpeg.Size = new Size(32, 19);
            // 
            // menuSettings
            // 
            menuSettings.Name = "menuSettings";
            menuSettings.Size = new Size(32, 19);
            // 
            // menuHelp
            // 
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(32, 19);
            // 
            // menuAbout
            // 
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(32, 19);
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabChannels);
            tabControl.Controls.Add(tabRecordings);
            tabControl.Controls.Add(tabScheduled);
            tabControl.Controls.Add(tabSettings);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 24);
            tabControl.Margin = new Padding(4);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1500, 789);
            tabControl.TabIndex = 0;
            // 
            // tabChannels
            // 
            tabChannels.Controls.Add(splitContainer1);
            tabChannels.Controls.Add(panel1);
            tabChannels.Location = new Point(4, 34);
            tabChannels.Margin = new Padding(4);
            tabChannels.Name = "tabChannels";
            tabChannels.Padding = new Padding(4);
            tabChannels.Size = new Size(1492, 751);
            tabChannels.TabIndex = 0;
            tabChannels.Text = "Channels";
            tabChannels.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(4, 4);
            splitContainer1.Margin = new Padding(4);
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
            splitContainer1.Size = new Size(1484, 681);
            splitContainer1.SplitterDistance = 312;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // lstCategories
            // 
            lstCategories.Dock = DockStyle.Fill;
            lstCategories.FormattingEnabled = true;
            lstCategories.ItemHeight = 25;
            lstCategories.Location = new Point(0, 0);
            lstCategories.Margin = new Padding(4);
            lstCategories.Name = "lstCategories";
            lstCategories.Size = new Size(312, 681);
            lstCategories.TabIndex = 0;
            lstCategories.SelectedIndexChanged += LstCategories_SelectedIndexChanged;
            // 
            // lstChannels
            // 
            lstChannels.Columns.AddRange(new ColumnHeader[] { colChannelName, colStreamId });
            lstChannels.ContextMenuStrip = contextMenuChannels;
            lstChannels.Dock = DockStyle.Fill;
            lstChannels.FullRowSelect = true;
            lstChannels.Location = new Point(0, 31);
            lstChannels.Margin = new Padding(4);
            lstChannels.Name = "lstChannels";
            lstChannels.Size = new Size(1167, 650);
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
            contextMenuChannels.ImageScalingSize = new Size(24, 24);
            contextMenuChannels.Items.AddRange(new ToolStripItem[] { menuRecordNow, menuScheduleRecording, menuViewEpg, menuSeparator1, menuCopyStreamUrl });
            contextMenuChannels.Name = "contextMenuChannels";
            contextMenuChannels.Size = new Size(241, 138);
            // 
            // menuRecordNow
            // 
            menuRecordNow.Name = "menuRecordNow";
            menuRecordNow.Size = new Size(240, 32);
            menuRecordNow.Text = "Record Now";
            menuRecordNow.Click += BtnRecord_Click;
            // 
            // menuScheduleRecording
            // 
            menuScheduleRecording.Name = "menuScheduleRecording";
            menuScheduleRecording.Size = new Size(240, 32);
            menuScheduleRecording.Text = "Schedule Recording";
            menuScheduleRecording.Click += BtnScheduleRecord_Click;
            // 
            // menuViewEpg
            // 
            menuViewEpg.Name = "menuViewEpg";
            menuViewEpg.Size = new Size(240, 32);
            menuViewEpg.Text = "View EPG";
            menuViewEpg.Click += MenuViewEpg_Click;
            // 
            // menuSeparator1
            // 
            menuSeparator1.Name = "menuSeparator1";
            menuSeparator1.Size = new Size(237, 6);
            // 
            // menuCopyStreamUrl
            // 
            menuCopyStreamUrl.Name = "menuCopyStreamUrl";
            menuCopyStreamUrl.Size = new Size(240, 32);
            menuCopyStreamUrl.Text = "Copy Stream URL";
            menuCopyStreamUrl.Click += MenuCopyStreamUrl_Click;
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Top;
            txtSearch.Location = new Point(0, 0);
            txtSearch.Margin = new Padding(4);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search channels...";
            txtSearch.Size = new Size(1167, 31);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnScheduleRecord);
            panel1.Controls.Add(btnRecord);
            panel1.Controls.Add(btnStopPreview);
            panel1.Controls.Add(btnRefresh);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(4, 685);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1484, 62);
            panel1.TabIndex = 1;
            // 
            // btnScheduleRecord
            // 
            btnScheduleRecord.Location = new Point(475, 12);
            btnScheduleRecord.Margin = new Padding(4);
            btnScheduleRecord.Name = "btnScheduleRecord";
            btnScheduleRecord.Size = new Size(188, 38);
            btnScheduleRecord.TabIndex = 3;
            btnScheduleRecord.Text = "Schedule Recording";
            btnScheduleRecord.UseVisualStyleBackColor = true;
            btnScheduleRecord.Click += BtnScheduleRecord_Click;
            // 
            // btnRecord
            // 
            btnRecord.Location = new Point(312, 12);
            btnRecord.Margin = new Padding(4);
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new Size(150, 38);
            btnRecord.TabIndex = 2;
            btnRecord.Text = "Record Now";
            btnRecord.UseVisualStyleBackColor = true;
            btnRecord.Click += BtnRecord_Click;
            // 
            // btnStopPreview
            // 
            btnStopPreview.Location = new Point(150, 12);
            btnStopPreview.Margin = new Padding(4);
            btnStopPreview.Name = "btnStopPreview";
            btnStopPreview.Size = new Size(150, 38);
            btnStopPreview.TabIndex = 1;
            btnStopPreview.Text = "Stop Preview";
            btnStopPreview.UseVisualStyleBackColor = true;
            btnStopPreview.Visible = false;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(12, 12);
            btnRefresh.Margin = new Padding(4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(125, 38);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // tabRecordings
            // 
            tabRecordings.Controls.Add(lstRecordings);
            tabRecordings.Controls.Add(panel2);
            tabRecordings.Location = new Point(4, 34);
            tabRecordings.Margin = new Padding(4);
            tabRecordings.Name = "tabRecordings";
            tabRecordings.Padding = new Padding(4);
            tabRecordings.Size = new Size(1492, 751);
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
            lstRecordings.Location = new Point(4, 4);
            lstRecordings.Margin = new Padding(4);
            lstRecordings.Name = "lstRecordings";
            lstRecordings.Size = new Size(1484, 681);
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
            // contextMenuRecordings
            // 
            contextMenuRecordings.ImageScalingSize = new Size(24, 24);
            contextMenuRecordings.Name = "contextMenuRecordings";
            contextMenuRecordings.Size = new Size(61, 4);
            // 
            // panel2
            // 
            panel2.Controls.Add(btnOpenRecordingFolder);
            panel2.Controls.Add(btnDeleteRecording);
            panel2.Controls.Add(btnPlayRecording);
            panel2.Controls.Add(btnStopRecording);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(4, 685);
            panel2.Margin = new Padding(4);
            panel2.Name = "panel2";
            panel2.Size = new Size(1484, 62);
            panel2.TabIndex = 1;
            // 
            // btnOpenRecordingFolder
            // 
            btnOpenRecordingFolder.Location = new Point(500, 12);
            btnOpenRecordingFolder.Margin = new Padding(4);
            btnOpenRecordingFolder.Name = "btnOpenRecordingFolder";
            btnOpenRecordingFolder.Size = new Size(188, 38);
            btnOpenRecordingFolder.TabIndex = 3;
            btnOpenRecordingFolder.Text = "Open Folder";
            btnOpenRecordingFolder.UseVisualStyleBackColor = true;
            btnOpenRecordingFolder.Click += BtnOpenRecordingFolder_Click;
            // 
            // btnDeleteRecording
            // 
            btnDeleteRecording.Location = new Point(338, 12);
            btnDeleteRecording.Margin = new Padding(4);
            btnDeleteRecording.Name = "btnDeleteRecording";
            btnDeleteRecording.Size = new Size(150, 38);
            btnDeleteRecording.TabIndex = 2;
            btnDeleteRecording.Text = "Delete";
            btnDeleteRecording.UseVisualStyleBackColor = true;
            btnDeleteRecording.Click += BtnDeleteRecording_Click;
            // 
            // btnPlayRecording
            // 
            btnPlayRecording.Location = new Point(175, 12);
            btnPlayRecording.Margin = new Padding(4);
            btnPlayRecording.Name = "btnPlayRecording";
            btnPlayRecording.Size = new Size(150, 38);
            btnPlayRecording.TabIndex = 1;
            btnPlayRecording.Text = "Play";
            btnPlayRecording.UseVisualStyleBackColor = true;
            btnPlayRecording.Click += BtnPlayRecording_Click;
            // 
            // btnStopRecording
            // 
            btnStopRecording.Location = new Point(12, 12);
            btnStopRecording.Margin = new Padding(4);
            btnStopRecording.Name = "btnStopRecording";
            btnStopRecording.Size = new Size(150, 38);
            btnStopRecording.TabIndex = 0;
            btnStopRecording.Text = "Stop";
            btnStopRecording.UseVisualStyleBackColor = true;
            btnStopRecording.Click += BtnStopRecording_Click;
            // 
            // tabScheduled
            // 
            tabScheduled.Controls.Add(lstScheduled);
            tabScheduled.Controls.Add(panel3);
            tabScheduled.Location = new Point(4, 34);
            tabScheduled.Margin = new Padding(4);
            tabScheduled.Name = "tabScheduled";
            tabScheduled.Size = new Size(1492, 751);
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
            lstScheduled.Margin = new Padding(4);
            lstScheduled.Name = "lstScheduled";
            lstScheduled.Size = new Size(1492, 689);
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
            panel3.Location = new Point(0, 689);
            panel3.Margin = new Padding(4);
            panel3.Name = "panel3";
            panel3.Size = new Size(1492, 62);
            panel3.TabIndex = 1;
            // 
            // btnRefreshScheduled
            // 
            btnRefreshScheduled.Location = new Point(338, 12);
            btnRefreshScheduled.Margin = new Padding(4);
            btnRefreshScheduled.Name = "btnRefreshScheduled";
            btnRefreshScheduled.Size = new Size(150, 38);
            btnRefreshScheduled.TabIndex = 2;
            btnRefreshScheduled.Text = "Refresh";
            btnRefreshScheduled.UseVisualStyleBackColor = true;
            btnRefreshScheduled.Click += BtnRefreshScheduled_Click;
            // 
            // btnRemoveSchedule
            // 
            btnRemoveSchedule.Location = new Point(175, 12);
            btnRemoveSchedule.Margin = new Padding(4);
            btnRemoveSchedule.Name = "btnRemoveSchedule";
            btnRemoveSchedule.Size = new Size(150, 38);
            btnRemoveSchedule.TabIndex = 1;
            btnRemoveSchedule.Text = "Remove";
            btnRemoveSchedule.UseVisualStyleBackColor = true;
            btnRemoveSchedule.Click += BtnRemoveSchedule_Click;
            // 
            // btnAddSchedule
            // 
            btnAddSchedule.Location = new Point(12, 12);
            btnAddSchedule.Margin = new Padding(4);
            btnAddSchedule.Name = "btnAddSchedule";
            btnAddSchedule.Size = new Size(150, 38);
            btnAddSchedule.TabIndex = 0;
            btnAddSchedule.Text = "Add";
            btnAddSchedule.UseVisualStyleBackColor = true;
            // 
            // tabSettings
            // 
            tabSettings.Controls.Add(groupBoxStreamlink);
            tabSettings.Controls.Add(groupBox2);
            tabSettings.Controls.Add(groupBoxSources);
            tabSettings.Location = new Point(4, 34);
            tabSettings.Margin = new Padding(4);
            tabSettings.Name = "tabSettings";
            tabSettings.Size = new Size(1492, 751);
            tabSettings.TabIndex = 3;
            tabSettings.Text = "Settings";
            tabSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(txtRecordingPath);
            groupBox2.Controls.Add(btnBrowseRecordingPath);
            groupBox2.Location = new Point(25, 500);
            groupBox2.Margin = new Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4);
            groupBox2.Size = new Size(750, 125);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Recording Settings";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(25, 50);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(135, 25);
            label4.TabIndex = 0;
            label4.Text = "Recording Path:";
            // 
            // txtRecordingPath
            // 
            txtRecordingPath.Location = new Point(181, 46);
            txtRecordingPath.Margin = new Padding(4);
            txtRecordingPath.Name = "txtRecordingPath";
            txtRecordingPath.Size = new Size(424, 31);
            txtRecordingPath.TabIndex = 1;
            // 
            // btnBrowseRecordingPath
            // 
            btnBrowseRecordingPath.Location = new Point(619, 44);
            btnBrowseRecordingPath.Margin = new Padding(4);
            btnBrowseRecordingPath.Name = "btnBrowseRecordingPath";
            btnBrowseRecordingPath.Size = new Size(94, 38);
            btnBrowseRecordingPath.TabIndex = 2;
            btnBrowseRecordingPath.Text = "Browse";
            btnBrowseRecordingPath.UseVisualStyleBackColor = true;
            btnBrowseRecordingPath.Click += BtnBrowseRecordingPath_Click;
            // 
            // groupBoxSources
            // 
            groupBoxSources.Controls.Add(lblSourcesInfo);
            groupBoxSources.Controls.Add(btnManageSources);
            groupBoxSources.Location = new Point(25, 25);
            groupBoxSources.Margin = new Padding(4);
            groupBoxSources.Name = "groupBoxSources";
            groupBoxSources.Padding = new Padding(4);
            groupBoxSources.Size = new Size(750, 150);
            groupBoxSources.TabIndex = 0;
            groupBoxSources.TabStop = false;
            groupBoxSources.Text = "IPTV Sources";
            // 
            // lblSourcesInfo
            // 
            lblSourcesInfo.Location = new Point(25, 38);
            lblSourcesInfo.Margin = new Padding(4, 0, 4, 0);
            lblSourcesInfo.Name = "lblSourcesInfo";
            lblSourcesInfo.Size = new Size(700, 56);
            lblSourcesInfo.TabIndex = 0;
            lblSourcesInfo.Text = "Configure multiple IPTV sources including Xtream Codes, Enigma2 boxes, and M3U playlists.\r\nAdd additional EPG sources for enhanced program information.";
            // 
            // btnManageSources
            // 
            btnManageSources.Location = new Point(25, 94);
            btnManageSources.Margin = new Padding(4);
            btnManageSources.Name = "btnManageSources";
            btnManageSources.Size = new Size(188, 38);
            btnManageSources.TabIndex = 1;
            btnManageSources.Text = "Manage Sources...";
            btnManageSources.UseVisualStyleBackColor = true;
            btnManageSources.Click += BtnManageSources_Click;
            // 
            // menuAssignEpg
            // 
            menuAssignEpg.Name = "menuAssignEpg";
            menuAssignEpg.Size = new Size(32, 19);
            // 
            // btnViewEpg
            // 
            btnViewEpg.Location = new Point(0, 0);
            btnViewEpg.Name = "btnViewEpg";
            btnViewEpg.Size = new Size(75, 23);
            btnViewEpg.TabIndex = 0;
            // 
            // menuPlayRecording
            // 
            menuPlayRecording.Name = "menuPlayRecording";
            menuPlayRecording.Size = new Size(32, 19);
            // 
            // menuStopRecording
            // 
            menuStopRecording.Name = "menuStopRecording";
            menuStopRecording.Size = new Size(32, 19);
            // 
            // menuSeparator2
            // 
            menuSeparator2.Name = "menuSeparator2";
            menuSeparator2.Size = new Size(6, 6);
            // 
            // menuOpenFolder
            // 
            menuOpenFolder.Name = "menuOpenFolder";
            menuOpenFolder.Size = new Size(32, 19);
            // 
            // menuDeleteRecording
            // 
            menuDeleteRecording.Name = "menuDeleteRecording";
            menuDeleteRecording.Size = new Size(32, 19);
            // 
            // groupBox1
            // 
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(200, 100);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(470, 155);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(100, 30);
            btnConnect.TabIndex = 6;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Visible = false;
            btnConnect.Click += BtnConnect_Click;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(120, 117);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(450, 31);
            txtPassword.TabIndex = 5;
            txtPassword.Visible = false;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(120, 77);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(450, 31);
            txtUsername.TabIndex = 3;
            txtUsername.Visible = false;
            // 
            // txtServerUrl
            // 
            txtServerUrl.Location = new Point(0, 0);
            txtServerUrl.Name = "txtServerUrl";
            txtServerUrl.Size = new Size(100, 31);
            txtServerUrl.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 120);
            label3.Name = "label3";
            label3.Size = new Size(73, 20);
            label3.TabIndex = 4;
            label3.Text = "Password:";
            label3.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 80);
            label2.Name = "label2";
            label2.Size = new Size(78, 20);
            label2.TabIndex = 2;
            label2.Text = "Username:";
            label2.Visible = false;
            // 
            // label1
            // 
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(0, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(100, 23);
            lblStatus.TabIndex = 0;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 813);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 18, 0);
            statusStrip.Size = new Size(1500, 32);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(129, 25);
            toolStripStatusLabel.Text = "Not connected";
            // 
            // groupBoxStreamlink
            // 
            groupBoxStreamlink.Controls.Add(txtStreamlinkOptions);
            groupBoxStreamlink.Controls.Add(lblStreamlinkOptions);
            groupBoxStreamlink.Controls.Add(btnCheckStreamlink);
            groupBoxStreamlink.Controls.Add(cboStreamlinkQuality);
            groupBoxStreamlink.Controls.Add(lblStreamlinkQuality);
            groupBoxStreamlink.Controls.Add(chkUseStreamlink);
            groupBoxStreamlink.Controls.Add(lblStreamlinkInfo);
            groupBoxStreamlink.Location = new Point(25, 350);
            groupBoxStreamlink.Name = "groupBoxStreamlink";
            groupBoxStreamlink.Size = new Size(750, 125);
            groupBoxStreamlink.TabIndex = 4;
            groupBoxStreamlink.TabStop = false;
            groupBoxStreamlink.Text = "Streamlink Settings";
            // 
            // lblStreamlinkInfo
            // 
            lblStreamlinkInfo.AutoSize = true;
            lblStreamlinkInfo.Location = new Point(25, 28);
            lblStreamlinkInfo.Name = "lblStreamlinkInfo";
            lblStreamlinkInfo.Size = new Size(540, 25);
            lblStreamlinkInfo.TabIndex = 0;
            lblStreamlinkInfo.Text = "Use Streamlink for better compatibility with various stream sources";
            // 
            // chkUseStreamlink
            // 
            chkUseStreamlink.AutoSize = true;
            chkUseStreamlink.Location = new Point(25, 58);
            chkUseStreamlink.Name = "chkUseStreamlink";
            chkUseStreamlink.Size = new Size(263, 29);
            chkUseStreamlink.TabIndex = 1;
            chkUseStreamlink.Text = "Use Streamlink for recording";
            chkUseStreamlink.UseVisualStyleBackColor = true;
            chkUseStreamlink.CheckedChanged += ChkUseStreamlink_CheckedChanged;
            // 
            // lblStreamlinkQuality
            // 
            lblStreamlinkQuality.AutoSize = true;
            lblStreamlinkQuality.Location = new Point(310, 60);
            lblStreamlinkQuality.Name = "lblStreamlinkQuality";
            lblStreamlinkQuality.Size = new Size(72, 25);
            lblStreamlinkQuality.TabIndex = 2;
            lblStreamlinkQuality.Text = "Quality:";
            // 
            // cboStreamlinkQuality
            // 
            cboStreamlinkQuality.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStreamlinkQuality.FormattingEnabled = true;
            cboStreamlinkQuality.Items.AddRange(new object[] { "best", "worst", "1080p", "720p", "480p", "360p", "source" });
            cboStreamlinkQuality.Location = new Point(389, 60);
            cboStreamlinkQuality.Name = "cboStreamlinkQuality";
            cboStreamlinkQuality.Size = new Size(150, 33);
            cboStreamlinkQuality.TabIndex = 3;
            // 
            // btnCheckStreamlink
            // 
            btnCheckStreamlink.Location = new Point(555, 56);
            btnCheckStreamlink.Name = "btnCheckStreamlink";
            btnCheckStreamlink.Size = new Size(158, 35);
            btnCheckStreamlink.TabIndex = 4;
            btnCheckStreamlink.Text = "Check Streamlink";
            btnCheckStreamlink.UseVisualStyleBackColor = true;
            btnCheckStreamlink.Click += BtnCheckStreamlink_Click;
            // 
            // lblStreamlinkOptions
            // 
            lblStreamlinkOptions.AutoSize = true;
            lblStreamlinkOptions.Location = new Point(25, 94);
            lblStreamlinkOptions.Name = "lblStreamlinkOptions";
            lblStreamlinkOptions.Size = new Size(80, 25);
            lblStreamlinkOptions.TabIndex = 5;
            lblStreamlinkOptions.Text = "Options:";
            lblStreamlinkOptions.Visible = false;
            // 
            // txtStreamlinkOptions
            // 
            txtStreamlinkOptions.Location = new Point(111, 90);
            txtStreamlinkOptions.Name = "txtStreamlinkOptions";
            txtStreamlinkOptions.PlaceholderText = "Additional streamlink options";
            txtStreamlinkOptions.Size = new Size(428, 31);
            txtStreamlinkOptions.TabIndex = 6;
            txtStreamlinkOptions.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1500, 845);
            Controls.Add(tabControl);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new Padding(4);
            Name = "Form1";
            Text = "IPTV DVR - Xtream Codes";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            tabControl.ResumeLayout(false);
            tabChannels.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuChannels.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tabRecordings.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tabScheduled.ResumeLayout(false);
            panel3.ResumeLayout(false);
            tabSettings.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBoxSources.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            groupBoxStreamlink.ResumeLayout(false);
            groupBoxStreamlink.PerformLayout();
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
        private ToolStripMenuItem menuAssignEpg;
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
        private GroupBox groupBoxSources;
        private Label lblSourcesInfo;
        private Button btnManageSources;
        private GroupBox groupBoxEpg;
        private Label lblEpgInfo;
        private Button btnManageEpgSources;
        private GroupBox groupBox2;
        private Label label4;
        private TextBox txtRecordingPath;
        private Button btnBrowseRecordingPath;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtServerUrl;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnConnect;
        private Label lblStatus;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private GroupBox groupBoxStreamlink;
        private Label lblStreamlinkInfo;
        private CheckBox chkUseStreamlink;
        private Label lblStreamlinkQuality;
        private ComboBox cboStreamlinkQuality;
        private Button btnCheckStreamlink;
        private Label lblStreamlinkOptions;
        private TextBox txtStreamlinkOptions;
    }
}
