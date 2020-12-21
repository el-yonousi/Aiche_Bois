using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Aiche_Bois
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        /*declaration des variables*/
        public static int idClient = 0;
        public static int idFacture = 0;
        public static string btnAddClick;
        public static List<Client> Clients = new List<Client>();
        public static int indxFacture = 0;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormClient());
        }
    }
}
