﻿using LibKaseya;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

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

        private readonly MainData mainData;
        private readonly NotifyIcon notifyIcon;
        private readonly NamedPipeListener<string> pipeListener;
        //private List<Agent> agents = new List<Agent>();
        private readonly Settings Settings;

        private readonly Timer timerAuto;
        private readonly System.Windows.Controls.ContextMenu trayMenu;
        private readonly System.Windows.Controls.MenuItem traySettingsOnLC_Ask;
        private readonly System.Windows.Controls.MenuItem traySettingsOnLC_UseAlt;
        private readonly System.Windows.Controls.MenuItem traySettingsOnLC_UseDefault;
        private readonly System.Windows.Controls.MenuItem traySettingsOnLC_UseLC;
        private readonly System.Windows.Controls.MenuItem traySettingsOnRC_UseAlt;
        private readonly System.Windows.Controls.MenuItem traySettingsOnRC_UseLC;
        //private string Kaseya.Token = "";
        public MainWindow() {
            InitializeComponent();

            mainData = new MainData();
            DataContext = mainData;

            trayMenu = (System.Windows.Controls.ContextMenu)this.FindResource("trayMenu");
            traySettingsOnRC_UseAlt = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnRC_UseAlt");
            traySettingsOnRC_UseLC = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnRC_UseLC");
            traySettingsOnLC_UseDefault = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnLC_UseDefault");
            traySettingsOnLC_UseAlt = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnLC_UseAlt");
            traySettingsOnLC_UseLC = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnLC_UseLC");
            traySettingsOnLC_Ask = (System.Windows.Controls.MenuItem)LogicalTreeHelper.FindLogicalNode(trayMenu, "traySettingsOnLC_Ask");
            notifyIcon = new NotifyIcon {
                BalloonTipIcon = ToolTipIcon.Info,
                Text = "KLCProxy",
                Icon = Properties.Resources.Split45
            };
            notifyIcon.BalloonTipClosed += NotifyIcon_BalloonTipClosed;
            notifyIcon.MouseClick += NotifyIcon_MouseClick;

            txtVersion.Text = Properties.Resources.BuildDate.Trim();

            timerAuto = new Timer {
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

        private void AddAgentToList(string agentID) {
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
            btnAlternative.ContextMenu.IsOpen = true;
        }

        private void BtnOriginal_Click(object sender, RoutedEventArgs e) {
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

                    menuToolsAddThis.IsEnabled = true;
                    menuToolsAddJumpBox.IsEnabled = true;
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

            System.Windows.Forms.DialogResult result;
            string[] arrAdmins = new string[] { "administrator", "brandadmin", "adminc", "company" };
            if (arrAdmins.Contains(displayUser.ToLower()))
                result = System.Windows.Forms.DialogResult.Yes;
            else {
                string textConfirm = string.Format("Agent: {0}\r\nUser: {1}\r\n{2}", agentName, displayUser, displayGWG);
                result = System.Windows.Forms.MessageBox.Show("Connect to:\r\n\r\n" + textConfirm, "Connecting to " + agentName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            return (result == System.Windows.Forms.DialogResult.Yes);
        }

        private void ContextAlternativeLaunch_Click(object sender, RoutedEventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                command.SetForLiveConnect();

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "Alt";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(true, LaunchExtra.None);
            }
        }

        private void ContextAlternativePrivate_Click(object sender, RoutedEventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = listAgent.SelectedItem as Agent;

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
                command.SetForRemoteControl(true, true);

                if (agent.Watch && agent.Online == 0) {
                    agent.WaitLabel = "Fi-P";
                    agent.WaitCommand = command;
                    RefreshAgentsList(false);
                } else
                    command.Launch(true, LaunchExtra.None);
            }
        }

        private void ContextAlternativeShared_Click(object sender, RoutedEventArgs e) {
            if (listAgent.SelectedIndex > -1) {
                Agent agent = (listAgent.SelectedItem as Agent);

                KLCCommand command = KLCCommand.Example(agent.ID, Kaseya.Token);
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
                    agent.WaitLabel = "RC";
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
                        bool? result;
                        Dispatcher.Invoke((Action)delegate {
                            WindowAskMe winAskMe = new WindowAskMe();
                            result = winAskMe.ShowDialog();
                            if (result == true) {
                                if(winAskMe.ReturnUseAlternative)
                                    command.Launch(true, LaunchExtra.None);
                                else
                                    command.Launch(false, Settings.Extra);
                            }
                        });
                        break;
                }
            } else {
                if (Settings.RedirectToAlternative)
                    command.Launch(true, LaunchExtra.None);
                else
                    command.Launch(false, Settings.Extra);
            }

            Dispatcher.Invoke((Action)delegate {
                menuToolsAddThis.IsEnabled = true;
                menuToolsAddJumpBox.IsEnabled = true;
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
            Settings.AlwaysOnTop = menuSettingsAlwaysOnTop.IsChecked;
        }

        private void MenuSettingsMinimizeToTray_Click(object sender, RoutedEventArgs e) {
            Settings.AddToSystemTray = menuSettingsMinimizeToTray.IsChecked;
        }

        private void MenuSettingsOnLC_Click(object sender, RoutedEventArgs e) {
            if (sender == menuSettingsOnLC_UseDefault || sender == traySettingsOnLC_UseDefault)
                Settings.OnLiveConnect = Settings.OnLiveConnectAction.Default;
            else if (sender == menuSettingsOnLC_UseAlt || sender == traySettingsOnLC_UseAlt)
                Settings.OnLiveConnect = Settings.OnLiveConnectAction.UseAlternative;
            else if (sender == menuSettingsOnLC_UseLC || sender == traySettingsOnLC_UseLC)
                Settings.OnLiveConnect = Settings.OnLiveConnectAction.UseLiveConnect;
            else if (sender == menuSettingsOnLC_Ask || sender == traySettingsOnLC_Ask)
                Settings.OnLiveConnect = Settings.OnLiveConnectAction.Prompt;

            UpdateOnRCandLC();
        }

        private void MenuSettingsOnRC_Click(object sender, RoutedEventArgs e) {
            Settings.RedirectToAlternative = (sender == menuSettingsOnRC_UseAlt || sender == traySettingsOnRC_UseAlt);
            UpdateOnRCandLC();
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
            notifyIcon.ShowBalloonTip(3000, "Toast Test", "This is an example toast.", ToolTipIcon.None);
        }

        private void MenuSettingsToastWhenOnline_Click(object sender, RoutedEventArgs e) {
            Settings.ToastWhenOnline = menuSettingsToastWhenOnline.IsChecked;
        }

        private void MenuSettingsUseBypass_Click(object sender, RoutedEventArgs e) {
            menuSettingsUseProxy.IsChecked = ConfigureHandler.ToggleProxy(false);
            menuSettingsUseProxy.Header = "Use KLCProxy"; //This gets rid of the "(updates path)"
            ConfigureHandler.ToggleProxy(menuSettingsUseBypass.IsChecked, true);
        }

        private void MenuSettingsUseHawk_Click(object sender, RoutedEventArgs e) {
            if (Settings.Extra == LaunchExtra.Hawk)
                Settings.Extra = LaunchExtra.None;
            else
                Settings.Extra = LaunchExtra.Hawk;

            menuSettingsUseHawk.IsChecked = (Settings.Extra == LaunchExtra.Hawk);
            menuSettingsUseWolf.IsChecked = (Settings.Extra == LaunchExtra.Wolf);
        }

        private void MenuSettingsUseProxy_Click(object sender, RoutedEventArgs e) {
            ConfigureHandler.ToggleProxy(menuSettingsUseProxy.IsChecked);
            if (menuSettingsUseProxy.IsChecked)
                menuSettingsUseBypass.IsChecked = false;
            menuSettingsUseProxy.Header = "Use KLCProxy"; //This gets rid of the "(updates path)"
        }

        private void MenuSettingsUseWolf_Click(object sender, RoutedEventArgs e) {
            if (Settings.Extra == LaunchExtra.Wolf)
                Settings.Extra = LaunchExtra.None;
            else
                Settings.Extra = LaunchExtra.Wolf;

            menuSettingsUseHawk.IsChecked = (Settings.Extra == LaunchExtra.Hawk);
            menuSettingsUseWolf.IsChecked = (Settings.Extra == LaunchExtra.Wolf);
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

        private void MenuToolsAddJumpBox_Click(object sender, RoutedEventArgs e) {
            AddAgentToList("111111111111111"); //EIT Teamviewer
        }

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
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Do you really want to kill Kaseya Live Connect?", "KLCProxy", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                KLCCommand.LaunchFromBase64("");
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
            notifyIcon.ShowBalloonTip(3000, "Agent Online", agentName + " is now online.", ToolTipIcon.None);
        }

        private void NotifyIcon_BalloonTipClosed(object sender, EventArgs e) {
            if (!Settings.AddToSystemTray)
                notifyIcon.Visible = false;
        }

        private void NotifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (this.WindowState == WindowState.Normal) {
                    Hide();
                    this.WindowState = WindowState.Minimized;
                } else {
                    ShowMe();
                }
            } else {
                trayMenu.IsOpen = true;
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
                System.Windows.Forms.MessageBox.Show("Seems we don't have permission to write to " + Settings.FileName, "KLCProxy: Save Settings");
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
            this.WindowState = WindowState.Normal;
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
            ShowMe();
        }

        private void TrayExit_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void UpdateOnRCandLC() {
            menuSettingsOnRC_UseAlt.IsChecked = traySettingsOnRC_UseAlt.IsChecked = Settings.RedirectToAlternative;
            menuSettingsOnRC_UseLC.IsChecked = traySettingsOnRC_UseLC.IsChecked = !Settings.RedirectToAlternative;

            menuSettingsOnLC_UseDefault.IsChecked = traySettingsOnLC_UseDefault.IsChecked = (Settings.OnLiveConnect == Settings.OnLiveConnectAction.Default);
            menuSettingsOnLC_UseAlt.IsChecked = traySettingsOnLC_UseAlt.IsChecked = (Settings.OnLiveConnect == Settings.OnLiveConnectAction.UseAlternative);
            menuSettingsOnLC_UseLC.IsChecked = traySettingsOnLC_UseLC.IsChecked = (Settings.OnLiveConnect == Settings.OnLiveConnectAction.UseLiveConnect);
            menuSettingsOnLC_Ask.IsChecked = traySettingsOnLC_Ask.IsChecked = (Settings.OnLiveConnect == Settings.OnLiveConnectAction.Prompt);
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            SaveSettings();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            //Something in here is slow

            ConfigureHandler.ProxyState proxyState = ConfigureHandler.IsProxyEnabled();
            if (proxyState == ConfigureHandler.ProxyState.Enabled)
                menuSettingsUseProxy.IsChecked = true;
            else if (proxyState == ConfigureHandler.ProxyState.BypassToFinch)
                menuSettingsUseBypass.IsChecked = true;

            DebuggingService ds = new DebuggingService();
            if (ds.RunningInDebugMode()) {
                Title += " [Debug]";
                KLCCommand.UseDebugAlternativeFirst = true;

                if (proxyState == ConfigureHandler.ProxyState.EnabledButDifferent)
                    menuSettingsUseProxy.Header = "Use KLCProxy (update path)";
            } else {
                if (proxyState == ConfigureHandler.ProxyState.EnabledButDifferent) {
                    ConfigureHandler.ToggleProxy(true);
                    menuSettingsUseProxy.IsChecked = true;
                }
                menuSettingsToastTest.Visibility = Visibility.Collapsed;
            }

            try {
                using (RegistryKey view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)) {
                    RegistryKey subkey = view32.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\X86"); //Actually in WOW6432Node
                    if (subkey != null) {
                        int vcRuntimeBld = (int)subkey.GetValue("Bld");
                        if (vcRuntimeBld >= 23026) //2015
                            borderVcRedist.Visibility = Visibility.Collapsed;
                    }
                    subkey.Close();
                }
            } catch (Exception) {
            }

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

            if (File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLCAlt.exe") || File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\KLC-Finch.exe")) {
            } else {
                menuSettingsOnRC_UseAlt.IsEnabled = false;
                menuSettingsOnRC_UseLC.IsEnabled = false;
                menuSettingsOnLC_UseAlt.IsEnabled = false;
                menuSettingsOnLC_UseLC.IsEnabled = false;
                menuSettingsOnLC_Ask.IsEnabled = false;

                Settings.RedirectToAlternative = false;
                Settings.OnLiveConnect = Settings.OnLiveConnectAction.Default;
            }

            if (File.Exists(@"C:\Program Files\Kaseya Live Connect-MITM\KaseyaLiveConnect.exe")) {
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
            menuSettingsToastWhenOnline.IsChecked = Settings.ToastWhenOnline;
            UpdateOnRCandLC();

            //menuToolsAHKAutotype.IsEnabled = File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\AutoType.ahk");
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
            else if (WindowState == WindowState.Minimized) {
                if (Settings.AddToSystemTray) {
                    Hide();
                    notifyIcon.Visible = true;
                }
            }
        }

        private void hyperlinkVcRedist_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
            Process.Start(new ProcessStartInfo(e.Uri.ToString()) { UseShellExecute = true });
        }

        private void MenuToolsLCDownload_Click(object sender, RoutedEventArgs e) {
            string versionLocal = "";
            string versionOnline = "";

            string klc = @"C:\Program Files\Kaseya Live Connect\KaseyaLiveConnect.exe";
            if (File.Exists(klc)) {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(klc);
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
                System.Windows.MessageBox.Show("Your KLC version matches VSA-WEB's!", "KLCProxy");
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
    }
}