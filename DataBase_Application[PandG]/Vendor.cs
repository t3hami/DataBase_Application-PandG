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
    public partial class Vendor : Form
    {
        MyConnection mc;
        OleDbCommand cmd;
        OleDbDataReader dr;
        string selection;
        public Vendor(string s)
        {
            InitializeComponent();
            selection = s;
        }

        private void Vendor_Load(object sender, EventArgs e)
        {
            mc = new MyConnection();
            if (selection == "add")
            {
                this.Text = "Add Vender";
                comboBox1.Visible = false;
                textBox13.Visible = false;
                comboBox2.Visible = true;
                textBox12.Visible = true;
                mc.conn.Open();
                cmd = new OleDbCommand("select GrpName from CusGroup;",mc.conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                    comboBox2.Items.Add(dr["GrpName"]);
                mc.conn.Close();
            }
            else if (selection == "approve")
            {
                this.Text = "Approve Vendor";
                button2.Enabled = false;
                button3.Enabled = false;
                button2.Text = "Approve";
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;
                textBox8.ReadOnly = true;
                textBox9.ReadOnly = true;
                textBox10.ReadOnly = true;
                textBox13.ReadOnly = true;
                mc.conn.Open();
                cmd = new OleDbCommand("select VID from Vendor where VStatus='Inactive';",mc.conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                    comboBox1.Items.Add(dr["VID"]);
                mc.conn.Close();
            }
            else if (selection == "search")
            {
                this.Text = "Search Vendor";
                button1.Visible = false;
                button2.Visible = false;
                button3.Enabled = false;
                comboBox2.Visible = false;
                textBox12.Visible = false;
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;
                textBox8.ReadOnly = true;
                textBox9.ReadOnly = true;
                textBox10.ReadOnly = true;
                textBox11.ReadOnly = true;
                textBox13.ReadOnly = true;
                mc.conn.Open();
                cmd = new OleDbCommand("select VID from Vendor;", mc.conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                    comboBox1.Items.Add(dr["VID"]);
                mc.conn.Close();
            }
            else if (selection == "update")
            {
                this.Text = "Update Vendor";
                button2.Text = "Update";
                button3.Enabled = false;
                button2.Enabled = false;
                comboBox2.Visible = false;
                textBox12.Visible = false;
                textBox13.ReadOnly = true;
                mc.conn.Open();
                cmd = new OleDbCommand("select VID from Vendor;",mc.conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                    comboBox1.Items.Add(dr["VID"]);
                mc.conn.Close();
            }
        }

        private void populateID()
        {
            comboBox1.Items.Clear();
            mc.conn.Open();
            cmd = new OleDbCommand("select VID from Vendor;",mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                comboBox1.Items.Add(dr["VID"]);
            mc.conn.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 1;
            mc.conn.Open();
            cmd = new OleDbCommand("select count(VID) from Vendor where VGroup='" + comboBox2.Text + "';", mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
                c = Convert.ToInt32(dr[0]);
            if (comboBox2.Text == "Consumer")
                textBox12.Text = "VEN/CON-00" + c.ToString() + "/" + System.DateTime.Today.Year;
            else if (comboBox2.Text == "HR")
                textBox12.Text = "VEN/HR-00" + c.ToString() + "/" + System.DateTime.Today.Year;
            else if (comboBox2.Text == "Marketing")
                textBox12.Text = "VEN/MRK-00" + c.ToString() + "/" + System.DateTime.Today.Year;
            else if (comboBox2.Text == "Sales")
                textBox12.Text = "VEN/SAL-00" + c.ToString() + "/" + System.DateTime.Today.Year;
            mc.conn.Close();
            textBox11.Text = "Inactive";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selection == "add")
            {
                mc.conn.Open();
                cmd = new OleDbCommand("insert into Vendor Values(@VID,@VName,@VCode,@VCity,@PH1,@PH2,@VAddress,@CPName,@CPPH,@VEmail,@VFax,@VGroup,'Inactive')", mc.conn);
                cmd.Parameters.AddWithValue("@VID", textBox12.Text);
                cmd.Parameters.AddWithValue("@VName", textBox1.Text);
                cmd.Parameters.AddWithValue("@VCode", textBox2.Text);
                cmd.Parameters.AddWithValue("@VCity", textBox3.Text);
                cmd.Parameters.AddWithValue("@PH1", textBox4.Text);
                cmd.Parameters.AddWithValue("@PH2", textBox5.Text);
                cmd.Parameters.AddWithValue("@VAddress", textBox6.Text);
                cmd.Parameters.AddWithValue("@CPName", textBox7.Text);
                cmd.Parameters.AddWithValue("@CPPH", textBox8.Text);
                cmd.Parameters.AddWithValue("@VEmail", textBox9.Text);
                cmd.Parameters.AddWithValue("@VFax", textBox10.Text);
                cmd.Parameters.AddWithValue("@VGroup", comboBox2.Text);
                cmd.ExecuteNonQuery();
                mc.conn.Close();
                MessageBox.Show("Vendor added!");
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;
                textBox8.ReadOnly = true;
                textBox9.ReadOnly = true;
                textBox10.ReadOnly = true;
                comboBox2.Enabled = false;
            }

            else if (selection == "approve")
            {
                mc.conn.Open();
                cmd = new OleDbCommand("update Vendor set VStatus='Active' where VID='"+comboBox1.Text+"';",mc.conn);
                cmd.ExecuteNonQuery();
                mc.conn.Close();
                MessageBox.Show("Vendor activated!");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                comboBox2.Text = "";
                comboBox1.Text = "";
                comboBox1.Items.Clear();
                mc.conn.Open();
                cmd = new OleDbCommand("select VID from Vendor where VStatus='Inactive';", mc.conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                    comboBox1.Items.Add(dr["VID"]);
                mc.conn.Close();
                button2.Enabled = false;
            }

            else if (selection == "update")
            {
                mc.conn.Open();
                cmd = new OleDbCommand("update Vendor set VName=@VName, VCode=@VCode, VCity=@VCity, PH1=@PH1, PH2=@PH2, VAddress=@VAddress, CPName=@CPName, CPPH=@CPPH, VEmail=@VEmail, VFax=@VFax where VID=@VID;",mc.conn);
                cmd.Parameters.AddWithValue("@VName",textBox1.Text);
                cmd.Parameters.AddWithValue("@VCode", textBox2.Text);
                cmd.Parameters.AddWithValue("@VCity", textBox3.Text);
                cmd.Parameters.AddWithValue("@PH1", textBox4.Text);
                cmd.Parameters.AddWithValue("@PH2", textBox5.Text);
                cmd.Parameters.AddWithValue("@VAddress", textBox6.Text);
                cmd.Parameters.AddWithValue("@CPName", textBox7.Text);
                cmd.Parameters.AddWithValue("@CPPH", textBox8.Text);
                cmd.Parameters.AddWithValue("@VEmail", textBox9.Text);
                cmd.Parameters.AddWithValue("@VFax", textBox10.Text);
                cmd.Parameters.AddWithValue("@VID", comboBox1.Text);
                cmd.ExecuteNonQuery();
                mc.conn.Close();
                MessageBox.Show("Vendor updated!");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                comboBox2.Text = "";
                comboBox1.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            comboBox2.Text = "";
            comboBox1.Text = "";

            if (selection == "add")
            {
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                textBox5.ReadOnly = false;
                textBox6.ReadOnly = false;
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox10.ReadOnly = false;
                comboBox2.Enabled = true;
                
            }
            button3.Enabled = false;
            button2.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selection == "approve")
            {
                mc.conn.Open();
                cmd = new OleDbCommand("select * from Vendor where VID='" + comboBox1.Text + "';", mc.conn);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox13.Text = dr["VGroup"].ToString();
                    textBox1.Text = dr["VName"].ToString();
                    textBox2.Text = dr["VCode"].ToString();
                    textBox3.Text = dr["VCity"].ToString();
                    textBox4.Text = dr["PH1"].ToString();
                    textBox5.Text = dr["PH2"].ToString();
                    textBox6.Text = dr["VAddress"].ToString();
                    textBox7.Text = dr["CPName"].ToString();
                    textBox8.Text = dr["CPPH"].ToString();
                    textBox9.Text = dr["VEmail"].ToString();
                    textBox10.Text = dr["VFax"].ToString();
                    textBox11.Text = dr["VStatus"].ToString();
                }
                mc.conn.Close();
            }
            else
            {
                mc.conn.Open();
                cmd = new OleDbCommand("select * from Vendor where VID='"+comboBox1.Text+"';", mc.conn);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox13.Text = dr["VGroup"].ToString();
                    textBox1.Text = dr["VName"].ToString();
                    textBox2.Text = dr["VCode"].ToString();
                    textBox3.Text = dr["VCity"].ToString();
                    textBox4.Text = dr["PH1"].ToString();
                    textBox5.Text = dr["PH2"].ToString();
                    textBox6.Text = dr["VAddress"].ToString();
                    textBox7.Text = dr["CPName"].ToString();
                    textBox8.Text = dr["CPPH"].ToString();
                    textBox9.Text = dr["VEmail"].ToString();
                    textBox10.Text = dr["VFax"].ToString();
                    textBox11.Text = dr["VStatus"].ToString();
                }
                mc.conn.Close();
            }
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Vendor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.f2.Enabled = true;
        }

    }
}
