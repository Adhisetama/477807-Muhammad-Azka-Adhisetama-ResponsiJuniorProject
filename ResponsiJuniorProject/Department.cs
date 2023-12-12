using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResponsiJuniorProject
{
    internal class Department
    {
        readonly NpgsqlConnection conn = new NpgsqlConnection(GlobalVariable.connString);

        public void Load(ComboBox cb)
        {
            conn.Open();
            cb.Items.Clear();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM departemen", conn);
            NpgsqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                cb.Items.Add(rd["id_dep"] + ". " + rd["nama_dep"]);
            }
            conn.Close();
        }
    }
}
