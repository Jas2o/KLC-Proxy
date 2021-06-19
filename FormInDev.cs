﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibKaseya;
using Microsoft.Win32;
using Newtonsoft.Json;
using nucs.JsonSettings;
using RestSharp;

namespace KLCProxy {
    public partial class FormInDev : Form {

        string lastAuthToken = "";
        List<Agent> agents = new List<Agent>();
        BindingSource agentsSource;

        public Settings Settings;

        public FormInDev() {
            InitializeComponent();

            //This is currently only here for future plans to adjust going to primary screen top left
            string pathSettings = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLCProxy-config.json";
            if (File.Exists(pathSettings))
                Settings = JsonSettings.Load<Settings>(pathSettings);
            else {
                Settings = JsonSettings.Construct<Settings>(pathSettings);
            }

            MoveToSettingsScreenCorner();

            agentsSource = new BindingSource();
            agentsSource.DataSource = agents;
            listAgent.DataSource = agentsSource;
            listAgent.DisplayMember = "Label";

            Kaseya.Start();

            NamedPipeListener<String> pipeListener = new NamedPipeListener<String>();
            pipeListener.MessageReceived += (sender, e) => {
                if (e.Message == "focus") {
                    Invoke(new Action(() => {
                        ShowMe();
                    }));
                } else if (e.Message.Contains("kaseyaliveconnect:///"))
                    LaunchFromArgument(e.Message.Replace("kaseyaliveconnect:///", ""));
                else if (e.Message.Contains("klcproxy:"))
                    CheckAndLoadToken(string.Join("", e.Message.ToCharArray().Where(Char.IsDigit)));
                else
                    AddAgentToList(e.Message);
            };
            pipeListener.Error += (sender, e) => MessageBox.Show(string.Format("Error ({0}): {1}", e.ErrorType, e.Exception.ToString()));
            pipeListener.Start();

            //clipboardMon = new ClipBoardMonitor();
            //clipboardMon.OnUpdate += UpdateClipboard;
            //UpdateClipboard(null, EventArgs.Empty);
        }

        private void MoveToSettingsScreenCorner() {
            Screen screen = Screen.PrimaryScreen;
            if (Settings.StartDisplay != "Default") {
                foreach (Screen scr in Screen.AllScreens) {
                    if (scr.DeviceName == Settings.StartDisplay || scr.Bounds.ToString() == Settings.StartDisplayFallback) {
                        screen = scr;
                        break;
                    }
                }
            }

            switch (Settings.StartCorner) {
                case 1: //Top-Right
                    this.Left = screen.WorkingArea.Left + screen.WorkingArea.Width - this.Size.Width + 7;
                    this.Top = screen.WorkingArea.Top;
                    break;
                case 2: //Bottom-Left
                    this.Left = screen.WorkingArea.Left - 7;
                    this.Top = screen.WorkingArea.Top + screen.WorkingArea.Height - this.Size.Height + 7;
                    break;
                case 3: //Bottom-Right
                    this.Left = screen.WorkingArea.Left + screen.WorkingArea.Width - this.Size.Width + 7;
                    this.Top = screen.WorkingArea.Top + screen.WorkingArea.Height - this.Size.Height + 7;
                    break;
                default: //Top-Left
                    this.Left = screen.WorkingArea.Left - 7;
                    this.Top = screen.WorkingArea.Top;
                    break;
            }
        }

        private bool CheckAndLoadToken(string token) {
            if (token != null) {
                try {
                    KaseyaAuth auth = KaseyaAuth.ApiAuthX(token);
                    lastAuthToken = token;

                    addThisComputerToolStripMenuItem.Enabled = true;
                    teamViewerToolStripMenuItem.Enabled = true;
                    addByGUIDToolStripMenuItem.Enabled = true;
                    return true;
                } catch (Exception) {
                    KaseyaAuth.RemoveCredentials();
                }
            }
            return false;
        }

