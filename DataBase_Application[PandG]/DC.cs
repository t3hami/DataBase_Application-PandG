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
    public partial class DC : Form
    {
        MyConnection mc;
        OleDbCommand cmd;
        OleDbDataReader dr;

        public DC()
        {
            InitializeComponent();
        }

        private void DC_Load(object sender, EventArgs e)
        {
            mc = new MyConnection();
            mc.conn.Open();
            cmd = new OleDbCommand("select SOID from SO where Approve='Approved' and Status='Open' and GoodsDeliver='No';", mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                comboBox1.Items.Add(dr["SOID"]);
            mc.conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            mc.conn.Open();
            cmd = new OleDbCommand("select PModel, PQty from SOProducts where SOID='" + comboBox1.Text + "';", mc.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
                dataGridView1.Rows.Add(dr["PModel"].ToString(), dr["PQty"].ToString());
            mc.conn.Close();
            textBox1.Text = "DC/" + comboBox1.Text;
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mc.conn.Open();
            cmd = new OleDbCommand("update SO set GoodsDeliver='Yes' where SOID='" + comboBox1.Text + "';", mc.conn);
            cmd.ExecuteNonQuery();
            cmd = new OleDbCommand("select count(DCID) from DC;", mc.conn);
            dr = cmd.ExecuteReader();
            int sno = 0;
            if (dr.Read())
                sno = Convert.ToInt32(dr[0]) + 1;
            cmd = new OleDbCommand("select CName, DCDate, DDate from SO where SOID='" + comboBox1.Text + "';", mc.conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cmd = new OleDbCommand("insert into DC Values(@DCID,@BaseDocument,'Open',@CName,@DCDate,@DDate,@SNO)", mc.conn);
                cmd.Parameters.AddWithValue("@DCID", textBox1.Text);
                cmd.Parameters.AddWithValue("@BaseDocument", comboBox1.Text);
                cmd.Parameters.AddWithValue("@CName", dr["CName"].ToString());
                cmd.Parameters.AddWithValue("@DCDate", dr["DCDate"].ToString());
                cmd.Parameters.AddWithValue("@DDate", dr["DDate"].ToString());
                cmd.Parameters.AddWithValue("@SNO", sno);
                cmd.ExecuteReader();
            }
            mc.conn.Close();
            MessageBox.Show("Record updated!");
            textBox1.Text = "";
            comboBox1.Items.Remove(comboBox1.Text);
            comboBox1.Text = "";
            dataGridView1.Rows.Clear();
        }

        private void DC_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.f2.Enabled = true;
        }
    }
}
