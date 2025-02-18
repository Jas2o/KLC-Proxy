using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace KLC_Proxy {
    /// <summary>
    /// Interaction logic for WindowAskMe.xaml
    /// </summary>
    public partial class WindowAskMe : Window {

        [DllImport("Shell32.dll", SetLastError = false)]
        private static extern int SHDefExtractIcon(string iconFile, int iconIndex, uint flags, ref IntPtr hiconLarge, ref IntPtr hiconSmall, uint iconSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static bool DestroyIcon(IntPtr handle);

        private const string path1 = @"C:\Program Files\Kaseya Live Connect\KaseyaLiveConnect.exe";
        private string path2 = Environment.ExpandEnvironmentVariables(@"%localappdata%\Apps\Kaseya Live Connect\KaseyaLiveConnect.exe");

        public bool ReturnUseAlternative;

        public WindowAskMe() {
            InitializeComponent();

            string path = System.IO.File.Exists(path1) ? path1 : path2;

            IntPtr hiconLarge = default;
            IntPtr hiconSmall = default;
            int kResult = SHDefExtractIcon(path, 0, 0, ref hiconLarge, ref hiconSmall, 64);
            if (kResult == 0) {
                Icon ico = System.Drawing.Icon.FromHandle(hiconLarge);
                imgOriginal.Source = Imaging.CreateBitmapSourceFromHIcon(
                    ico.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                ico.Dispose();
            } else {
                btnOriginal.Content = "Live Connect";
            }
            DestroyIcon(hiconSmall);
            DestroyIcon(hiconSmall);

            WindowUtilities.ActivateWindow(this);
            btnAlternative.Focus();
        }

        private void BtnOriginal_Click(object sender, RoutedEventArgs e) {
            ReturnUseAlternative = false;
            DialogResult = true;
            Close();
        }

        private void BtnAlternative_Click(object sender, RoutedEventArgs e) {
            ReturnUseAlternative = true;
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }
    }
}
