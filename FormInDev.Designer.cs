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
            this.forceAlternativeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceLiveConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useKLCProxyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useMITMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.authTokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSelectedLogs = new System.Windows.Forms.TextBox();
            this.btnAlternative = new KLCProxy.MenuButton();
            this.contextAlternative = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextAlternativeLaunch = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.contextAlternativeShared = new System.Windows.Forms.ToolStripMenuItem();
            this.contextAlternativePrivate = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOriginal = new KLCProxy.MenuButton();
            this.contextOriginal.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextAlternative.SuspendLayout();
            this.SuspendLayout();
            // 
            // listAgent
            // 
            this.listAgent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listAgent.FormattingEnabled = true;
            this.listAgent.Location = new System.Drawing.Point(12, 27);
            this.listAgent.Name = "listAgent";
            this.listAgent.Size = new System.Drawing.Size(157, 108);
            this.listAgent.TabIndex = 0;
            this.listAgent.SelectedIndexChanged += new System.EventHandler(this.listAgent_SelectedIndexChanged);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(176, 112);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(98, 23);
            this.btnRemove.TabIndex = 6;
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
            this.btnWatch.Location = new System.Drawing.Point(176, 83);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new System.Drawing.Size(98, 23);
            this.btnWatch.TabIndex = 11;
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
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // appsToolStripMenuItem
            // 
            this.appsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolAppExplorer,
            this.toolAppFinch,
            this.toolAppHawk});
            this.appsToolStripMenuItem.Name = "appsToolStripMenuItem";
            this.appsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.appsToolStripMenuItem.Text = "Apps";
            // 
            // toolAppExplorer
            // 
            this.toolAppExplorer.Name = "toolAppExplorer";
            this.toolAppExplorer.Size = new System.Drawing.Size(180, 22);
            this.toolAppExplorer.Text = "Explorer";
            this.toolAppExplorer.Click += new System.EventHandler(this.toolAppExplorer_Click);
            // 
            // toolAppFinch
            // 
            this.toolAppFinch.Name = "toolAppFinch";
            this.toolAppFinch.Size = new System.Drawing.Size(180, 22);
            this.toolAppFinch.Text = "Finch";
            this.toolAppFinch.Click += new System.EventHandler(this.toolAppFinch_Click);
            // 
            // toolAppHawk
            // 
            this.toolAppHawk.Name = "toolAppHawk";
            this.toolAppHawk.Size = new System.Drawing.Size(180, 22);
            this.toolAppHawk.Text = "Hawk/Shark";
            this.toolAppHawk.Click += new System.EventHandler(this.toolAppHawk_Click);
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
            this.forceAlternativeToolStripMenuItem,
            this.forceLiveConnectToolStripMenuItem,
            this.useKLCProxyToolStripMenuItem,
            this.useMITMToolStripMenuItem,
            this.toolStripSeparator5,
            this.authTokenToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // forceAlternativeToolStripMenuItem
            // 
            this.forceAlternativeToolStripMenuItem.Name = "forceAlternativeToolStripMenuItem";
            this.forceAlternativeToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.forceAlternativeToolStripMenuItem.Text = "Force Alternative";
            this.forceAlternativeToolStripMenuItem.Click += new System.EventHandler(this.forceAlternativeToolStripMenuItem_Click);
            // 
            // forceLiveConnectToolStripMenuItem
            // 
            this.forceLiveConnectToolStripMenuItem.Name = "forceLiveConnectToolStripMenuItem";
            this.forceLiveConnectToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.forceLiveConnectToolStripMenuItem.Text = "Force Live Connect";
            this.forceLiveConnectToolStripMenuItem.Click += new System.EventHandler(this.ForceLiveConnectToolStripMenuItem_Click);
            // 
            // useKLCProxyToolStripMenuItem
            // 
            this.useKLCProxyToolStripMenuItem.Name = "useKLCProxyToolStripMenuItem";
            this.useKLCProxyToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.useKLCProxyToolStripMenuItem.Text = "Use KLCProxy";
            this.useKLCProxyToolStripMenuItem.Click += new System.EventHandler(this.UseKLCProxyToolStripMenuItem_Click);
            // 
            // useMITMToolStripMenuItem
            // 
            this.useMITMToolStripMenuItem.Name = "useMITMToolStripMenuItem";
            this.useMITMToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.useMITMToolStripMenuItem.Text = "Use MITM";
            this.useMITMToolStripMenuItem.Click += new System.EventHandler(this.useMITMToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(172, 6);
            // 
            // authTokenToolStripMenuItem
            // 
            this.authTokenToolStripMenuItem.Name = "authTokenToolStripMenuItem";
            this.authTokenToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.authTokenToolStripMenuItem.Text = "Auth Token";
            this.authTokenToolStripMenuItem.Click += new System.EventHandler(this.authTokenToolStripMenuItem_Click);
            // 
            // txtSelectedLogs
            // 
            this.txtSelectedLogs.Location = new System.Drawing.Point(12, 141);
            this.txtSelectedLogs.Multiline = true;
            this.txtSelectedLogs.Name = "txtSelectedLogs";
            this.txtSelectedLogs.ReadOnly = true;
            this.txtSelectedLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSelectedLogs.Size = new System.Drawing.Size(262, 46);
            this.txtSelectedLogs.TabIndex = 15;
            this.txtSelectedLogs.Text = "Line 1\r\nLine 2\r\nLine 3";
            // 
            // btnAlternative
            // 
            this.btnAlternative.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAlternative.Location = new System.Drawing.Point(176, 27);
            this.btnAlternative.Menu = this.contextAlternative;
            this.btnAlternative.Name = "btnAlternative";
            this.btnAlternative.ShowMenuUnderCursor = true;
            this.btnAlternative.Size = new System.Drawing.Size(98, 23);
            this.btnAlternative.TabIndex = 8;
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
            this.btnOriginal.Location = new System.Drawing.Point(176, 54);
            this.btnOriginal.Menu = this.contextOriginal;
            this.btnOriginal.Name = "btnOriginal";
            this.btnOriginal.ShowMenuUnderCursor = true;
            this.btnOriginal.Size = new System.Drawing.Size(98, 23);
            this.btnOriginal.TabIndex = 16;
            this.btnOriginal.Text = "Original";
            this.btnOriginal.UseVisualStyleBackColor = true;
            // 
            // FormInDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 197);
            this.Controls.Add(this.btnOriginal);
            this.Controls.Add(this.txtSelectedLogs);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnWatch);
            this.Controls.Add(this.btnAlternative);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.listAgent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(302, 156);
            this.Name = "FormInDev";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "KLCProxy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormInDev_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextOriginal.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextAlternative.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem forceLiveConnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useKLCProxyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenshotToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem contextOriginalVNC;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem teamViewerToolStripMenuItem;
        private System.Windows.Forms.TextBox txtSelectedLogs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem authTokenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kLCRemoteDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cjasonpcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceAlternativeToolStripMenuItem;
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
    }
}

