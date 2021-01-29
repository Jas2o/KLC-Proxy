using Newtonsoft.Json;
using System;
using System.Data;
using System.Windows.Forms;

namespace KLCProxy
{
    public partial class FormJsonViewTable : HDshared.SnapForm
    {
        public FormJsonViewTable(Form parentForm, string input)
        {
            InitializeComponent();
            MoveBelow(parentForm);

            dynamic json = JsonConvert.DeserializeObject(input);
            DataTable dt = new DataTable();
            dt.Columns.Add("Key", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            foreach (var jValue in JExtensions.GetLeafValues(json))
            {
                DataRow row = dt.NewRow();
                row[0] = jValue.Path;
                row[1] = jValue.Value;
                dt.Rows.Add(row);
            }

            dataGridView1.DataSource = dt;
            textBoxJson.Text = input;
        }

        private void FormJsonViewTable_Shown(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
            textBoxJson.SelectionLength = 0;
        }
    }
}
