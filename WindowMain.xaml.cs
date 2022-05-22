using LibKaseya;
using Microsoft.Win32;
using Newtonsoft.Json;
using nucs.JsonSettings;
using RestSharp;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using Ookii.Dialogs.Wpf;

namespace KLCProxy {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        #region Disable Maximize Box

        private const int GWL_STYLE = -16;

        private const int WS_MAXIMIZEBOX = 0x10000;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        #endregion Disable Maximize Box

        public readonly MainData mainData;
        private readonly System.Windows.Forms.NotifyIcon notifyIcon;
        private readonly NamedPipeListener<string> pipeListener;
        //private List<Agent> agents = new List<Agent>();
        private Settings Settings;

        private readonly System.Windows.Forms.Timer timerAuto;
        private readonly System.Windows.Controls.ContextMenu trayMenu;
        /*
        private readonly System.Windows.Controls.MenuItem traySettingsOnLC_Ask;
        private readonly System.Windows.Controls.MenuItem traySettingsOnLC_UseAlt;
        private readonly System.Windows.Controls.MenuItem traySettingsOnLC_UseDefault;
        private readonly System.Windows.Controls.MenuItem traySettingsOnLC_UseLC;
        private readonly System.Windows.Controls.MenuItem traySettingsOnRC_UseAlt;
        private readonly System.Windows.Controls.MenuItem traySettingsOnRC_UseLC;
        */
        //private string Kaseya.Token = "";
        public MainWindow() {
            InitializeComponent();

            mainData = new MainData();
            DataContext = mainData;

            trayMenu = (System.Windows.Controls.ContextMenu)this.FindResource("trayMenu");
            /*
            traySettingsOnRC_UseAlt = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnRC_UseAlt");
            traySettingsOnRC_UseLC = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnRC_UseLC");
            traySettingsOnLC_UseDefault = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnLC_UseDefault");
            traySettingsOnLC_UseAlt = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnLC_UseAlt");
            traySettingsOnLC_UseLC = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnLC_UseLC");
            traySettingsOnLC_Ask = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnLC_Ask");
            */
            notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info,
                Text = "KLCProxy",
                Icon = Properties.Resources.Split45
            };
            notifyIcon.BalloonTipClosed += NotifyIcon_BalloonTipClosed;
            notifyIcon.MouseClick += NotifyIcon_MouseClick;

            txtVersion.Text = Properties.Resources.BuildDate.Trim();

            timerAuto = new System.Windows.Forms.Timer
            {
                Interval = 10000
            };
            timerAuto.Tick += TimerAuto_Tick;

            string pathSettings = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLCProxy-config.json";
            if (File.Exists(pathSettings))
                Settings = JsonSettings.Load<Settings>(pathSettings);
            else {
                Settings = JsonSettings.Construct<Settings>(pathSettings);
            }
            MoveToSettingsScreenCorner();

            Kaseya.Start();

