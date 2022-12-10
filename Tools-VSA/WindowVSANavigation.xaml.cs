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
using Microsoft.Win32;
using System.IO;

namespace KLCProxy
{
    /// <summary>
    /// Interaction logic for WindowVSANavigation.xaml
    /// </summary>
    public partial class WindowVSANavigation : Window
    {
        private string vsa;

        public WindowVSANavigation()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vsa = Kaseya.VSA.FirstOrDefault(x => x.Value.Token != null).Key;

            if (Kaseya.VSA[vsa].Token == null)
                btnImport.IsEnabled = btnImport2.IsEnabled = false;
            else
                lblTokenNotLoaded.Visibility = Visibility.Collapsed;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            IRestResponse response = Kaseya.GetRequest(vsa, "api/v1.5/navigation/modules");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                txtInput.Text = response.Content;
            }
        }

        private void btnImport2_Click(object sender, RoutedEventArgs e)
        {
            IRestResponse response = Kaseya.GetRequest(vsa, "api/v1.5/navigation/modulestap");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                txtInput.Text = response.Content;
            }
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text.Length < 10)
                return;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<!DOCTYPE NETSCAPE-Bookmark-file-1>");
                sb.AppendLine("<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=UTF-8\">");
                sb.AppendLine("<TITLE>Bookmarks</TITLE>");
                sb.AppendLine("<H1>Bookmarks</H1>");
                sb.AppendLine("<DL><p>");

                sb.AppendLine("\t<DT><H3>VSA</H3>");

                dynamic json = JsonConvert.DeserializeObject(txtInput.Text);
                foreach (dynamic module in json["Result"].Children())
                {
                    string modName = System.Web.HttpUtility.HtmlEncode((string)module["Name"]);
                    //int modFuncListID = (int)module["FuncListID"];

                    sb.AppendLine("\t<DL><p>");
                    sb.AppendLine("\t\t<DT><H3>" + modName + "</H3>");
                    sb.AppendLine("\t\t<DL><p>");

                    foreach (dynamic folder in module["Folders"].Children())
                    {
                        string folderName = System.Web.HttpUtility.HtmlEncode((string)folder["Name"]);
                        //int folderFuncListID = (int)folder["FuncListID"];

                        foreach (dynamic item in folder["Items"].Children())
                        {
                            string itemName = System.Web.HttpUtility.HtmlEncode((string)item["Name"]);
                            string itemFuncListID = item["FuncListID"].ToString(); //Base modules are int, additonal ones are not.

                            sb.AppendLine("\t\t\t<DT><A HREF=\"javascript:(function(){window.location.hash = &quot;#navigation:" + itemFuncListID + "%22;})();\">" + folderName + " : " + itemName + "</A>");
                        }

                    }

                    sb.AppendLine("\t\t</DL><p>");
                }

                sb.AppendLine("\t</DL><p>");
                sb.AppendLine("</DL><p>");
                //I hate this format.

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveDialog.Filter = "HTML Document (*.html)|*.html";
                saveDialog.FileName = "VSA Bookmarks";
                bool result = (bool)saveDialog.ShowDialog();
                if (result)
                {
                    File.WriteAllText(saveDialog.FileName, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "KLCProxy: VSA Navigation");
            }
        }

    }
}
