using System.Data.SqlClient;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace FoodXYZ
{
    public partial class Formsaksi : Form
    {
        DataTable t = new DataTable();
        private string id = "";

        public Formsaksi(string nama, string id)
        {
            InitializeComponent();
            FillCombo();
            this.StartPosition = FormStartPosition.CenterScreen;
            label15.Text = DateTime.Now.ToString("yyyyMMdd");
            label12.Text = nama;
            this.id = id;
        }

        private void Formsaksi_Load(object sender, EventArgs e)
        {
            showindatagridview();
        }

        private void showindatagridview()
        {
            t.Columns.Add("ID Transaksi", typeof(string));
            t.Columns.Add("Kode Barang", typeof(string));
            t.Columns.Add("Nama Barang", typeof(string));
            t.Columns.Add("Harga Satuan", typeof(string));
            t.Columns.Add("Quantitas", typeof(string));
            t.Columns.Add("Subtotal", typeof(string));
            dataGridView1.DataSource = t;
        }

        private void FillCombo()
        {

            //SQL Server Connection
            SqlConnection connection = new SqlConnection("Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            try
            {
                //Open connection
                connection.Open();

                //Create command
                SqlCommand cmd = new SqlCommand("SELECT kode_barang, nama_barang FROM tbl_barang", connection);

                //Execute reader
                SqlDataReader dr = cmd.ExecuteReader();

                //Clear combo box
                comboBox1.Items.Clear();

                //Loop through reader and add items to combo box
                while (dr.Read())
                {
                    //Use column names instead of indexes to retrieve data
                    string kode_barang = dr.GetString(0);
                    string nama_barang = dr.GetString(1);

                    //Add item to combo box
                    comboBox1.Items.Add(kode_barang + "-" + nama_barang);
                }

                //Close reader
                dr.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                //Display error message
                MessageBox.Show(ex.Message);
            }
           

        }


        private void button2_Click(object sender, EventArgs e)
        {
            string idd = t.Rows.Count.ToString();
            int id = Convert.ToInt32(idd) + 1;
            string idotoma = "TR" + id.ToString();

            string[] split = comboBox1.Text.Split('-');
            string kode_barang = split[0].Trim();
            string nama_barang = split[1].Trim();
            string hargaSatuan = textBox1.Text;
            string quantitas = textBox2.Text;
            int subtotal = Convert.ToInt32(hargaSatuan) * Convert.ToInt32(quantitas);

            t.Rows.Add(idotoma, kode_barang, nama_barang, hargaSatuan, quantitas, subtotal);
            label13.Text = "Rp. " + subtotal.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
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
                command.Parameters.AddWithValue("@id_user", this.id);
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

        private void button4_Click(object sender, EventArgs e)
        {
            string hargaSatuan = textBox1.Text;
            string quantitas = textBox2.Text;
            int subtotal = Convert.ToInt32(hargaSatuan) * Convert.ToInt32(quantitas);
            int uangBayar = Convert.ToInt32(textBox4.Text);

            if (uangBayar < subtotal)
            {
                MessageBox.Show("Uang Pembayaran Tidak Mencukupi!");
            }
            else
            {
                int kembalian = uangBayar - subtotal;
                label14.Text = "Rp. " + kembalian.ToString();
                textBox4.Clear();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string query = "INSERT INTO tbl_transaksi(no_transaksi, tgl_transaksi, total_bayar, id_user, id_barang) VALUES (@notrans, @tgl, @tobar, @id_user, @id_barang)";
            string query_barang = "SELECT id_barang FROM tbl_barang WHERE kode_barang = @kode_barang";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string tgl = DateTime.Now.ToString("yyyy/MM/dd");
                string notrans = DateTime.Now.ToString("yyyyMMdd");
                string id_user = id;

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlCommand cmd_barang = new SqlCommand(query_barang, connection);

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string kode_barang = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string tobar = dataGridView1.Rows[i].Cells[5].Value.ToString();

                    cmd_barang.Parameters.Clear();
                    cmd_barang.Parameters.AddWithValue("@kode_barang", kode_barang);

                    SqlDataReader dr_barang = cmd_barang.ExecuteReader();

                    if (dr_barang.Read())
                    {
                        string id_barang = dr_barang["id_barang"].ToString();

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@notrans", notrans);
                        cmd.Parameters.AddWithValue("@tgl", tgl);
                        cmd.Parameters.AddWithValue("@tobar", tobar);
                        cmd.Parameters.AddWithValue("@id_user", id_user);
                        cmd.Parameters.AddWithValue("@id_barang", id_barang);

                        dr_barang.Close();
                        SqlDataReader dr_transaksi = cmd.ExecuteReader();
                        dr_transaksi.Close();
                    }
                    else
                    {
                        dr_barang.Close();
                    }
                }
                MessageBox.Show("Data Transaksi Berhasil Disimpan!");
            }


            }

            private void button5_Click(object sender, EventArgs e)
        {
            //buat laporan ke dalam PDF
            // Membuat objek Document
            Document document = new Document(PageSize.A4, 50, 50, 25, 25);

            // Membuat objek PdfWriter dan menentukan path file output
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("C:/xampp2/output.pdf", FileMode.Create));

            // Membuka dokumen
            document.Open();

            // Membuat objek SqlConnection
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);

            // Membuat objek SqlCommand dan membuka koneksi database

            //berdasarkan nomor transaksi hari ini 
            string notrans = DateTime.Now.ToString("yyyyMMdd");
            string query = "SELECT * FROM tbl_transaksi where no_transaksi =" + notrans;
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            // Membuat objek SqlDataReader dan membaca data dari tabel
            SqlDataReader dataReader = command.ExecuteReader();

            // Menambahkan judul laporan
            iTextSharp.text.Font titleFont = new iTextSharp.text.Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 18, iTextSharp.text.Font.BOLD);
            Paragraph title = new Paragraph("Invoice Anda", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            // Menambahkan data tabel
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 1, 2, 2, 1, 2 });
            table.AddCell(new PdfPCell(new Phrase("ID", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("No Transaksi", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Tanggal Transaksi", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Total Bayar", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Id Barang", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });

            while (dataReader.Read())
            {
                string id_transaksi = dataReader["id_transaksi"].ToString();
                string noTransaksi = dataReader["no_transaksi"].ToString();
                string tgl_transaksi = dataReader["tgl_transaksi"].ToString();
                string jumlah = dataReader["total_bayar"].ToString();
                string id_barang = dataReader["id_barang"].ToString();


                table.AddCell(new PdfPCell(new Phrase(id_transaksi, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_LEFT });
                table.AddCell(new PdfPCell(new Phrase(noTransaksi, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_LEFT });
                table.AddCell(new PdfPCell(new Phrase(tgl_transaksi, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_LEFT });
                table.AddCell(new PdfPCell(new Phrase(jumlah, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(id_barang, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_RIGHT });


            }

            document.Add(table);

            // Menutup objek SqlDataReader, SqlCommand, dan SqlConnection
            dataReader.Close();
            command.Dispose();
            connection.Close();

            // Menutup dokumen
            document.Close();




        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ketika ComboBox1 diklik maka akan menampilkan data yang sesuai dengan kode barang yang dipilih
            string[] split = comboBox1.Text.Split('-');
            string kode_barang = split[0].Trim();
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=db_xyz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string query_barang = "SELECT harga_satuan FROM tbl_barang WHERE kode_barang = @kode_barang";
            //lakukan query
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd_barang = new SqlCommand(query_barang, connection);
                cmd_barang.Parameters.AddWithValue("@kode_barang", kode_barang);
                SqlDataReader dr_barang = cmd_barang.ExecuteReader();
                if (dr_barang.Read())
                {
                    string harga_satuan = dr_barang["harga_satuan"].ToString();
                    textBox1.Text = harga_satuan;
                }
                else
                {
                    MessageBox.Show("Data Barang Tidak Ditemukan!");
                }
                dr_barang.Close();
            }


        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //diisikan
            string hargaSatuan = textBox1.Text;
            string quantitas = textBox2.Text;
            int subtotal = Convert.ToInt32(hargaSatuan) * Convert.ToInt32(quantitas);
            textBox3.Text = subtotal.ToString();

        }
    }
}
