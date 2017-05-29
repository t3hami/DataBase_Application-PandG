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
    public partial class InvoicePayable : Form
    {
        MyConnection mc;
        OleDbCommand cmd;
        OleDbDataReader dr;
        double total = 0;
        double totalTax = 0;

        public InvoicePayable()
        {
            InitializeComponent();
        }

        private void Invoice_Load(object sender, EventArgs e)
        {
            mc = new MyConnection();
            mc.conn.Open();
            cmd = new OleDbCommand("select GRNID from GRN where status='Open';",mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                comboBox1.Items.Add(dr["GRNID"]);
            mc.conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mc.conn.Open();
            cmd = new OleDbCommand("select BaseDocument, VName, DDate from GRN where GRNID='"+comboBox1.Text+"';",mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox1.Text = dr["BaseDocument"].ToString();
                textBox2.Text = dr["VName"].ToString();
                textBox3.Text = dr["DDate"].ToString();
                cmd = new OleDbCommand("select TotalAmount from PO where POID='"+dr["BaseDocument"]+"';",mc.conn);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                    total = Convert.ToInt32(dr["TotalAmount"]);
                textBox4.Text = "Rs." + total.ToString();
                totalTax = Math.Round(((total * 0.17) + total), 0);
                textBox6.Text = "Rs." + totalTax.ToString();
            }
            mc.conn.Close();
            button1.Enabled = true;

            dataGridView1.Rows.Clear();
            mc.conn.Open();
            cmd = new OleDbCommand("select PModel, PQty from POProducts where POID='" + textBox1.Text + "';", mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                dataGridView1.Rows.Add(dr["PModel"].ToString(), dr["PQty"].ToString());
            cmd = new OleDbCommand("select count(InvoiceNo) from InvoicePayable;", mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
                textBox5.Text = (Convert.ToInt32(dr[0]) + 1).ToString();
            mc.conn.Close();
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mc.conn.Open();
            cmd = new OleDbCommand("update GRN set Status='Close' where GRNID='"+comboBox1.Text+"';",mc.conn);
            cmd.ExecuteNonQuery();
            DateTime RDate=System.DateTime.Now;
            cmd = new OleDbCommand("select GRDate from GRN where GRNID='"+comboBox1.Text+"';",mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
                RDate = Convert.ToDateTime(dr["GRDate"]);
            cmd = new OleDbCommand("select DCDate, DDate, VName, VID, VContactPerson, VCPPH from PO where POID='" + textBox1.Text + "';", mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cmd = new OleDbCommand("insert into InvoicePayable Values(@InvoiceNo,@VendorID,@VendorName,@ContactPerson,@CPPH,@DCDate,@DDate,@RDate,@AmountPayable,@GRNID);", mc.conn);
                cmd.Parameters.AddWithValue("@InvoiceNo", textBox5.Text);
                cmd.Parameters.AddWithValue("@VendorID", dr["VID"].ToString());
                cmd.Parameters.AddWithValue("@VendorName", dr["VName"].ToString());
                cmd.Parameters.AddWithValue("@ContactPerson", dr["VContactPerson"].ToString());
                cmd.Parameters.AddWithValue("@CPPH", dr["VCPPH"].ToString());
                cmd.Parameters.AddWithValue("@DCDate", dr["DCDate"].ToString());
                cmd.Parameters.AddWithValue("@DDate", dr["DDate"].ToString());
                cmd.Parameters.AddWithValue("@RDate", RDate);
                cmd.Parameters.AddWithValue("@AmountPayable", totalTax);
                cmd.Parameters.AddWithValue("@GRNID",comboBox1.Text);
                cmd.ExecuteNonQuery();
            }
            mc.conn.Close();
            MessageBox.Show("Invoice created!");
            comboBox1.Items.Remove(comboBox1.Text);
            comboBox1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            dataGridView1.Rows.Clear();
            button1.Enabled = false;
        }

        private void InvoicePayable_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.f2.Enabled = true;
        }
    }
}
