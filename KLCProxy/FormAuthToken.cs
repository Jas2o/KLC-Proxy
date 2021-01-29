using LibKaseya;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLCProxy
{
    public partial class FormAuthToken : Form
    {
        public FormAuthToken()
        {
            InitializeComponent();
        }

        public static string GetInput(string starter)
        {
            FormAuthToken form = new FormAuthToken();
            form.txtAuthToken.Text = starter;

            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK) {
                string token = form.txtAuthToken.Text.Trim();
                //Save the token until the computer is logged out
                KaseyaAuth.SetCredentials(token);
                return token;
            } else
                return starter;
        }

        private void btnAuthSave_Click(object sender, EventArgs e) {
            
        }
    }
}
