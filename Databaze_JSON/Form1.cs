using Newtonsoft.Json;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text.Json;

namespace Databaze_JSON
{
    public partial class Form1 : Form
    {
        string command;
        string json;

        MySqlConnection conn;
        MySqlCommand com = new MySqlCommand();

        List<Jmeno> jmena = new List<Jmeno>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connection = "server=localhost;uid=root;pwd=;database=jmena";
            conn = new MySqlConnection(connection);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddToDatabase();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadFromDatabase();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveToJson();
        }
        void LoadFromDatabase()
        {
            conn.Open();

            jmena.Clear();
            listBox1.Items.Clear();

            command = "Select jmeno from jmena";
            com = new MySqlCommand(command, conn);

            MySqlDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                listBox1.Items.Add(reader.GetString(0));
                var jmeno = new Jmeno()
                {
                    jmeno = reader.GetString(0),
                };
                jmena.Add(jmeno);
            }

            conn.Close();
        }
        void AddToDatabase()
        {
            conn.Open();

            string input = txtJmeno.Text;

            command = $"insert into jmena(jmeno) values('{input}')";
            com = new MySqlCommand(command, conn);

            var jmeno = new Jmeno()
            {
                jmeno = input,
            };

            com.ExecuteNonQuery();
            conn.Close();
        }
        void SaveToJson()
        {
            File.Delete("data.json");
            string path = Path.GetFullPath($"{System.AppDomain.CurrentDomain.BaseDirectory}data.json");

            json = JsonConvert.SerializeObject(jmena);

            foreach (var item in json)
            {
                File.AppendAllText(path, item.ToString());
            }
        }
    }
}