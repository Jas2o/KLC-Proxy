using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLCProxy {
    static class Program {

        private const string appName = "KLCProxy";
        private static Mutex mutex = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew) {
                string[] args = Environment.GetCommandLineArgs();
                if (args.Length > 1)
                    NamedPipeListener<String>.SendMessage(args[1]);
                return;
            } else {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormInDev());
                GC.KeepAlive(mutex);
            }
        }
    }
}
