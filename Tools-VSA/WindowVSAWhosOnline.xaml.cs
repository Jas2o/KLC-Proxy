using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LibKaseya;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace KLCProxy
{
    /// <summary>
    /// Interaction logic for WindowVSAWhosOnline.xaml
    /// </summary>
    public partial class WindowVSAWhosOnline : Window
    {
        public WindowVSAWhosOnline()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Kaseya.Token == null)
                btnCheck.IsEnabled = false;
            else
                lblTokenNotLoaded.Visibility = Visibility.Collapsed;
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.Items.Clear();

            try
            {
                IRestResponse response = Kaseya.GetRequest("api/v1.5/navigation/header/onlineadmins");
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
                MessageBox.Show(ex.ToString(), "KLCProxy: VSA Navigation");
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
