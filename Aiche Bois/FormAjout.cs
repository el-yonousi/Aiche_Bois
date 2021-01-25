using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class FormAjout : Form
    {
        /*decalaration des variables*/
        private readonly OleDbConnection connection = new OleDbConnection();
        private readonly List<string> getLstCmbList = new List<string>();
        public FormAjout()
        {
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/factures";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            connection.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = Type.accdb";
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
                MessageBox.Show("succes", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erreur:: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (String.IsNullOrEmpty(txtCmb.Text) || String.IsNullOrWhiteSpace(txtCmb.Text))
            {
                MessageBox.Show("le format du text et incorrect ou text est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("supprimer avec succes", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDeleteMesure_Click(object sender, EventArgs e)
        {
            if (lstCmb.Items.Count <= 0)
            {
                MessageBox.Show("la liste est vide:: ajouter des items");
                return;
            }
            if (lstCmb.SelectedIndex == -1)
            {
                MessageBox.Show("s'il vous plait selection une ligne a la litse");
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
    }
}
