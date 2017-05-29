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
    public partial class AboutMe : Form
    {
        public AboutMe()
        {
            InitializeComponent();
        }

        private void AboutMe_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = Environment.CurrentDirectory+"\\img.jpg";
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Text = "About Developer";
        }

        private void AboutMe_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.f1.Enabled = true;
        }

    }
}
