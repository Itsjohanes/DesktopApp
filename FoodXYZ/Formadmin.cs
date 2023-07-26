using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodXYZ
{
    public partial class Formadmin : Form
    {
        private string id_user = ""; 
        private string nama = "";
        public Formadmin(string id_user,string nama)
        {
            InitializeComponent();
            this.id_user = id_user;
            this.nama = nama; 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID_Log");
            dataTable.Columns.Add("Username");
            dataTable.Columns.Add("Waktu");
            dataTable.Columns.Add("Aktivitas");

            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT l.id_log, u.username, l.waktu, l.aktivitas " +
                   "FROM tbl_log l " +
                                  "JOIN tbl_user u " +
                                                 "ON l.id_user = u.id_user " +
                                                                "WHERE CONVERT(VARCHAR(10), l.waktu, 23) LIKE @tgl1 + '%'", connection);
            command.Parameters.AddWithValue("@tgl1", dateTimePicker1.Value.ToString("yyyy-MM-dd"));


            try
            {
                connection.Open();
                dataTable.Load(command.ExecuteReader());
                dataGridView1.DataSource = dataTable;

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                dataGridView1.Columns[0].HeaderText = "ID Log";
                dataGridView1.Columns[1].HeaderText = "Username";
                dataGridView1.Columns[2].HeaderText = "Waktu";
                dataGridView1.Columns[3].HeaderText = "Aktivitas";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }






        }

        private void button1_Click(object sender, EventArgs e)
        {
            //buat log Logout
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO tbl_log (id_user,waktu,aktivitas) VALUES (@id_user,@waktu,@aktivitas)", connection);
                command.Parameters.AddWithValue("@id_user", this.id_user);
                command.Parameters.AddWithValue("@waktu", DateTime.Now);
                command.Parameters.AddWithValue("@aktivitas", "Logout");
                command.ExecuteNonQuery();
                MessageBox.Show("Logout berhasil");
                this.Hide();
                Formlog form1 = new Formlog();
                form1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Formlap lap = new Formlap(this.nama,this.id_user);
            lap.Show();
            this.Hide();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Formuser user = new Formuser(nama, id_user);
            user.Show();
            this.Hide();

        }
    }
}