        private void Form1_Load(object sender, EventArgs e) {
            ConfigureHandler.ProxyState proxyState = ConfigureHandler.IsProxyEnabled();
            if (proxyState == ConfigureHandler.ProxyState.Enabled)
                useKLCProxyToolStripMenuItem.Checked = true;
            else if (proxyState == ConfigureHandler.ProxyState.BypassToFinch)
                bypassKLCProxyToolStripMenuItem.Checked = true;

            DebuggingService ds = new DebuggingService();
            if (ds.RunningInDebugMode()) {
                this.Text = this.Text + " [Debug]";
                KLCCommand.UseDebugAlternativeFirst = true;

                if (proxyState == ConfigureHandler.ProxyState.EnabledButDifferent)
                    useKLCProxyToolStripMenuItem.Text = "Use KLCProxy (update path)";
            } else {
                if (proxyState == ConfigureHandler.ProxyState.EnabledButDifferent) {
                    ConfigureHandler.ToggleProxy(true);
                    useKLCProxyToolStripMenuItem.Checked = true;
                }
                toolSettingsToastTest.Visible = false;
            }

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1) {
                if (args[1].Contains("kaseyaliveconnect:///")) {
                    string base64 = args[1].Replace("kaseyaliveconnect:///", "");
                    LaunchFromArgument(base64);
                } else if (args[1].Contains("klcproxy:")) {
                    CheckAndLoadToken(string.Join("", args[1].ToCharArray().Where(Char.IsDigit)));
                } else {
                    CheckAndLoadToken(KaseyaAuth.GetStoredAuth());
                    AddAgentToList(args[1]);
                }
            } else {
                CheckAndLoadToken(KaseyaAuth.GetStoredAuth());
            }

            timerAuto.Start();

            txtSelectedLogs.Clear();

            if (File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLCAlt.exe") || File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Finch.exe")) {
            } else {
                toolOnRC_UseAlt.Enabled = false;
                toolOnRC_UseLC.Enabled = false;
                toolOnLC_UseAlt.Enabled = false;
                toolOnLC_UseLC.Enabled = false;
                toolOnLC_Ask.Enabled = false;

                Settings.RedirectToAlternative = false;
                Settings.OnLiveConnect = Settings.OnLiveConnectAction.Default;
            }

            if (File.Exists(@"C:\Program Files\Kaseya Live Connect-MITM\KaseyaLiveConnect.exe")) {
                //useMITMToolStripMenuItem.Checked = true;
            } else {
                useMITMToolStripMenuItem.Enabled = false;
                if (Settings.Extra == LaunchExtra.Hawk)
                    Settings.Extra = LaunchExtra.None;
            }

            if (File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\KLC-Wolf\bin\Debug\KLC-Wolf.exe")) {
            } else {
                useWolfToolStripMenuItem.Enabled = false;
                if(Settings.Extra == LaunchExtra.Wolf)
                    Settings.Extra = LaunchExtra.None;
            }

            this.TopMost = toolSettingsAlwaysOnTop.Checked = Settings.AlwaysOnTop;
            notifyIcon.Visible = toolSettingsMinimizeToTray.Checked = Settings.AddToSystemTray;
            useMITMToolStripMenuItem.Checked = (Settings.Extra == LaunchExtra.Hawk);
            useWolfToolStripMenuItem.Checked = (Settings.Extra == LaunchExtra.Wolf);
            toolSettingsToastWhenOnline.Checked = Settings.ToastWhenOnline;
            UpdateOnRCandLC();

