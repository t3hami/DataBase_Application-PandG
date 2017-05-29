using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DataBase_Application_PandG_
{
    static class Program
    {
        public static Form1 f1;
        public static Form2 f2;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            f1 = new Form1();
            Application.Run(f1);
        }
    }
}
