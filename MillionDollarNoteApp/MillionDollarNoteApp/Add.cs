using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cool_namespace_name;

namespace MillionDollarNoteApp
{
    public partial class Add : Form
    {
        private AwesomeSuperTruperSqliteApi api = new AwesomeSuperTruperSqliteApi("MahStuff");
        private bool updateData = false;
        private string title, body, id;

        public Add(string note_id = null)
        {
            InitializeComponent();

            if (note_id != null)
            {
                id = note_id;
                string[] cols = { "title", "body" };
                Dictionary<string, string> data = api.single_stuff("notes", cols, "id", note_id);

                textBox1.Text = data["title"];
                richTextBox1.Text = data["body"];

                updateData = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            title = textBox1.Text.ToString();
            body = richTextBox1.Text.ToString();

            if (updateData)
            {
                update();
            }
            else
            {
                add();
            }
        }

        private void add()
        {
            string[] col_tmpl = { "title", "body" };
            string[] vals = { "\"" + title + "\"", "\"" + body + "\"" };
            if (! api.insert_stuff("notes", col_tmpl, vals))
            {
                MessageBox.Show(api.error);
            }
            else
            {
                MessageBox.Show("Changes saved!");
            }
        }

        private void update()
        {
            if ( api.update_mah_stuff("notes", "title", "\"" + title + "\"", "id", id) )
            {
                if ( api.update_mah_stuff("notes", "body", "\"" + body + "\"", "id", id) )
                {
                    MessageBox.Show("Changes saved!");
                }
                else
                {
                    MessageBox.Show(api.error);
                }
            }
            else
            {
                MessageBox.Show(api.error);
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
