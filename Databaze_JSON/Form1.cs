using Newtonsoft.Json;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text.Json;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.Devices;
using Org.BouncyCastle.Crypto.Tls;

namespace Databaze_JSON
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();

        string command;
        string json;
        char[] letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};

        MySqlConnection conn;
        MySqlCommand com = new MySqlCommand();

        List<Jmeno> jmena = new List<Jmeno>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connection = "server=localhost;uid=root;pwd=;database=#";//Add your database on '#' character
            conn = new MySqlConnection(connection);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddToDatabase(txtJmeno.Text);
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
        void AddToDatabase(string input)
        {
            conn.Open();

            command = $"insert into jmena(jmeno) values('{input}')";
            com = new MySqlCommand(command, conn);

            var jmeno = new Jmeno()
            {
                jmeno = input,
            };

            com.ExecuteNonQuery();
            conn.Close();

            LoadFromDatabase();
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

        private void button4_Click(object sender, EventArgs e)
        {
            CreateWord();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadJson();
        }
        void LoadJson()
        {
            if (File.Exists(Path.GetFullPath($"{System.AppDomain.CurrentDomain.BaseDirectory}data.json")))
            {
                listBox1.Items.Clear();
                jmena.Clear();

                string path = Path.GetFullPath($"{System.AppDomain.CurrentDomain.BaseDirectory}data.json");

                jmena = JsonConvert.DeserializeObject<List<Jmeno>>(File.ReadAllText(path));

                foreach (var item in jmena)
                {
                    listBox1.Items.Add(item.jmeno);
                }

            }
            else
                MessageBox.Show("No data have been saved yet.");
        }
        void CreateWord()
        {
            string word = "";

            for (int i = 0; i < rnd.Next(2, 16); i++)
            {
                if(i == 0)
                {
                    word += letters[rnd.Next(0, letters.Length)].ToString().ToUpper();
                }
                else
                {
                    word += letters[rnd.Next(0, letters.Length)].ToString();
                }
            }

            AddToDatabase(word);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearDatabase();
        }
        void ClearDatabase()
        {
            conn.Open();

            command = "delete from jmena";

            com = new MySqlCommand(command, conn);
            com.ExecuteNonQuery();

            conn.Close();

            LoadFromDatabase();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClearJSON();
        }
        void ClearJSON()
        {
            if (File.Exists(Path.GetFullPath($"{System.AppDomain.CurrentDomain.BaseDirectory}data.json")))
                File.Delete(Path.GetFullPath($"{System.AppDomain.CurrentDomain.BaseDirectory}data.json"));
            else
                MessageBox.Show("No current JSON file existing!");
        }
    }
}