using System;
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
using Newtonsoft.Json;
using nucs.JsonSettings;
using RestSharp;

namespace KLCProxy {
    public partial class FormInDev : HDshared.SnapForm {

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

            MoveToScreenPrimaryTopLeft();

            agentsSource = new BindingSource();
            agentsSource.DataSource = agents;
            listAgent.DataSource = agentsSource;
            listAgent.DisplayMember = "Label";

            Kaseya.Start();

            NamedPipeListener<String> pipeListener = new NamedPipeListener<String>();
            pipeListener.MessageReceived += (sender, e) => LaunchFromArgument(e.Message);
            pipeListener.Error += (sender, e) => MessageBox.Show(string.Format("Error ({0}): {1}", e.ErrorType, e.Exception.ToString()));
            pipeListener.Start();

            //clipboardMon = new ClipBoardMonitor();
            //clipboardMon.OnUpdate += UpdateClipboard;
            //UpdateClipboard(null, EventArgs.Empty);
        }

        private bool CheckAndLoadToken(string token) {
            if (token != null) {
                try {
                    KaseyaAuth auth = KaseyaAuth.ApiAuthX(token);
                    lastAuthToken = token;

                    cjasonpcToolStripMenuItem.Visible = cjasonpcToolStripMenuItem.Enabled = (auth.UserName == "company.com.au/username");
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
            }
            ds = null;

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1) {
                string base64 = args[1].Replace("kaseyaliveconnect:///", "");
                LaunchFromArgument(base64);
            } else {
                CheckAndLoadToken(KaseyaAuth.GetStoredAuth());
            }

            timerAuto.Start();

            txtSelectedLogs.Clear();



            if (File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLCAlt.exe") || File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Finch.exe")) {
                forceAlternativeToolStripMenuItem.Enabled = true;
            } else {
                forceAlternativeToolStripMenuItem.Enabled = false;
                Settings.RedirectToAlternative = false;
            }

            if (File.Exists(@"C:\Program Files\Kaseya Live Connect-MITM\KaseyaLiveConnect.exe")) {
                //useMITMToolStripMenuItem.Checked = true;
            } else {
                useMITMToolStripMenuItem.Enabled = false;
                Settings.UseMITM = false;
            }

            forceLiveConnectToolStripMenuItem.Checked = Settings.ForceLiveConnect;
            forceAlternativeToolStripMenuItem.Checked = Settings.RedirectToAlternative;
            useMITMToolStripMenuItem.Checked = Settings.UseMITM;

            aHKAutoTypeToolStripMenuItem.Enabled = File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\AutoType.ahk");
        }

        private void FormInDev_FormClosing(object sender, FormClosingEventArgs e) {
            Settings.Save();
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

            if (Settings.RedirectToAlternative) {
                command.Launch(true, false);
            } else {
                if (Settings.ForceLiveConnect)
                    command.ForceLiveConnect();

                command.Launch(false, Settings.UseMITM);
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
                new FormJsonViewTable(this, output).Show();
            }
        }

        private void ScreenshotToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormKaseyaScreenshot(this).ShowDialog();
        }

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

        private void cjasonpcToolStripMenuItem_Click(object sender, EventArgs e) {
            AddAgentToList("429424626294329"); //EIT-JASON-PC
        }

        private void AddAgentToList(string agentID) {
            IRestResponse responseTV = Kaseya.GetRequest(lastAuthToken, "api/v1.0/assetmgmt/agents?$filter=AgentId eq " + agentID + "M");
            dynamic resultTV = JsonConvert.DeserializeObject(responseTV.Content);

            if ((int)resultTV["TotalRecords"] == 1) {
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
                        command.SetForRemoteControl(k.Contains(":Private"), !Settings.ForceLiveConnect);
                        Debug.WriteLine("Relaunching");
                        command.Launch(false, Settings.UseMITM);
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
        private void ForceLiveConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forceLiveConnectToolStripMenuItem.Checked = Settings.ForceLiveConnect = !Settings.ForceLiveConnect;
        }

        private void UseKLCProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useKLCProxyToolStripMenuItem.Checked = ConfigureHandler.ToggleProxy(!useKLCProxyToolStripMenuItem.Checked);
            useKLCProxyToolStripMenuItem.Text = "Use KLCProxy"; //This gets rid of the "(updates path)"
        }

        private void authTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGenericEntry entry = new FormGenericEntry("Auth Token", "Your Kaseya authorization token is already loaded whenever KLCProxy is launched from VSA. Use this to force load and save your token to Windows Credential Manager (for this session).", lastAuthToken, "Save for Session");
            DialogResult result = entry.ShowDialog();
            if (result == DialogResult.OK && entry.ReturnInput.Length > 0) {
                if(CheckAndLoadToken(entry.ReturnInput))
                    KaseyaAuth.SetCredentials(lastAuthToken);
            }
        }
        #endregion

        private void AddAgentToList(KLCCommand command)
        {
            Agent agent = agents.FirstOrDefault(x => x.ID == command.payload.agentId);

            if (agent == null)
            {
                agents.Add(new Agent(command.payload.agentId, command.payload.auth.Token));

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

        private void forceAlternativeToolStripMenuItem_Click(object sender, EventArgs e) {
            forceAlternativeToolStripMenuItem.Checked = Settings.RedirectToAlternative = !Settings.RedirectToAlternative;
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
                    command.Launch(true, false);
            }
        }

        private void useMITMToolStripMenuItem_Click(object sender, EventArgs e) {
            useMITMToolStripMenuItem.Checked = Settings.UseMITM = !Settings.UseMITM;
        }

        private void addByGUIDToolStripMenuItem_Click(object sender, EventArgs e) {
            FormGenericEntry entry = new FormGenericEntry("Add Agent by GUID", "Agent GUID:", "", "Add");
            DialogResult result = entry.ShowDialog();
            if(result == DialogResult.OK && entry.ReturnInput.Length > 0) {
                AddAgentToList(entry.ReturnInput);
            }
        }

        private void iTGlueTeamVToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("https://company.itglue.com/1432194/passwords/11018769"); //TeamV
        }

        private void kLCExplorerToolStripMenuItem_Click(object sender, EventArgs e) {
            string pathKLCEx = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLCEx.exe";
            if (File.Exists(pathKLCEx)) {
                Process process = new Process();
                process.StartInfo.FileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLCEx.exe";
                process.Start();
            }
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
                    command.Launch(false, Settings.UseMITM);
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
                    command.Launch(false, Settings.UseMITM);
            }
        }

        private void contextOriginalShared_Click(object sender, EventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, lastAuthToken);
                command.SetForRemoteControl(false, !Settings.ForceLiveConnect);

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "RC";
                    agent.WaitCommand = command;
                } else {
                    if(ConnectPromptWithAdminBypass(agent))
                        command.Launch(false, Settings.UseMITM);
                }
            }
        }

        private bool ConnectPromptWithAdminBypass(Agent agent) {
            dynamic agentApi = agent.GetAgentInfoFromAPI(lastAuthToken);

            string agentName = agentApi["Result"]["AgentName"];
            string agentDWG = agentApi["Result"]["DomainWorkgroup"];
            string agentUserLast = agentApi["Result"]["LastLoggedInUser"];
            string agentUserCurrent = agentApi["Result"]["CurrentUser"];

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

            string displayGroup = agentApi["Result"]["MachineGroup"];
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
                command.SetForRemoteControl(true, !Settings.ForceLiveConnect);

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "RC-P";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(false, Settings.UseMITM);
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
                    command.Launch(true, false);
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
                        command.Launch(true, false);
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
                    command.Launch(true, false);
            }
        }

    }
}
