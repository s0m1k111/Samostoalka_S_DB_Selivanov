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

namespace Samostoalka_S_DB_Selivanov
{
    public partial class Form1 : Form
    {
        private SqlConnection connect;
        private DataGridView dataGridView1;
        private TextBox textBox1, textBox2, textBox3;
        private Button button1, button2;
        private Label label1, label2, label3;

        public Form1()
        {
            InitializeComponent();
            CreateComponents();
            connect = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\source\\repos\\Samostoalka_S_DB_Selivanov\\DB\\Database1.mdf;Integrated Security=True");
        }

        private void CreateComponents()
        {
            this.Text = "Управление базой данных";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            dataGridView1 = new DataGridView();
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Location = new Point(10, 10);
            dataGridView1.Size = new Size(760, 300);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;
            this.Controls.Add(dataGridView1);

            GroupBox groupBox = new GroupBox();
            groupBox.Text = "Добавить новую территорию";
            groupBox.Location = new Point(10, 320);
            groupBox.Size = new Size(760, 150);
            this.Controls.Add(groupBox);

            label1 = new Label();
            label1.Text = "TerritoryID:";
            label1.Location = new Point(10, 25);
            label1.Size = new Size(100, 20);
            groupBox.Controls.Add(label1);

            textBox1 = new TextBox();
            textBox1.Name = "textBox1";
            textBox1.Location = new Point(110, 25);
            textBox1.Size = new Size(200, 20);
            groupBox.Controls.Add(textBox1);

            label2 = new Label();
            label2.Text = "TerritoryDescription:";
            label2.Location = new Point(10, 55);
            label2.Size = new Size(100, 20);
            groupBox.Controls.Add(label2);

            textBox2 = new TextBox();
            textBox2.Name = "textBox2";
            textBox2.Location = new Point(110, 55);
            textBox2.Size = new Size(200, 20);
            groupBox.Controls.Add(textBox2);

            label3 = new Label();
            label3.Text = "RegionID:";
            label3.Location = new Point(10, 85);
            label3.Size = new Size(100, 20);
            groupBox.Controls.Add(label3);

            textBox3 = new TextBox();
            textBox3.Name = "textBox3";
            textBox3.Location = new Point(110, 85);
            textBox3.Size = new Size(200, 20);
            groupBox.Controls.Add(textBox3);

            button1 = new Button();
            button1.Name = "button1";
            button1.Text = "Добавить";
            button1.Location = new Point(320, 25);
            button1.Size = new Size(100, 30);
            button1.BackColor = Color.LightBlue;
            button1.Click += button1_Click;
            groupBox.Controls.Add(button1);

            button2 = new Button();
            button2.Name = "button2";
            button2.Text = "Обновить данные";
            button2.Location = new Point(320, 65);
            button2.Size = new Size(100, 30);
            button2.BackColor = Color.LightGreen;
            button2.Click += button2_Click;
            groupBox.Controls.Add(button2);

            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCustomersData();
        }

        private void LoadCustomersData()
        {
            try
            {
                connect.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Customers", connect);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                    connect.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) ||
                    string.IsNullOrEmpty(textBox2.Text) ||
                    string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }

                connect.Open();

                string query = $"INSERT INTO Territories (TerritoryID, TerritoryDescription, RegionID) " +
                              $"VALUES (N'{textBox1.Text}', N'{textBox2.Text}', {int.Parse(textBox3.Text)})";

                SqlCommand command = new SqlCommand(query, connect);
                command.ExecuteNonQuery();

                MessageBox.Show("Данные успешно добавлены!");

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                    connect.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadCustomersData();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connect != null && connect.State == ConnectionState.Open)
                connect.Close();
        }
    }
}
