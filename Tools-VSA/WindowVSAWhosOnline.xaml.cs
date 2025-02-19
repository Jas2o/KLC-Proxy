using System;
using System.Linq;
using System.Windows;
using LibKaseya;
using Newtonsoft.Json;
using RestSharp;

namespace KLC_Proxy {
    /// <summary>
    /// Interaction logic for WindowVSAWhosOnline.xaml
    /// </summary>
    public partial class WindowVSAWhosOnline : Window
    {
        private string vsa;

        public WindowVSAWhosOnline()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vsa = Kaseya.VSA.FirstOrDefault(x => x.Value.Token != null).Key;

            if (vsa == null || Kaseya.VSA[vsa].Token == null)
                btnCheck.IsEnabled = false;
            else
                lblTokenNotLoaded.Visibility = Visibility.Collapsed;
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.Items.Clear();

            try
            {
                RestResponse response = Kaseya.GetRequest(vsa, "api/v1.5/navigation/header/onlineadmins");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic json = JsonConvert.DeserializeObject(response.Content);
                    foreach (dynamic user in json["Result"].Children())
                    {
                        DataItemOnline diUser = new DataItemOnline(user);
                        Dispatcher.Invoke((Action)delegate {
                            dataGrid.Items.Add(diUser);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "KLC-Proxy: VSA Navigation");
            }
        }

        public class DataItemOnline
        {
            public string AdminName { get; private set; }
            public string AdminIP { get; private set; }
            public int SecondsUntilExpiration { get; private set; }
            public int SecondsWithoutActivity { get; private set; }
            public DateTime LastActivity { get; private set; }

            public DataItemOnline(dynamic user)
            {
                AdminName = (string)user["AdminName"];
                AdminIP = (string)user["AdminIP"];
                SecondsUntilExpiration = (int)user["SecondsUntilExpiration"];
                SecondsWithoutActivity = (int)user["SecondsWithoutActivity"];
                LastActivity = (DateTime)user["LastActivity"];
            }
        }
    }
}
