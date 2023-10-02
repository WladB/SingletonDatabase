using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatabaseManager;
using Loging;

namespace SingletonDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DbManager db = DbManager.getInstance();
        Logger Log = Logger.getInstance();
        List<string> fields = new List<string>();
      

        private void button2_Click(object sender, EventArgs e)
        {
            Log.Info($"зберігання поля з назвою: {textBox2.Text}, тип даних поля: {textBox3.Text}, у таблицю: {textBox1.Text}");
            fields.Add(textBox2.Text + " " + textBox3.Text);
            textBox2.Text = "";
            textBox3.Text = "";
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool check = false;
            foreach (string s in fields)
            {
                if (s.Contains("not null primary key"))
                    check = true;
                break;

            };
            if (!check)
            {
                fields[0] += " not null primary key";
            }
            db.CreateTable(textBox1.Text, fields.ToArray());
            Log.Info($"зберігання таблиці {textBox1.Text}");
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            fields.Clear();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            Table table = new Table();
            Log.Info("Перехід на форму Table");
            table.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Log.Info("Відкриття форми Form1");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.Info("Форму Form1 закрито");
            Log.SaveInfo();
        }
    }
}
