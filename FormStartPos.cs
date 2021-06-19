using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLCProxy {
    public partial class FormStartPos : Form {

        public string ReturnDisplayName, ReturnDisplayFallback;
        public int ReturnCornerIndex;

        public FormStartPos(string startDisplay, string startDisplayFallback, int startCorner) {
            InitializeComponent();

            cmbMonitor.Items.Add("Default");
            cmbMonitor.SelectedIndex = 0;
            foreach (Screen screen in Screen.AllScreens) {
                cmbMonitor.Items.Add(screen.DeviceName);
                if (screen.DeviceName == startDisplay)
                    cmbMonitor.SelectedIndex = cmbMonitor.Items.Count - 1;
            }
            txtFallback.Text = startDisplayFallback;

            cmbCorner.SelectedIndex = startCorner;
        }

        private void cmbMonitor_SelectedIndexChanged(object sender, EventArgs e) {
            if (cmbMonitor.SelectedIndex == 0)
                txtFallback.Text = "";
            else
                txtFallback.Text = Screen.AllScreens[cmbMonitor.SelectedIndex - 1].Bounds.ToString();
        }

        private void btnDetect_Click(object sender, EventArgs e) {
            Rectangle testArea = new Rectangle(this.Bounds.X, this.Bounds.Y, 10, 10);

            for (int i = 0; i < cmbMonitor.Items.Count; i++) {
                if (Screen.AllScreens[i].Bounds.Contains(testArea)) {
                    cmbMonitor.SelectedIndex = i + 1; //Skip past Default

                    int checkX = Screen.AllScreens[i].Bounds.X + (Screen.AllScreens[i].Bounds.Width / 2);
                    int checkY = Screen.AllScreens[i].Bounds.Y + (Screen.AllScreens[i].Bounds.Height / 2);

                    int pos = 0;
                    if (this.Bounds.X > checkX)
                        pos += 1;
                    if (this.Bounds.Y > checkY)
                        pos += 2;
                    cmbCorner.SelectedIndex = pos;
                    break;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            ReturnDisplayName = cmbMonitor.Text.ToString();
            ReturnDisplayFallback = txtFallback.Text.ToString();
            ReturnCornerIndex = cmbCorner.SelectedIndex;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
