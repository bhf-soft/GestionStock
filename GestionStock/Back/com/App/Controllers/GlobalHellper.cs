using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GestionStock.Back.com.App.Controllers
{
    class GlobalHellper
    {
        public static string  GetSettings(string FilePath,string Composant)
        {
            var data = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines(FilePath))
                data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
            return data[Composant];
        }

        public static void search(DataGrid grid, TextBox word, string table, string condition)
        {
            SqlCommand cmd;

            string stringCo = @"data source='"+ GetSettings("GlobalSettings.txt", "ServerName") + "'; initial catalog='" + GetSettings("GlobalSettings.txt", "DataBase") + "'; integrated security=true";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = stringCo;

            if (word.Text.Equals(null) || word.Text.Equals(""))
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                SqlDataReader dr;
                dt.Clear();
                cmd = new SqlCommand("SELECT TOP 100 * FROM " + table + " ", con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    grid.ItemsSource = dt.DefaultView;
                }
                con.Close();
                Console.WriteLine("search with null");
            }
            else
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                SqlDataReader dr;
                dt.Clear();
                cmd = new SqlCommand("SELECT TOP 100 * FROM " + table + " where " + condition + " LIKE '%" + word.Text + "%'", con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    grid.ItemsSource = dt.DefaultView;
                }
                con.Close();
                Console.WriteLine("search with not null");
            }
        }
    }
}
