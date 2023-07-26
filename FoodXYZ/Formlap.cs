using MySql.Data.MySqlClient;
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
    public partial class Formlap : Form
    {
        private string id_user = "";
        private string nama = "";

        public Formlap(string nama,string id_user)
        {
            InitializeComponent();
            this.id_user = id_user;
            this.StartPosition = FormStartPosition.CenterScreen;
            setTable();
            this.nama = nama;
            label4.Text = nama;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Formadmin log = new Formadmin(id_user,nama);
            log.Show();
            this.Hide();
            this.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Formuser user = new Formuser(nama,id_user);
            user.Show();
            this.Hide();

        }

        private void Formlap_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd/MM/yyyy";
        }

        private void setTable()
        {
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT t.id_transaksi, t.tgl_transaksi, t.total_bayar, u.nama " +
                "FROM tbl_transaksi t " +
                "JOIN tbl_user u " +
                "ON t.id_user = u.id_user", connection);
            DataTable dataTable = new DataTable();

            try
            {
                connection.Open();

                dataTable.Load(command.ExecuteReader());

                dataGridView1.DataSource = dataTable;

                dataGridView1.Columns[0].HeaderText = "ID Transaksi";
                dataGridView1.Columns[1].HeaderText = "Tanggal Transaksi";
                dataGridView1.Columns[2].HeaderText = "Total Pembayaran";
                dataGridView1.Columns[3].HeaderText = "Nama Kasir";
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

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd;
            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                cmd = new SqlCommand("SELECT t.id_transaksi, t.tgl_transaksi, t.total_bayar, u.nama " +
                    "FROM tbl_transaksi t " +
                    "JOIN tbl_user u " +
                    "ON t.id_user = u.id_user " +
                    "WHERE t.tgl_transaksi " +
                    "BETWEEN @startDate AND @endDate", connection);

                cmd.Parameters.AddWithValue("@startDate", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@endDate", dateTimePicker2.Value.Date);

                dt.Load(cmd.ExecuteReader());

                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].HeaderText = "ID Transaksi";
                dataGridView1.Columns[1].HeaderText = "Tanggal Transaksi";
                dataGridView1.Columns[2].HeaderText = "Total Pembayaran";
                dataGridView1.Columns[3].HeaderText = "Nama Kasir";
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

        private void button5_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_transaksi", connection);

            try
            {
                connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    this.chart1.Series["Omset"].Points.AddXY(dr.GetDateTime(1).ToString("dd/MM/yyyy"), dr.GetInt32(2));
                }
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
