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
    public partial class User : Form
    {
        MyConnection mc;
        OleDbCommand cmd;
        OleDbDataReader dr;
        string selection;

        public User(string s)
        {
            InitializeComponent();
            selection = s;
        }

        private void User_Load(object sender, EventArgs e)
        {
            mc = new MyConnection();
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            if (selection == "add")
            {
                comboBox1.Visible = false;
                textBox2.Visible = false;
                this.Text = "Add User";
                button1.Text = "Add";
                comboBox2.Items.Add("standard");
                comboBox2.Items.Add("supervisor");
            }
            else if (selection == "delete")
            {
                this.Text = "Delete User";
                button1.Text = "Delete";
                textBox1.Visible = false;
                comboBox2.Visible = false;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.Visible = false;
                label4.Visible = false;
                populate();
            }
            else if (selection == "search")
            {
                this.Text = "Search User";
                button1.Visible = false;
                textBox1.Visible = false;
                comboBox2.Visible = false;
                textBox4.Visible = false;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                label4.Visible = false;
                populate();
            }
            else if (selection == "update")
            {
                this.Text = "Update User";
                button1.Text = "Update";
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox4.Visible = false;
                label4.Visible = false;
                populate();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                mc = new MyConnection();
                mc.conn.Open();
                cmd = new OleDbCommand("select Name from Users where Name='" + textBox1.Text + "';", mc.conn);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    label5.Visible = true;
                    label5.Text = "X";
                    label5.ForeColor = Color.Red;
                }
                else
                {
                    label5.Visible = true;
                    label5.ForeColor = Color.Blue;
                    label5.Text = ((char)0x221A).ToString();
                    check();
                }
                mc.conn.Close();
            }
            else
            {
                label5.Visible = false;
            }
            check();
        }

        private void check()
        {
            if (textBox1.Text != "" && comboBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && label5.Text != "X" && label6.Text != "X" && label7.Text != "X")
                button1.Enabled = true;
            else
                button1.Enabled = false;
        }

        private void populate()
        {
            mc.conn.Open();
            cmd = new OleDbCommand("select Name from Users;", mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                comboBox1.Items.Add(dr["Name"]);
            mc.conn.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selection == "add")
            {
                if (comboBox2.Text != "")
                {
                    label6.Visible = true;
                    label6.ForeColor = Color.Blue;
                    label6.Text = ((char)0x221A).ToString();
                }
                else
                {
                    label6.Visible = true;
                    label6.Text = "X";
                    label6.ForeColor = Color.Red;
                }
                check();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (selection == "add")
            {
                if (textBox3.Text == textBox4.Text)
                {
                    label7.Visible = true;
                    label7.ForeColor = Color.Blue;
                    label7.Text = ((char)0x221A).ToString();
                }
                else
                {
                    label7.Visible = true;
                    label7.Text = "X";
                    label7.ForeColor = Color.Red;
                }
                check();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (selection == "add")
            {
                if (textBox4.Text != "")
                {
                    if (textBox3.Text == textBox4.Text)
                    {
                        label7.Visible = true;
                        label7.ForeColor = Color.Blue;
                        label7.Text = ((char)0x221A).ToString();
                    }
                    else
                    {
                        label7.Visible = true;
                        label7.Text = "X";
                        label7.ForeColor = Color.Red;
                    }
                }
                else
                    label7.Visible = false;
                check();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            mc.conn.Open();
            if (selection == "add")
            {
                cmd = new OleDbCommand("insert into Users Values(@Name,@Password,@Role);", mc.conn);
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Password", textBox3.Text);
                cmd.Parameters.AddWithValue("@Role", comboBox2.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("User added!");
                textBox1.Text = "";
                comboBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                button1.Enabled = false;
            }
            else if (selection == "delete")
            {
                cmd = new OleDbCommand("delete from Users where Name='" + comboBox1.Text + "';", mc.conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("User deleted!");
                comboBox1.Items.Remove(comboBox1.Text);
                comboBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }

            else if (selection == "update")
            {
                cmd = new OleDbCommand("update Users set [Password]=@Password, Role=@Role where Name=@Name;",mc.conn);
                cmd.Parameters.AddWithValue("@Name",comboBox1.Text);
                cmd.Parameters.AddWithValue("@Password",textBox3.Text);
                cmd.Parameters.AddWithValue("@Role",comboBox2.Text);
                cmd.ExecuteReader();
                MessageBox.Show("User updated!");
                comboBox1.Text = "";
                comboBox2.Text = "";
                textBox3.Text = "";
            }

            mc.conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            mc.conn.Open();
            cmd = new OleDbCommand("select * from Users where Name='" + comboBox1.Text + "';", mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox2.Text = dr["Role"].ToString();
                comboBox2.Text = dr["Role"].ToString();
                textBox3.Text = dr["Password"].ToString();
                button1.Enabled = true;
            }
            mc.conn.Close();
            textBox3.PasswordChar = '\0';
            if (selection == "update")
            {
                button1.Enabled = true;
                comboBox2.Items.Add("standard");
                comboBox2.Items.Add("supervisor");
            }
        }

        private void User_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.f2.Enabled = true;
        }
    }
}
