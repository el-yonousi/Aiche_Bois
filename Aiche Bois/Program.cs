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
        public static string document_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\aiche_bois";
        public static string generale_path = Environment.GetEnvironmentVariable("OneDriveConsumer") + "\\aiche_bois";

        /* this variable to add type bois into database */
        public static string btnAddTypeClick;

        //public password for databse access
        public static string Path;

        [STAThread]
        [Obsolete]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new f_user());
        }
    }
}
