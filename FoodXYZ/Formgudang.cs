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
    public partial class Formgudang : Form
    {
        private string id_user = "";

        public int id_barang = 0;
        public Formgudang(string id_user)
        {
            InitializeComponent();
            this.id_user = id_user;

        }

        private void Formgudang_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            //set value Combo box

            comboBox1.Items.Add("Botol");
            comboBox1.Items.Add("Bungkus");
            setTable();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //koneksikan
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                //insert
                SqlCommand command = new SqlCommand("INSERT INTO tbl_barang (kode_barang, nama_barang, expired_date, harga_satuan, jumlah_barang,satuan) VALUES (@kode_barang, @nama_barang, @expired, @harga_satuan, @jumlah_barang, @satuan)", connection);

                command.Parameters.AddWithValue("@kode_barang", textBox1.Text);
                command.Parameters.AddWithValue("@nama_barang", textBox3.Text);
                command.Parameters.AddWithValue("@expired", dateTimePicker1.Value);
                command.Parameters.AddWithValue("@harga_satuan", textBox4.Text);
                command.Parameters.AddWithValue("@jumlah_barang", textBox2.Text);
                command.Parameters.AddWithValue("@satuan", comboBox1.Text);
                //execute
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
        private void setTable()
        {
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT id_barang,kode_barang,nama_barang,expired_date,jumlah_barang,satuan,harga_satuan from tbl_barang",connection);
            DataTable dataTable = new DataTable();

            try
            {
                connection.Open();

                dataTable.Load(command.ExecuteReader());

                dataGridView1.DataSource = dataTable;

                dataGridView1.Columns[0].HeaderText = "ID Barang";
                dataGridView1.Columns[1].HeaderText = "Kode Barang";
                dataGridView1.Columns[2].HeaderText = "Nama Barang";
                dataGridView1.Columns[3].HeaderText = "Expired Date";
                dataGridView1.Columns[4].HeaderText = "Jumlah Barang";
                dataGridView1.Columns[5].HeaderText = "Satuan";
                dataGridView1.Columns[6].HeaderText = "Harga Satuan";

                


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
            //Ambil id_barang
            this.id_barang = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            //Ambil data dari database
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);


            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM tbl_barang WHERE id_barang = @id_barang", connection);
                //masukan data ke dalam textBox
                command.Parameters.AddWithValue("@id_barang", this.id_barang);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    textBox1.Text = dataReader["kode_barang"].ToString();
                    textBox3.Text = dataReader["nama_barang"].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(dataReader["expired_date"].ToString());
                    textBox4.Text = dataReader["harga_satuan"].ToString();
                    textBox2.Text = dataReader["jumlah_barang"].ToString();
                    comboBox1.Text = dataReader["satuan"].ToString();
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









            //

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id_barang = this.id_barang;
            //koneksikan
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE  FROM tbl_barang WHERE id_barang = @id_barang", connection);
                command.Parameters.AddWithValue("@id_barang", id_barang);
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

        private void button3_Click(object sender, EventArgs e)
        {
            //koneksikan
            int id_barang = this.id_barang;

            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                //edit
                SqlCommand command = new SqlCommand("UPDATE tbl_barang SET kode_barang = @kode_barang, nama_barang = @nama_barang, expired_date = @expired, harga_satuan = @harga_satuan, jumlah_barang = @jumlah_barang, satuan = @satuan WHERE id_barang = @id_barang", connection);
                   
                command.Parameters.AddWithValue("@kode_barang", textBox1.Text);
                command.Parameters.AddWithValue("@nama_barang", textBox3.Text);
                command.Parameters.AddWithValue("@expired", dateTimePicker1.Value);
                command.Parameters.AddWithValue("@harga_satuan", textBox4.Text);
                command.Parameters.AddWithValue("@jumlah_barang", textBox2.Text);
                command.Parameters.AddWithValue("@satuan", comboBox1.Text);
                command.Parameters.AddWithValue("@id_barang", id_barang);
                //execute
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

        private void button5_Click(object sender, EventArgs e)
        {
            string namaBarang = textBox5.Text;
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT id_barang,kode_barang,nama_barang,expired_date,jumlah_barang,satuan,harga_satuan from tbl_barang WHERE nama_barang LIKE @nama_barang", connection);
            command.Parameters.AddWithValue("@nama_barang", "%" + namaBarang + "%");
            DataTable dataTable = new DataTable();
            try
            {
                connection.Open();

                dataTable.Load(command.ExecuteReader());

                dataGridView1.DataSource = dataTable;

                dataGridView1.Columns[0].HeaderText = "ID Barang";
                dataGridView1.Columns[1].HeaderText = "Kode Barang";
                dataGridView1.Columns[2].HeaderText = "Nama Barang";
                dataGridView1.Columns[3].HeaderText = "Expired Date";
                dataGridView1.Columns[4].HeaderText = "Jumlah Barang";
                dataGridView1.Columns[5].HeaderText = "Satuan";
                dataGridView1.Columns[6].HeaderText = "Harga Satuan";




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
    }
}
