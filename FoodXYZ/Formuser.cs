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
    public partial class Formuser : Form
    {
        private string nama = "";
        private string id_user = "";
        private int idd_user = 0;
        public Formuser(string nama, string id_user)
        {
            InitializeComponent();
            this.nama = nama;
            this.id_user = id_user;
            setTable();
            //memasukan value untuk combobox
            comboBox1.Items.Add("Admin");
            comboBox1.Items.Add("Kasir");
            comboBox1.Items.Add("Gudang");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string tipe_user = comboBox1.Text;
            string alamat = textBox1.Text;
            string nama = textBox2.Text;
            string username = textBox3.Text;
            string telpon = textBox4.Text;
            string password = textBox5.Text;

            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            //masukan ke table tbl_user
            try
            {
                connection.Open();
                //insert
                SqlCommand command = new SqlCommand("INSERT INTO tbl_user (tipe_user,alamat,nama,username,telpon,password) VALUES (@tipe_user,@alamat,@nama,@username,@telpon,@password)", connection);
                command.Parameters.AddWithValue("@tipe_user", tipe_user);
                command.Parameters.AddWithValue("@alamat", alamat);
                command.Parameters.AddWithValue("@nama", nama);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@telpon", telpon);
                command.Parameters.AddWithValue("@password", password);

                command.ExecuteNonQuery();
                MessageBox.Show("Data berhasil ditambahkan");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            setTable();

        }

        public void setTable()
        {
            //mengeset table dataGridView
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT id_user,tipe_user,nama,alamat,telpon from tbl_user", connection);
            DataTable dataTable = new DataTable();

            try
            {
                connection.Open();

                dataTable.Load(command.ExecuteReader());

                dataGridView1.DataSource = dataTable;

                dataGridView1.Columns[0].HeaderText = "id_user";
                dataGridView1.Columns[1].HeaderText = "Tipe User";
                dataGridView1.Columns[2].HeaderText = "Nama";
                dataGridView1.Columns[3].HeaderText = "Alamat";
                dataGridView1.Columns[4].HeaderText = "Telepon";
               




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

        private void button3_Click(object sender, EventArgs e)
        {
            Formadmin lap = new Formadmin(id_user, nama);
            lap.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Formlap lap = new Formlap(nama, id_user);
            lap.Show();
            this.Hide();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //ambil id_user dari table
            this.idd_user = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            //query
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);


            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM tbl_user WHERE id_user = @id_user", connection);
                //masukan data ke dalam textBox
                command.Parameters.AddWithValue("@id_user", this.idd_user);
                SqlDataReader dataReader = command.ExecuteReader();
                comboBox1.Text = "";
                if (dataReader.Read())
                {
                    comboBox1.SelectedText = dataReader.GetString(1);
                    textBox1.Text = dataReader.GetString(3);
                    textBox2.Text = dataReader.GetString(2);
                    textBox3.Text = dataReader.GetString(5);
                    textBox4.Text = dataReader.GetString(4);
                    textBox5.Text = dataReader.GetString(6);

                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan");
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

        private void button6_Click(object sender, EventArgs e)
        {
            int idd = this.idd_user;
            //koneksikan
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE  FROM tbl_user WHERE id_user = @id_user", connection);
                command.Parameters.AddWithValue("@id_user", idd);
                command.ExecuteNonQuery();
                MessageBox.Show("Data berhasil dihapus");
                setTable();



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

            string tipe_user = comboBox1.Text;
            string alamat = textBox1.Text;
            string nama = textBox2.Text;
            string username = textBox3.Text;
            string telpon = textBox4.Text;
            string password = textBox5.Text;

            int idd = this.idd_user;

            

            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                //UPDATE
                SqlCommand command = new SqlCommand("UPDATE tbl_user SET tipe_user = @tipe_user, alamat = @alamat, nama = @nama, username = @username, telpon = @telpon, password = @password where id_user = @id_userr",connection);
               
                command.Parameters.AddWithValue("@tipe_user", tipe_user);
                command.Parameters.AddWithValue("@alamat", alamat);
                command.Parameters.AddWithValue("@nama", nama);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@telpon", telpon);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@id_userr", idd);

                command.ExecuteNonQuery();
                MessageBox.Show("Data berhasil diubah");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            setTable();



        }

        private void button7_Click(object sender, EventArgs e)
        {
            string namaUser = textBox6.Text;
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            //mencari berdasarkan nama user
            SqlCommand command = new SqlCommand("SELECT * FROM tbl_user WHERE nama LIKE @nama", connection);
            command.Parameters.AddWithValue("@nama", "%" + namaUser + "%");

            DataTable dataTable = new DataTable();
            try
            {
                connection.Open();

                dataTable.Load(command.ExecuteReader());

                dataGridView1.DataSource = dataTable;

                dataGridView1.Columns[0].HeaderText = "id_user";
                dataGridView1.Columns[1].HeaderText = "Tipe User";
                dataGridView1.Columns[2].HeaderText = "Nama";
                dataGridView1.Columns[3].HeaderText = "Alamat";
                dataGridView1.Columns[4].HeaderText = "Telepon";




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
    }
}
