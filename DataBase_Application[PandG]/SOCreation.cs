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
    public partial class SOCreation : Form
    {
        MyConnection mc;
        OleDbCommand cmd;
        OleDbDataReader dr;
        string[] prds = new string[50];
        int[] qty = new int[50];
        int counter = 0;
        int price = 0;

        public SOCreation()
        {
            InitializeComponent();
        }

        private void SOCreation_Load(object sender, EventArgs e)
        {
            mc = new MyConnection();
            mc.conn.Open();
            cmd = new OleDbCommand("select CID from Customer where CStatus='Active';",mc.conn);
            dr = cmd.ExecuteReader();
            while(dr.Read())
                comboBox1.Items.Add(dr["CID"]);
            cmd = new OleDbCommand("select PModel from Products;", mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                comboBox2.Items.Add(dr["PModel"]);
            mc.conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mc.conn.Open();
            cmd = new OleDbCommand("select * from Customer where CID='"+comboBox1.Text+"';",mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox2.Text = dr["Cname"].ToString();
                textBox3.Text = dr["CAddress"].ToString();
                textBox4.Text = dr["City"].ToString();
                textBox5.Text = dr["PH1"].ToString();
                textBox6.Text = dr["PH2"].ToString();
                textBox7.Text = dr["ContactPerson"].ToString();
                textBox8.Text = dr["CPPH"].ToString();
                textBox9.Text = dr["CEmail"].ToString();
                textBox10.Text = dr["CreditLimit"].ToString();
                textBox11.Text = dr["CGroup"].ToString();
            }
            cmd = new OleDbCommand("select count(SOID) from SO where CDept='" + textBox11.Text + "';", mc.conn);
            dr = cmd.ExecuteReader();
            int c = 1;
            if (dr.Read())
                c = Convert.ToInt32(dr[0]);
            mc.conn.Close();
            if (textBox11.Text == "Consumer")
                textBox1.Text = "SO/CON-00" + c.ToString() + "/" + System.DateTime.Now.Year;
            else if (textBox11.Text == "HR")
                textBox1.Text = "SO/HR-00" + c.ToString() + "/" + System.DateTime.Now.Year;
            else if (textBox11.Text == "Sales")
                textBox1.Text = "SO/SAL-00" + c.ToString() + "/" + System.DateTime.Now.Year;
            else if (textBox11.Text == "Marketing")
                textBox1.Text = "SO/MRK-00" + c.ToString() + "/" + System.DateTime.Now.Year;
            dateTimePicker1.Enabled = true;
            comboBox2.Enabled = true;
            mc.conn.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            mc.conn.Open();
            cmd = new OleDbCommand("select PName, BasePrice from Products where PModel='" + comboBox2.Text + "';", mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox12.Text = dr["PName"].ToString();
                price = Convert.ToInt32(dr["BasePrice"].ToString());
            }
            textBox13.Text = "Rs." + price.ToString();
            mc.conn.Close();
            textBox14.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            (new SOCreation()).Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox14.Text != "")
            {
                prds[counter] = comboBox2.Text;
                qty[counter] = Convert.ToInt32(textBox14.Text);
                dataGridView1.Rows.Add(comboBox2.Text, price * Convert.ToInt32(textBox14.Text));
                label20.Text = (Convert.ToInt32(label20.Text) + price * Convert.ToInt32(textBox14.Text)).ToString();
                counter++;
                comboBox2.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";
                textBox14.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = true;
            }
            else
                MessageBox.Show("Please enter product quantity.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int sno = 1;
            mc.conn.Open();
            cmd = new OleDbCommand("select count(SOID) from SO;", mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
                sno = Convert.ToInt32(dr[0])+1;
            cmd = new OleDbCommand("insert into SO Values(@SOID,@DCDate,@DDate,'Open','Not Approved',@CDept,@CName,@CID,@CContactPerson,@CCPPH,@TotalAmount,@SNO,'No');", mc.conn);
            cmd.Parameters.AddWithValue("@SOID", textBox1.Text);
            cmd.Parameters.AddWithValue("@DCDate", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@DDate", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@CDept", textBox11.Text);
            cmd.Parameters.AddWithValue("@CName", textBox2.Text);
            cmd.Parameters.AddWithValue("@CID", comboBox1.Text);
            cmd.Parameters.AddWithValue("@CContactPerson", textBox7.Text);
            cmd.Parameters.AddWithValue("@CCPPH", textBox8.Text);
            cmd.Parameters.AddWithValue("@TotalAmount", label20.Text);
            cmd.Parameters.AddWithValue("@SNO", sno);
            cmd.ExecuteNonQuery();
            for (int i = 0; i < counter; i++)
            {
                cmd = new OleDbCommand("insert into SOProducts(SOID,PModel,PQty) Values(@SOID,@PModel,@PQty);", mc.conn);
                cmd.Parameters.AddWithValue("@SOID", textBox1.Text);
                cmd.Parameters.AddWithValue("@PModel", prds[i]);
                cmd.Parameters.AddWithValue("@PQty", qty[i]);
                cmd.ExecuteNonQuery();
            }
            mc.conn.Close();
            MessageBox.Show("Order inserted!");
        }

        private void SOCreation_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.f2.Enabled = true;
        }

    }
}
