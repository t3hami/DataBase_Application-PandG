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
    public partial class POCreation : Form
    {
        MyConnection mc;
        OleDbCommand cmd;
        OleDbDataReader dr;
        string[] prds = new string[50];
        int[] qty = new int[50];
        int counter = 0;
        int price;

        public POCreation()
        {
            InitializeComponent();
        }

        private void POCreation_Load(object sender, EventArgs e)
        {
            mc = new MyConnection();
            mc.conn.Open();
            cmd = new OleDbCommand("select VID from Vendor where VStatus='Active';",mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                comboBox1.Items.Add(dr["VID"]);
            cmd = new OleDbCommand("select PModel from Products;",mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                comboBox2.Items.Add(dr["PModel"]);
            mc.conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mc.conn.Open();
            cmd = new OleDbCommand("select * from Vendor where VID='"+comboBox1.Text+"';",mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox2.Text = dr["VName"].ToString();
                textBox3.Text = dr["VCode"].ToString();
                textBox4.Text = dr["VCity"].ToString();
                textBox5.Text = dr["PH1"].ToString();
                textBox6.Text = dr["PH2"].ToString();
                textBox7.Text = dr["VAddress"].ToString();
                textBox8.Text = dr["CPName"].ToString();
                textBox9.Text = dr["CPPH"].ToString();
                textBox10.Text = dr["VEmail"].ToString();
                textBox11.Text = dr["VFax"].ToString();
                textBox15.Text = dr["VGroup"].ToString();
            }
            cmd = new OleDbCommand("select count(POID) from PO where VDept='"+textBox15.Text+"';",mc.conn);
            dr = cmd.ExecuteReader();
            int c = 1;
            if(dr.Read())
                c = Convert.ToInt32(dr[0]);
            mc.conn.Close();
            if (textBox15.Text == "Consumer")
                textBox1.Text = "PO/CON-00" + c.ToString() +"/"+ System.DateTime.Now.Year;
            else if (textBox15.Text == "HR")
                textBox1.Text = "PO/HR-00" + c.ToString() + "/" + System.DateTime.Now.Year;
            else if (textBox15.Text == "Sales")
                textBox1.Text = "PO/SAL-00" + c.ToString() + "/" + System.DateTime.Now.Year;
            else if (textBox15.Text == "Marketing")
                textBox1.Text = "PO/MRK-00" + c.ToString() + "/" + System.DateTime.Now.Year;
            dateTimePicker1.Enabled = true;
            comboBox2.Enabled = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            mc.conn.Open();
            cmd = new OleDbCommand("select PName, BasePrice from Products where PModel='"+comboBox2.Text+"';",mc.conn);
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            (new POCreation()).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int sno = 1;
            mc.conn.Open();
            cmd = new OleDbCommand("select count(POID) from PO;",mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
                sno = Convert.ToInt32(dr[0])+1;
            cmd = new OleDbCommand("insert into PO Values(@POID,@DCDate,@DDate,'Open','Not Approved',@VDept,@VName,@VID,@VContactPerson,@VCPPH,@TotalAmount,@SNO,'No');",mc.conn);
            cmd.Parameters.AddWithValue("@POID",textBox1.Text);
            cmd.Parameters.AddWithValue("@DCDate", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@DDate", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@VDept", textBox15.Text);
            cmd.Parameters.AddWithValue("@VName", textBox2.Text);
            cmd.Parameters.AddWithValue("@VID", comboBox1.Text);
            cmd.Parameters.AddWithValue("@VContactPerson", textBox8.Text);
            cmd.Parameters.AddWithValue("@VCPPH", textBox9.Text);
            cmd.Parameters.AddWithValue("@TotalAmount", label20.Text);
            cmd.Parameters.AddWithValue("@SNO", sno);
            cmd.ExecuteNonQuery();
            for (int i = 0; i < counter; i++)
            {
                cmd = new OleDbCommand("insert into POProducts Values(@POID,@PModel,@PQty);",mc.conn);
                cmd.Parameters.AddWithValue("@POID",textBox1.Text);
                cmd.Parameters.AddWithValue("@PModel", prds[i]);
                cmd.Parameters.AddWithValue("@PQty", qty[i]);
                cmd.ExecuteNonQuery();
            }
            mc.conn.Close();
            MessageBox.Show("Order inserted!");
        }

        private void POCreation_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.f2.Enabled = true;
        }

    }
}
