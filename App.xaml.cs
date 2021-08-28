using System;
using System.Threading;
using System.Windows;

namespace KLCProxy {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private const string appName = "KLCProxy2";
        private static Mutex mutex = null;

        public App() : base() {
            mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew) {
                string[] args = Environment.GetCommandLineArgs();
                if (args.Length > 1) {
                    NamedPipeListener<string>.SendMessage("KLCProxy2", args[1]);
                } else {
                    NamedPipeListener<string>.SendMessage("KLCProxy2", "focus");
                }

                Current.Shutdown();
            }
        }

    }
}
