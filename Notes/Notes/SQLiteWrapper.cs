using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.IO;

namespace cool_namespace_name
{
    class SQLiteWrapper
    {
        private SQLiteConnection con;
        private SQLiteCommand command;
        public string error;

        public SQLiteWrapper(string file)
        {
            if (!File.Exists(file+".db"))
            {
                SQLiteConnection.CreateFile(file+".db");
            }

            con = new SQLiteConnection("Data Source="+file+".db;Version=3;");
            con.Open();
        }

        public void createTable(string table_name, string[] columns)
        {
            string sql = "CREATE TABLE IF NOT EXISTS " + table_name + " ( ";
            sql += string.Join(",", columns);
            sql += " );";

            try
            {
                command = new SQLiteCommand(sql, con);
                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                error = ex.Message + " " + sql;
            }
        }

        public bool insert(string table, string[] table_structure, string[] vals)
        {
            string sql = "INSERT INTO " + table + " ( " + string.Join(",", table_structure) + " ) " + "VALUES ( " +
                string.Join(",", vals) + " );";

            try
            {
                command = new SQLiteCommand(sql, con);
                command.ExecuteNonQuery();

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

        public List<Dictionary<string, string>> getData(string table, string[] columns)
        {
            string sql = "select * from "+ table + ";";
            Console.WriteLine(sql);

            List<Dictionary<string, string>> response = new List<Dictionary<string, string>>();

            command = new SQLiteCommand(sql, con);
            try
            { 
                SQLiteDataReader reader = command.ExecuteReader();

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

        public Dictionary<string, string> singleRecord(string table, string[] columns, string select_col, string key)
        {

            string sql = "select * from " + table + " WHERE "+ select_col +"="+key+";";
            Console.WriteLine(sql);

            command = new SQLiteCommand(sql, con);
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            try
            {
                SQLiteDataReader reader = command.ExecuteReader();

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

        public bool update(string table, string col_set, string val, string col_where, string key)
        {
            string sql = "UPDATE " + table + " SET " + col_set + " = " + val +" WHERE " + col_where + " = "+ key + ";";

            try
            {
                command = new SQLiteCommand(sql, con);
                command.ExecuteNonQuery();

                error = null;
                return true;
            }
            catch (SQLiteException ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool delete(string table, string where, string key)
        {
            string sql = "DELETE FROM " + table + " WHERE " + where + " = " + key + ";";

            Console.WriteLine(sql);

            try
            {
                command = new SQLiteCommand(sql, con);
                command.ExecuteNonQuery();
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
