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
    public partial class InvoiceRecievable : Form
    {
        MyConnection mc;
        OleDbCommand cmd;
        OleDbDataReader dr;
        double total=0,totalTax=0;

        public InvoiceRecievable()
        {
            InitializeComponent();
        }

        private void InvoiceRecievable_Load(object sender, EventArgs e)
        {
            mc = new MyConnection();
            mc.conn.Open();
            cmd = new OleDbCommand("select DCID from DC where status='Open';", mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                comboBox1.Items.Add(dr["DCID"]);
            mc.conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mc.conn.Open();
            cmd = new OleDbCommand("select BaseDocument, CName, DDate from DC where DCID='" + comboBox1.Text + "';", mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox1.Text = dr["BaseDocument"].ToString();
                textBox2.Text = dr["CName"].ToString();
                textBox3.Text = dr["DDate"].ToString();
                cmd = new OleDbCommand("select TotalAmount from SO where SOID='" + dr["BaseDocument"] + "';", mc.conn);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                    total = Convert.ToInt32(dr["TotalAmount"]);
                textBox4.Text = "Rs." + total.ToString();
                totalTax = Math.Round((total * 1.17), 0);
                textBox6.Text = "Rs." + totalTax.ToString();
            }
            mc.conn.Close();
            button1.Enabled = true;

            dataGridView1.Rows.Clear();
            mc.conn.Open();
            cmd = new OleDbCommand("select PModel, PQty from SOProducts where SOID='" + textBox1.Text + "';", mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                dataGridView1.Rows.Add(dr["PModel"].ToString(), dr["PQty"].ToString());
            cmd = new OleDbCommand("select count(InvoiceNo) from InvoiceRecievable;", mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
                textBox5.Text = (Convert.ToInt32(dr[0]) + 1).ToString();
            mc.conn.Close();
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mc.conn.Open();
            cmd = new OleDbCommand("update DC set Status='Close' where DCID='" + comboBox1.Text + "';", mc.conn);
            cmd.ExecuteNonQuery();
            cmd = new OleDbCommand("select DCDate, DDate, CName, CID, CContactPerson, CCPPH from SO where SOID='" + textBox1.Text + "';", mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cmd = new OleDbCommand("insert into InvoiceRecievable Values(@InvoiceNo,@CustomerID,@CustomerName,@ContactPerson,@CPPH,@DCDate,@DDate,@RDate,@AmountRecievable,@DCID);", mc.conn);
                cmd.Parameters.AddWithValue("@InvoiceNo", textBox5.Text);
                cmd.Parameters.AddWithValue("@CustomerID", dr["CID"].ToString());
                cmd.Parameters.AddWithValue("@CustomerName", dr["CName"].ToString());
                cmd.Parameters.AddWithValue("@ContactPerson", dr["CContactPerson"].ToString());
                cmd.Parameters.AddWithValue("@CPPH", dr["CCPPH"].ToString());
                cmd.Parameters.AddWithValue("@DCDate", dr["DCDate"].ToString());
                cmd.Parameters.AddWithValue("@DDate", dr["DDate"].ToString());
                cmd.Parameters.AddWithValue("@RDate", dr["DCDate"].ToString());
                cmd.Parameters.AddWithValue("@AmountRecievable", totalTax);
                cmd.Parameters.AddWithValue("@DCID", comboBox1.Text);
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

        private void InvoiceRecievable_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.f2.Enabled = true;
        }
    }
}
