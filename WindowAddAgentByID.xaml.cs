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
    public partial class WindowAddAgentByID : Window {

        public string ReturnAddress;
        public string ReturnGUID;

        public WindowAddAgentByID(string address="", string agentID="") {
            InitializeComponent();

            foreach (string vsa in LibKaseya.Kaseya.VSA.Keys)
            {
                cmbAddress.Items.Add(vsa);
                if (vsa == address || cmbAddress.Items.Count == 1)
                    cmbAddress.SelectedIndex = cmbAddress.Items.Count - 1;
            }

            txtValue.Text = agentID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            txtValue.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            if (cmbAddress.SelectedIndex == -1)
                return;

            ReturnAddress = cmbAddress.SelectedItem.ToString();
            ReturnGUID = txtValue.Text;

            if (ReturnAddress.Length > 0 && ReturnGUID.Length > 0)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter)
                btnSave_Click(sender, e);
        }

    }
}
