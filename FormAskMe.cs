using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLCProxy {
    public partial class FormAskMe : Form {

        [DllImport("Shell32.dll", SetLastError = false)]
        private static extern int SHDefExtractIcon(string iconFile, int iconIndex, uint flags, ref IntPtr hiconLarge, ref IntPtr hiconSmall, uint iconSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static bool DestroyIcon(IntPtr handle);

        public FormAskMe() {
            InitializeComponent();

            IntPtr hiconLarge = default(IntPtr);
            IntPtr hiconSmall = default(IntPtr);
            int kResult = SHDefExtractIcon(@"C:\Program Files\Kaseya Live Connect\KaseyaLiveConnect.exe", 0, 0, ref hiconLarge, ref hiconSmall, 64);
            if (kResult == 0) {
                Icon ico = Icon.FromHandle(hiconLarge);
                btnOriginal.Image = ico.ToBitmap();
                ico.Dispose();
                btnOriginal.Text = "";
            }
            DestroyIcon(hiconSmall);
            DestroyIcon(hiconSmall);
            
            this.ActiveControl = btnAlternative;
        }

        private void FormAskMe_Load(object sender, EventArgs e) {
        }

        private void btnOriginal_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.No;
            Close();
        }

        private void btnAlternative_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}
