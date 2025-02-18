using Newtonsoft.Json;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KLC_Proxy {
    /// <summary>
    /// Interaction logic for WindowJsonViewTable.xaml
    /// </summary>
    public partial class WindowJsonViewTable : Window {

        public DataTable DTsource { get; set; }
        public string TXTsource { get; set; }

        public WindowJsonViewTable(DataTable dt, string input) {
            DTsource = dt;
            TXTsource = input;

            DataContext = this;
            InitializeComponent();
        }

        public WindowJsonViewTable(string input) {
            dynamic json = JsonConvert.DeserializeObject(input);
            DataTable dt = new DataTable();
            dt.Columns.Add("Key", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            foreach (var jValue in JExtensions.GetLeafValues(json)) {
                //Console.WriteLine("{jValue.Path} = {jValue.Value}");
                if (jValue.Path.Contains("_href"))
                    continue;

                DataRow row = dt.NewRow();
                row[0] = jValue.Path;
                row[1] = jValue.Value;
                dt.Rows.Add(row);
            }

            DTsource = dt;
            TXTsource = JExtensions.JsonPrettify(input);

            DataContext = this;
            InitializeComponent();
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e) {
            //Built-in WPF clipboard copy mode adds a newline on the end.
            //So let's do all this bullshit to not have that.

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key != Key.LeftCtrl && e.Key != Key.RightCtrl) {
                if (e.Key == Key.C) {
                    if (dataGrid.SelectedCells.Count > 0) {
                        var selected = dataGrid.SelectedCells[0].Column.GetCellContent(dataGrid.SelectedCells[0].Item);
                        if(selected is TextBlock)
                            Clipboard.SetDataObject(((TextBlock)selected).Text);
                    }
                }
            }
        }
    }
}
