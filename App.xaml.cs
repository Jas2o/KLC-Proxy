using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KLCProxy {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private const string appName = "KLCProxy2";
        private static Mutex mutex = null;

        public App() : base() {
            if (!Debugger.IsAttached) {
                //Setup exception handling rather than closing rudely.
                AppDomain.CurrentDomain.UnhandledException += (sender, args) => ShowUnhandledException(args.ExceptionObject as Exception, "AppDomain.CurrentDomain.UnhandledException");
                TaskScheduler.UnobservedTaskException += (sender, args) => {
                    ShowUnhandledExceptionFromSrc(args.Exception, "TaskScheduler.UnobservedTaskException");
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
                    NamedPipeListener<string>.SendMessage("KLCProxy2", true, args[1]);
                } else {
                    NamedPipeListener<string>.SendMessage("KLCProxy2", true, "focus");
                }

                Current.Shutdown();
            }
        }

        public static void ShowUnhandledExceptionFromSrc(Exception e, string source) {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                new WindowException(e, source + " - " + e.GetType().ToString()).Show();
            });
        }

        void ShowUnhandledException(Exception e, string unhandledExceptionType) {
            new WindowException(e, unhandledExceptionType).Show(); //, Debugger.IsAttached
        }

    }
}
