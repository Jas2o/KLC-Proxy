using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KLCProxy
{
    /// <summary>
    /// Interaction logic for WindowSettings.xaml
    /// </summary>
    public partial class WindowSettings : Window
    {
        private Settings settings;

        public WindowSettings()
        {
            InitializeComponent();
        }

        public WindowSettings(ref Settings settings)
        {
            InitializeComponent();
            this.settings = settings;

            switch(settings.ProxyState)
            {
                case ConfigureHandler.ProxyState.Disabled:
                    cmbFromVSA.SelectedIndex = 0;
                    break;
                case ConfigureHandler.ProxyState.Enabled:
                case ConfigureHandler.ProxyState.EnabledButDifferent:
                    cmbFromVSA.SelectedIndex = 1;
                    break;
                case ConfigureHandler.ProxyState.BypassToFinch:
                    cmbFromVSA.SelectedIndex = 2;
                    break;
            }

            cmbOnRemoteControl.SelectedIndex = (settings.RedirectToAlternative ? 1 : 0);
            cmbOnLiveConnect.SelectedIndex = (int)settings.OnLiveConnect;
            cmbOnOneClick.SelectedIndex = (int)settings.OnOneClick;
            chkOverrideRCSharedtoLC.IsChecked = settings.OverrideRCSharedtoLC;
            //chkOverrideAltCanary.IsChecked = settings.OverrideAltCanary;
            UpdateDisplay();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Left = Owner.Left;
            //this.Top = Owner.Top + 34; //56
            //this.Width = Owner.Width;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (settings != null)
            {
                settings.RedirectToAlternative = (cmbOnRemoteControl.SelectedIndex == 1);
                settings.OnLiveConnect = (Settings.OnLiveConnectAction)cmbOnLiveConnect.SelectedIndex;
                settings.OnOneClick = (Settings.OnLiveConnectAction)cmbOnOneClick.SelectedIndex;
                settings.OverrideRCSharedtoLC = (bool)chkOverrideRCSharedtoLC.IsChecked;
                //settings.OverrideAltCanary = (bool)chkOverrideAltCanary.IsChecked;
            }
        }

        private void cmbFromVSA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Collapsed)
                return;

            switch(cmbFromVSA.SelectedIndex)
            {
                case 0:
                    //Default (Live Connect)
                    settings.ProxyState = ConfigureHandler.ToggleProxy(false);
                    break;
                case 1:
                    //Use KLC Proxy
                    settings.ProxyState = ConfigureHandler.ToggleProxy(true);
                    break;
                case 2:
                    //Use KLC-Finch
                    settings.ProxyState = ConfigureHandler.ToggleProxy(true, true);
                    break;
            }

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (cmbFromVSA.SelectedIndex == 1)
            {
                cmbOnRemoteControl.IsEnabled = cmbOnLiveConnect.IsEnabled = cmbOnOneClick.IsEnabled = chkOverrideRCSharedtoLC.IsEnabled = true;
                cmbOnRemoteControl.Opacity = cmbOnLiveConnect.Opacity = cmbOnOneClick.Opacity = chkOverrideRCSharedtoLC.Opacity = 1.0;
            }
            else
            {
                cmbOnRemoteControl.IsEnabled = cmbOnLiveConnect.IsEnabled = cmbOnOneClick.IsEnabled = chkOverrideRCSharedtoLC.IsEnabled = false;
                cmbOnRemoteControl.Opacity = cmbOnLiveConnect.Opacity = cmbOnOneClick.Opacity = chkOverrideRCSharedtoLC.Opacity = 0.5;
            }
        }
    }
}
