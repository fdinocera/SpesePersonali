using System.Data.SqlClient;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpesePersonali
{
    public partial class Form1 : Form
    {

        string connectionString = @"Server=DESKTOP-R7JUVDU\SQLEXPRESS03;Database=SpesePersonali;Trusted_Connection=True;Encrypt=False;";

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 0;
            //comboBox3.SelectedIndex = 0;
        }

        private void RecordAggiungi()
        {
            var data = dateTimePicker1.Value;
            var importo = textBox1.Text;
            var categoria = comboBox1.SelectedIndex;
            if (categoria == -1)
            {
                MessageBox.Show("Scegliere una categoria!");
                return;
            }
            var descrizione = textBox2.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Spese
                                    (Data, Importo, Categoria, Descrizione)
                                    VALUES
                                    (@data, @importo, @categoria, @descrizione)
                                    ";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@data", data);
                        cmd.Parameters.AddWithValue("@importo", importo);
                        cmd.Parameters.AddWithValue("@categoria", categoria);
                        cmd.Parameters.AddWithValue("@descrizione", descrizione);

                        //recupera id generato da db
                        int idGenerato = Convert.ToInt32(cmd.ExecuteScalar());

                        // Carica dati in listview
                        var item = new ListViewItem(idGenerato.ToString());
                        item.SubItems.Add(data.ToShortDateString());
                        item.SubItems.Add(importo);
                        item.SubItems.Add(GetCategoria(categoria));
                        item.SubItems.Add(descrizione);
                        listView1.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RecordAggiungi();
        }

        private static string GetCategoria(int c)
        {
            if (c == 0) return "Assicurazioni";
            if (c == 1) return "Forniture";
            return "Riparazioni";
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            int categoria = comboBox1.SelectedIndex;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "";

                    if (categoria == 0)
                    {
                        query = @"SELECT * FROMA Spese";
                    }

                    //query = @"INSERT INTO Spese
                    //                (Data, Importo, Categoria, Descrizione)
                    //                VALUES
                    //                (@data, @importo, @categoria, @descrizione)
                    //                ";


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {


                        listView1.Items.Clear();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string idGenerato = reader["Id"].ToString();
                                string data = reader["Data"].ToString();
                                string importo = reader["Importo"].ToString();
                                int categoriaDb = Convert.ToInt32(reader["Categoria"]);
                                string descrizione = reader["Descrizione"].ToString();


                                // Carica dati in listview
                                var item = new ListViewItem(idGenerato.ToString());
                                item.SubItems.Add(data);
                                item.SubItems.Add(importo);
                                item.SubItems.Add(GetCategoria(categoria));
                                item.SubItems.Add(descrizione);
                                listView1.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }



        }
    }
}
