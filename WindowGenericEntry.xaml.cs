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

namespace KLCProxy {
    public partial class WindowGenericEntry : Window {

        public string ReturnInput;

        public WindowGenericEntry(string title, string label, string value, string buttonText) {
            InitializeComponent();

            Title = title;
            lblLabel.Content = label;
            txtValue.Text = value;
            btnSave.Content = buttonText;

            if (label == "")
                lblLabel.Visibility = Visibility.Collapsed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            txtValue.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            ReturnInput = txtValue.Text;

            this.DialogResult = true;
            this.Close();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter)
                btnSave_Click(sender, e);
        }

    }
}
