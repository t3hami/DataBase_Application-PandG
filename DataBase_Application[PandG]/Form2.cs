using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataBase_Application_PandG_
{
    public partial class Form2 : Form
    {
        bool close;
        string username;
        string role;
        public Form2(string u, string r)
        {
            InitializeComponent();
            username = u;
            role = r;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = Environment.CurrentDirectory+"\\timelines.jpg";
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            toolStripLabel1.Text = "Username:";
            toolStripLabel2.Text = username;
            toolStrip1.BackColor = Color.LightGray;
            
            //toolStrip1.ForeColor = Color.White;
            close = true;
            if (role == "supervisor")
            {
                button6.Enabled = false;
                button9.Enabled = false;
                button1.Enabled = false;
                button14.Enabled = false;
                button12.Enabled = false;
                button3.Enabled = false;
                button21.Enabled = false;
                button22.Enabled = false;
                button23.Enabled = false;
                button24.Enabled = false;
            }
            else if (role== "standard")
            {
                button7.Enabled = false;
                button2.Enabled = false;
                button15.Enabled = false;
                button4.Enabled = false;
                button21.Enabled = false;
                button22.Enabled = false;
                button23.Enabled = false;
                button24.Enabled = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Customer c = new Customer("add");
            c.Show();
            this.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Customer c = new Customer("approve");
            c.Show();
            this.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Customer c = new Customer("search");
            c.Show();
            this.Enabled = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Customer c = new Customer("update");
            c.Show();
            this.Enabled = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Program.f1.Show();
            
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close)
                Program.f1.Close();
            else
            {
                Program.f1.Show();
                Program.f1.textBox1.Text = "";
                Program.f1.textBox2.Text = "";
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.f1.Show();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            close = false;
            Program.f2.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Vendor v = new Vendor("add");
            v.Show();
            this.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vendor v = new Vendor("approve");
            v.Show();
            this.Enabled = false;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Vendor v = new Vendor("search");
            v.Show();
            this.Enabled = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Vendor v = new Vendor("update");
            v.Show();
            this.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            POCreation poc = new POCreation();
            poc.Show();
            this.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            POApproval poa = new POApproval();
            poa.Show();
            this.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GRN grn = new GRN();
            grn.Show();
            this.Enabled = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            InvoicePayable i = new InvoicePayable();
            i.Show();
            this.Enabled = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            (new SOCreation()).Show();
            this.Enabled = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            (new SOApproval()).Show();
            this.Enabled = false;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            (new DC()).Show();
            this.Enabled = false;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            (new InvoiceRecievable()).Show();
            this.Enabled = false;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            (new User("add")).Show();
            this.Enabled = false;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            (new User("delete")).Show();
            this.Enabled = false;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            (new User("search")).Show();
            this.Enabled = false;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            (new User("update")).Show();
            this.Enabled = false;
        }


    }
}
