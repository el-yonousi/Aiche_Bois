using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class User : Form
    {
        private string fileName = "aicheBois.accdb";
        private string fileNameType = "type.accdb";
        private string sourcePath = Application.StartupPath;
        private string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\aiche bois\\base donnée";
        public User()
        {
            // Use Path class to manipulate file and directory paths.
            string destFile = Path.Combine(targetPath, fileName);

            if (!Directory.Exists(targetPath))
            {
                string[] files = Directory.GetFiles(sourcePath);
                Directory.CreateDirectory(targetPath);

                //// Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = Path.GetFileName(s);
                    if (fileName == "aicheBois.accdb")
                    {
                        destFile = Path.Combine(targetPath, fileName);
                        File.Copy(s, destFile, true);
                    }
                    if (fileName == fileNameType)
                    {
                        destFile = Path.Combine(targetPath, fileNameType);
                        File.Copy(s, destFile, true);
                    }
                }
            }

            InitializeComponent();
        }

        /// <summary>
        /// this is button connect to verify connection to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Compose the connect string.
            Program.Path = 
                    "Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + Path.Combine(targetPath, fileName) +
                    ";Mode=Share Deny None" +
                    ";Jet OLEDB:Database Password=" + txtPassWord.Text + ";";

            Program.PathType =
                    "Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + Path.Combine(targetPath, fileNameType) +
                    ";Mode=Share Deny None" +
                    ";Jet OLEDB:Database Password=" + txtPassWord.Text + ";";
            try
            {
                // Try to connect to the database.
                OleDbConnection conn = new OleDbConnection(Program.Path);
                conn.Open();
                conn.Close();

                OleDbConnection conntype = new OleDbConnection(Program.PathType);
                conntype.Open();
                conntype.Close();

                FormClient formClient = new FormClient();
                this.Hide();
                formClient.ShowDialog();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        /// <summary>
        /// button cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// encrypt password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            txtPassWord.UseSystemPasswordChar = true;
        }

        /// <summary>
        /// decrypt password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showPassWord_Click(object sender, EventArgs e)
        {
            txtPassWord.UseSystemPasswordChar = false;
        }

        //bool check = false;
        ///// <summary>
        ///// this is a dark, light mode
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void DarkLight_Click(object sender, EventArgs e)
        //{
        //    if (check)
        //    {
        //        Program.Color = System.Drawing.Color.FromArgb(47, 47, 47);
        //        this.ForeColor = System.Drawing.Color.FromArgb(255, 244, 228);
        //        check = false;
        //    }
        //    else
        //    {
        //        Program.Color = System.Drawing.Color.FromArgb(255, 170, 0);
        //        this.ForeColor = System.Drawing.Color.FromArgb(47, 47, 47);
        //        check = true;
        //    }
        //    this.BackColor = Program.Color;
        //    showPassWord.IconColor = Program.Color;
        //}
    }
}
