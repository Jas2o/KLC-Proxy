using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace KLC_Proxy {
    /// <summary>
    /// Interaction logic for WindowStartPos.xaml
    /// </summary>
    public partial class WindowStartPos : Window {

        public string ReturnDisplayName, ReturnDisplayFallback;
        public int ReturnCornerIndex;

        public WindowStartPos(string startDisplay, string startDisplayFallback, int startCorner) {
            InitializeComponent();

            cmbMonitor.Items.Add("Default");
            cmbMonitor.SelectedIndex = 0;
            foreach (Screen screen in Screen.AllScreens) {
                cmbMonitor.Items.Add(screen.DeviceName);
                if (screen.DeviceName == startDisplay)
                    cmbMonitor.SelectedIndex = cmbMonitor.Items.Count - 1;
            }
            txtFallback.Content = startDisplayFallback;

            cmbCorner.SelectedIndex = startCorner;
        }

        private void CmbMonitor_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cmbMonitor.SelectedIndex == 0)
                txtFallback.Content = "";
            else
                txtFallback.Content = Screen.AllScreens[cmbMonitor.SelectedIndex - 1].Bounds.ToString();
        }

        private void BtnDetect_Click(object sender, RoutedEventArgs e) {
            System.Drawing.Rectangle testArea = new System.Drawing.Rectangle((int)Left, (int)Top, 10, 10);

            for (int i = 0; i < cmbMonitor.Items.Count; i++) {
                if (Screen.AllScreens[i].Bounds.Contains(testArea)) {
                    cmbMonitor.SelectedIndex = i + 1; //Skip past Default

                    int checkX = Screen.AllScreens[i].Bounds.X + (Screen.AllScreens[i].Bounds.Width / 2);
                    int checkY = Screen.AllScreens[i].Bounds.Y + (Screen.AllScreens[i].Bounds.Height / 2);

                    int pos = 0;
                    if ((int)Left > checkX)
                        pos += 1;
                    if ((int)Top > checkY)
                        pos += 2;
                    cmbCorner.SelectedIndex = pos;
                    break;
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e) {
            ReturnDisplayName = cmbMonitor.Text.ToString();
            ReturnDisplayFallback = txtFallback.Content.ToString();
            ReturnCornerIndex = cmbCorner.SelectedIndex;

            DialogResult = true;
            Close();
        }

    }
}
