using LibKaseya;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KLC_Proxy {
    /// <summary>
    /// Interaction logic for WindowBookmarks.xaml
    /// </summary>
    public partial class WindowBookmarks : Window
    {
        private int startCorner;
        private ObservableCollection<Bookmark> ObservableList { get; set; }

        public WindowBookmarks()
        {
            InitializeComponent();
        }

        public WindowBookmarks(MainWindow mainWindow, int startCorner)
        {
            InitializeComponent();

            ObservableList = new ObservableCollection<Bookmark>();
            foreach (Bookmark bookmark in App.Shared.Bookmarks)
                ObservableList.Add(bookmark);
            dataGrid.ItemsSource = ObservableList;

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
                menuBar.ClearValue(Menu.BackgroundProperty);
                dataGrid.HeadersVisibility = DataGridHeadersVisibility.None;
                lblInstructions.Visibility = Visibility.Visible;
                btnAddAllFromList.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
                btnMoveUp.Visibility = Visibility.Collapsed;
                btnMoveDown.Visibility = Visibility.Collapsed;
                btnSave.Visibility = Visibility.Collapsed;
                dataGrid.Columns[1].Visibility = Visibility.Collapsed;
                dataGrid.Columns[3].Visibility = Visibility.Collapsed;
                groupAdd.Visibility = Visibility.Collapsed;
            } else
            {
                menuBar.Background = Brushes.PeachPuff;
                dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                lblInstructions.Visibility = Visibility.Collapsed;
                btnAddAllFromList.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
                btnMoveUp.Visibility = Visibility.Visible;
                btnMoveDown.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                dataGrid.Columns[1].Visibility = Visibility.Visible;
                dataGrid.Columns[3].Visibility = Visibility.Visible;
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
                //if (selected.Note.StartsWith("https://"))
                    //Process.Start(selected.Note);
                //}

                ((MainWindow)Owner).AddAgentToList(selected);
            }
        }

        private void btnToggleEdit_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.IsReadOnly = !dataGrid.IsReadOnly;
            UpdateVisibility();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            dataGrid.IsReadOnly = true;
            App.Shared.Bookmarks.Clear();
            foreach (Bookmark bookmark in ObservableList)
                App.Shared.Bookmarks.Add(bookmark);
            App.Shared.Save();
            UpdateVisibility();

            /*
            dataGrid.IsReadOnly = true;
            Bookmarks.List = bookmarks.ObservableList.ToList();
            Bookmarks.Save();
            UpdateVisibility();
            */
        }

        private void btnNewAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!long.TryParse(txtNewGUID.Text, out _))
            {
                MessageBox.Show("Agent GUIDs only contain numeric digits.");
                return;
            }
            /*
            if (txtNewNote.Text.StartsWith("https://")) {
                MessageBox.Show("URL should start with https://");
                return;
            }
            */
            ObservableList.Add(new Bookmark(txtNewDisplay.Text, txtNewVSA.Text, txtNewGUID.Text, txtNewNote.Text));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count > 1)
            {
                for (int i = dataGrid.SelectedItems.Count; i > 0; i--)
                {
                    ObservableList.Remove((Bookmark)dataGrid.SelectedItems[i - 1]);
                }
            }
            else if (dataGrid.SelectedIndex > -1)
            {
                int index = dataGrid.SelectedIndex;
                if (index == dataGrid.Items.Count - 1)
                    index--;
                ObservableList.Remove(dataGrid.SelectedItem as Bookmark);
                
                dataGrid.SelectedIndex = index;
                dataGrid.Focus();
            }
        }

        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            Bookmark selected = (Bookmark)dataGrid.SelectedItem;

            if (selected == null)
                return;

            int newIndex = dataGrid.Items.IndexOf(selected) - 1;
            if(newIndex > -1)
            {
                ObservableList.Remove(selected);
                ObservableList.Insert(newIndex, selected);
                dataGrid.SelectedIndex = newIndex;
            }

            dataGrid.Focus();
        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            Bookmark selected = (Bookmark)dataGrid.SelectedItem;

            if (selected == null)
                return;

            int newIndex = dataGrid.Items.IndexOf(selected) + 1;
            if (newIndex < dataGrid.Items.Count)
            {
                ObservableList.Remove(selected);
                ObservableList.Insert(newIndex, selected);
                dataGrid.SelectedIndex = newIndex;
            }

            dataGrid.Focus();
        }

        private void btnAddAllFromList_Click(object sender, RoutedEventArgs e)
        {
            List<Agent> listMain = ((MainWindow)Owner).mainData.ListAgent.ToList();
            foreach (Agent agent in listMain) {
                ObservableList.Add(new Bookmark(agent.Name, agent.VSA, agent.ID, ""));
            }
        }

        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (dataGrid.IsReadOnly)
                return;

            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            {
                if (e.SystemKey == Key.Up)
                {
                    btnMoveUp_Click(sender, e);
                    e.Handled = true;
                }
                else if (e.SystemKey == Key.Down)
                {
                    btnMoveDown_Click(sender, e);
                    e.Handled = true;
                }
            }
        }
    }
}