            try {
                pipeListener = new NamedPipeListener<string>("KLCProxy2", true);
                pipeListener.MessageReceived += (sender, e) => {
                    //System.Windows.MessageBox.Show(e.Message);

                    if (e.Message == "focus") {
                        Dispatcher.Invoke((Action)delegate {
                            ShowMe();
                        });
                    } else if (e.Message.Contains("liveconnect:///"))
                        LaunchFromArgument(e.Message.Replace("liveconnect:///", ""));
                    else if (e.Message.Contains("klcproxy:"))
                        CheckAndLoadToken(string.Join("", e.Message.ToCharArray().Where(Char.IsDigit)));
                    else
                        AddAgentToList(e.Message);
                };
                pipeListener.Error += (sender, e) => System.Windows.MessageBox.Show(string.Format("Error ({0}): {1}", e.ErrorType, e.Exception.ToString()));
                pipeListener.Start();
            } catch(Exception ex) {
                new WindowException(ex, "Named Pipe Setup").ShowDialog();
            }
        }

        private void AddAgentToList(KLCCommand command) {
            Agent agent = mainData.ListAgent.FirstOrDefault(x => x.ID == command.payload.agentId);

            if (agent == null) {
                Dispatcher.Invoke((Action)delegate {
                    mainData.ListAgent.Add(new Agent(command.payload.agentId, new Agent.StatusChange(NotifyForAgent)));
                    RefreshAgentsList(true);

                    if (mainData.ListAgent.Count == 1)
                        SelectedAgentRefreshRemoteControlLogs();
                });
            }
        }

        public void AddAgentToList(string agentID) {
            IRestResponse responseTV = Kaseya.GetRequest("api/v1.0/assetmgmt/agents?$filter=AgentId eq " + agentID + "M");
            if (responseTV.StatusCode == System.Net.HttpStatusCode.OK) {
                dynamic resultTV = JsonConvert.DeserializeObject(responseTV.Content);

                if (resultTV["TotalRecords"] != null && (int)resultTV["TotalRecords"] == 1) {
                    KLCCommand command = KLCCommand.Example(resultTV["Result"][0]["AgentId"].ToString(), Kaseya.Token);
                    command.SetForRemoteControl(false, true);
                    AddAgentToList(command);
                }
            }
        }

        private void BtnAlternative_Click(object sender, RoutedEventArgs e) {
            Agent agent = listAgent.SelectedItem as Agent;
            contextOriginalOneClick.IsEnabled = (agent != null && agent.OneClickAccess);
            contextAlternativeOneClick.IsEnabled = (agent != null && agent.OneClickAccess);

            btnAlternative.ContextMenu.IsOpen = true;
        }

        private void BtnOriginal_Click(object sender, RoutedEventArgs e) {
            Agent agent = listAgent.SelectedItem as Agent;
            contextOriginalOneClick.IsEnabled = (agent != null && agent.OneClickAccess);
            contextAlternativeOneClick.IsEnabled = (agent != null && agent.OneClickAccess);

            btnOriginal.ContextMenu.IsOpen = true;
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e) {
            if(listAgent.SelectedItems.Count > 1) {
                for(int i = listAgent.SelectedItems.Count; i > 0; i--) {
                    mainData.ListAgent.Remove((Agent)listAgent.SelectedItems[i-1]);
                }
                RefreshAgentsList(false);
            } else if (listAgent.SelectedIndex > -1) {
                int index = listAgent.SelectedIndex;
                if (index == listAgent.Items.Count - 1)
                    index--;
                mainData.ListAgent.Remove((listAgent.SelectedItem as Agent));
                RefreshAgentsList(false);
                listAgent.SelectedIndex = index;
                listAgent.Focus();
            }
        }

        private void BtnWatch_Click(object sender, RoutedEventArgs e) {
            foreach (Agent agent in listAgent.SelectedItems) {
                agent.Watch = !agent.Watch;
                if (!agent.Watch) {
                    agent.WaitLabel = "";
                    agent.WaitCommand = null;
                }
                agent.Refresh();
            }
            RefreshAgentsList(false);

            /*
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);
                agent.Watch = !agent.Watch;
                if (!agent.Watch) {
                    agent.WaitLabel = "";
                    agent.WaitCommand = null;
                }
                agent.Refresh(Kaseya.Token);

                RefreshAgentsList(false);
            }
            */
        }

        private bool CheckAndLoadToken(string token) {
            if (token != null) {
                try {
                    Kaseya.LoadToken(token);
                    KaseyaAuth auth = KaseyaAuth.ApiAuthX(token);

                    menuToolsBookmarks.IsEnabled = true;
                    menuToolsAddThis.IsEnabled = true;
                    menuToolsAddGUID.IsEnabled = true;
                    return true;
                } catch (Exception) {
                    KaseyaAuth.RemoveCredentials();
                }
            }
            return false;
        }

        private bool ConnectPromptWithAdminBypass(Agent agent) {
            dynamic agentApi = agent.GetAgentInfoFromAPI();

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

            string[] arrAdmins = new string[] { "administrator", "brandadmin", "adminc", "company" };
            if (arrAdmins.Contains(displayUser.ToLower()))
                return true;

            using (TaskDialog dialog = new TaskDialog())
            {
                dialog.WindowTitle = "KLCProxy";
                dialog.MainInstruction = "Confirm connection";
                dialog.Content = string.Format("Agent: {0}\r\nUser: {1}\r\n{2}", agentName, displayUser, displayGWG);
                dialog.MainIcon = TaskDialogIcon.Information;
                dialog.CenterParent = true;

                TaskDialogButton tdbYes = new TaskDialogButton(ButtonType.Yes);
                TaskDialogButton tdbCancel = new TaskDialogButton(ButtonType.Cancel);
                dialog.Buttons.Add(tdbYes);
                dialog.Buttons.Add(tdbCancel);

                TaskDialogButton button = dialog.ShowDialog(this);
                if (button == tdbYes)
                    return true;
            }

            return false;
        }

        private void ContextAlternativeLaunch_Click(object sender, RoutedEventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                command.SetForLiveConnect();

                if (agent.Watch && agent.Online == 0) {
                    if(Settings.Extra == LaunchExtra.Canary)
                        agent.WaitLabel = "CAN";
                    else
                        agent.WaitLabel = "Alt";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(true, Settings.Extra);
            }
        }

        private void ContextAlternativePrivate_Click(object sender, RoutedEventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = listAgent.SelectedItem as Agent;

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                command.SetForRemoteControl(true, true);

                if (agent.Watch && agent.Online == 0) {
                    if (Settings.Extra == LaunchExtra.Canary)
                        agent.WaitLabel = "CAN-P";
                    else
                        agent.WaitLabel = "Alt-P";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(true, Settings.Extra);
            }
        }

        private void ContextAlternativeShared_Click(object sender, RoutedEventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                command.SetForRemoteControl(false, true);

                if (agent.Watch && agent.Online == 0) {
                    if (Settings.Extra == LaunchExtra.Canary)
                        agent.WaitLabel = "CAN-S";
                    else
                        agent.WaitLabel = "Alt-S";
                    agent.WaitCommand = command;
                } else {
                    if (ConnectPromptWithAdminBypass(agent))
                        command.Launch(true, Settings.Extra);
                }
            }
        }

        private void ContextOriginalLiveConnect_Click(object sender, RoutedEventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = listAgent.SelectedItem as Agent;

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                command.SetForLiveConnect();

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "LC";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(false, Settings.Extra);
            }
        }

        private void ContextOriginalPrivate_Click(object sender, RoutedEventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = listAgent.SelectedItem as Agent;

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                command.SetForRemoteControl(true, true);

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "RC-P";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(false, Settings.Extra);
            }
        }

        private void ContextOriginalShared_Click(object sender, RoutedEventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                command.SetForRemoteControl(false, true);

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "RC-S";
                    agent.WaitCommand = command;
                } else {
                    if (ConnectPromptWithAdminBypass(agent))
                        command.Launch(false, Settings.Extra);
                }
            }
        }

        private void ContextOriginalTerminal_Click(object sender, RoutedEventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = listAgent.SelectedItem as Agent;

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                command.SetForTerminal(agent.OSType.Contains("Mac OS"));

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "LC-T";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(false, Settings.Extra);
            }
        }

        private void LaunchFromArgument(string base64) {
            KLCCommand command = KLCCommand.NewFromBase64(base64);
            Kaseya.LoadToken(command.payload.auth.Token);
            Kaseya.LaunchNotify(command.launchNotifyUrl);
            AddAgentToList(command);

            if(Settings.OverrideRCSharedtoLC && command.payload.navId == "remotecontrol/shared")
                command.SetForLiveConnect();

            if (command.payload.navId == "dashboard") {
                switch (Settings.OnLiveConnect) {
                    case Settings.OnLiveConnectAction.Default:
                        if (Settings.RedirectToAlternative)
                            command.Launch(true, Settings.Extra);
                        else
                            command.Launch(false, Settings.Extra);
                        break;

                    case Settings.OnLiveConnectAction.UseLiveConnect:
                        command.Launch(false, Settings.Extra);
                        break;

                    case Settings.OnLiveConnectAction.UseAlternative:
                        command.Launch(true, Settings.Extra);
                        break;

                    case Settings.OnLiveConnectAction.Prompt:
                        bool? result;
                        Dispatcher.Invoke((Action)delegate {
                            WindowAskMe winAskMe = new WindowAskMe();
                            result = winAskMe.ShowDialog();
                            if (result == true) {
                                if (winAskMe.ReturnUseAlternative)
                                    command.Launch(true, Settings.Extra);
                                else
                                    command.Launch(false, Settings.Extra);
                            }
                        });
                        break;
                }
            } else if(command.payload.navId == "remotecontrol/1-click") {
                switch (Settings.OnOneClick)
                {
                    case Settings.OnLiveConnectAction.UseLiveConnect:
                        command.Launch(false, Settings.Extra);
                        break;

                    case Settings.OnLiveConnectAction.UseAlternative:
                        command.Launch(true, Settings.Extra);
                        break;

                    //case Settings.OnLiveConnectAction.Default:
                    //case Settings.OnLiveConnectAction.Prompt:
                    default:
                        if (Settings.RedirectToAlternative)
                            command.Launch(true, Settings.Extra);
                        else
                            command.Launch(false, Settings.Extra);
                        break;
                }
            } else {
                if (Settings.RedirectToAlternative)
                    command.Launch(true, Settings.Extra);
                else
                    command.Launch(false, Settings.Extra);
            }

            Dispatcher.Invoke((Action)delegate {
                menuToolsBookmarks.IsEnabled = true;
                menuToolsAddThis.IsEnabled = true;
                menuToolsAddGUID.IsEnabled = true;
            });
        }

        private void LaunchKLCEx() {
            string pathKLCEx = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLCEx.exe";
            if (File.Exists(pathKLCEx)) {
                Process process = new Process();
                process.StartInfo.FileName = pathKLCEx;
                KaseyaAuth.SetCredentials(Kaseya.Token);
                //process.StartInfo.Arguments = "klcex:" + Kaseya.Token;
                process.Start();
            }
        }

        private void listAgent_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            if (listAgent.SelectedIndex > -1)
                SelectedAgentRefreshRemoteControlLogs();
        }

        private void listAgent_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (listAgent.SelectedIndex > -1)
                SelectedAgentRefreshRemoteControlLogs();
        }

        private void MenuAppsAuth_Click(object sender, RoutedEventArgs e) {
            Topmost = false;
            string newToken = WindowAuthToken.GetInput(Kaseya.Token, this);
            if (Kaseya.Token != newToken) {
                if (CheckAndLoadToken(newToken))
                    KaseyaAuth.SetCredentials(Kaseya.Token);
            }
            Topmost = Settings.AlwaysOnTop;
        }

        private void MenuAppsExplorer_Click(object sender, RoutedEventArgs e) {
            LaunchKLCEx();
        }

        private void MenuAppsFinch_Click(object sender, RoutedEventArgs e) {
            string pathKLCFinch = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Finch.exe";
            if (File.Exists(pathKLCFinch)) {
                Process process = new Process();
                process.StartInfo.FileName = pathKLCFinch;
                process.Start();
            }
        }

        private void MenuAppsFinchCharm_Click(object sender, RoutedEventArgs e)
        {
            //string pathKLCFinch = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Finch.exe";
            string pathKLCFinch = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Canary.exe";
            if (File.Exists(pathKLCFinch))
            {
                Process process = new Process();
                process.StartInfo.FileName = pathKLCFinch;
                process.StartInfo.Arguments = "-charm";
                process.Start();
            }
        }

        private void MenuAppsCanary_Click(object sender, RoutedEventArgs e)
        {
            string pathKLCCanary = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Canary.exe";
            if (File.Exists(pathKLCCanary))
            {
                Process process = new Process();
                process.StartInfo.FileName = pathKLCCanary;
                process.Start();
            }
        }

        private void MenuAppsHawk_Click(object sender, RoutedEventArgs e) {
            string pathKLCHawk = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Hawk.exe";
            if (!File.Exists(pathKLCHawk))
                pathKLCHawk = pathKLCHawk.Replace(@"\Build\", @"\KLC-Hawk\bin\");

            if (File.Exists(pathKLCHawk)) {
                Process process = new Process();
                process.StartInfo.FileName = pathKLCHawk;
                process.Start();
            }
        }

        private void MenuSettingsAlwaysOnTop_Click(object sender, RoutedEventArgs e) {
            this.Topmost = Settings.AlwaysOnTop = menuSettingsAlwaysOnTop.IsChecked;
        }

        private void MenuSettingsMinimizeToTray_Click(object sender, RoutedEventArgs e) {
            notifyIcon.Visible = Settings.AddToSystemTray = menuSettingsMinimizeToTray.IsChecked;
        }

        private void MenuSettingsStartPos_Click(object sender, RoutedEventArgs e) {
            WindowStartPos form = new WindowStartPos(Settings.StartDisplay, Settings.StartDisplayFallback, Settings.StartCorner) {
                Owner = this
            };
            if (form.ShowDialog() == true) {
                Settings.StartDisplay = form.ReturnDisplayName;
                Settings.StartDisplayFallback = form.ReturnDisplayFallback;
                Settings.StartCorner = form.ReturnCornerIndex;
                MoveToSettingsScreenCorner();
                SaveSettings();
            }
        }

        private void MenuSettingsToastTest_Click(object sender, RoutedEventArgs e) {
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(3000, "Toast Test", "This is an example toast.", System.Windows.Forms.ToolTipIcon.None);
        }

        private void MenuSettingsToastWhenOnline_Click(object sender, RoutedEventArgs e) {
            Settings.ToastWhenOnline = menuSettingsToastWhenOnline.IsChecked;
        }

        private void MenuSettingsUseHawk_Click(object sender, RoutedEventArgs e) {
            if (Settings.Extra == LaunchExtra.Hawk)
                Settings.Extra = LaunchExtra.None;
            else
                Settings.Extra = LaunchExtra.Hawk;

            menuSettingsUseHawk.IsChecked = (Settings.Extra == LaunchExtra.Hawk);
            menuSettingsUseWolf.IsChecked = (Settings.Extra == LaunchExtra.Wolf);
            menuSettingsUseCanary.IsChecked = (Settings.Extra == LaunchExtra.Canary);
        }

        private void MenuSettingsUseWolf_Click(object sender, RoutedEventArgs e) {
            if (Settings.Extra == LaunchExtra.Wolf)
                Settings.Extra = LaunchExtra.None;
            else
                Settings.Extra = LaunchExtra.Wolf;

            menuSettingsUseHawk.IsChecked = (Settings.Extra == LaunchExtra.Hawk);
            menuSettingsUseWolf.IsChecked = (Settings.Extra == LaunchExtra.Wolf);
            menuSettingsUseCanary.IsChecked = (Settings.Extra == LaunchExtra.Canary);
        }

        private void MenuSettingsUseCanary_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Extra == LaunchExtra.Canary)
                Settings.Extra = LaunchExtra.None;
            else
                Settings.Extra = LaunchExtra.Canary;

            menuSettingsUseHawk.IsChecked = (Settings.Extra == LaunchExtra.Hawk);
            menuSettingsUseWolf.IsChecked = (Settings.Extra == LaunchExtra.Wolf);
            menuSettingsUseCanary.IsChecked = (Settings.Extra == LaunchExtra.Canary);
        }

        private void MenuToolsAddGUID_Click(object sender, RoutedEventArgs e) {
            Topmost = false;
            WindowGenericEntry entry = new WindowGenericEntry("Add Agent by GUID", "Agent GUID:", "", "Add") {
                Owner = this
            };
            if (entry.ShowDialog() == true && entry.ReturnInput.Length > 0) {
                AddAgentToList(entry.ReturnInput);
            }
            Topmost = Settings.AlwaysOnTop;
        }

        /*
        private void MenuToolsAddJumpBox_Click(object sender, RoutedEventArgs e) {
            AddAgentToList("111111111111111"); //EIT Teamviewer
        }
        */

        private void MenuToolsAddThis_Click(object sender, RoutedEventArgs e) {
            string val = "";

            try {
                using (RegistryKey view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)) {
                    RegistryKey subkey = view32.OpenSubKey(@"SOFTWARE\Kaseya\Agent\AGENT11111111111111"); //Actually in WOW6432Node
                    if (subkey != null)
                        val = subkey.GetValue("AgentGUID").ToString();
                    subkey.Close();
                }

                if (val.Length > 0)
                    AddAgentToList(val);
            } catch (Exception) {
            }
        }

        private void MenuToolsAHKAutotype_Click(object sender, RoutedEventArgs e) {
            Process process = new Process();
            process.StartInfo.FileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\AutoType.ahk";
            process.Start();
        }

        private void MenuToolsITGlueJumpBox_Click(object sender, RoutedEventArgs e) {
            Process.Start("https://company.itglue.com/1432194/passwords/11018769"); //TeamV
        }

        private void MenuToolsLCKillAll_Click(object sender, RoutedEventArgs e) {
            using (TaskDialog dialog = new TaskDialog())
            {
                dialog.WindowTitle = "KLCProxy";
                dialog.Content = "Do you really want to kill Kaseya Live Connect?";
                dialog.MainIcon = TaskDialogIcon.Information;
                dialog.CenterParent = true;

                TaskDialogButton tdbYes = new TaskDialogButton(ButtonType.Yes);
                TaskDialogButton tdbCancel = new TaskDialogButton(ButtonType.Cancel);
                dialog.Buttons.Add(tdbYes);
                dialog.Buttons.Add(tdbCancel);

                TaskDialogButton button = dialog.ShowDialog(this);
                if (button == tdbYes)
                    KLCCommand.LaunchFromBase64("");
            }
        }

        /*
        private void menuToolsLCReconnect_Click(object sender, RoutedEventArgs e) {
            //Part of KLCProxy Classic

            List<string> listKaseya = new List<string>();
            WindowSnapCollection windows = WindowSnap.GetAllWindows(true, true);
            foreach (WindowSnap snap in windows) {
                if (snap.ProcessName.Contains("Kaseya") && !snap.WindowTitle.Contains("Kaseya"))
                    listKaseya.Add(snap.WindowTitle);
            }

            if (listKaseya.Count > 0) {
                Debug.WriteLine("Killing Live Connect");
                KLCCommand.LaunchFromBase64(""); // Kill Kaseya
                bool loop = true;
                while (loop) {
                    int numProcAEP = Process.GetProcessesByName("Kaseya.AdminEndPoint").Count();
                    int numProcKLC = Process.GetProcessesByName("kaseyaLiveConnect").Count();
                    if (numProcAEP == 0 && numProcKLC == 0)
                        loop = false;
                }
                Debug.WriteLine("Killed");

                //Relaunch
                foreach (string k in listKaseya) {
                    string match = k.Substring(0, k.IndexOf(":"));
                    Agent agent = agents.FirstOrDefault(a => a.Name == match);
                    if (agent != null) {
                        KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                        command.SetForRemoteControl(k.Contains(":Private"), true);// Old: rcOnly=!Settings.ForceLiveConnect
                        Debug.WriteLine("Relaunching");
                        command.Launch(false, Settings.Extra);
                        Thread.Sleep(100);
                    }
                }
            }
        }
        */

        private void MenuToolsViewAgentInfo_Click(object sender, RoutedEventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);
                Newtonsoft.Json.Linq.JObject agentApi = agent.GetAgentInfoFromAPI();
                new WindowJsonViewTable(agentApi.ToString()).Show();
            }
        }

        private void MoveToSettingsScreenCorner() {
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.PrimaryScreen;
            if (Settings.StartDisplay != "Default") {
                foreach (System.Windows.Forms.Screen scr in System.Windows.Forms.Screen.AllScreens) {
                    if (scr.DeviceName == Settings.StartDisplay || scr.Bounds.ToString() == Settings.StartDisplayFallback) {
                        screen = scr;
                        break;
                    }
                }
            }

            switch (Settings.StartCorner) {
                case 1: //Top-Right
                    this.Left = screen.WorkingArea.Left + screen.WorkingArea.Width - this.Width + 7;
                    this.Top = screen.WorkingArea.Top;
                    break;

                case 2: //Bottom-Left
                    this.Left = screen.WorkingArea.Left - 7;
                    this.Top = screen.WorkingArea.Top + screen.WorkingArea.Height - this.Height + 7;
                    break;

                case 3: //Bottom-Right
                    this.Left = screen.WorkingArea.Left + screen.WorkingArea.Width - this.Width + 7;
                    this.Top = screen.WorkingArea.Top + screen.WorkingArea.Height - this.Height + 7;
                    break;

                default: //Top-Left
                    this.Left = screen.WorkingArea.Left - 7;
                    this.Top = screen.WorkingArea.Top;
                    break;
            }
        }

        private void NotifyForAgent(string agentName) {
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(3000, "Agent Online", agentName + " is now online.", System.Windows.Forms.ToolTipIcon.None);
        }

        private void NotifyIcon_BalloonTipClosed(object sender, EventArgs e) {
            if (!Settings.AddToSystemTray)
                notifyIcon.Visible = false;
        }

        private void NotifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Minimized;
                else
                    SystemCommands.RestoreWindow(this);
                trayMenu.IsOpen = false;
            } else {
                trayMenu.IsOpen = !trayMenu.IsOpen;
            }
        }

        private void RefreshAgentsList(bool selectLast = false) {
            Dispatcher.Invoke((Action)delegate {
                if (selectLast) {
                    listAgent.SelectedIndex = listAgent.Items.Count - 1;
                    listAgent.ScrollIntoView(listAgent.SelectedItem);
                }
            });
        }

        private void SaveSettings() {
            try {
                Settings.Save();
            } catch (Exception) {
                MessageBox.Show("Seems we don't have permission to write to " + Settings.FileName, "KLCProxy: Save Settings");
            }
        }

        private void SelectedAgentRefreshRemoteControlLogs() {
            BackgroundWorker bw = new BackgroundWorker();
            //bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args) {
                Agent agent = null;
                Dispatcher.Invoke((Action)delegate {
                    agent = (listAgent.SelectedItem as Agent);
                });

                if (agent != null) {
                    string theText = agent.GetAgentRemoteControlLogsRecent();
                    Dispatcher.Invoke((Action)delegate {
                        txtSelectedLogs.Text = theText;
                    });
                }
            });

            //bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            //delegate (object o, RunWorkerCompletedEventArgs args) {
            //});

            bw.RunWorkerAsync();
        }

        private void ShowMe() {
            Show();
            //this.WindowState = WindowState.Normal;
            this.Activate();
            this.Focus();

            //this.Topmost = true;
            //this.Topmost = Settings.AlwaysOnTop;
        }

        private void TimerAuto_Tick(object sender, EventArgs e) {
            bool changes = false;
            foreach (Agent agent in listAgent.Items) {
                if (agent.Watch) {
                    agent.Refresh();
                    changes = true;
                }
            }

            if (changes)
                RefreshAgentsList(false);
        }

        private void TrayAppExplorer_Click(object sender, RoutedEventArgs e) {
            LaunchKLCEx();
        }

        private void TrayAppProxy_Click(object sender, RoutedEventArgs e) {
            NotifyIcon_MouseClick(sender, new System.Windows.Forms.MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0));
        }

        private void TrayExit_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            SaveSettings();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            //Something in here is slow

            Settings.ProxyState = ConfigureHandler.IsProxyEnabled();

            bool isDebug = new DebuggingService().RunningInDebugMode();
            if (isDebug) {
                Title += " [Debug]";
                KLCCommand.UseDebugAlternativeFirst = true;
            } else {
                menuSettingsToastTest.Visibility = Visibility.Collapsed;
            }

            try {
                using (RegistryKey view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)) {
                    RegistryKey subkey = view32.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\X86"); //Actually in WOW6432Node
                    if (subkey != null) {
                        int vcRuntimeBld = (int)subkey.GetValue("Bld");
                        if (vcRuntimeBld >= 23026) //2015
                            tbIssueVisualC.Visibility = Visibility.Collapsed;
                    }
                    subkey.Close();
                }
            } catch (Exception) {
            }

            tbIssuePath.Visibility = Visibility.Collapsed;
            switch (Settings.ProxyState)
            {
                case ConfigureHandler.ProxyState.Enabled:
                    break;
                case ConfigureHandler.ProxyState.EnabledButDifferent:
                    if (isDebug)
                        tbIssuePath.Visibility = Visibility.Collapsed;
                    else
                    {
                        ConfigureHandler.ToggleProxy(true);
                        Settings.ProxyState = ConfigureHandler.ProxyState.Enabled;
                    }
                    break;
                case ConfigureHandler.ProxyState.BypassToFinch:
                    break;
                default:
                    break;
            }

            UpdateIssueBoxVisibility();

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1) {
                if (args[1].Contains("liveconnect:///")) {
                    string base64 = args[1].Replace("liveconnect:///", "");
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

            if (File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Finch.exe")) {
            } else {
                Settings.RedirectToAlternative = false;
                Settings.OnLiveConnect = Settings.OnLiveConnectAction.Default;
                Settings.OnOneClick = Settings.OnLiveConnectAction.Default;
            }

            if (File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Canary.exe")) {
            } else
            {
                menuSettingsUseCanary.IsEnabled = false;
                menuSettingsUseCharm.IsEnabled = false;
                menuSettingsUseCanary.Visibility = Visibility.Collapsed;
                menuSettingsUseCharm.Visibility = Visibility.Collapsed;
                menuAppsFinchCharm.Visibility = Visibility.Collapsed;
                menuAppsCanary.Visibility = Visibility.Collapsed;
            }

            if (File.Exists(@"C:\Program Files\Kaseya Live Connect-MITM\KaseyaLiveConnect.exe") || File.Exists(Environment.ExpandEnvironmentVariables(@"%localappdata%\Apps\Kaseya Live Connect-MITM\KaseyaLiveConnect.exe"))) {
                //useMITMToolStripMenuItem.Checked = true;
            } else {
                menuSettingsUseHawk.IsEnabled = false;
                if (Settings.Extra == LaunchExtra.Hawk)
                    Settings.Extra = LaunchExtra.None;
            }

            if (File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\KLC-Wolf\bin\Debug\KLC-Wolf.exe")) {
            } else {
                menuSettingsUseWolf.Visibility = Visibility.Collapsed;
                if (Settings.Extra == LaunchExtra.Wolf)
                    Settings.Extra = LaunchExtra.None;
            }

            this.Topmost = menuSettingsAlwaysOnTop.IsChecked = Settings.AlwaysOnTop;
            notifyIcon.Visible = menuSettingsMinimizeToTray.IsChecked = Settings.AddToSystemTray;
            menuSettingsUseHawk.IsChecked = (Settings.Extra == LaunchExtra.Hawk);
            menuSettingsUseWolf.IsChecked = (Settings.Extra == LaunchExtra.Wolf);
            menuSettingsUseCanary.IsChecked = (Settings.Extra == LaunchExtra.Canary);

            menuSettingsToastWhenOnline.IsChecked = Settings.ToastWhenOnline;

            //menuToolsAHKAutotype.IsEnabled = File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\AutoType.ahk");

        }

        private void UpdateIssueBoxVisibility()
        {
            if (tbIssuePath.Visibility == Visibility.Collapsed && tbIssueVisualC.Visibility == Visibility.Collapsed)
                borderIssue.Visibility = Visibility.Collapsed;
            else
                borderIssue.Visibility = Visibility.Visible;
        }

        private void Window_SourceInitialized(object sender, EventArgs e) {
            //Disable maximize button while still allowing resize
            IntPtr hwnd = new WindowInteropHelper((Window)sender).Handle;
            int value = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (int)(value & ~WS_MAXIMIZEBOX));
        }

        private void Window_StateChanged(object sender, EventArgs e) {
            //In WPF if we want to enable resize without Maximize we can't set the Window to NoResize
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else {
                //Console.WriteLine("Window_StateChanged: " + WindowState);
                if(WindowState == WindowState.Normal)
                {
                    ShowMe();
                } else if (WindowState == WindowState.Minimized) {
                    if (Settings.AddToSystemTray)
                    {
                        Hide();
                        notifyIcon.Visible = true;
                    }
                }
            }
        }

        private void hyperlinkVcRedist_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
            Process.Start(new ProcessStartInfo(e.Uri.ToString()) { UseShellExecute = true });
            tbIssueVisualC.Visibility = Visibility.Collapsed;
            UpdateIssueBoxVisibility();
        }

        private void MenuToolsLCDownload_Click(object sender, RoutedEventArgs e) {
            string versionLocal = "";
            string versionOnline = "";

            string klc1 = @"C:\Program Files\Kaseya Live Connect\KaseyaLiveConnect.exe";
            string klc2 = Environment.ExpandEnvironmentVariables(@"%localappdata%\Apps\Kaseya Live Connect-MITM\KaseyaLiveConnect.exe");
            if (File.Exists(klc1)) {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(klc1);
                versionLocal = versionInfo.FileVersion;
            } else if (File.Exists(klc2))
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(klc2);
                versionLocal = versionInfo.FileVersion;
            }

            RestClient client = new RestClient("https://vsa-web.company.com.au/vsapres/api/session/AppVersions/1");
            IRestResponse response = client.Execute(new RestRequest());
            if (response.ResponseStatus == ResponseStatus.Completed) {
                try {
                    string unescaped = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(response.Content);
                    dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(unescaped);
                    foreach (dynamic key in json["data"].Children()) {
                        if (key["platform"] == "KaseyaLiveConnect-Win64") {
                            versionOnline = (string)key["version"];
                        }
                    }
                } catch (Exception) {
                    //e.g. Cloudflare error
                }
            }

            if (versionOnline.Length > 0 && versionOnline != versionLocal) {
                Process.Start("https://vsa-web.company.com.au/ManagedFiles/VSAHiddenFiles/KaseyaLiveConnect/win64/LiveConnect.exe");
            } else {
                using (TaskDialog dialog = new TaskDialog())
                {
                    dialog.WindowTitle = "KLCProxy";
                    dialog.Content = "Your KLC version matches VSA-WEB's!";
                    //dialog.MainIcon = TaskDialogIcon.Information;
                    dialog.CenterParent = true;

                    TaskDialogButton tdbOk = new TaskDialogButton(ButtonType.Ok);
                    dialog.Buttons.Add(tdbOk);

                    dialog.ShowDialog(this);
                }
            }
        }

        private void listAgent_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key != Key.LeftCtrl && e.Key != Key.RightCtrl) {
                if (e.Key == Key.C) {
                    StringBuilder sb = new StringBuilder();
                    foreach (Agent agent in listAgent.SelectedItems) {
                        sb.Append(agent.ComputerName + "\r\n");
                    }
                    if(sb.Length > 0)
                        System.Windows.Clipboard.SetDataObject(sb.ToString().Trim());
                }
            }
        }

        private void ContextOriginalOneClick_Click(object sender, RoutedEventArgs e)
        {
            if (listAgent.SelectedIndex > -1)
            {
                Agent agent = listAgent.SelectedItem as Agent;

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                command.SetForRemoteControl_OneClick();

                if (agent.Watch && agent.Online == 0)
                {
                    agent.WaitLabel = "RC-1C";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                }
                else
                    command.Launch(false, Settings.Extra);
            }
        }

        private void ContextAlternativeOneClick_Click(object sender, RoutedEventArgs e)
        {
            if (listAgent.SelectedIndex > -1)
            {
                Agent agent = listAgent.SelectedItem as Agent;

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                command.SetForRemoteControl_OneClick();

                if (agent.Watch && agent.Online == 0)
                {
                    agent.WaitLabel = "Alt-1C";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                }
                else
                    command.Launch(true, LaunchExtra.None);
            }
        }

        private void MenuSettings_Click(object sender, RoutedEventArgs e)
        {
            WindowSettings winSettings = new WindowSettings(ref Settings)
            {
                Owner = this
            };
            winSettings.ShowDialog();
        }

        private void hyperlinkPathUpdate_Click(object sender, RoutedEventArgs e)
        {
            ConfigureHandler.ToggleProxy(true);
            tbIssuePath.Visibility = Visibility.Collapsed;
            UpdateIssueBoxVisibility();
        }

        private void menuTools_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            Visibility vis = (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Shift) ? Visibility.Visible : Visibility.Collapsed;
            sepToolsVSA.Visibility = vis;
            menuToolsVSA.Visibility = vis;
            menuToolsVSANav.Visibility = vis;
            menuToolsVSAWhosOnline.Visibility = vis;
            menuToolsLCSaaS.Visibility = vis;
        }

        private void MenuToolsLCSaaS_Click(object sender, RoutedEventArgs e)
        {
            WindowSaaS winSaaS = new WindowSaaS();
            winSaaS.ShowDialog();
        }

        private void MenuSettingsUseCharm_Click(object sender, RoutedEventArgs e)
        {
            Settings.OverrideAltCharm = !Settings.OverrideAltCharm;
        }

        private void MenuToolsBookmarks_Click(object sender, RoutedEventArgs e)
        {
            WindowBookmarks winBookmarks = new WindowBookmarks(this, Settings.StartCorner);
            winBookmarks.ShowDialog();
        }

        private void menuToolsVSANav_Click(object sender, RoutedEventArgs e)
        {
            WindowVSANavigation winNavigation = new WindowVSANavigation();
            winNavigation.ShowDialog();
        }

        private void menuToolsVSAWhosOnline_Click(object sender, RoutedEventArgs e)
        {
            WindowVSAWhosOnline winWhoOnline = new WindowVSAWhosOnline();
            winWhoOnline.ShowDialog();
        }
    }
}