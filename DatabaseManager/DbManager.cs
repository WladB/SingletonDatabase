using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseManager
{
   public class DbManager
    {
        SqlConnection connect;
        SqlCommand cmd;
        static DbManager instance;
        public static DbManager getInstance() {
            if (instance == null) {
                instance = new DbManager();
            }
            return instance;
        }
        DbManager()
        {
            connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DataBase.mdf;Integrated Security=True");
            cmd = new SqlCommand();
            cmd.Connection = connect;
        }

        public void CreateTable(string tableName, string[] columns)
        {
            try
            {
                cmd.CommandText = "create table " + tableName + "(";
                foreach (string s in columns)
                {
                    cmd.CommandText += s + ", ";
                };
                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2);
                cmd.CommandText += ");";
                connect.Open();
                cmd.ExecuteNonQuery();
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connect.Close();
            }
        }
        public void ViewTable(List<string> box, string TableName)
        {
            box.Clear();
            cmd.CommandText = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{TableName}';";
            connect.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                box.Add(reader.GetString(3));
            }
            reader.Close();
            connect.Close();
        }

        public void AllTables(ListBox box)
        {
            box.Items.Clear();
            cmd.CommandText = "SELECT * FROM sys.Tables";
            connect.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                box.Items.Add(reader.GetString(0));
            }
            reader.Close();
            connect.Close();
        }

        public void ViewTable(ListBox box, string TableName)
        {
            box.Items.Clear();
            cmd.CommandText = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{TableName}';";
            connect.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                box.Items.Add(reader.GetString(3));
            }
            reader.Close();
            connect.Close();
        }

        public void ViewPK(ListBox box, string TableName)
        {
            box.Items.Clear();
            cmd.CommandText = $"EXEC sp_pkeys @table_name = '{TableName}';";
            connect.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                box.Items.Add(reader.GetString(3));
            }
            reader.Close();
            connect.Close();
        }

        public void TableRecords(string tableName, DataGridView grid)
        {
            try
            {
                connect.Open();
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableName}", connect);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, tableName);
                grid.DataSource = dataSet.Tables[tableName];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connect.Close();
        }

        public string AddRecord(string table_name, List<string> fieldscontent)
        {
            try
            {
                List<string> list = new List<string>();

                ViewTable(list, table_name);
                cmd.CommandText = $"INSERT INTO {table_name}(";

                foreach (string str in list)
                {
                    cmd.CommandText += str + ", ";
                };
                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2);
                cmd.CommandText += ") ";
                cmd.CommandText += "VALUES(";
                foreach (string str in fieldscontent)
                {
                    cmd.CommandText += "'" + str + "', ";
                };
                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2);
                cmd.CommandText += ");";
                MessageBox.Show(cmd.CommandText);
                connect.Open();
                cmd.ExecuteNonQuery();
                connect.Close();
                return cmd.CommandText;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                connect.Close();
                return "";
            }

        }
        public void DeleteRecord(string table_name, string primarykey)
        {
            try
            {

                cmd.CommandText = $"DELETE FROM {table_name} WHERE {ViewPK(table_name)} = {primarykey};";


                connect.Open();
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            connect.Close();
        }
        public void UpdateRecord(string table_name, CheckedListBox fields, List<string> fieldscontent, string primarykey)
        {
            try
            {


                cmd.CommandText = $"UPDATE {table_name} SET ";
                int i = 0;
                foreach (string str in fields.CheckedItems)
                {
                    cmd.CommandText += str + "='" + fieldscontent[i] + "', ";
                    i++;
                };
                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2);
                cmd.CommandText += $" WHERE {ViewPK(table_name)} = {primarykey};";

                MessageBox.Show(cmd.CommandText);
                connect.Open();
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            connect.Close();
        }
        public void UpdateRecord(string table_name, string columnName, string fieldscontent, string primarykey)
        {
            try
            {
                cmd.CommandText = $"UPDATE {table_name} SET {columnName} = '{fieldscontent}'";
                cmd.CommandText += $" WHERE {ViewPK(table_name)} = {primarykey};";

                MessageBox.Show(cmd.CommandText);
                connect.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            connect.Close();
        }

        public string ViewPK(string TableName)
        {
            string str = "";
            cmd.CommandText = $"EXEC sp_pkeys @table_name = '{TableName}';";
            connect.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                str = reader.GetString(3);
            }
            reader.Close();
            connect.Close();
            return str;
        }
    }
}
