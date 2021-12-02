using Microsoft.Win32;
using System;
using System.IO;

namespace KLCProxy
{
    public class ConfigureHandler
    {
        public enum ProxyState { Disabled, Enabled, EnabledButDifferent, BypassToFinch };

        public static ProxyState IsProxyEnabled()
        {
            InitializeCurrentUserValue();
            RegistryKey subkey = Registry.ClassesRoot.OpenSubKey(@"liveconnect\shell\open\command", false);
            //RegistryKey subkeyCU = Registry.CurrentUser.OpenSubKey(@"Software\Classes\liveconnect\shell\open\command", true);

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
            //subkeyCU.Close();
            return ret;
        }

        private static string ExpectedValue() {
            return string.Format("\"{0}\" \"%1\"", System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private static string ExpectedValueFinch() {
            return string.Format("\"{0}\" \"%1\"", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\KLC-Finch.exe");
        }

        public static ProxyState ToggleProxy(bool enabled, bool bypass=false)
        {
            ProxyState value = ProxyState.Disabled;

            InitializeCurrentUserValue();

            RegistryKey subkey = Registry.ClassesRoot.OpenSubKey(@"liveconnect\shell\open\command", false);
            RegistryKey subkeyCU = Registry.CurrentUser.OpenSubKey(@"Software\Classes\liveconnect\shell\open\command", true);

            if (enabled) {
                if (bypass)
                {
                    subkeyCU.SetValue("", ExpectedValueFinch());
                    value = ProxyState.BypassToFinch;
                }
                else
                {
                    subkeyCU.SetValue("", ExpectedValue());
                    value = ProxyState.Enabled;
                }
            } else
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\liveconnect\shell\open");

            subkey.Close();
            subkeyCU.Close();

            return value;
        }

        private static void InitializeCurrentUserValue()
        {
            if (Registry.CurrentUser.OpenSubKey(@"Software\Classes\liveconnect\shell\open\command", false) == null)
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\liveconnect"))
                {
                    key.SetValue("", "URL:Live Connect Protocol");
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
