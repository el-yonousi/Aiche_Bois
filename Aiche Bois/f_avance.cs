using System;
using System.Data.OleDb;
using System.Globalization;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class f_avance : Form
    {
        /// <summary>
        /// this is a message custom box
        /// </summary>
        f_message message;

        /// <summary>
        /// c'est l'access a extereiur Client data base
        /// </summary>
        private readonly OleDbConnection connection = new OleDbConnection();

        /// <summary>
        /// perndre l'id de formulaire presedent;
        /// </summary>
        private long idClient;

        /// <summary>
        /// c'est le variable pour verifier le check box
        /// </summary>
        bool checkAvance;

        /// <summary>
        /// c'est formulaire de traitment
        /// </summary>
        /// <param name="idClient"></param>
        public f_avance(String idClient)
        {
            /*
             * get idClient from fomr client
             */

            this.idClient = Convert.ToInt64(idClient);


            /*
             * set database connection
             */
            connection.ConnectionString = Program.Path;

            InitializeComponent();
        }

        /// <summary>
        /// charger les donnees a partir de l'id prendre
        /// </summary>
        public void remplirData()
        {
            /**
             * upload database id
             */
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand
                {
                    Connection = connection,
                    CommandText =
                    "select * from query where idClient = " + idClient
                };

                OleDbDataReader readerClient = command.ExecuteReader();
                while (readerClient.Read())
                {
                    l_id_client.Text = "N" + Convert.ToInt32(readerClient["idClient"]).ToString("D4");
                    l_nom_client.Text = readerClient["nomClient"].ToString();
                    l_date_client.Text = Convert.ToDateTime(readerClient["dateClient"]).ToString("d");
                    checkAvance = Convert.ToBoolean((readerClient["cavance"]).ToString());
                    l_price_avance_client.Text = Convert.ToDouble(readerClient["prixTotalAvance"]).ToString("F2");
                    l_price_rest_client.Text = Convert.ToDouble(readerClient["rest"]).ToString("F2");
                    l_price_total_client.Text = Convert.ToDouble(readerClient["total"]).ToString("F2");
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                LogFile.Message(ex);
            }
        }

        /// <summary>
        /// on affiche la formulaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Avance_Load(object sender, EventArgs e)
        {
            /*
             * upload data from data base
             */
            remplirData();
            b_save.Enabled = false;
        }

        /// <summary>
        /// c'est button modifeir le prix avance est confirmer le triatmenet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            /**
             * updating database id
             */
            try
            {
                connection.Open();

                if (ckMines.Checked)
                {
                    if (double.Parse(t_mines_price.Text) > double.Parse(l_price_avance_client.Text))
                    {
                        message = new f_message("le montant doit etre moins à montant payée", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                        message.ShowDialog();
                        t_mines_price.Clear();
                        t_mines_price.Focus();
                        connection.Close();
                        return;
                    }
                    OleDbCommand command = new OleDbCommand
                    {
                        Connection = connection,
                        CommandText = "UPDATE client SET prixTotalAvance = prixTotalAvance - " + double.Parse(t_mines_price.Text) + " where idClient = " + idClient
                    };
                    command.ExecuteNonQuery();
                    t_mines_price.Clear();
                    t_mines_price.Focus();

                    connection.Close();
                }
                else
                {
                    if (double.Parse(t_price_add_cLient.Text) > double.Parse(l_price_rest_client.Text))
                    {
                        message = new f_message("le Prix ​​à payer est plus grand a le reste", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                        message.ShowDialog();
                        t_price_add_cLient.Clear();
                        t_price_add_cLient.Focus();
                        connection.Close();
                        return;
                    }

                    OleDbCommand command = new OleDbCommand
                    {
                        Connection = connection,
                        CommandText = "UPDATE client SET prixTotalAvance = prixTotalAvance + " + double.Parse(t_price_add_cLient.Text) + " where idClient = " + idClient
                    };
                    command.ExecuteNonQuery();
                    t_price_add_cLient.Clear();
                    t_price_add_cLient.Focus();

                    connection.Close();
                }

                // call function to full a fields
                remplirData();
            }
            catch (Exception ex)
            {
                connection.Close();
                LogFile.Message(ex);
            }

        }

        /// <summary>
        /// c'est evenet verifier le tye des donnees saisir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrix_KeyPress(object sender, KeyPressEventArgs e)
        {
            char comVer = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (double.TryParse(Clipboard.GetText(), out double vlr) && e.KeyChar == 22)
            {
                e.Handled = true;
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != comVer)
            {
                e.Handled = true;
            }

            if (e.KeyChar == comVer && (((sender as TextBox).Text.IndexOf(comVer) > -1) || (sender as TextBox).TextLength == 0))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// verifier c'est la champs n'est pas vide
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrix_TextChanged(object sender, EventArgs e)
        {
            if (!ckMines.Checked)
            {
                if (t_price_add_cLient.TextLength > 0)
                    b_save.Enabled = true;
                else
                    b_save.Enabled = false;
            }
        }

        /// <summary>
        /// verify field text if not empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMines_TextChanged(object sender, EventArgs e)
        {
            if (ckMines.Checked)
            {
                if (t_mines_price.TextLength > 0)
                    b_save.Enabled = true;
                else
                    b_save.Enabled = false;
            }
        }

        /// <summary>
        /// c'est le button pour annuler le traitement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// if button is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckMines_CheckedChanged(object sender, EventArgs e)
        {
            t_mines_price.Enabled = ckMines.Checked;
            t_price_add_cLient.Enabled = !ckMines.Checked;
            if (ckMines.Checked)
                t_mines_price.Focus();
            else
                t_price_add_cLient.Focus();
        }

        /// <summary>
        /// textBox Style on Enter event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMines_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(47, 47, 47);
            ((TextBox)sender).ForeColor = System.Drawing.Color.FromArgb(255, 170, 0);
        }

        /// <summary>
        /// textBox Style on Leave event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMines_Leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(255, 244, 228);
            ((TextBox)sender).ForeColor = System.Drawing.Color.FromArgb(47, 47, 47);
        }
    }
}
