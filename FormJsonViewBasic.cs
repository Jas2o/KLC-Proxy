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
    public partial class FormJsonViewBasic : Form
    {
        public FormJsonViewBasic(string input) {
            InitializeComponent();

            textBoxJson.Text = input;
        }

        private void FormJsonViewBasic_Shown(object sender, EventArgs e) {
            textBoxJson.SelectionLength = 0;
        }
    }
}
