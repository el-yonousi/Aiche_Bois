using System;
using System.Data.OleDb;
using System.Globalization;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class FormAvance : Form
    {
        /// <summary>
        /// this is a message custom box
        /// </summary>
        FormMessage message;

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
        public FormAvance(String idClient)
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
                    lblIDClient.Text = "N" + Convert.ToInt32(readerClient["idClient"]).ToString("D4");
                    lblNomClient.Text = readerClient["nomClient"].ToString();
                    lblDateClient.Text = Convert.ToDateTime(readerClient["dateClient"]).ToString("d");
                    checkAvance = Convert.ToBoolean((readerClient["cavance"]).ToString());
                    lblAvancePrixClient.Text = Convert.ToDouble(readerClient["prixTotalAvance"]).ToString("F2");
                    lblRestPrixClient.Text = Convert.ToDouble(readerClient["rest"]).ToString("F2");
                    lblTotalPrixClient.Text = Convert.ToDouble(readerClient["total"]).ToString("F2");
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Error:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connection.Close();
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
            btnConfirm.Enabled = false;
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
                    if (double.Parse(txtMines.Text) > double.Parse(lblAvancePrixClient.Text))
                    {
                        message = new FormMessage("le montant doit etre moins à montant payée", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                        message.ShowDialog();
                        txtMines.Clear();
                        txtMines.Focus();
                        connection.Close();
                        return;
                    }
                    OleDbCommand command = new OleDbCommand
                    {
                        Connection = connection,
                        CommandText = "UPDATE client SET prixTotalAvance = prixTotalAvance - " + double.Parse(txtMines.Text) + " where idClient = " + idClient
                    };
                    command.ExecuteNonQuery();
                    txtMines.Clear();
                    txtMines.Focus();

                    connection.Close();
                }
                else
                {
                    if (double.Parse(txtPrixCLient.Text) > double.Parse(lblRestPrixClient.Text))
                    {
                        message = new FormMessage("le Prix ​​à payer est plus grand a le reste", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                        message.ShowDialog();
                        txtPrixCLient.Clear();
                        txtPrixCLient.Focus();
                        connection.Close();
                        return;
                    }

                    OleDbCommand command = new OleDbCommand
                    {
                        Connection = connection,
                        CommandText = "UPDATE client SET prixTotalAvance = prixTotalAvance + " + double.Parse(txtPrixCLient.Text) + " where idClient = " + idClient
                    };
                    command.ExecuteNonQuery();
                    txtPrixCLient.Clear();
                    txtPrixCLient.Focus();

                    connection.Close();
                }

                message = new FormMessage("le montant a été modifié avec succès", "Succès", true, FontAwesome.Sharp.IconChar.CheckCircle);
                message.ShowDialog();
                
                // call function to full a fields
                remplirData();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Error:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connection.Close();
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
                if (txtPrixCLient.TextLength > 0)
                    btnConfirm.Enabled = true;
                else
                    btnConfirm.Enabled = false;
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
                if (txtMines.TextLength > 0)
                    btnConfirm.Enabled = true;
                else
                    btnConfirm.Enabled = false;
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

        private void ckMines_CheckedChanged(object sender, EventArgs e)
        {
            txtMines.Enabled = ckMines.Checked;
            txtPrixCLient.Enabled = !ckMines.Checked;
        }

        private void txtMines_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(47, 47, 47);
            ((TextBox)sender).ForeColor = System.Drawing.Color.FromArgb(255, 170, 0);
        }

        private void txtMines_Leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(255, 244, 228);
            ((TextBox)sender).ForeColor = System.Drawing.Color.FromArgb(47, 47, 47);
        }
    }
}
