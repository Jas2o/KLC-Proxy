using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew) {
                string[] args = Environment.GetCommandLineArgs();
                if (args.Length > 1) {
                    NamedPipeListener<string>.SendMessage(args[1]);
                } else {
                    NamedPipeListener<string>.SendMessage("focus");
                }

                Current.Shutdown();
            }
        }

    }
}
