using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using LibKaseya;

namespace KLC_Proxy {
    /// <summary>
    /// Interaction logic for WindowEcho.xaml
    /// </summary>
    public partial class WindowEcho : Window
    {
        private BackgroundWorker bw;

        public WindowEcho()
        {
            InitializeComponent();

            foreach (string vsa in LibKaseya.Kaseya.VSA.Keys)
            {
                cmbAddress.Items.Add(vsa);
                if (cmbAddress.Items.Count == 1)
                    cmbAddress.SelectedIndex = cmbAddress.Items.Count - 1;
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            string vsaAddress = cmbAddress.SelectedItem.ToString();

            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args) {
                bool loop = true;
                try
                {
                    while (loop)
                    {
                        if (bw.CancellationPending)
                            loop = false;

                        bool response = Kaseya.GetRequestEcho(vsaAddress);
                        Dispatcher.Invoke((Action)delegate
                        {
                            if (response)
                            {
                                lblLabel.Content = "Success";
                                loop = false;
                                System.Media.SystemSounds.Beep.Play();
                            }
                            else
                            {
                                lblLabel.Content = "Fail";
                                Task.Delay(5000);
                            }
                        });
                    } 
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke((Action)delegate
                    {
                        lblLabel.Content = "Exception"; //VAS unknown?
                    });
                }
            });

            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(delegate (object o, RunWorkerCompletedEventArgs args) {
                btnTest.IsEnabled = true;
                btnStop.IsEnabled = false;
                progressBar.IsIndeterminate = false;
            });

            btnTest.IsEnabled = false;
            btnStop.IsEnabled = true;
            progressBar.IsIndeterminate = true;
            bw.RunWorkerAsync();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            bw.CancelAsync();
        }
    }
}
