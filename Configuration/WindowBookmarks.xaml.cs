using LibKaseya;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace KLCProxy
{
    /// <summary>
    /// Interaction logic for WindowBookmarks.xaml
    /// </summary>
    public partial class WindowBookmarks : Window
    {
        private int startCorner;
        private Bookmarks bookmarks;

        public WindowBookmarks()
        {
            InitializeComponent();
        }

        public WindowBookmarks(MainWindow mainWindow, int startCorner)
        {
            InitializeComponent();

            Bookmarks.Load();
            bookmarks = new Bookmarks();
            DataContext = bookmarks;

            Owner = mainWindow;
            this.startCorner = startCorner;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateVisibility();

            this.Top = Owner.Top + 60;

            switch (startCorner)
            {
                case 1: //Top-Right
                case 3: //Bottom-Right
                    this.Left = Owner.Left - Width + 17;
                    break;

                case 2: //Bottom-Left
                default: //Top-Left
                    this.Left = Owner.Left + 180;
                    break;
            }
        }

        private void UpdateVisibility()
        {
            if(dataGrid.IsReadOnly)
            {
                border.Background = Brushes.Transparent;
                lblInstructions.Visibility = Visibility.Visible;
                btnAddAllFromList.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
                btnMoveUp.Visibility = Visibility.Collapsed;
                btnMoveDown.Visibility = Visibility.Collapsed;
                btnSave.Visibility = Visibility.Collapsed;
                dataGrid.Columns[2].Visibility = Visibility.Collapsed;
                groupAdd.Visibility = Visibility.Collapsed;
            } else
            {
                border.Background = Brushes.PeachPuff;
                lblInstructions.Visibility = Visibility.Collapsed;
                btnAddAllFromList.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
                btnMoveUp.Visibility = Visibility.Visible;
                btnMoveDown.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                dataGrid.Columns[2].Visibility = Visibility.Visible;
                groupAdd.Visibility = Visibility.Visible;
            }
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!dataGrid.IsReadOnly)
                return;

            Bookmark selected = (Bookmark)dataGrid.SelectedItem;

            if (selected != null)
            {
                if (selected.Type == "URL")
                {
                    if (selected.Value.StartsWith("https://"))
                        Process.Start(selected.Value);
                }
                else if (selected.Type == "Agent")
                    ((MainWindow)Owner).AddAgentToList(selected.Value);
                else
                    Console.WriteLine("Unexpected bookmark type '" + selected.Type + "' with value: " + selected.Value);
            }
        }

        private void btnToggleEdit_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.IsReadOnly = !dataGrid.IsReadOnly;
            UpdateVisibility();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.IsReadOnly = true;
            Bookmarks.List = bookmarks.ObservableList.ToList();
            Bookmarks.Save();
            UpdateVisibility();
        }

        private void cmbNewType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtNewValue == null)
                return;

            string text = ((ComboBoxItem)cmbNewType.SelectedItem).Content.ToString();

            if (text == "Agent")
                txtNewValue.Text = "(Agent GUID here)";
            if (text == "URL")
                txtNewValue.Text = "https://";
        }

        private void btnNewAdd_Click(object sender, RoutedEventArgs e)
        {
            string text = ((ComboBoxItem)cmbNewType.SelectedItem).Content.ToString();

            if (text == "Agent" && !long.TryParse(txtNewValue.Text, out _))
            {
                MessageBox.Show("Agent GUIDs only contain numeric digits.");
                return;
            }
            if (text == "URL" && txtNewValue.Text.StartsWith("https://")) {
                MessageBox.Show("URL should start with https://");
                return;
            }

            bookmarks.ObservableList.Add(new Bookmark("Agent", txtNewDisplay.Text, txtNewValue.Text));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Bookmark selected = (Bookmark)dataGrid.SelectedItem;

            if (selected == null)
                return;

            bookmarks.ObservableList.Remove(selected);
        }

        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            Bookmark selected = (Bookmark)dataGrid.SelectedItem;

            if (selected == null)
                return;

            int newIndex = bookmarks.ObservableList.IndexOf(selected) - 1;
            if(newIndex > -1)
            {
                bookmarks.ObservableList.Remove(selected);
                bookmarks.ObservableList.Insert(newIndex, selected);
                dataGrid.SelectedIndex = newIndex;
            }
        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            Bookmark selected = (Bookmark)dataGrid.SelectedItem;

            if (selected == null)
                return;

            int newIndex = bookmarks.ObservableList.IndexOf(selected) + 1;
            if (newIndex < bookmarks.ObservableList.Count)
            {
                bookmarks.ObservableList.Remove(selected);
                bookmarks.ObservableList.Insert(newIndex, selected);
                dataGrid.SelectedIndex = newIndex;
            }
        }

        private void btnAddAllFromList_Click(object sender, RoutedEventArgs e)
        {
            List<LibKaseya.Agent> listMain = ((MainWindow)Owner).mainData.ListAgent.ToList();
            foreach (LibKaseya.Agent agent in listMain) {
                bookmarks.ObservableList.Add(new Bookmark("Agent", agent.Name, agent.ID));
            }
        }
    }
}
