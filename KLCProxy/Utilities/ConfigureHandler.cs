using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLCProxy
{
    class ConfigureHandler
    {
        public enum ProxyState { Disabled, Enabled, EnabledButDifferent };

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

            subkey.Close();
            subkeyCU.Close();
            return ret;
        }

        private static string ExpectedValue() {
            return string.Format("\"{0}\" \"%1\"", System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public static bool ToggleProxy(bool enabled)
        {
            InitializeCurrentUserValue();

            RegistryKey subkey = Registry.ClassesRoot.OpenSubKey(@"kaseyaliveconnect\shell\open\command", false);
            RegistryKey subkeyCU = Registry.CurrentUser.OpenSubKey(@"Software\Classes\kaseyaliveconnect\shell\open\command", true);

            if(enabled)
                subkeyCU.SetValue("", ExpectedValue());
            else
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
