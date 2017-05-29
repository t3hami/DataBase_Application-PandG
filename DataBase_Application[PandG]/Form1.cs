using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace DataBase_Application_PandG_
{
    public partial class Form1 : Form
    {
        MyConnection mc;
        OleDbCommand cmd;
        OleDbDataReader dr;

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mc = new MyConnection();
            mc.conn.Open();
            cmd = new OleDbCommand("select * from Users where Name='"+textBox1.Text+"';",mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                if (dr["Password"].ToString() == textBox2.Text)
                {
                    this.Hide();
                    Program.f2 = new Form2(dr["Name"].ToString(), dr["Role"].ToString());
                    Program.f2.Show();
                }
                else
                    MessageBox.Show("Wrong password!");

            }
            else
                MessageBox.Show("Usermame not found!");
            /*if ((textBox1.Text == "dba" && textBox2.Text == "123") || (textBox1.Text=="tehami" && textBox2.Text=="123"))
            {
                this.Hide();
                Program.f2 = new Form2(textBox1.Text);
                Program.f2.Show();
            }
            else
                MessageBox.Show("Incorrect username or password!");*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AboutMe am = new AboutMe();
            am.Show();
            this.Enabled = false;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = Environment.CurrentDirectory + "\\Logo.png";
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

    }
}
