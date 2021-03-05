using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class frm_ajout : Form
    {
        /*decalaration des variables*/
        private readonly OleDbConnection connection = new OleDbConnection();

        /// <summary>
        /// Pour stocker des articles
        /// </summary>
        private readonly List<string> getLstCmbList = new List<string>();
        f_message message;
        bool check = false;

        /// <summary>
        /// form constructor by default
        /// </summary>
        public frm_ajout()
        {
            connection.ConnectionString = Program.PathType;
            InitializeComponent();
        }

        /// <summary>
        /// save data on database
        /// </summary>
        /// <param name="tableDB"></param>
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
                foreach (string st in lt_type_bois_pvc.Items)
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
                connection.Close();
                LogFile.Message(ex);
            }
        }

        /// <summary>
        /// fill list box
        /// </summary>
        /// <param name="typeBois"></param>
        private void remplirListe(string typeBois)
        {
            lt_type_bois_pvc.Items.Clear();
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand
                {
                    Connection = connection,
                    CommandText = "SELECT Libelle FROM " + typeBois
                };
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lt_type_bois_pvc.Items.Add(reader["Libelle"].ToString());
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
        /// on load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ajout_Load(object sender, EventArgs e)
        {
            t_type_bois_pvc.Focus();
            lblEcris.Text = Text = ("ajouter " + Program.btnAddTypeClick).ToUpper();

            /*remplir liste on affichage*/
            remplirListe(Program.btnAddTypeClick);
        }

        /// <summary>
        /// add to list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLstCmb_Click(object sender, EventArgs e)
        {
            check = true;
            if (String.IsNullOrEmpty(t_type_bois_pvc.Text) || String.IsNullOrWhiteSpace(t_type_bois_pvc.Text))
            {
                message = new f_message("la format du text et incorrect ou text est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_type_bois_pvc.Focus();
                return;
            }

            // check if item already exists
            foreach (String s_type in lt_type_bois_pvc.Items)
            {
                if (t_type_bois_pvc.Text.ToLower() == s_type.ToLower())
                {
                    message = new f_message("Le type saisi existe déjà", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    t_type_bois_pvc.Focus();
                    return;
                }
            }

            lt_type_bois_pvc.Items.Add(t_type_bois_pvc.Text.ToUpper());
            t_type_bois_pvc.Clear();
            t_type_bois_pvc.Focus();
        }

        /// <summary>
        /// buttton for save types on database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            /*suavgarder les donnees dans dataBase*/
            suavgarderDonnees(Program.btnAddTypeClick.ToUpper());

            check = false;
            /*fermer la fonetre*/
            Close();

        }

        /// <summary>
        /// delete selected item
        /// </summary>
        /// <param name="tableDB"></param>
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
                    CommandText = "delete from " + tableDB + " where Libelle='" + lt_type_bois_pvc.SelectedItem + "'"
                };
                command.ExecuteNonQuery();

                /*fermer la connection a database*/
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                LogFile.Message(ex);
            }
        }

        /// <summary>
        /// button delete mesure from datagride mesure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteMesure_Click(object sender, EventArgs e)
        {
            if (lt_type_bois_pvc.Items.Count <= 0)
            {
                message = new f_message("la liste est vide:: ajouter des items", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }
            if (lt_type_bois_pvc.SelectedIndex == -1)
            {
                message = new f_message("s'il vous plaît sélectionner une ligne à la liste", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            /*supprimer la ligne de liste selectionner*/
            lt_type_bois_pvc.Items.RemoveAt(lt_type_bois_pvc.SelectedIndex);

            /*supprimer la ligne de dataBase selectionner*/
            deleteLigneListe(Program.btnAddTypeClick.ToUpper());
        }

        /// <summary>
        /// button close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// on text changed for t_type_bois_pvc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCmb_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(t_type_bois_pvc.Text))
                this.AcceptButton = b_save_list;
            else
                this.AcceptButton = b_add_to_list;
        }

        /// <summary>
        /// on form closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormAjout_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (check)
            {
                message = new f_message("il ya des enregistrement n'est pas sauvegardé, voulez vous sauvegarder ou n", "Question", true, true, FontAwesome.Sharp.IconChar.Question);
                if (DialogResult.Yes == message.ShowDialog())
                {
                    /*suavgarder les donnees dans dataBase*/
                    suavgarderDonnees(Program.btnAddTypeClick.ToUpper());
                }
            }
        }

        /// <summary>
        /// on keydown event for t_type_bois_pvc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void t_type_bois_pvc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                b_add_to_list.PerformClick();
        }
    }
}
