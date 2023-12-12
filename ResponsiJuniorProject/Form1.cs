using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace ResponsiJuniorProject
{
    public partial class Form1 : Form
    {
        readonly NpgsqlConnection conn = new NpgsqlConnection(GlobalVariable.connString);
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            conn.Open();
            dgvKaryawan.DataSource = null;
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM karyawan", conn);
            DataTable dt = new DataTable();
            NpgsqlDataReader rd = cmd.ExecuteReader();
            dt.Load(rd);
            dgvKaryawan.DataSource = dt;
            conn.Close();
        }

        private void LoadDepartement()
        {
            conn.Open();
            cbDep.Items.Clear();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM departemen", conn);
            NpgsqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                cbDep.Items.Add(rd["id_dep"] + ". " + rd["nama_dep"]);
            }
            conn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbIdDepartemen.AcceptsReturn = true;
                           
            tbIdDepartemen.Text = "ID Departemen:\t1. HR : HR\t2. ENG : Engineer\t3. DEV : Developer\t4. PM : Product M\t5. FIN : Finance";
            LoadData();
            LoadDepartement();
        }

        private void dgvKaryawan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int selectedRowIndex = dgvKaryawan.SelectedCells[0].RowIndex;
                DataGridViewRow r = dgvKaryawan.Rows[selectedRowIndex];
                tbNama.Text = r.Cells["nama"].Value.ToString();
                cbDep.SelectedIndex = (int)r.Cells["id_dep"].Value - 1;
            }
            catch
            {
                cbDep.SelectedIndex = -1;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO karyawan (nama, id_dep) VALUES (@nama, @id_dep)";

            string name = tbNama.Text;
            int id_dep = cbDep.SelectedIndex + 1;

            cmd.Parameters.AddWithValue("@nama", name);
            cmd.Parameters.AddWithValue("@id_dep", id_dep);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("data berhasil dimasukkan");
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            conn.Close();
            LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection= conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE karyawan SET nama = @nama, id_dep = @id_dep WHERE id_karyawan = @id_karyawan";

            string nama = tbNama.Text;
            int id_dep = cbDep.SelectedIndex + 1;

            int selectedRowIndex = dgvKaryawan.SelectedCells[0].RowIndex;
            DataGridViewRow r = dgvKaryawan.Rows[selectedRowIndex];

            int id_karyawan = (int)r.Cells["id_karyawan"].Value;

            cmd.Parameters.AddWithValue("@nama", nama);
            cmd.Parameters.AddWithValue("@id_dep", id_dep);
            cmd.Parameters.AddWithValue("@id_karyawan", id_karyawan);


            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("data berhasil diupdate");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            conn.Close();
            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "DELETE FROM karyawan WHERE id_karyawan = @id_karyawan";

            int selectedRowIndex = dgvKaryawan.SelectedCells[0].RowIndex;
            DataGridViewRow r = dgvKaryawan.Rows[selectedRowIndex];
            int id_karyawan = (int)r.Cells["id_karyawan"].Value;

            cmd.Parameters.AddWithValue("@id_karyawan", id_karyawan);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("data berhasil didelete");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            conn.Close();
            LoadData();
        }
    }
}
