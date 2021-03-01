using System;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class f_user : Form
    {
        private string fileName = "aicheBois.accdb";
        private string fileNameType = "type.accdb";
        private string sourcePath = Application.StartupPath + "\\Resources\\";
        private string targetPath = Program.generale_path + "\\base_donnee";
        public f_user()
        {
            if (System.Diagnostics.Process.GetProcessesByName("aiche bois").Length > 0)
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
                    Application.Restart();
                    Environment.Exit(0);
                }

                InitializeComponent();
            }
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
                    ";Jet OLEDB:Database Password=" + t_pass_word.Text + ";";

            Program.PathType =
                    "Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + Path.Combine(targetPath, fileNameType) +
                    ";Mode=Share Deny None" +
                    ";Jet OLEDB:Database Password=" + t_pass_word.Text + ";";
            try
            {
                // Try to connect to the database.
                OleDbConnection conn = new OleDbConnection(Program.Path);
                conn.Open();
                conn.Close();

                OleDbConnection conntype = new OleDbConnection(Program.PathType);
                conntype.Open();
                conntype.Close();

                f_main_client formClient = new f_main_client();
                this.Hide();
                formClient.ShowDialog();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                LogFile.Message(ex);
            }
        }

        /// <summary>
        /// button cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// encrypt password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            t_pass_word.UseSystemPasswordChar = true;
        }

        /// <summary>
        /// decrypt password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showPassWord_Click(object sender, EventArgs e)
        {
            t_pass_word.UseSystemPasswordChar = false;
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            t_user_name.Text = Environment.UserName.ToString().ToUpper();
        }
    }
}
