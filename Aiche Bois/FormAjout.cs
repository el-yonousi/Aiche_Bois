using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class FormAjout : Form
    {
        /*decalaration des variables*/
        private readonly OleDbConnection connection = new OleDbConnection();

        /// <summary>
        /// Pour stocker des articles
        /// </summary>
        private readonly List<string> getLstCmbList = new List<string>();
        FormMessage message;
        bool check = false;
        public FormAjout()
        {
            connection.ConnectionString = Program.PathType;
            InitializeComponent();
        }

        /*sauvgarder les donnees dans dataBase*/
        private void suavgarderDonnees(string tableDB)
        {
            try
            {
                /*
                 * this query to save data in ms access database
                 * command.CommandText = "insert into PVC (Libelle) values('" + st + "')";
                 * this query to edit/update data in ms access database
                 * command.CommandText = "update PVC set Libelle = '" + st + "' where  IDPVC = " + txtID.Text + "" ; separate txtField by comma
                 * this query to delete data in ms access database
                 * command.CommandText = "delete from PVC where IDPVC="+txtID+"";
                 * this query to delete data in ms access database
                 * command.CommandText = "select * from PVC";
                 */
                connection.Open();
                OleDbCommand comDelete = new OleDbCommand
                {
                    Connection = connection,
                    CommandText = "delete * from " + tableDB
                };
                comDelete.ExecuteNonQuery();
                foreach (string st in lstCmb.Items)
                {
                    OleDbCommand commandInsert = new OleDbCommand
                    {
                        Connection = connection,
                        CommandText = "insert into " + tableDB + " (Libelle) values('" + st.ToString() + "')"
                    };
                    commandInsert.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connection.Close();
            }
        }

        /*remlir la liste de dataBase*/
        private void remplirListe(string typeBois)
        {
            lstCmb.Items.Clear();
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand
                {
                    Connection = connection,
                    CommandText = "select * from " + typeBois
                };
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lstCmb.Items.Add(reader["Libelle"].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connection.Close();
            }
        }
        private void Ajout_Load(object sender, EventArgs e)
        {
            txtCmb.Focus();
            lblEcris.Text = Text = ("ajouter " + Program.btnAddTypeClick).ToUpper();

            /*remplir liste on affichage*/
            remplirListe(Program.btnAddTypeClick);
        }

        private void btnAddLstCmb_Click(object sender, EventArgs e)
        {
            check = true;
            if (String.IsNullOrEmpty(txtCmb.Text) || String.IsNullOrWhiteSpace(txtCmb.Text))
            {
                message = new FormMessage("la format du text et incorrect ou text est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtCmb.Focus();
                return;
            }

            lstCmb.Items.Add(txtCmb.Text.ToUpper());
            txtCmb.Clear();
            txtCmb.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            /*suavgarder les donnees dans dataBase*/
            suavgarderDonnees(Program.btnAddTypeClick.ToUpper());
            message = new FormMessage("Ajouté Avec Succès", "Succès", true, FontAwesome.Sharp.IconChar.CheckCircle);
            message.ShowDialog();
            /*fermer la fonetre*/
            Close();

        }

        /*supprimer la ligne selectionner de dataBase*/
        private void deleteLigneListe(string tableDB)
        {
            /*supprimer la ligne de dataBase selectionner*/
            try
            {
                /*ouvrier la connection a database*/
                connection.Open();
                OleDbCommand command = new OleDbCommand
                {
                    Connection = connection,

                    /*query pour supprimer la ligne selectionner*/
                    CommandText = "delete from " + tableDB + " where Libelle='" + lstCmb.SelectedItem + "'"
                };
                command.ExecuteNonQuery();

                /*fermer la connection a database*/
                connection.Close();
                message = new FormMessage("Supprimer avec succès", "Succès", true, FontAwesome.Sharp.IconChar.CheckCircle);
                message.ShowDialog();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connection.Close();
            }
        }
        private void btnDeleteMesure_Click(object sender, EventArgs e)
        {
            if (lstCmb.Items.Count <= 0)
            {
                message = new FormMessage("la liste est vide:: ajouter des items", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }
            if (lstCmb.SelectedIndex == -1)
            {
                message = new FormMessage("s'il vous plaît sélectionner une ligne à la liste", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            /*supprimer la ligne de liste selectionner*/
            lstCmb.Items.RemoveAt(lstCmb.SelectedIndex);

            /*supprimer la ligne de dataBase selectionner*/
            deleteLigneListe(Program.btnAddTypeClick.ToUpper());
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtCmb_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCmb.Text))
                this.AcceptButton = btnSave;
            else
                this.AcceptButton = btnAddLstCmb;
        }

        private void FormAjout_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (check)
            {
                message = new FormMessage("il ya des enregistrement n'est pas sauvegardé, voulez vous sauvegarder ou n", "Question", true, true, FontAwesome.Sharp.IconChar.Question);
                if (DialogResult.Yes == message.ShowDialog())
                {
                    /*suavgarder les donnees dans dataBase*/
                    suavgarderDonnees(Program.btnAddTypeClick.ToUpper());
                }
            }
        }
    }
}
