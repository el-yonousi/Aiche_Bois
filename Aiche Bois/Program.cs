using System;
using System.Windows.Forms;

namespace Aiche_Bois
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        /* generalae path for this project */
        //private string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\aiche bois\\base donnée";
        public static string generale_path = Environment.GetEnvironmentVariable("OneDriveConsumer") + "\\aiche_bois";

        /* this variable to add type bois into database */
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
