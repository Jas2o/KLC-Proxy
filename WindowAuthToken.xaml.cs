using LibKaseya;
using System.Windows;

namespace KLCProxy {
    /// <summary>
    /// Interaction logic for WindowAuthToken.xaml
    /// </summary>
    public partial class WindowAuthToken : Window {
        public WindowAuthToken() {
            InitializeComponent();
        }

        public static string GetInput(string starter, Window owner) {
            WindowAuthToken form = new WindowAuthToken();
            form.Owner = owner;
            form.txtAuthToken.Password = starter;

            bool? result = form.ShowDialog();
            if (result == true) {
                string token = form.txtAuthToken.Password.Trim();
                //Save the token until the computer is logged out
                KaseyaAuth.SetCredentials(token);
                return token;
            } else
                return starter;
        }

        public string ResponseText {
            get { return txtAuthToken.Password; }
            set { txtAuthToken.Password = value; }
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            DialogResult = true;
        }

    }
}
