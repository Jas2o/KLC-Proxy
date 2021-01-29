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
    public partial class FormGenericEntry : Form {

        public string ReturnInput;

        public FormGenericEntry(string title, string label, string existing="", string buttontext="") {
            InitializeComponent();

            this.Text = title;
            lblGeneric.Text = label;
            txtInput.Text = existing;
            btnSave.Text = buttontext;
        }

        private void btnSave_Click(object sender, EventArgs e) {
            ReturnInput = txtInput.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter)
                btnSave_Click(this, new EventArgs());
        }
    }
}
