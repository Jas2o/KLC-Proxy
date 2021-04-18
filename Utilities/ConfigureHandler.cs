using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLCProxy
{
    class ConfigureHandler
    {
        public enum ProxyState { Disabled, Enabled, EnabledButDifferent, BypassToFinch };

        public static ProxyState IsProxyEnabled()
        {
            InitializeCurrentUserValue();
            RegistryKey subkey = Registry.ClassesRoot.OpenSubKey(@"kaseyaliveconnect\shell\open\command", false);
            RegistryKey subkeyCU = Registry.CurrentUser.OpenSubKey(@"Software\Classes\kaseyaliveconnect\shell\open\command", true);

            string val = "";
            if (subkey != null)
                val = subkey.GetValue("").ToString();

            ProxyState ret = ProxyState.Disabled;

            if (val == ExpectedValue())
                ret = ProxyState.Enabled;
            else if (val.Contains("KLCProxy.exe"))
                ret = ProxyState.EnabledButDifferent;
            else if (val.Contains("KLC-Finch.exe"))
                ret = ProxyState.BypassToFinch;

            subkey.Close();
            subkeyCU.Close();
            return ret;
        }

        private static string ExpectedValue() {
            return string.Format("\"{0}\" \"%1\"", System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private static string ExpectedValueFinch() {
            return string.Format("\"{0}\" \"%1\"", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\KLC-Finch.exe");
        }

        public static bool ToggleProxy(bool enabled, bool bypass=false)
        {
            InitializeCurrentUserValue();

            RegistryKey subkey = Registry.ClassesRoot.OpenSubKey(@"kaseyaliveconnect\shell\open\command", false);
            RegistryKey subkeyCU = Registry.CurrentUser.OpenSubKey(@"Software\Classes\kaseyaliveconnect\shell\open\command", true);

            if (enabled) {
                if (bypass)
                    subkeyCU.SetValue("", ExpectedValueFinch());
                else
                    subkeyCU.SetValue("", ExpectedValue());
            } else
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\kaseyaliveconnect\shell\open");

            subkey.Close();
            subkeyCU.Close();

            return enabled;
        }

        private static void InitializeCurrentUserValue()
        {
            if (Registry.CurrentUser.OpenSubKey(@"Software\Classes\kaseyaliveconnect\shell\open\command", false) == null)
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\kaseyaliveconnect"))
                {
                    key.SetValue("", "URL:Kaseya Live Connect Protocol");
                    key.SetValue("URL Protocol", "");

                    using (var commandKey = key.CreateSubKey(@"shell\open\command"))
                    {
                        commandKey.SetValue("", "");
                    }
                }
            }
        }

    }
}