            aHKAutoTypeToolStripMenuItem.Enabled = File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\AutoType.ahk");
        }

        private void SaveSettings() {
            try {
                Settings.Save();
            } catch (Exception) {
                MessageBox.Show("Seems we don't have permission to write to " + Settings.FileName, "KLCProxy: Save Settings");
            }
        }

        private void FormInDev_FormClosing(object sender, FormClosingEventArgs e) {
            SaveSettings();
        }

        private void timerAuto_Tick(object sender, EventArgs e)
        {
            bool changes = false;
            foreach (Agent agent in listAgent.Items)
            {
                if (agent.Watch)
                {
                    agent.Refresh(lastAuthToken);
                    changes = true;
                }
            }

            if (changes)
                RefreshAgentsList(false);
        }

        public static string JsonPrettify(string json) {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter()) {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }

        private void LaunchFromArgument(string base64) {
            KLCCommand command = KLCCommand.NewFromBase64(base64);
            lastAuthToken = command.payload.auth.Token;

            AddAgentToList(command);

            if (command.payload.navId == "dashboard") {
                switch (Settings.OnLiveConnect) {
                    case Settings.OnLiveConnectAction.Default:
                        if (Settings.RedirectToAlternative)
                            command.Launch(true, LaunchExtra.None);
                        else
                            command.Launch(false, Settings.Extra);
                        break;
                    case Settings.OnLiveConnectAction.UseLiveConnect:
                        command.Launch(false, Settings.Extra);
                        break;
                    case Settings.OnLiveConnectAction.UseAlternative:
                        command.Launch(true, LaunchExtra.None);
                        break;
                    case Settings.OnLiveConnectAction.Prompt:
                        DialogResult result;
                        menuStrip1.Invoke(new Action(() => {
                            result = new FormAskMe().ShowDialog();
                            if (result == DialogResult.Yes)
                                command.Launch(true, LaunchExtra.None);
                            else if (result == DialogResult.No)
                                command.Launch(false, Settings.Extra);
                        }));
                        break;
                }
            } else {
                if(Settings.RedirectToAlternative)
                    command.Launch(true, LaunchExtra.None);
                else
                    command.Launch(false, Settings.Extra);
            }

            menuStrip1.Invoke(new Action(() => {
                teamViewerToolStripMenuItem.Enabled = true;
            }));
        }

        #region Menu: Strip
        // Tools
        private void ViewInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listAgent.SelectedIndex > -1)
            {
                Agent agent = (listAgent.SelectedItem as Agent);
                Newtonsoft.Json.Linq.JObject agentApi = agent.GetAgentInfoFromAPI(lastAuthToken);
                string output = JsonPrettify(agentApi.ToString());
                //dynamic json = Base64ToJSON((listAgent.SelectedItem as Agent).Base64);
                //output += "\r\n\r\n" + JsonPrettify(json.ToString());
                new FormJsonViewTable(output).Show();
            }
        }

        /*
        private void ScreenshotToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            new FormKaseyaScreenshot(this).ShowDialog();
            this.TopMost = Settings.AlwaysOnTop;
        }
        */

        private void AHKAutoTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\AutoType.ahk";
            process.Start();
        }

        private void JumpBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddAgentToList("111111111111111"); //EIT Teamviewer
        }

        private void AddAgentToList(string agentID) {
            IRestResponse responseTV = Kaseya.GetRequest(lastAuthToken, "api/v1.0/assetmgmt/agents?$filter=AgentId eq " + agentID + "M");
            dynamic resultTV = JsonConvert.DeserializeObject(responseTV.Content);

            if (resultTV["TotalRecords"] != null && (int)resultTV["TotalRecords"] == 1) {
                KLCCommand command = KLCCommand.Example(resultTV["Result"][0]["AgentId"].ToString(), lastAuthToken);
                command.SetForRemoteControl(false, true);
                AddAgentToList(command);
            }
        }

        private void ReconnectCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> listKaseya = new List<string>();
            WindowSnapCollection windows = WindowSnap.GetAllWindows(true, true);
            foreach (WindowSnap snap in windows)
            {
                if (snap.ProcessName.Contains("Kaseya") && !snap.WindowTitle.Contains("Kaseya"))
                    listKaseya.Add(snap.WindowTitle);
            }

            if (listKaseya.Count > 0)
            {
                Debug.WriteLine("Killing Live Connect");
                KLCCommand.LaunchFromBase64(""); // Kill Kaseya
                bool loop = true;
                while (loop)
                {
                    int numProcAEP = Process.GetProcessesByName("Kaseya.AdminEndPoint").Count();
                    int numProcKLC = Process.GetProcessesByName("kaseyaLiveConnect").Count();
                    if (numProcAEP == 0 && numProcKLC == 0)
                        loop = false;
                }
                Debug.WriteLine("Killed");

                //Relaunch
                foreach (string k in listKaseya)
                {
                    string match = k.Substring(0, k.IndexOf(":"));
                    Agent agent = agents.FirstOrDefault(a => a.Name == match);
                    if (agent != null)
                    {
                        KLCCommand command = KLCCommand.Example(agent.ID, lastAuthToken);
                        command.SetForRemoteControl(k.Contains(":Private"), true);// Old: rcOnly=!Settings.ForceLiveConnect
                        Debug.WriteLine("Relaunching");
                        command.Launch(false, Settings.Extra);
                        Thread.Sleep(100);
                    }
                }
            }
        }

        private void KillAllLiveConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to kill Kaseya Live Connect?", "KLCProxy", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
                KLCCommand.LaunchFromBase64("");
        }

        //Settings
        /*
        private void ForceLiveConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forceLiveConnectToolStripMenuItem.Checked = Settings.ForceLiveConnect = !Settings.ForceLiveConnect;
        }
        */

        private void UseKLCProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useKLCProxyToolStripMenuItem.Checked = ConfigureHandler.ToggleProxy(!useKLCProxyToolStripMenuItem.Checked);
            if (useKLCProxyToolStripMenuItem.Checked)
                bypassKLCProxyToolStripMenuItem.Checked = false;
            useKLCProxyToolStripMenuItem.Text = "Use KLCProxy"; //This gets rid of the "(updates path)"
        }

        private void authTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            FormGenericEntry entry = new FormGenericEntry("Auth Token", "Your Kaseya authorization token is already loaded whenever KLCProxy is launched from VSA. Use this to force load and save your token to Windows Credential Manager (for this session).", lastAuthToken, "Save for Session");
            DialogResult result = entry.ShowDialog();
            if (result == DialogResult.OK && entry.ReturnInput.Length > 0) {
                if(CheckAndLoadToken(entry.ReturnInput))
                    KaseyaAuth.SetCredentials(lastAuthToken);
            }
            this.TopMost = Settings.AlwaysOnTop;
        }
        #endregion

        private void AddAgentToList(KLCCommand command)
        {
            Agent agent = agents.FirstOrDefault(x => x.ID == command.payload.agentId);

            if (agent == null)
            {
                agents.Add(new Agent(command.payload.agentId, command.payload.auth.Token, new Agent.StatusChange(NotifyForAgent)));

                RefreshAgentsList(true);

                if (agents.Count == 1)
                    SelectedAgentRefreshRemoteControlLogs();
            }
        }

        private void RefreshAgentsList(bool selectLast = false)
        {
            if (listAgent.InvokeRequired)
            {
                listAgent.Invoke(new Action(() => {
                    agentsSource.ResetBindings(false);
                    if(selectLast)
                        listAgent.SelectedIndex = listAgent.Items.Count - 1;
                }));
            }
            else
            {
                agentsSource.ResetBindings(false);
                if (selectLast)
                    listAgent.SelectedIndex = listAgent.Items.Count - 1;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listAgent.SelectedIndex > -1)
            {
                agents.Remove((listAgent.SelectedItem as Agent));

                RefreshAgentsList(false);
            }
        }

        private void btnWatch_Click(object sender, EventArgs e)
        {
            if (listAgent.SelectedIndex > -1)
            {
                Agent agent = (listAgent.SelectedItem as Agent);
                agent.Watch = !agent.Watch;
                if(!agent.Watch) {
                    agent.WaitLabel = "";
                    agent.WaitCommand = null;
                }
                agent.Refresh(lastAuthToken);

                RefreshAgentsList(false);
            }
        }

        private void listAgent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listAgent.SelectedIndex > -1)
                SelectedAgentRefreshRemoteControlLogs();
        }

        private void SelectedAgentRefreshRemoteControlLogs()
        {
            BackgroundWorker bw = new BackgroundWorker();
            //bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args) {
                Agent agent = null;
                listAgent.Invoke(new Action(() =>
                {
                    agent = (listAgent.SelectedItem as Agent);
                }));

                if (agent != null) {
                    string theText = agent.GetAgentRemoteControlLogs(lastAuthToken);

                    //if (txtSelectedLogs.InvokeRequired)
                    //{
                    txtSelectedLogs.Invoke(new Action(() => {
                        txtSelectedLogs.Text = theText;
                    }));
                    //}
                    //else
                    //{
                    //}
                }
            });

            //bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            //delegate (object o, RunWorkerCompletedEventArgs args) {
            //});

            bw.RunWorkerAsync();
        }

        private void kLCRemoteDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = @"C:\Program Files\Kaseya Live Connect\KaseyaLiveConnect.exe";
            process.StartInfo.Arguments = "--enable-logging --log-level=0 --v=4 --remote-debugging-port=9999";
            process.Start();

            Process.Start(@"http://localhost:9999");
        }

        private void alternativeToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, lastAuthToken);

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "Alt";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(true, LaunchExtra.None);
            }
        }

        private void useMITMToolStripMenuItem_Click(object sender, EventArgs e) {
            if (Settings.Extra == LaunchExtra.Hawk)
                Settings.Extra = LaunchExtra.None;
            else
                Settings.Extra = LaunchExtra.Hawk;

            useMITMToolStripMenuItem.Checked = (Settings.Extra == LaunchExtra.Hawk);
            useWolfToolStripMenuItem.Checked = (Settings.Extra == LaunchExtra.Wolf);
        }

        private void addByGUIDToolStripMenuItem_Click(object sender, EventArgs e) {
            this.TopMost = false;
            FormGenericEntry entry = new FormGenericEntry("Add Agent by GUID", "Agent GUID:", "", "Add");
            DialogResult result = entry.ShowDialog();
            if(result == DialogResult.OK && entry.ReturnInput.Length > 0) {
                AddAgentToList(entry.ReturnInput);
            }
            this.TopMost = Settings.AlwaysOnTop;
        }

        private void iTGlueTeamVToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("https://company.itglue.com/1432194/passwords/11018769"); //TeamV
        }

        private void contextOriginalLiveConnect_Click(object sender, EventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, lastAuthToken);
                command.SetForLiveConnect();

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "LC";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(false, Settings.Extra);
            }
        }

        private void contextOriginalTerminal_Click(object sender, EventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, lastAuthToken);
                command.SetForTerminal(agent.OSType.Contains("Mac OS"));

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "LC-T";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(false, Settings.Extra);
            }
        }

        private void contextOriginalShared_Click(object sender, EventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, lastAuthToken);
                command.SetForRemoteControl(false, true);

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "RC";
                    agent.WaitCommand = command;
                } else {
                    if(ConnectPromptWithAdminBypass(agent))
                        command.Launch(false, Settings.Extra);
                }
            }
        }

        private bool ConnectPromptWithAdminBypass(Agent agent) {
            dynamic agentApi = agent.GetAgentInfoFromAPI(lastAuthToken);

            string agentName = agentApi["Result"]["AgentName"];
            string agentDWG = agentApi["Result"]["DomainWorkgroup"];
            string agentUserLast = agentApi["Result"]["LastLoggedInUser"];
            string agentUserCurrent = agentApi["Result"]["CurrentUser"];

            if (agentDWG == null) agentDWG = "";
            if (agentUserLast == null) agentUserLast = "";
            if (agentUserCurrent == null) agentUserCurrent = "";

            /*
            string agentLastReboot = agentApi["Result"]["LastRebootTime"];
            string agentOSType = agentApi["Result"]["OSType"];
            string agentOSInfo = agentApi["Result"]["OSInfo"];
            string agentNetIP = agentApi["Result"]["IPAddress"];
            string agentNetDefaultGW = agentApi["Result"]["DefaultGateway"];
            string agentNetConnectionGW = agentApi["Result"]["ConnectionGatewayIP"];
            string agentNetDHCPServer = agentApi["Result"]["DHCPServer"];
            string agentNetDNS1 = agentApi["Result"]["DNSServer1"];
            string agentNetDNS2 = agentApi["Result"]["DNSServer2"];
            */

            //string displayGroup = agentApi["Result"]["MachineGroup"];
            string displayUser = (agentUserCurrent != "" ? agentUserCurrent : agentUserLast);
            string displayGWG = "";
            if (agentApi["Result"]["OSType"] != "Mac OS X")
                displayGWG = (agentDWG.Contains("(d") ? "Domain: " : "Workgroup: ") + agentDWG.Substring(0, agentDWG.IndexOf(" ("));

            DialogResult result;
            string[] arrAdmins = new string[] { "administrator", "brandadmin", "adminc", "company" };
            if (arrAdmins.Contains(displayUser.ToLower()))
                result = DialogResult.Yes;
            else {
                string textConfirm = string.Format("Agent: {0}\r\nUser: {1}\r\n{2}", agentName, displayUser, displayGWG);
                result = MessageBox.Show("Connect to:\r\n\r\n" + textConfirm, "Connecting to " + agentName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            return (result == DialogResult.Yes);
        }

        private void contextOriginalPrivate_Click(object sender, EventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, lastAuthToken);
                command.SetForRemoteControl(true, true);

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "RC-P";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(false, Settings.Extra);
            }
        }

        private void contextOriginalVNC_Click(object sender, EventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);
                string url = "https://vsa-web.company.com.au/HelpDeskTab/kVncCtl.asp?acctGuid=" + agent.ID + "&encryptRelay=1&tryDirect=&rSes=0"; // &722903
                Process.Start(url);
            }
        }

        private void contextAlternativeLaunch_Click(object sender, EventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, lastAuthToken);
                command.SetForLiveConnect();

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "Alt";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(true, LaunchExtra.None);
            }
        }

        private void contextAlternativeShared_Click(object sender, EventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, lastAuthToken);
                command.SetForRemoteControl(false, true);

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "Fi";
                    agent.WaitCommand = command;
                } else {
                    if (ConnectPromptWithAdminBypass(agent))
                        command.Launch(true, LaunchExtra.None);
                }
            }
        }

        private void contextAlternativePrivate_Click(object sender, EventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, lastAuthToken);
                command.SetForRemoteControl(true, true);

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "Fi-P";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(true, LaunchExtra.None);
            }
        }

        private void toolAppHawk_Click(object sender, EventArgs e) {
            string pathKLCHawk = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Hawk.exe";
            if (!File.Exists(pathKLCHawk))
                pathKLCHawk = pathKLCHawk.Replace(@"\Build\", @"\KLC-Hawk\bin\");

            if (File.Exists(pathKLCHawk)) {
                Process process = new Process();
                process.StartInfo.FileName = pathKLCHawk;
                process.Start();
            }
        }

        private void toolAppFinch_Click(object sender, EventArgs e) {
            string pathKLCFinch = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Finch.exe";
            if (File.Exists(pathKLCFinch)) {
                Process process = new Process();
                process.StartInfo.FileName = pathKLCFinch;
                process.Start();
            }
        }

        private void toolAppExplorer_Click(object sender, EventArgs e) {
            LaunchKLCEx();
        }

        private void LaunchKLCEx() {
            string pathKLCEx = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLCEx.exe";
            if (File.Exists(pathKLCEx)) {
                Process process = new Process();
                process.StartInfo.FileName = pathKLCEx;
                process.StartInfo.Arguments = "klcex:" + lastAuthToken;
                process.Start();
            }
        }

        private void toolSettingsToastWhenOnline_Click(object sender, EventArgs e) {
            toolSettingsToastWhenOnline.Checked = Settings.ToastWhenOnline = !toolSettingsToastWhenOnline.Checked;
        }

        private void toolSettingsToastTest_Click(object sender, EventArgs e) {
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(3000, "Toast Test", "This is an example toast.", ToolTipIcon.None);
        }

        private void NotifyForAgent(string agentName) {
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(3000, "Agent Online", agentName + " is now online.", ToolTipIcon.None);
        }

        private void notifyIcon_BalloonTipClosed(object sender, EventArgs e) {
            if(!Settings.AddToSystemTray)
                notifyIcon.Visible = false;
        }

        private void toolOnRemoteControl_Click(object sender, EventArgs e) {
            Settings.RedirectToAlternative = (sender == toolOnRC_UseAlt || sender == contextOnRC_UseAlt);

            UpdateOnRCandLC();
        }

        private void toolOnLiveConnect_Click(object sender, EventArgs e) {
            if (sender == toolOnLC_UseDefault || sender == contextOnLC_UseDefault)
                Settings.OnLiveConnect = Settings.OnLiveConnectAction.Default;
            else if(sender == toolOnLC_UseAlt || sender == contextOnLC_UseAlt)
                Settings.OnLiveConnect = Settings.OnLiveConnectAction.UseAlternative;
            else if(sender == toolOnLC_UseLC || sender == contextOnLC_UseLC)
                Settings.OnLiveConnect = Settings.OnLiveConnectAction.UseLiveConnect;
            else if (sender == toolOnLC_Ask || sender == contextOnLC_Ask)
                Settings.OnLiveConnect = Settings.OnLiveConnectAction.Prompt;

            UpdateOnRCandLC();
        }

        private void UpdateOnRCandLC() {
            toolOnRC_UseAlt.Checked = contextOnRC_UseAlt.Checked = Settings.RedirectToAlternative;
            toolOnRC_UseLC.Checked = contextOnRC_UseLC.Checked = !Settings.RedirectToAlternative;

            toolOnLC_UseDefault.Checked = contextOnLC_UseDefault.Checked = (Settings.OnLiveConnect == Settings.OnLiveConnectAction.Default);
            toolOnLC_UseAlt.Checked = contextOnLC_UseAlt.Checked = (Settings.OnLiveConnect == Settings.OnLiveConnectAction.UseAlternative);
            toolOnLC_UseLC.Checked = contextOnLC_UseLC.Checked = (Settings.OnLiveConnect == Settings.OnLiveConnectAction.UseLiveConnect);
            toolOnLC_Ask.Checked = contextOnLC_Ask.Checked = (Settings.OnLiveConnect == Settings.OnLiveConnectAction.Prompt);
        }

        private void bypassKLCProxyToolStripMenuItem_Click(object sender, EventArgs e) {
            useKLCProxyToolStripMenuItem.Checked = ConfigureHandler.ToggleProxy(false);
            useKLCProxyToolStripMenuItem.Text = "Use KLCProxy"; //This gets rid of the "(updates path)"
            bypassKLCProxyToolStripMenuItem.Checked = ConfigureHandler.ToggleProxy(!bypassKLCProxyToolStripMenuItem.Checked, true);
        }

        private void FormInDev_Resize(object sender, EventArgs e) {
            if (Settings.AddToSystemTray && this.WindowState == FormWindowState.Minimized) {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (this.WindowState == FormWindowState.Normal) {
                    Hide();
                    this.WindowState = FormWindowState.Minimized;
                } else {
                    ShowMe();
                }
            } else {
                contextTrayIcon.Show();
            }
        }

        private void ShowMe() {
            Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.Focus();

            //this.TopMost = true;
            //this.TopMost = Settings.AlwaysOnTop;
        }

        private void toolSettingsMinimizeToTray_Click(object sender, EventArgs e) {
            notifyIcon.Visible = toolSettingsMinimizeToTray.Checked = Settings.AddToSystemTray = !toolSettingsMinimizeToTray.Checked;
        }

        private void toolSettingsAlwaysOnTop_Click(object sender, EventArgs e) {
            this.TopMost = toolSettingsAlwaysOnTop.Checked = Settings.AlwaysOnTop = !toolSettingsAlwaysOnTop.Checked;
        }

        private void trayContextShow_Click(object sender, EventArgs e) {
            ShowMe();
            //notifyIcon.Visible = false;
        }

        private void trayContextExit_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void trayContextExplorer_Click(object sender, EventArgs e) {
            LaunchKLCEx();
        }

        private void useWolfToolStripMenuItem_Click(object sender, EventArgs e) {
            if (Settings.Extra == LaunchExtra.Wolf)
                Settings.Extra = LaunchExtra.None;
            else
                Settings.Extra = LaunchExtra.Wolf;

            useMITMToolStripMenuItem.Checked = (Settings.Extra == LaunchExtra.Hawk);
            useWolfToolStripMenuItem.Checked = (Settings.Extra == LaunchExtra.Wolf);
        }

        private void addThisComputerToolStripMenuItem_Click(object sender, EventArgs e) {
            string val = "";

            using (RegistryKey view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)) {
                RegistryKey subkey = view32.OpenSubKey(@"SOFTWARE\Kaseya\Agent\AGENT11111111111111"); //Actually in WOW6432Node
                if (subkey != null)
                    val = subkey.GetValue("AgentGUID").ToString();
                subkey.Close();
            }

            if(val.Length > 0)
                AddAgentToList(val);
        }

        private void toolSettingsStartPos_Click(object sender, EventArgs e) {
            FormStartPos form = new FormStartPos(Settings.StartDisplay, Settings.StartDisplayFallback, Settings.StartCorner);
            DialogResult result = form.ShowDialog(this);
            if(result == DialogResult.OK) {
                Settings.StartDisplay = form.ReturnDisplayName;
                Settings.StartDisplayFallback = form.ReturnDisplayFallback;
                Settings.StartCorner = form.ReturnCornerIndex;
                MoveToSettingsScreenCorner();
                SaveSettings();
            }
        }

        private void AdjustDropDownOpening(object sender, EventArgs e) {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem.HasDropDownItems == false) {
                return; // not a drop down item
            }

            foreach (Screen screen in Screen.AllScreens) {
                if (screen.Bounds.IntersectsWith(this.Bounds)) {
                    int checkX = screen.Bounds.X + (screen.Bounds.Width / 2);
                    int checkY = screen.Bounds.Y + (screen.Bounds.Height / 2);

                    if (this.Bounds.Y > checkY) {
                        if (this.Bounds.X > checkX)
                            menuItem.DropDownDirection = ToolStripDropDownDirection.AboveLeft;
                        else
                            menuItem.DropDownDirection = ToolStripDropDownDirection.AboveRight;
                    } else {
                        if (this.Bounds.X > checkX)
                            menuItem.DropDownDirection = ToolStripDropDownDirection.BelowLeft;
                        else
                            menuItem.DropDownDirection = ToolStripDropDownDirection.BelowRight;
                    }

                    break;
                }
            }
        }

    }
}
