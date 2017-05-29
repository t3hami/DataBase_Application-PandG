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
    public partial class MyConnection : Form
    {
        public OleDbConnection conn;
        public MyConnection()
        {
            InitializeComponent();
            conn = new OleDbConnection();
            conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Environment.CurrentDirectory + "\\PC_DB.accdb";
        }
    }
}
