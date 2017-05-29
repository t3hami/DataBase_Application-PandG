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
    public partial class GRN : Form
    {
        MyConnection mc;
        OleDbCommand cmd;
        OleDbDataReader dr;

        public GRN()
        {
            InitializeComponent();
        }

        private void GRN_Load(object sender, EventArgs e)
        {
            mc = new MyConnection();
            mc.conn.Open();
            cmd = new OleDbCommand("select POID from PO where Approve='Approved' and Status='Open' and GoodRecieved='No';",mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                comboBox1.Items.Add(dr["POID"]);
            mc.conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            mc.conn.Open();
            cmd = new OleDbCommand("select PModel, PQty from POProducts where POID='"+comboBox1.Text+"';",mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                dataGridView1.Rows.Add(dr["PModel"].ToString(),dr["PQty"].ToString());
            mc.conn.Close();
            textBox1.Text = "GRN/" + comboBox1.Text;
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mc.conn.Open();
            cmd = new OleDbCommand("update PO set GoodRecieved='Yes' where POID='"+comboBox1.Text+"';",mc.conn);
            cmd.ExecuteNonQuery();
            cmd = new OleDbCommand("select count(GRNID) from GRN;",mc.conn);
            dr = cmd.ExecuteReader();
            int sno = 0;
            if(dr.Read())
                sno=Convert.ToInt32(dr[0])+1;
            cmd = new OleDbCommand("select VName, DCDate, DDate from PO where POID='"+comboBox1.Text+"';",mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cmd = new OleDbCommand("insert into GRN Values(@GRNID,@BaseDocument,'Open',@VName,@DCDate,@DDate,@GRDate,@SNO)", mc.conn);
                cmd.Parameters.AddWithValue("@GRNID", textBox1.Text);
                cmd.Parameters.AddWithValue("@BaseDocument", comboBox1.Text);
                cmd.Parameters.AddWithValue("@VName", dr["VName"].ToString());
                cmd.Parameters.AddWithValue("@DCDate", dr["DCDate"].ToString());
                cmd.Parameters.AddWithValue("@DDate", dr["DDate"].ToString());
                cmd.Parameters.AddWithValue("@GRDate", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@SNO", sno);
                cmd.ExecuteReader();
            }
                cmd = new OleDbCommand("select PModel, PQty from POProducts where POID='" + comboBox1.Text + "';", mc.conn);
                dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cmd = new OleDbCommand("insert into GRNProducts Values(@GRNID,@PModel,@PQty);",mc.conn);
                cmd.Parameters.AddWithValue("@GRNID",textBox1.Text);
                cmd.Parameters.AddWithValue("@PModel", dr["PModel"]);
                cmd.Parameters.AddWithValue("@PQty", dr["PQty"]);
                cmd.ExecuteNonQuery();
            }
            mc.conn.Close();
            MessageBox.Show("Record updated!");
            textBox1.Text="";
            comboBox1.Items.Remove(comboBox1.Text);
            comboBox1.Text = "";
            dataGridView1.Rows.Clear();
        }

        private void GRN_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.f2.Enabled = true;
        }
    }
}
