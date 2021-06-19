using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLCProxy
{
    public partial class FormKaseyaScreenshot : HDshared.SnapForm
    {
        public FormKaseyaScreenshot(Form parentForm)
        {
            InitializeComponent();
            MoveBelow(parentForm);
        }

        private void FormKaseyaScreenshot_Load(object sender, EventArgs e)
        {
            ListKaseyaWindows();
            timerKaseya.Start();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            ListKaseyaWindows();
        }

        private void listKaseya_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListKaseyaWindows();
        }
        private void ListKaseyaWindows()
        {
            listKaseya.Items.Clear();
            WindowSnapCollection windows = WindowSnap.GetAllWindows(true, true);
            foreach (WindowSnap snap in windows)
            {
                if (snap.ProcessName.Contains("Kaseya") && !snap.WindowTitle.Contains("Kaseya"))
                    listKaseya.Items.Add(snap);
            }
        }

        private void linkKaseya_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Screenshots");
        }

        private void listKaseya_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerKaseya.Stop();
            timerKaseya.Start();
            if (this.listKaseya.SelectedItem != null)
            {
                WindowSnap snap = this.listKaseya.SelectedItem as WindowSnap;
                snap.Capture();
                pictureBox1.Image = snap.Image;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            btnCapture_Click(sender, e);
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (this.listKaseya.SelectedItem != null)
            {
                WindowSnap snap = this.listKaseya.SelectedItem as WindowSnap;
                snap.Capture();
                pictureBox1.Image = snap.Image;

                string name = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.InvariantCulture) + " " + snap.WindowTitle.Substring(0, snap.WindowTitle.IndexOf(':'));

                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Screenshots";

                System.IO.Directory.CreateDirectory(path);
                snap.Image.Save(path + "\\" + name + ".png");
            }
        }

        private void timerKaseya_Tick(object sender, EventArgs e)
        {
            ListKaseyaWindows();
        }
    }
}
