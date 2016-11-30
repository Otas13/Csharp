using cool_namespace_name;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MillionDollarNoteApp
{
    public partial class Notes : Form
    {
        AwesomeSuperTruperSqliteApi api = new AwesomeSuperTruperSqliteApi("MahStuff");

        public Notes()
        {
            InitializeComponent();


            string[] cols_template = { "id INTEGER PRIMARY KEY AUTOINCREMENT", "title VARCHAR(30)", "body VARCHAR(500)" };
            api.create_table_plz("notes", cols_template);

            dataGridView2.DataSource = loadData();

            dataGridView2.Columns[0].Visible = false;

            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add add = new Add();
            add.Owner = this;
            add.Closed += new EventHandler(refreshGridView);
            add.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView2.SelectedCells[0].OwningRow;
            string id = (string) row.Cells[0].Value;
           
            
            Add add = new Add(id);
            add.FormBorderStyle = FormBorderStyle.FixedSingle;
            add.Owner = this;
            add.Closed += new EventHandler(refreshGridView);
            add.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView2.SelectedCells[0].OwningRow;

            string id = (string) row.Cells[0].Value;

            if(api.gtfo("notes", "id", id))
            {
                MessageBox.Show("Data successfully removed.");
                refreshGridView(null, null);
            }
            else
            {
                MessageBox.Show(api.error);
            }
        }

        private void refreshGridView(object sender, EventArgs e)
        {
            dataGridView2.DataSource = loadData();
            dataGridView2.Update();
            dataGridView2.Refresh();
        }

        private DataTable loadData()
        {
            string[] cols = { "id", "title", "body" };
            
            List<Dictionary<string, string>> shits = api.get_stuff("notes", cols);

            DataTable table = new DataTable();

            foreach (string name in cols)
            {
                table.Columns.Add(name);
            }


            foreach (Dictionary<string, string> row in shits)
            {
                DataRow newRow = table.NewRow();
                for (int i = 0; i < cols.Length; i++)
                {
                    newRow[cols[i]] = row[cols[i]];
                }
                table.Rows.Add(newRow);
            }

            return table;
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow row = dataGridView2.SelectedCells[0].OwningRow;
            Show show = new Show(row);
            show.Owner = this;
            show.FormBorderStyle = FormBorderStyle.FixedSingle;
            show.ShowDialog();
        }
    }
}
