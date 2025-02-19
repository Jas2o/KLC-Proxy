using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace KLC_Proxy {
    public partial class WindowAddAgentByID : Window {

        public string ReturnAddress;
        public string ReturnGUID;

        private List<string> thisPC_Agent;
        private List<string> thisPC_Address;
        private List<string> thisPC_GUID;

        public WindowAddAgentByID(string address="", string agentID="") {
            InitializeComponent();
            expanderThisPC.Visibility = Visibility.Collapsed;

            foreach (string vsa in LibKaseya.Kaseya.VSA.Keys)
            {
                cmbAddress.Items.Add(vsa);
                if (vsa == address || cmbAddress.Items.Count == 1)
                    cmbAddress.SelectedIndex = cmbAddress.Items.Count - 1;
            }

            txtValue.Text = agentID;
        }

        public WindowAddAgentByID(List<string> listAgent, List<string> listAddress, List<string> listGUID) {
            InitializeComponent();
            expanderThisPC.IsExpanded = true;

            thisPC_Agent = listAgent;
            thisPC_Address = listAddress;
            thisPC_GUID = listGUID;

            for(int i = 0; i < listAgent.Count; i++) {
                cmbThisPC.Items.Add(listAgent[i]);
                cmbAddress.Items.Add(listAddress[i]);
            }

            if (cmbThisPC.Items.Count > 0)
                cmbThisPC.SelectedIndex = 0;
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

        private void cmbThisPC_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            cmbAddress.SelectedIndex = cmbThisPC.SelectedIndex;
            txtValue.Text = thisPC_GUID[cmbThisPC.SelectedIndex];
        }
    }
}
