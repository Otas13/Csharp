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
    public partial class Show : Form
    {
        public Show(DataGridViewRow row)
        {
            InitializeComponent();

            body.MaximumSize = new Size(300, 150);
            body.AutoSize = true;

            this.title.Text = row.Cells[1].Value.ToString();
            this.body.Text = row.Cells[2].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
