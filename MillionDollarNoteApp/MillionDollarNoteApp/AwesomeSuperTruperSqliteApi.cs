using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.IO;

namespace cool_namespace_name
{
    class AwesomeSuperTruperSqliteApi
    {
        private SQLiteConnection super_truper_con;
        private SQLiteCommand awesome_command;
        public string error;

        public AwesomeSuperTruperSqliteApi(string file)
        {
            if (!File.Exists(file+".db"))
            {
                SQLiteConnection.CreateFile(file+".db");
            }

            super_truper_con = new SQLiteConnection("Data Source="+file+".db;Version=3;");
            super_truper_con.Open();
        }

        public void create_table_plz(string table_name, string[] columns)
        {
            string sql = "CREATE TABLE IF NOT EXISTS " + table_name + " ( ";
            sql += string.Join(",", columns);
            sql += " );";

            try
            {
                awesome_command = new SQLiteCommand(sql, super_truper_con);
                awesome_command.ExecuteNonQuery();
                Console.WriteLine("Mkay");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("OMG!\n" +ex.Message);
                 //throw ex;
            }
        }

        public bool insert_stuff(string table, string[] table_structure, string[] vals)
        {
            string sql = "INSERT INTO " + table + " ( " + string.Join(",", table_structure) + " ) " + "VALUES ( " +
                string.Join(",", vals) + " );";
            Console.WriteLine(sql);
            try
            {
                awesome_command = new SQLiteCommand(sql, super_truper_con);
                awesome_command.ExecuteNonQuery();

                error = null;
                return true;
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                error = ex.Message + " " + sql;
                return false;
            }
        }

        public List<Dictionary<string, string>> get_stuff(string table, string[] columns)
        {
            string sql = "select * from "+ table + ";";
            Console.WriteLine(sql);

            List<Dictionary<string, string>> response = new List<Dictionary<string, string>>();

            awesome_command = new SQLiteCommand(sql, super_truper_con);
            try
            { 
                SQLiteDataReader reader = awesome_command.ExecuteReader();

                // brainfuck commin' ...
                while( reader.Read())
                {
                    Dictionary<string, string> tmp = new Dictionary<string, string>();

                    for (int i = 0; i < columns.Length; i++)
                    {
                        tmp[columns[i]] = (string) reader[columns[i]].ToString();
                    }

                    response.Add(tmp);
                }
            }
            catch(SQLiteException ex)
            {
                error = ex.Message;
            }

            return response;
        }

        public Dictionary<string, string> single_stuff(string table, string[] columns, string select_col, string key)
        {

            string sql = "select * from " + table + " WHERE "+ select_col +"="+key+";";
            Console.WriteLine(sql);

            awesome_command = new SQLiteCommand(sql, super_truper_con);
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            try
            {
                SQLiteDataReader reader = awesome_command.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < columns.Length; i++)
                    {
                        tmp[columns[i]] = (string) reader[columns[i]].ToString();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                error = ex.Message;
            }

            error = null;
            return tmp;
        }

        public bool update_mah_stuff(string table, string col_set, string val, string col_where, string key)
        {
            string sql = "UPDATE " + table + " SET " + col_set + " = " + val +" WHERE " + col_where + " = "+ key + ";";

            try
            {
                awesome_command = new SQLiteCommand(sql, super_truper_con);
                awesome_command.ExecuteNonQuery();

                error = null;
                return true;
            }
            catch (SQLiteException ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool gtfo(string table, string where, string key)
        {
            string sql = "DELETE FROM " + table + " WHERE " + where + " = " + key + ";";

            Console.WriteLine(sql);

            try
            {
                awesome_command = new SQLiteCommand(sql, super_truper_con);
                awesome_command.ExecuteNonQuery();
                error = null;
                return true;
            }
            catch (SQLiteException ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
