using LibKaseya;
using nucs.JsonSettings;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KLC_Proxy {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private const string appName = "KLC-Proxy2";
        private static Mutex mutex = null;
        public static KLCShared Shared;

        public App() : base() {
            if (!Debugger.IsAttached) {
                //Setup exception handling rather than closing rudely.
                AppDomain.CurrentDomain.UnhandledException += (sender, args) => ShowUnhandledException(args.ExceptionObject as Exception, "AppDomain.CurrentDomain.UnhandledException");
                TaskScheduler.UnobservedTaskException += (sender, args) => {
                    //ShowUnhandledExceptionFromSrc(args.Exception, "TaskScheduler.UnobservedTaskException");
                    args.SetObserved();
                };

                Dispatcher.UnhandledException += (sender, args) => {
                    args.Handled = true;
                    ShowUnhandledExceptionFromSrc(args.Exception, "Dispatcher.UnhandledException");
                };
            }

            mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew) {
                string[] args = Environment.GetCommandLineArgs();
                if (args.Length > 1) {
                    NamedPipeListener.SendMessage("KLC-Proxy2", true, args[1]);
                } else {
                    NamedPipeListener.SendMessage("KLC-Proxy2", true, "focus");
                }

                Current.Shutdown();
            }

            string pathShared = Path.GetDirectoryName(Environment.ProcessPath) + @"\KLC-Shared.json";
            if (File.Exists(pathShared))
                Shared = JsonSettings.Load<KLCShared>(pathShared);
            else
                Shared = JsonSettings.Construct<KLCShared>(pathShared);
        }

        public static void ShowUnhandledExceptionFromSrc(Exception e, string source) {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                new WindowException(e, source + " - " + e.GetType().ToString()).Show();
            });
        }

        static void ShowUnhandledException(Exception e, string unhandledExceptionType) {
            new WindowException(e, unhandledExceptionType).Show(); //, Debugger.IsAttached
        }

    }
}
