using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace FoodXYZ
{
    public partial class Formlog : Form
    {
        public Formlog()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            //SQL Server Connection
            SqlConnection connection = new SqlConnection("Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            connection.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_user WHERE username = @username AND password = @password", connection);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                
                dr.Read();
                string tipe_user = dr.GetString(1);
                string nama = dr.GetString(2);
                int idd = dr.GetInt32(0);
                string id = idd.ToString();
                connection.Close();

                if (tipe_user == "Admin")
                {
                    connection.Open();
                    //Insert log ke tabel tbl_log
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO tbl_log (id_user, waktu, aktivitas) VALUES (@id_user, @waktu, @aktivitas)", connection);
                    cmd2.Parameters.AddWithValue("@id_user", id);
                    cmd2.Parameters.AddWithValue("@waktu", DateTime.Now);
                    cmd2.Parameters.AddWithValue("@aktivitas", "Login");
                    cmd2.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Login Berhasil: Halo Admin.");
                    Formadmin lap = new Formadmin(id,nama);
                    lap.Show();
                    this.Hide();
                }
                else if (tipe_user == "Kasir")
                {
                    connection.Open();
                    //Insert log ke tabel tbl_log
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO tbl_log (id_user, waktu, aktivitas) VALUES (@id_user, @waktu, @aktivitas)", connection);
                    cmd2.Parameters.AddWithValue("@id_user", id);
                    cmd2.Parameters.AddWithValue("@waktu", DateTime.Now);
                    cmd2.Parameters.AddWithValue("@aktivitas", "Login");
                    cmd2.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Login Berhasil: Selamat Datang di FoodXYZ.");
                    Formsaksi saksi = new Formsaksi(nama, id);
                    saksi.Show();
                    this.Hide();
                }else if(tipe_user == "Gudang")
                {
                    connection.Open();
                    //Insert log ke tabel tbl_log
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO tbl_log (id_user, waktu, aktivitas) VALUES (@id_user, @waktu, @aktivitas)", connection);
                    cmd2.Parameters.AddWithValue("@id_user", id);
                    cmd2.Parameters.AddWithValue("@waktu", DateTime.Now);
                    cmd2.Parameters.AddWithValue("@aktivitas", "Login");
                    cmd2.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Login Berhasil: Selamat Datang di FoodXYZ.");
                    Formgudang gudang = new Formgudang(id);
                    gudang.Show();
                    this.Hide();

                }
                this.Hide();
            }
            else
            {
                MessageBox.Show("Login Gagal: Username atau Password yang Anda masuki tidak sesuai.");
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Username")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Username";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Password")
            {
                textBox2.Text = "";
                textBox2.PasswordChar = '*';
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Password";
                textBox2.PasswordChar = '*';
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void Formlog_Load(object sender, EventArgs e)
        {

        }
    }
}
