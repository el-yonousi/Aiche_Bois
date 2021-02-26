using System;
using System.Windows.Forms;

namespace Aiche_Bois
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        /*declaration des variables*/
        public static string btnAddTypeClick;

        //public password for databse access
        public static string Path;
        public static string PathType;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new f_user());
        }
    }
}
