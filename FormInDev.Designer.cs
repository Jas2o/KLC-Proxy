namespace KLCProxy {
    partial class FormInDev {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInDev));
            this.listAgent = new System.Windows.Forms.ListBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.timerAuto = new System.Windows.Forms.Timer(this.components);
            this.contextOriginal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextOriginalLiveConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOriginalTerminal = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextOriginalShared = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOriginalPrivate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.contextOriginalVNC = new System.Windows.Forms.ToolStripMenuItem();
            this.btnWatch = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.appsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolAppExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolAppFinch = new System.Windows.Forms.ToolStripMenuItem();
            this.toolAppHawk = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.authTokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenshotToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aHKAutoTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.teamViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iTGlueTeamVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.cjasonpcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addByGUIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.liveConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reconnectCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.killAllLiveConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kLCRemoteDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useKLCProxyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useMITMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.onRemoteControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolOnRC_UseAlt = new System.Windows.Forms.ToolStripMenuItem();
            this.toolOnRC_UseLC = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.onLiveConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolOnLC_UseAlt = new System.Windows.Forms.ToolStripMenuItem();
            this.toolOnLC_UseLC = new System.Windows.Forms.ToolStripMenuItem();
            this.toolOnLC_Ask = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSettingsToastTest = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSettingsToastWhenOnline = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSelectedLogs = new System.Windows.Forms.TextBox();
            this.btnAlternative = new KLCProxy.MenuButton();
            this.contextAlternative = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextAlternativeLaunch = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.contextAlternativeShared = new System.Windows.Forms.ToolStripMenuItem();
            this.contextAlternativePrivate = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOriginal = new KLCProxy.MenuButton();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.toolOnLC_UseDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOriginal.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextAlternative.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listAgent
            // 
            this.listAgent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listAgent.FormattingEnabled = true;
            this.listAgent.Location = new System.Drawing.Point(3, 3);
            this.listAgent.Name = "listAgent";
            this.listAgent.Size = new System.Drawing.Size(174, 115);
            this.listAgent.TabIndex = 0;
            this.listAgent.SelectedIndexChanged += new System.EventHandler(this.listAgent_SelectedIndexChanged);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(3, 90);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(98, 23);
            this.btnRemove.TabIndex = 0;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // timerAuto
            // 
            this.timerAuto.Interval = 10000;
            this.timerAuto.Tick += new System.EventHandler(this.timerAuto_Tick);
            // 
            // contextOriginal
            // 
            this.contextOriginal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextOriginalLiveConnect,
            this.contextOriginalTerminal,
            this.toolStripSeparator1,
            this.contextOriginalShared,
            this.contextOriginalPrivate,
            this.toolStripSeparator3,
            this.contextOriginalVNC});
            this.contextOriginal.Name = "contextMenuStrip1";
            this.contextOriginal.Size = new System.Drawing.Size(198, 126);
            // 
            // contextOriginalLiveConnect
            // 
            this.contextOriginalLiveConnect.Name = "contextOriginalLiveConnect";
            this.contextOriginalLiveConnect.Size = new System.Drawing.Size(197, 22);
            this.contextOriginalLiveConnect.Text = "Live Connect";
            this.contextOriginalLiveConnect.Click += new System.EventHandler(this.contextOriginalLiveConnect_Click);
            // 
            // contextOriginalTerminal
            // 
            this.contextOriginalTerminal.Name = "contextOriginalTerminal";
            this.contextOriginalTerminal.Size = new System.Drawing.Size(197, 22);
            this.contextOriginalTerminal.Text = "Terminal";
            this.contextOriginalTerminal.Click += new System.EventHandler(this.contextOriginalTerminal_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
            // 
            // contextOriginalShared
            // 
            this.contextOriginalShared.Name = "contextOriginalShared";
            this.contextOriginalShared.Size = new System.Drawing.Size(197, 22);
            this.contextOriginalShared.Text = "Remote Control";
            this.contextOriginalShared.Click += new System.EventHandler(this.contextOriginalShared_Click);
            // 
            // contextOriginalPrivate
            // 
            this.contextOriginalPrivate.Name = "contextOriginalPrivate";
            this.contextOriginalPrivate.Size = new System.Drawing.Size(197, 22);
            this.contextOriginalPrivate.Text = "Private Remote Control";
            this.contextOriginalPrivate.Click += new System.EventHandler(this.contextOriginalPrivate_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(194, 6);
            // 
            // contextOriginalVNC
            // 
            this.contextOriginalVNC.Name = "contextOriginalVNC";
            this.contextOriginalVNC.Size = new System.Drawing.Size(197, 22);
            this.contextOriginalVNC.Text = "VNC";
            this.contextOriginalVNC.Click += new System.EventHandler(this.contextOriginalVNC_Click);
            // 
            // btnWatch
            // 
            this.btnWatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWatch.Location = new System.Drawing.Point(3, 61);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new System.Drawing.Size(98, 23);
            this.btnWatch.TabIndex = 0;
            this.btnWatch.Text = "Watch";
            this.btnWatch.UseVisualStyleBackColor = true;
            this.btnWatch.Click += new System.EventHandler(this.btnWatch_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.appsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(286, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // appsToolStripMenuItem
            // 
            this.appsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolAppExplorer,
            this.toolAppFinch,
            this.toolAppHawk,
            this.toolStripSeparator5,
            this.authTokenToolStripMenuItem});
            this.appsToolStripMenuItem.Name = "appsToolStripMenuItem";
            this.appsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.appsToolStripMenuItem.Text = "Apps";
            // 
            // toolAppExplorer
            // 
            this.toolAppExplorer.Name = "toolAppExplorer";
            this.toolAppExplorer.Size = new System.Drawing.Size(138, 22);
            this.toolAppExplorer.Text = "Explorer";
            this.toolAppExplorer.Click += new System.EventHandler(this.toolAppExplorer_Click);
            // 
            // toolAppFinch
            // 
            this.toolAppFinch.Name = "toolAppFinch";
            this.toolAppFinch.Size = new System.Drawing.Size(138, 22);
            this.toolAppFinch.Text = "Finch";
            this.toolAppFinch.Click += new System.EventHandler(this.toolAppFinch_Click);
            // 
            // toolAppHawk
            // 
            this.toolAppHawk.Name = "toolAppHawk";
            this.toolAppHawk.Size = new System.Drawing.Size(138, 22);
            this.toolAppHawk.Text = "Hawk/Shark";
            this.toolAppHawk.Click += new System.EventHandler(this.toolAppHawk_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(135, 6);
            // 
            // authTokenToolStripMenuItem
            // 
            this.authTokenToolStripMenuItem.Name = "authTokenToolStripMenuItem";
            this.authTokenToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.authTokenToolStripMenuItem.Text = "Auth Token";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewInfoToolStripMenuItem,
            this.screenshotToolToolStripMenuItem,
            this.aHKAutoTypeToolStripMenuItem,
            this.toolStripSeparator4,
            this.teamViewerToolStripMenuItem,
            this.iTGlueTeamVToolStripMenuItem,
            this.toolStripSeparator7,
            this.cjasonpcToolStripMenuItem,
            this.addByGUIDToolStripMenuItem,
            this.toolStripSeparator2,
            this.liveConnectToolStripMenuItem,
            this.reconnectCurrentToolStripMenuItem,
            this.killAllLiveConnectToolStripMenuItem,
            this.kLCRemoteDebugToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // viewInfoToolStripMenuItem
            // 
            this.viewInfoToolStripMenuItem.Name = "viewInfoToolStripMenuItem";
            this.viewInfoToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.viewInfoToolStripMenuItem.Text = "View Agent Info";
            this.viewInfoToolStripMenuItem.Click += new System.EventHandler(this.ViewInfoToolStripMenuItem_Click);
            // 
            // screenshotToolToolStripMenuItem
            // 
            this.screenshotToolToolStripMenuItem.Name = "screenshotToolToolStripMenuItem";
            this.screenshotToolToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.screenshotToolToolStripMenuItem.Text = "Screenshot Tool";
            this.screenshotToolToolStripMenuItem.Click += new System.EventHandler(this.ScreenshotToolToolStripMenuItem_Click);
            // 
            // aHKAutoTypeToolStripMenuItem
            // 
            this.aHKAutoTypeToolStripMenuItem.Name = "aHKAutoTypeToolStripMenuItem";
            this.aHKAutoTypeToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.aHKAutoTypeToolStripMenuItem.Text = "AHK AutoType";
            this.aHKAutoTypeToolStripMenuItem.Click += new System.EventHandler(this.AHKAutoTypeToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(176, 6);
            // 
            // teamViewerToolStripMenuItem
            // 
            this.teamViewerToolStripMenuItem.Enabled = false;
            this.teamViewerToolStripMenuItem.Name = "teamViewerToolStripMenuItem";
            this.teamViewerToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.teamViewerToolStripMenuItem.Text = "Add: JumpBox";
            this.teamViewerToolStripMenuItem.Click += new System.EventHandler(this.JumpBoxToolStripMenuItem_Click);
            // 
            // iTGlueTeamVToolStripMenuItem
            // 
            this.iTGlueTeamVToolStripMenuItem.Name = "iTGlueTeamVToolStripMenuItem";
            this.iTGlueTeamVToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.iTGlueTeamVToolStripMenuItem.Text = "IT Glue TeamV";
            this.iTGlueTeamVToolStripMenuItem.Click += new System.EventHandler(this.iTGlueTeamVToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(176, 6);
            // 
            // cjasonpcToolStripMenuItem
            // 
            this.cjasonpcToolStripMenuItem.Enabled = false;
            this.cjasonpcToolStripMenuItem.Name = "cjasonpcToolStripMenuItem";
            this.cjasonpcToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.cjasonpcToolStripMenuItem.Text = "Add: EIT-Jason-PC";
            this.cjasonpcToolStripMenuItem.Visible = false;
            this.cjasonpcToolStripMenuItem.Click += new System.EventHandler(this.cjasonpcToolStripMenuItem_Click);
            // 
            // addByGUIDToolStripMenuItem
            // 
            this.addByGUIDToolStripMenuItem.Enabled = false;
            this.addByGUIDToolStripMenuItem.Name = "addByGUIDToolStripMenuItem";
            this.addByGUIDToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.addByGUIDToolStripMenuItem.Text = "Add by GUID";
            this.addByGUIDToolStripMenuItem.Click += new System.EventHandler(this.addByGUIDToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(176, 6);
            // 
            // liveConnectToolStripMenuItem
            // 
            this.liveConnectToolStripMenuItem.Enabled = false;
            this.liveConnectToolStripMenuItem.Name = "liveConnectToolStripMenuItem";
            this.liveConnectToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.liveConnectToolStripMenuItem.Text = "Live Connect:";
            // 
            // reconnectCurrentToolStripMenuItem
            // 
            this.reconnectCurrentToolStripMenuItem.Name = "reconnectCurrentToolStripMenuItem";
            this.reconnectCurrentToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.reconnectCurrentToolStripMenuItem.Text = "Reconnect Current";
            this.reconnectCurrentToolStripMenuItem.Click += new System.EventHandler(this.ReconnectCurrentToolStripMenuItem_Click);
            // 
            // killAllLiveConnectToolStripMenuItem
            // 
            this.killAllLiveConnectToolStripMenuItem.Name = "killAllLiveConnectToolStripMenuItem";
            this.killAllLiveConnectToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.killAllLiveConnectToolStripMenuItem.Text = "Kill All Live Connect";
            this.killAllLiveConnectToolStripMenuItem.Click += new System.EventHandler(this.KillAllLiveConnectToolStripMenuItem_Click);
            // 
            // kLCRemoteDebugToolStripMenuItem
            // 
            this.kLCRemoteDebugToolStripMenuItem.Name = "kLCRemoteDebugToolStripMenuItem";
            this.kLCRemoteDebugToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.kLCRemoteDebugToolStripMenuItem.Text = "Remote Debug";
            this.kLCRemoteDebugToolStripMenuItem.Visible = false;
            this.kLCRemoteDebugToolStripMenuItem.Click += new System.EventHandler(this.kLCRemoteDebugToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.useKLCProxyToolStripMenuItem,
            this.useMITMToolStripMenuItem,
            this.toolStripSeparator9,
            this.onRemoteControlToolStripMenuItem,
            this.toolOnRC_UseAlt,
            this.toolOnRC_UseLC,
            this.toolStripSeparator10,
            this.onLiveConnectToolStripMenuItem,
            this.toolOnLC_UseDefault,
            this.toolOnLC_UseAlt,
            this.toolOnLC_UseLC,
            this.toolOnLC_Ask,
            this.toolStripSeparator6,
            this.toolSettingsToastTest,
            this.toolSettingsToastWhenOnline});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // useKLCProxyToolStripMenuItem
            // 
            this.useKLCProxyToolStripMenuItem.Name = "useKLCProxyToolStripMenuItem";
            this.useKLCProxyToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.useKLCProxyToolStripMenuItem.Text = "Use KLCProxy";
            this.useKLCProxyToolStripMenuItem.Click += new System.EventHandler(this.UseKLCProxyToolStripMenuItem_Click);
            // 
            // useMITMToolStripMenuItem
            // 
            this.useMITMToolStripMenuItem.Name = "useMITMToolStripMenuItem";
            this.useMITMToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.useMITMToolStripMenuItem.Text = "Use MITM";
            this.useMITMToolStripMenuItem.Click += new System.EventHandler(this.useMITMToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(201, 6);
            // 
            // onRemoteControlToolStripMenuItem
            // 
            this.onRemoteControlToolStripMenuItem.Enabled = false;
            this.onRemoteControlToolStripMenuItem.Name = "onRemoteControlToolStripMenuItem";
            this.onRemoteControlToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.onRemoteControlToolStripMenuItem.Text = "On Remote Control...";
            // 
            // toolOnRC_UseAlt
            // 
            this.toolOnRC_UseAlt.Name = "toolOnRC_UseAlt";
            this.toolOnRC_UseAlt.Size = new System.Drawing.Size(204, 22);
            this.toolOnRC_UseAlt.Text = "Use Alternative";
            this.toolOnRC_UseAlt.Click += new System.EventHandler(this.toolOnRemoteControl_Click);
            // 
            // toolOnRC_UseLC
            // 
            this.toolOnRC_UseLC.Name = "toolOnRC_UseLC";
            this.toolOnRC_UseLC.Size = new System.Drawing.Size(204, 22);
            this.toolOnRC_UseLC.Text = "Use Live Connect";
            this.toolOnRC_UseLC.Click += new System.EventHandler(this.toolOnRemoteControl_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(201, 6);
            // 
            // onLiveConnectToolStripMenuItem
            // 
            this.onLiveConnectToolStripMenuItem.Enabled = false;
            this.onLiveConnectToolStripMenuItem.Name = "onLiveConnectToolStripMenuItem";
            this.onLiveConnectToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.onLiveConnectToolStripMenuItem.Text = "On \'Live Connect\'...";
            // 
            // toolOnLC_UseAlt
            // 
            this.toolOnLC_UseAlt.Name = "toolOnLC_UseAlt";
            this.toolOnLC_UseAlt.Size = new System.Drawing.Size(204, 22);
            this.toolOnLC_UseAlt.Text = "Use Alternative";
            this.toolOnLC_UseAlt.Click += new System.EventHandler(this.toolOnLiveConnect_Click);
            // 
            // toolOnLC_UseLC
            // 
            this.toolOnLC_UseLC.Name = "toolOnLC_UseLC";
            this.toolOnLC_UseLC.Size = new System.Drawing.Size(204, 22);
            this.toolOnLC_UseLC.Text = "Use Live Connect";
            this.toolOnLC_UseLC.Click += new System.EventHandler(this.toolOnLiveConnect_Click);
            // 
            // toolOnLC_Ask
            // 
            this.toolOnLC_Ask.Name = "toolOnLC_Ask";
            this.toolOnLC_Ask.Size = new System.Drawing.Size(204, 22);
            this.toolOnLC_Ask.Text = "Ask Me";
            this.toolOnLC_Ask.Click += new System.EventHandler(this.toolOnLiveConnect_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(201, 6);
            // 
            // toolSettingsToastTest
            // 
            this.toolSettingsToastTest.Name = "toolSettingsToastTest";
            this.toolSettingsToastTest.Size = new System.Drawing.Size(204, 22);
            this.toolSettingsToastTest.Text = "[Debug] Test Toast";
            this.toolSettingsToastTest.Click += new System.EventHandler(this.toolSettingsToastTest_Click);
            // 
            // toolSettingsToastWhenOnline
            // 
            this.toolSettingsToastWhenOnline.Name = "toolSettingsToastWhenOnline";
            this.toolSettingsToastWhenOnline.Size = new System.Drawing.Size(204, 22);
            this.toolSettingsToastWhenOnline.Text = "Toast when Online";
            this.toolSettingsToastWhenOnline.Click += new System.EventHandler(this.toolSettingsToastWhenOnline_Click);
            // 
            // txtSelectedLogs
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtSelectedLogs, 2);
            this.txtSelectedLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSelectedLogs.Location = new System.Drawing.Point(3, 121);
            this.txtSelectedLogs.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.txtSelectedLogs.Multiline = true;
            this.txtSelectedLogs.Name = "txtSelectedLogs";
            this.txtSelectedLogs.ReadOnly = true;
            this.txtSelectedLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSelectedLogs.Size = new System.Drawing.Size(280, 49);
            this.txtSelectedLogs.TabIndex = 0;
            this.txtSelectedLogs.Text = "Line 1\r\nLine 2\r\nLine 3";
            // 
            // btnAlternative
            // 
            this.btnAlternative.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAlternative.Location = new System.Drawing.Point(3, 3);
            this.btnAlternative.Menu = this.contextAlternative;
            this.btnAlternative.Name = "btnAlternative";
            this.btnAlternative.ShowMenuUnderCursor = true;
            this.btnAlternative.Size = new System.Drawing.Size(98, 23);
            this.btnAlternative.TabIndex = 0;
            this.btnAlternative.Text = "Alternative";
            this.btnAlternative.UseVisualStyleBackColor = true;
            // 
            // contextAlternative
            // 
            this.contextAlternative.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextAlternativeLaunch,
            this.toolStripSeparator8,
            this.contextAlternativeShared,
            this.contextAlternativePrivate});
            this.contextAlternative.Name = "contextMenuStrip1";
            this.contextAlternative.Size = new System.Drawing.Size(129, 76);
            // 
            // contextAlternativeLaunch
            // 
            this.contextAlternativeLaunch.Name = "contextAlternativeLaunch";
            this.contextAlternativeLaunch.Size = new System.Drawing.Size(128, 22);
            this.contextAlternativeLaunch.Text = "Launch";
            this.contextAlternativeLaunch.Click += new System.EventHandler(this.contextAlternativeLaunch_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(125, 6);
            // 
            // contextAlternativeShared
            // 
            this.contextAlternativeShared.Name = "contextAlternativeShared";
            this.contextAlternativeShared.Size = new System.Drawing.Size(128, 22);
            this.contextAlternativeShared.Text = "RC Shared";
            this.contextAlternativeShared.Click += new System.EventHandler(this.contextAlternativeShared_Click);
            // 
            // contextAlternativePrivate
            // 
            this.contextAlternativePrivate.Name = "contextAlternativePrivate";
            this.contextAlternativePrivate.Size = new System.Drawing.Size(128, 22);
            this.contextAlternativePrivate.Text = "RC Private";
            this.contextAlternativePrivate.Click += new System.EventHandler(this.contextAlternativePrivate_Click);
            // 
            // btnOriginal
            // 
            this.btnOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOriginal.Location = new System.Drawing.Point(3, 32);
            this.btnOriginal.Menu = this.contextOriginal;
            this.btnOriginal.Name = "btnOriginal";
            this.btnOriginal.ShowMenuUnderCursor = true;
            this.btnOriginal.Size = new System.Drawing.Size(98, 23);
            this.btnOriginal.TabIndex = 0;
            this.btnOriginal.Text = "Original";
            this.btnOriginal.UseVisualStyleBackColor = true;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "notifyIcon";
            this.notifyIcon.BalloonTipClosed += new System.EventHandler(this.notifyIcon_BalloonTipClosed);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel1.Controls.Add(this.listAgent, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSelectedLogs, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(286, 173);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnAlternative);
            this.flowLayoutPanel1.Controls.Add(this.btnOriginal);
            this.flowLayoutPanel1.Controls.Add(this.btnWatch);
            this.flowLayoutPanel1.Controls.Add(this.btnRemove);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(180, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(106, 121);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // toolOnLC_UseDefault
            // 
            this.toolOnLC_UseDefault.Name = "toolOnLC_UseDefault";
            this.toolOnLC_UseDefault.Size = new System.Drawing.Size(204, 22);
            this.toolOnLC_UseDefault.Text = "Same as Remote Control";
            this.toolOnLC_UseDefault.Click += new System.EventHandler(this.toolOnLiveConnect_Click);
            // 
            // FormInDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 197);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(302, 236);
            this.Name = "FormInDev";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "KLCProxy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormInDev_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextOriginal.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextAlternative.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listAgent;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Timer timerAuto;
        private MenuButton btnAlternative;
        private System.Windows.Forms.ContextMenuStrip contextOriginal;
        private System.Windows.Forms.ToolStripMenuItem contextOriginalLiveConnect;
        private System.Windows.Forms.ToolStripMenuItem contextOriginalTerminal;
        private System.Windows.Forms.ToolStripMenuItem contextOriginalShared;
        private System.Windows.Forms.ToolStripMenuItem contextOriginalPrivate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button btnWatch;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem killAllLiveConnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aHKAutoTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reconnectCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useKLCProxyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenshotToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem contextOriginalVNC;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem teamViewerToolStripMenuItem;
        private System.Windows.Forms.TextBox txtSelectedLogs;
        private System.Windows.Forms.ToolStripMenuItem kLCRemoteDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cjasonpcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useMITMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addByGUIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iTGlueTeamVToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private MenuButton btnOriginal;
        private System.Windows.Forms.ContextMenuStrip contextAlternative;
        private System.Windows.Forms.ToolStripMenuItem contextAlternativeLaunch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem contextAlternativeShared;
        private System.Windows.Forms.ToolStripMenuItem contextAlternativePrivate;
        private System.Windows.Forms.ToolStripMenuItem appsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolAppHawk;
        private System.Windows.Forms.ToolStripMenuItem toolAppFinch;
        private System.Windows.Forms.ToolStripMenuItem toolAppExplorer;
        private System.Windows.Forms.ToolStripMenuItem liveConnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem toolSettingsToastWhenOnline;
        private System.Windows.Forms.ToolStripMenuItem toolSettingsToastTest;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem onLiveConnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolOnLC_UseAlt;
        private System.Windows.Forms.ToolStripMenuItem toolOnLC_UseLC;
        private System.Windows.Forms.ToolStripMenuItem toolOnLC_Ask;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem onRemoteControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolOnRC_UseAlt;
        private System.Windows.Forms.ToolStripMenuItem toolOnRC_UseLC;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem authTokenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolOnLC_UseDefault;
    }
}

