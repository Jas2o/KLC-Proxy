using Microsoft.Win32;
using System;
using System.IO;

namespace KLC_Proxy
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
            else if (val.Contains("KLC-Proxy.exe"))
                ret = ProxyState.EnabledButDifferent;
            else if (val.Contains("KLC-Finch.exe"))
                ret = ProxyState.BypassToFinch;

            subkey.Close();
            //subkeyCU.Close();
            return ret;
        }

        private static string ExpectedValue() {
            return string.Format("\"{0}\" \"%1\"", Environment.ProcessPath);
        }

        private static string ExpectedValueFinch() {
            return string.Format("\"{0}\" \"%1\"", Path.GetDirectoryName(Environment.ProcessPath) + "\\KLC-Finch.exe");
        }

        public static ProxyState ToggleProxy(bool enabled, bool bypass=false)
        {
            ProxyState value = ProxyState.Disabled;

            InitializeCurrentUserValue();

            RegistryKey subkey1 = Registry.ClassesRoot.OpenSubKey(@"liveconnect\shell\open\command", false);
            RegistryKey subkey1CU = Registry.CurrentUser.OpenSubKey(@"Software\Classes\liveconnect\shell\open\command", true);

            RegistryKey subkey2 = Registry.ClassesRoot.OpenSubKey(@"kaseyaliveconnect\shell\open\command", false);
            RegistryKey subkey2CU = Registry.CurrentUser.OpenSubKey(@"Software\Classes\kaseyaliveconnect\shell\open\command", true);

            if (enabled)
            {
                if (bypass)
                {
                    subkey1CU.SetValue("", ExpectedValueFinch());
                    subkey2CU.SetValue("", ExpectedValueFinch());
                    value = ProxyState.BypassToFinch;
                }
                else
                {
                    subkey1CU.SetValue("", ExpectedValue());
                    subkey2CU.SetValue("", ExpectedValue());
                    value = ProxyState.Enabled;
                }
            }
            else
            {
                string pathLocal = Environment.ExpandEnvironmentVariables(@"%localappdata%\Apps\Kaseya Live Connect\KaseyaLiveConnect.exe");
                if (File.Exists(pathLocal))
                {
                    subkey1CU.SetValue("", string.Format("\"{0}\" \"%1\"", pathLocal));
                    subkey2CU.SetValue("", string.Format("\"{0}\" \"%1\"", pathLocal));
                } else
                {
                    Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\liveconnect\shell\open");
                    Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\kaseyaliveconnect\shell\open");
                }
            }

            subkey1.Close();
            subkey1CU.Close();
            subkey2.Close();
            subkey2CU.Close();

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

            if (Registry.CurrentUser.OpenSubKey(@"Software\Classes\kaseyaliveconnect\shell\open\command", false) == null)
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\kaseyaliveconnect"))
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
