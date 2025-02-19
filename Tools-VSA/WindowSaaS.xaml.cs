using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace KLC_Proxy {
    /// <summary>
    /// Interaction logic for WindowSaaS.xaml
    /// </summary>
    public partial class WindowSaaS : Window
    {
        private static string[] fieldSplit = new string[] { " - " };
        private BackgroundWorker bw;

        public WindowSaaS()
        {
            InitializeComponent();
        }

        private void hyperlinkKaseyaStatus_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.ToString()) { UseShellExecute = true });
        }

        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            string[] lines = txtInput.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();

            foreach(string line in lines)
            {
                string[] fields = line.Split(fieldSplit, StringSplitOptions.None);
                //i = line.IndexOf(" - ");
                if (fields.Length > 1 && fields[0].Length < 6) { //Ignore ones like Andromeda02 - US01
                    sb.AppendLine(line);

                    //DataItemSaaS dis = new DataItemSaaS(fields[1], fields[0], "KLC");
                    //dataGrid.Items.Add(dis);
                }
            }

            txtInput.Text = sb.ToString();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            tabResults.IsSelected = true;
            if (bw != null && bw.IsBusy)
                return;

            dataGrid.Items.Clear();
            string temp = txtInput.Text;

            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            //bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args) {
                foreach (KeyValuePair<string, LibKaseya.KaseyaVSA> vsa in LibKaseya.Kaseya.VSA)
                {
                    DataItemSaaS disKnown = new DataItemSaaS(vsa.Key, "", vsa.Key, GetKLCversion(vsa.Value.Client));
                    Dispatcher.Invoke((Action)delegate
                    {
                        dataGrid.Items.Add(disKnown);
                    });
                }

                string[] lines = temp.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (bw.CancellationPending)
                        return;

                    string[] fields = line.Split(fieldSplit, StringSplitOptions.None);
                    if (fields.Length > 1 && fields[0].Length < 6) { //Ignore ones like Andromeda02 - US01
                        string url = string.Format(@"https://{0}.kaseya.net", fields[1]);
                        DataItemSaaS dis = new DataItemSaaS(fields[1], fields[0], url, GetKLCversion(url));
                        Dispatcher.Invoke((Action)delegate {
                            dataGrid.Items.Add(dis);
                        });
                    }
                }
            });

            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(delegate (object o, RunWorkerCompletedEventArgs args) {
                btnStop.IsEnabled = false;
                progressRun.IsIndeterminate = false;
            });

            btnStop.IsEnabled = true;
            progressRun.IsIndeterminate = true;
            bw.RunWorkerAsync();
        }

        private string GetKLCversion(RestClient client)
        {
            RestResponse response = client.Execute(new RestRequest("/vsapres/api/session/AppVersions/1"));
            /*
            if (response.ResponseStatus == ResponseStatus.Error)
            {
                client = new RestClient(url.Replace("0", ""))
                {
                    Timeout = 5000
                };
                response = client.Execute(new RestRequest());
            }
            */

            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                try
                {
                    string unescaped = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(response.Content);
                    dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(unescaped);
                    foreach (dynamic key in json["data"].Children())
                    {
                        if (key["platform"] == "KaseyaLiveConnect-Win64")
                        {
                            return key["version"];
                        }
                    }
                }
                catch (Exception)
                {
                    //e.g. Cloudflare error
                }
            }

            return "?";
        }

        private string GetKLCversion(string url)
        {
            if (!url.StartsWith("https://"))
                url = "https://" + url;

            RestClientOptions options = new RestClientOptions(url) {
                Timeout = TimeSpan.FromSeconds(5)
            };
            RestClient client = new RestClient();
            return GetKLCversion(client);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if(bw != null)
            {
                bw.CancelAsync();
                progressRun.IsIndeterminate = false;
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataItemSaaS dis = (DataItemSaaS)dataGrid.SelectedItem;
            if(dis != null)
            {
                txtSelected.Text = string.Format("{0}/ManagedFiles/VSAHiddenFiles/KaseyaLiveConnect/win64/LiveConnect.exe", dis.BaseURL);
            }
        }
    }

    public class DataItemSaaS
    {
        public string VSA { get; private set; }
        public string Region { get; private set; }
        public string BaseURL { get; private set; }
        public string KLC { get; private set; }

        public DataItemSaaS(string v, string r, string baseUrl, string k)
        {
            VSA = v;
            Region = r;
            BaseURL = baseUrl;
            KLC = k;
        }
    }
}
