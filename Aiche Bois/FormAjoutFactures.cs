﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class FormAjoutFactures : Form
    {
        /*Declaration des variables*/

        /// <summary>
        /// c'est l'access a extereiur Client data base
        /// </summary>
        private readonly OleDbConnection connectionClient = new OleDbConnection();

        /// <summary>
        /// c'est l'access a extereiur type data base
        /// </summary>
        private readonly OleDbConnection connectionType = new OleDbConnection();

        /// <summary>
        /// c'est la list qui stocker les donnes de Mesure
        /// </summary>
        private readonly List<Mesure> mesures = new List<Mesure>();

        /// <summary>
        /// c'est la list qui stocker les donnes de Pvc
        /// </summary>
        private readonly List<Pvc> pvcs = new List<Pvc>();

        /// <summary>
        /// c'est la list qui stocker les donnes de Facture
        /// </summary>
        private readonly List<Facture> factures = new List<Facture>();

        /// <summary>
        /// c'est la list qui stocker les donnes de Client
        /// </summary>
        private readonly List<Client> clients = new List<Client>();

        /// <summary>
        /// c'est variable determiner si le click on button ajouter ou modifier
        /// </summary>
        private string btnDeterminClick = "";
        private string idClient = "";
        public FormAjoutFactures(String idClient, String btnClick)
        {
            /*
             * difiner la connnection vers la base de donnee
             */
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/factures";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            connectionClient.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + path + "/aicheBois.accdb";
            connectionType.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + path + "/Type.accdb";

            /*
             * prendre id a partir de la form presedent
             */
            this.idClient = idClient;
            btnDeterminClick = btnClick;

            InitializeComponent();
        }

        /// <summary>
        /// method qui vider les text box
        /// </summary>
        private void ViderTxtBox()
        {
            /*vider les objet*/
            txtTypeDeBois.Clear();
            txtQuantite.Clear();
            txtLargeur.Clear();
            txtLongueur.Clear();
            txtEpaisseur.Clear();
            txtTotalMesure.Clear();
            txtPrixTotalMesure.Text = "0.00";
            txtMetrageDeFeuille.Clear();
            cmbNbrCantoPvc.SelectedItem = null;
            cmbTypeDuMetres.SelectedIndex = 1;
            txtTaillePVC.Clear();
            txtPrixMetreLPVC.Clear();
            txtTotaleTaillPVC.Clear();
            txtPrixTotalPVC.Text = "0.00";
            mesures.Clear();
            dtGMesure.Rows.Clear();
            dtGridPvc.Rows.Clear();
            chSeulPVC.Checked = false;
            checkSeulPVC(true);
            lstTypeBois.Focus();
        }

        /// <summary>
        /// method qui calcul data grid view mesure
        /// </summary>
        /// <param name="mesures"></param>
        private void RemplirDataMesure(List<Mesure> mesures)
        {
            double totale = 0;
            foreach (Mesure msr in mesures)
            {
                if (msr.Epaisseur == 0 || double.IsNaN(msr.Epaisseur))
                {
                    totale += ((msr.Largeur * Math.Pow(10, -2)) * (msr.Longueur * Math.Pow(10, -2))) * msr.Quantite;
                }
                else
                {
                    totale += ((msr.Largeur * Math.Pow(10, -2)) * (msr.Longueur * Math.Pow(10, -2)) * (msr.Epaisseur * Math.Pow(10, -2))) * msr.Quantite;
                }
            }
            if (cmbTypeDuMetres.SelectedIndex != 0)
            {
                txtTotalMesure.Text = totale.ToString("F2");
            }
        }

        /// <summary>
        /// remplir comboBox PVC de dataBase
        /// </summary>
        private void RemplirComboBxPvc()
        {
            try
            {
                connectionType.Open();
                OleDbCommand command = new OleDbCommand
                {
                    Connection = connectionType,
                    CommandText = "select * from PVC"
                };
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cmbNbrCantoPvc.Items.Add(reader["Libelle"].ToString());
                }
                connectionType.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// remplir listeBox MDF, LATTE, STD de dataBase
        /// </summary>
        /// <param name="typeBois"></param>
        private void remplirListe(string typeBois)
        {
            lstTypeBois.Items.Clear();
            try
            {
                connectionType.Open();
                OleDbCommand command = new OleDbCommand
                {
                    Connection = connectionType,
                    CommandText = "select * from " + typeBois
                };
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lstTypeBois.Items.Add(reader["Libelle"].ToString());
                }
                connectionType.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// remplir listeBox MDF, LATTE, STD de dataBase
        /// </summary>
        /// <param name="typeBois"></param>
        /// <returns></returns>
        private List<string> remplirRechercheListe(string typeBois)
        {
            List<string> vs = new List<string>();
            lstTypeBois.Items.Clear();
            try
            {
                connectionType.Open();
                OleDbCommand command = new OleDbCommand
                {
                    Connection = connectionType,
                    CommandText = "select * from " + typeBois
                };
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    vs.Add(reader["Libelle"].ToString());
                }
                connectionType.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return vs;
        }

        /// <summary>
        /// initialise l'identificateur de facture
        /// </summary>
        private void idFacture()
        {
            this.Text = "Facture Numéro: " + (factures.Count + 1).ToString("D2");
        }

        /*final des methods*/

        /// <summary>
        /// c'est l'ouvrier du form pour ajouter les donnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjouterFacture_Load(object sender, EventArgs e)
        {
            /*initialise combo box*/
            cmbTypeDuMetres.SelectedIndex = 1;
            cmbTypeDeBois.SelectedIndex = 0;
            dtDateClient.Value = DateTime.Today;
            checkAvance.Checked = false;
            txtPrixAvanceClient.Enabled = false;
            txtPrixAvanceClient.Text = "0.00";
            cmbOrtnPVC.SelectedIndex = 0;

            /*remple comboBox PVC*/
            RemplirComboBxPvc();

            /*
            * affihcer on mode desactiver les champs de pvc
            */
            enabledTxtBxPvc(false);

            if (btnDeterminClick == "edit")
            {
                try
                {
                    connectionClient.Open();
                    String query = "SELECT * FROM CLIENT WHERE IDCLIENT = " + long.Parse(idClient);
                    

                    //client
                    var commandClient = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = query
                    };
                    OleDbDataReader readerClient = commandClient.ExecuteReader();
                    while (readerClient.Read())
                    {
                        dtDateClient.Value = Convert.ToDateTime(readerClient["dateClient"].ToString());
                        txtNomClient.Text = readerClient["nomClient"].ToString();
                        checkAvance.Checked = Convert.ToBoolean(readerClient["chAvance"].ToString());
                        txtPrixAvanceClient.Text = readerClient["prixTotalAvance"].ToString();
                        txtPrixRestClient.Text = readerClient["prixTotalRest"].ToString();
                        txtPrixTotalClient.Text = readerClient["prixTotalClient"].ToString();
                    }

                    //facture
                    query = "SELECT * FROM FACTURE WHERE IDCLIENT = " + long.Parse(idClient);
                    long idFacture = 0;
                    String pvcTest = "";
                    var commandFacture = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = query
                    };
                    OleDbDataReader readerFacture = commandFacture.ExecuteReader();
                    while (readerFacture.Read())
                    {
                        idFacture = long.Parse(readerFacture["IDFACTURE"].ToString());

                        Facture facture = new Facture();
                        
                        facture.TypeDeBois = readerFacture["typeDeBois"].ToString();
                        facture.Categorie = readerFacture["categorie"].ToString();
                        facture.Metrage = readerFacture["metrage"].ToString();
                        facture.PrixMetres = double.Parse(readerFacture["prixMetres"].ToString());
                        facture.TotalMesure = double.Parse(readerFacture["totalMesure"].ToString());
                        facture.PrixTotalMesure = double.Parse(readerFacture["prixTotalMesure"].ToString());

                        if (!string.IsNullOrEmpty(cmbNbrCantoPvc.Text))
                        {
                            facture.TypePVC = cmbNbrCantoPvc.Text;
                            facture.CheckPVC = chSeulPVC.Checked;
                            facture.TailleCanto = txtTaillePVC.Text;
                            facture.PrixMitresLinear = double.Parse(txtPrixMetreLPVC.Text);
                            facture.TotalTaillPVC = double.Parse(txtTotaleTaillPVC.Text);
                            facture.PrixTotalPVC = double.Parse(txtPrixTotalPVC.Text);
                        }
                        else
                        {
                            facture.TypePVC = "---";
                            facture.CheckPVC = chSeulPVC.Checked;
                            facture.TailleCanto = "0.0";
                            facture.PrixMitresLinear = 0.0;
                            facture.TotalTaillPVC = 0.0;
                            facture.PrixTotalPVC = 0.0;
                        }

                        pvcTest = readerFacture["typePVC"].ToString();
                        if (pvcTest != "---")
                        {
                            cmbNbrCantoPvc.Text = pvcTest;
                            chSeulPVC.Checked = bool.Parse(readerFacture["checkPVC"].ToString());
                            txtTotaleTaillPVC.Text = readerFacture["totalTaillPVC"].ToString();
                            txtTaillePVC.Text = readerFacture["tailleCanto"].ToString();
                            txtPrixMetreLPVC.Text = readerFacture["prixMitresLinear"].ToString();
                            txtPrixTotalPVC.Text = readerFacture["prixTotalPVC"].ToString();

                            //Pvc
                            query = "SELECT * FROM PVC WHERE IDFACTURE = " + idFacture;
                            var commandPVC = new OleDbCommand
                            {
                                Connection = connectionClient,
                                CommandText = query
                            };
                            OleDbDataReader readerPVC = commandPVC.ExecuteReader();
                            while (readerPVC.Read())
                            {
                                Pvc pvc = new Pvc();
                                pvc.Qte = Convert.ToDouble(readerPVC["quantite"].ToString());
                                pvc.Largr = Convert.ToDouble(readerPVC["largeur"].ToString());
                                pvc.Longr = Convert.ToDouble(readerPVC["longueur"].ToString());
                                pvc.Ortn = readerPVC["orientation"].ToString();
                            }
                        }

                        if (cmbTypeDuMetres.SelectedIndex != 2)
                        {
                            for (int i = 0; i < dtGMesure.Rows.Count; i++)
                            {
                                mesures.Add(new Mesure(Convert.ToDouble(dtGMesure.Rows[i].Cells[0].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[1].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value), cmbTypeDuMetres.Text));
                            }
                        }
                        else
                        {
                            mesures.Clear();
                            for (int i = 0; i < dtGMesure.Rows.Count; i++)
                            {
                                mesures.Add(new Mesure(Convert.ToDouble(dtGMesure.Rows[i].Cells[0].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[1].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value), cmbTypeDuMetres.Text));
                            }
                        }

                        facture.Mesures = mesures;
                        facture.Pvcs = pvcs;
                        factures.Add(facture);
                    }

                    //Mesure
                    query = "SELECT * FROM MESURE WHERE IDFACTURE = " + idFacture;
                    var commandMesure = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = query
                    };
                    OleDbDataReader readerMesure = commandMesure.ExecuteReader();
                    while (readerMesure.Read())
                    {
                        if (readerMesure["type"].ToString() == "m3")
                        {
                            cmbTypeDuMetres.SelectedIndex = 2;
                            dtGMesure.Rows.Add(readerMesure["quantite"].ToString(),
                                           readerMesure["largeur"].ToString(),
                                           readerMesure["longueur"].ToString(),
                                           readerMesure["eppaiseur"].ToString());
                        }
                        else
                        {
                            if (readerMesure["type"].ToString() == "feuille")
                                cmbTypeDuMetres.SelectedIndex = 0;
                            else
                                cmbTypeDuMetres.SelectedIndex = 1;

                            dtGMesure.Rows.Add(readerMesure["quantite"].ToString(),
                                               readerMesure["largeur"].ToString(),
                                               readerMesure["longueur"].ToString());
                        }
                    }
                    connectionClient.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                /*initialise IdFacture, IdClient*/
                idFacture();
            }
        }

        /// <summary>
        /// c'est la selection de type du bois changer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCateogorie_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.btnAddTypeClick = cmbTypeDeBois.Text;
            remplirListe(cmbTypeDeBois.Text);
            /*remplirLstTypeBois();*/
            if (lstTypeBois.Items.Count <= 0)
            {
                txtTypeDeBois.Clear();
                return;
            }
            lstTypeBois.SelectedIndex = 0;
            txtTypeDeBois.Text = lstTypeBois.SelectedItem.ToString();
        }

        /// <summary>
        /// c'est la selection de liste des types du bois dans la zone du text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstTypeBois_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTypeBois.Items.Count <= 0 || lstTypeBois.SelectedIndex == -1)
            {
                txtTypeDeBois.Clear();
                return;
            }

            txtTypeDeBois.Text = lstTypeBois.SelectedItem.ToString();
        }

        /// <summary>
        /// c'est le button qui ouvrier la form pour ajouter nouveux type du PVC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btncmbNbrCanto_Click(object sender, EventArgs e)
        {
            Program.btnAddTypeClick = "PVC";

            /*ouvrier de fenétre*/
            FormAjout ajout = new FormAjout();
            ajout.ShowDialog();

            /*vider le comboBox PVC*/
            cmbNbrCantoPvc.Items.Clear();

            /*remplir comboBox par les donneesde dataBase*/
            RemplirComboBxPvc();
        }

        /// <summary>
        /// c'est le button qui ouvrier la form pour ajouter nouveux type du bois
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCmbCategorie_Click(object sender, EventArgs e)
        {
            Program.btnAddTypeClick = cmbTypeDeBois.Text.ToUpper();

            FormAjout ajout = new FormAjout();
            ajout.ShowDialog();

            /*remplirLstTypeBois();*/
            remplirListe(Program.btnAddTypeClick);
        }

        /// <summary>
        /// c'est champs qui chercher dans la listes des bois
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            /*si le textBox est vide, remlir tous les items*/
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                foreach (string st1 in remplirRechercheListe(cmbTypeDeBois.Text))
                {
                    lstTypeBois.Items.Add(st1);
                }
            }
            else
            {
                List<string> vs1 = new List<string>();
                foreach (string st2 in remplirRechercheListe(cmbTypeDeBois.Text))
                {
                    if (st2.Contains(txtSearch.Text.ToUpper()))
                    {
                        vs1.Add(st2);
                    }
                }

                lstTypeBois.Items.Clear();
                foreach (string st3 in vs1)
                {
                    lstTypeBois.Items.Add(st3);
                }
            }
            /*selectionner le premiere ligne*/
            if (lstTypeBois.Items.Count != 0)
            {
                lstTypeBois.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// c'est le button qui returner a la fomr presedent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// c'est button qui vider les champs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ViderTxtBox();
        }

        /// <summary>
        /// this method return true or false for Empty textBox
        /// </summary>
        /// <returns></returns>
        private bool checkIsNullOrEmpty()
        {
            if (string.IsNullOrEmpty(txtNomClient.Text))
            {
                MessageBox.Show(txtNomClient.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomClient.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtTypeDeBois.Text))
            {
                MessageBox.Show(txtTypeDeBois.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lstTypeBois.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPrixMetreMesure.Text))
            {
                MessageBox.Show(txtPrixMetreMesure.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrixMetreMesure.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtTotalMesure.Text))
            {
                MessageBox.Show(txtTotalMesure.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTotalMesure.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPrixTotalMesure.Text))
            {
                MessageBox.Show(txtPrixTotalMesure.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrixTotalMesure.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtMetrageDeFeuille.Text))
            {
                MessageBox.Show(txtMetrageDeFeuille.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMetrageDeFeuille.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCategorie.Text))
            {
                MessageBox.Show(txtCategorie.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategorie.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPrixTotalClient.Text))
            {
                MessageBox.Show(txtPrixTotalClient.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrixTotalClient.Focus();
                return false;
            }
            if (checkAvance.Checked)
                if (string.IsNullOrEmpty(txtPrixAvanceClient.Text))
                {
                    MessageBox.Show(txtPrixAvanceClient.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrixAvanceClient.Focus();
                    return false;
                }
            if (!string.IsNullOrEmpty(cmbNbrCantoPvc.Text))
            {
                if (string.IsNullOrEmpty(txtTaillePVC.Text))
                {
                    MessageBox.Show(txtTaillePVC.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTaillePVC.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(txtPrixMetreLPVC.Text))
                {
                    MessageBox.Show(txtPrixMetreLPVC.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrixMetreLPVC.Focus();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// c'est method key press pour verifier si l'autilisateur saiser seulment les nombres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuantite_KeyPress(object sender, KeyPressEventArgs e)
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
        /// c'est activer ou desactiver le montant avance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAvance_CheckedChanged(object sender, EventArgs e)
        {
            txtPrixAvanceClient.Enabled = checkAvance.Checked;
            txtPrixAvanceClient.Text = "0.00";
        }

        /// <summary>
        /// c'est la selection pour choisier le type de bois
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTypeDuMetres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTypeDuMetres.SelectedIndex == 0)
            {
                dtGMesure.Rows.Clear();
                mesures.Clear();
                //txtTotalMesure.Clear();
                lblTypeDuMetres.Text = "Prix ​​au feuille";
                lblMesure.Text = "Nombre de Feuilles";
                lblEpaisseur.Visible = false;
                txtEpaisseur.Visible = false;
                if (dtGMesure.ColumnCount > 3)
                {
                    dtGMesure.ColumnCount -= 1;
                }

                txtQuantite.Focus();
                txtTotalMesure.Enabled = true;
            }
            else if (cmbTypeDuMetres.SelectedIndex == 1)
            {
                dtGMesure.Rows.Clear();
                mesures.Clear();
                txtTotalMesure.Clear();
                lblTypeDuMetres.Text = "Prix ​​au mètre carré";
                lblMesure.Text = "Volume Total de la Mesure de mètre carré";
                lblEpaisseur.Visible = false;
                txtEpaisseur.Visible = false;
                if (dtGMesure.ColumnCount > 3)
                {
                    dtGMesure.ColumnCount -= 1;
                }

                txtQuantite.Focus();
                txtTotalMesure.Enabled = false;
            }
            else
            {
                dtGMesure.Rows.Clear();
                mesures.Clear();
                txtTotalMesure.Clear();
                lblTypeDuMetres.Text = "Prix ​​au mètre cube";
                lblMesure.Text = "Volume Total de la Mesure de mètre cube";
                lblEpaisseur.Visible = true;
                txtEpaisseur.Visible = true;
                if (dtGMesure.ColumnCount == 3)
                {
                    dtGMesure.ColumnCount += 1;
                    dtGMesure.Columns[3].HeaderText = "Epssr";
                }
                txtQuantite.Focus();
                txtTotalMesure.Enabled = false;
            }
        }

        /// <summary>
        /// c'est le button pour ajouter les donness dans datagrid mesure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddMesure_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtQuantite.Text))
            {
                MessageBox.Show("saisir la valeur de " + txtQuantite.Tag, "attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantite.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtLargeur.Text))
            {
                MessageBox.Show("saisir la valeur de " + txtLargeur.Tag, "attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLargeur.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtLongueur.Text))
            {
                MessageBox.Show("saisir la valeur de " + txtLongueur.Tag, "attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLongueur.Focus();
                return;
            }

            foreach (Mesure msr1 in mesures)
            {
                if (cmbTypeDuMetres.SelectedIndex == 2)
                {
                    if (double.Parse(txtLongueur.Text) == msr1.Longueur && double.Parse(txtLargeur.Text) == msr1.Largeur && double.Parse(txtQuantite.Text) == msr1.Quantite && double.Parse(txtEpaisseur.Text) == msr1.Epaisseur)
                    {
                        MessageBox.Show("c'est mesure exist deja");
                        return;
                    }
                }
                else if (double.Parse(txtLongueur.Text) == msr1.Longueur && double.Parse(txtLargeur.Text) == msr1.Largeur && double.Parse(txtQuantite.Text) == msr1.Quantite)
                {
                    MessageBox.Show("c'est mesure exist deja");
                    return;
                }
            }

            if (cmbTypeDuMetres.SelectedIndex == 2)
            {
                if (string.IsNullOrEmpty(txtEpaisseur.Text))
                {
                    MessageBox.Show("saisir la valeur de " + txtEpaisseur.Tag, "attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEpaisseur.Focus();
                    return;
                }
                Mesure mesure = new Mesure
                {
                    Quantite = double.Parse(txtQuantite.Text),
                    Largeur = double.Parse(txtLargeur.Text),
                    Longueur = double.Parse(txtLongueur.Text),
                    Epaisseur = double.Parse(txtEpaisseur.Text),
                    Type = cmbTypeDuMetres.Text
                };
                mesures.Add(mesure);
                dtGMesure.Rows.Add(mesure.Quantite, mesure.Largeur, mesure.Longueur, mesure.Epaisseur);
                txtEpaisseur.Clear();
            }
            else
            {
                Mesure mesure = new Mesure
                {
                    Quantite = double.Parse(txtQuantite.Text),
                    Largeur = double.Parse(txtLargeur.Text),
                    Longueur = double.Parse(txtLongueur.Text),
                    Type = cmbTypeDuMetres.Text
                };
                mesures.Add(mesure);
                dtGMesure.Rows.Add(mesure.Quantite, mesure.Largeur, mesure.Longueur);
            }
            RemplirDataMesure(mesures);
            txtQuantite.Clear();
            txtLargeur.Clear();
            txtLongueur.Clear();
            txtQuantite.Focus();
        }

        /// <summary>
        /// c'est le button qui modifier les lignes selectionner a data grid mesure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditMesure_Click(object sender, EventArgs e)
        {
            if (dtGMesure.SelectedRows.Count <= 0)
            {
                MessageBox.Show("selectionnes une ligne pour modifier");
                return;
            }

            if (string.IsNullOrEmpty(txtQuantite.Text) || string.IsNullOrEmpty(txtLargeur.Text) || string.IsNullOrEmpty(txtLongueur.Text))
            {
                MessageBox.Show("saiser les valeur de mesure");
                return;
            }

            int indx = dtGMesure.CurrentRow.Index;

            if (cmbTypeDuMetres.SelectedIndex == 2)
            {
                if (string.IsNullOrEmpty(txtEpaisseur.Text))
                {
                    MessageBox.Show("saiser les valeur de mesure");
                    txtEpaisseur.Focus();
                    return;
                }
                mesures[indx].Quantite = double.Parse(txtQuantite.Text);
                mesures[indx].Largeur = double.Parse(txtLargeur.Text);
                mesures[indx].Longueur = double.Parse(txtLongueur.Text);
                mesures[indx].Epaisseur = double.Parse(txtEpaisseur.Text);
                mesures[indx].Type = cmbTypeDuMetres.Text;
                dtGMesure.Rows.Clear();
                foreach (Mesure msr in mesures)
                {
                    dtGMesure.Rows.Add(msr.Quantite, msr.Largeur, msr.Longueur, msr.Epaisseur);
                }
            }
            else
            {
                mesures[indx].Quantite = double.Parse(txtQuantite.Text);
                mesures[indx].Largeur = double.Parse(txtLargeur.Text);
                mesures[indx].Longueur = double.Parse(txtLongueur.Text);
                mesures[indx].Type = cmbTypeDuMetres.Text;
                dtGMesure.Rows.Clear();
                foreach (Mesure msr in mesures)
                {
                    dtGMesure.Rows.Add(msr.Quantite, msr.Largeur, msr.Longueur);
                }
            }
            RemplirDataMesure(mesures);
            MessageBox.Show("modifier avec succes", "Modifier", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// c'est button qui supprimer la selection de ligne a data grid mesure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteMesure_Click(object sender, EventArgs e)
        {
            if (dtGMesure.SelectedRows.Count <= 0)
            {
                MessageBox.Show("selectionnes une ligne pour supprimer");
                return;
            }

            int indx = dtGMesure.CurrentRow.Index;
            for (int i = 0; i < mesures.Count; i++)
            {
                if (mesures[indx] == mesures[i])
                {
                    mesures.RemoveAt(i);
                }
            }

            dtGMesure.Rows.Clear();
            foreach (Mesure msr in mesures)
            {
                if (cmbTypeDuMetres.SelectedIndex == 2)
                {
                    dtGMesure.Rows.Add(msr.Quantite, msr.Largeur, msr.Longueur, msr.Epaisseur);
                }
                else
                {
                    dtGMesure.Rows.Add(msr.Quantite, msr.Largeur, msr.Longueur);
                }
            }
            RemplirDataMesure(mesures);
            MessageBox.Show("supprimer avec succes", "Supprimer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// c'est la selection de ligne a data grid mesure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGMesure_SelectionChanged(object sender, EventArgs e)
        {
            if (dtGMesure.SelectedRows.Count < 0)
            {
                MessageBox.Show("la liste est vide");
                return;
            }

            try
            {
                if (dtGMesure.RowCount != 0)
                {
                    int indx = dtGMesure.CurrentRow.Index;
                    if (cmbTypeDuMetres.SelectedIndex == 2)
                    {
                        txtQuantite.Text = dtGMesure.Rows[indx].Cells[0].Value.ToString();
                        txtLargeur.Text = dtGMesure.Rows[indx].Cells[1].Value.ToString();
                        txtLongueur.Text = dtGMesure.Rows[indx].Cells[2].Value.ToString();
                        txtEpaisseur.Text = dtGMesure.Rows[indx].Cells[3].Value.ToString();
                    }
                    else
                    {
                        txtQuantite.Text = dtGMesure.Rows[indx].Cells[0].Value.ToString();
                        txtLargeur.Text = dtGMesure.Rows[indx].Cells[1].Value.ToString();
                        txtLongueur.Text = dtGMesure.Rows[indx].Cells[2].Value.ToString();
                    }
                }
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// c'est le button de click pour ajouter des text box et datagrids a les listes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (checkIsNullOrEmpty())
            {

                Facture facture = new Facture();
                List<Mesure> vs = new List<Mesure>();
                if (cmbTypeDuMetres.SelectedIndex != 2)
                {
                    for (int i = 0; i < dtGMesure.Rows.Count; i++)
                    {
                        vs.Add(new Mesure(Convert.ToDouble(dtGMesure.Rows[i].Cells[0].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[1].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value), cmbTypeDuMetres.Text));
                    }
                }
                else
                {
                    vs.Clear();
                    for (int i = 0; i < dtGMesure.Rows.Count; i++)
                    {
                        vs.Add(new Mesure(Convert.ToDouble(dtGMesure.Rows[i].Cells[0].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[1].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value), cmbTypeDuMetres.Text));
                    }
                }

                facture.Mesures = vs;
                facture.Pvcs = pvcs;
                facture.TypeDeBois = txtTypeDeBois.Text;
                facture.Categorie = txtCategorie.Text;
                facture.Metrage = txtMetrageDeFeuille.Text;
                facture.PrixMetres = double.Parse(txtPrixMetreMesure.Text);
                facture.TotalMesure = double.Parse(txtTotalMesure.Text);
                facture.PrixTotalMesure = double.Parse(txtPrixTotalMesure.Text);

                if (!string.IsNullOrEmpty(cmbNbrCantoPvc.Text))
                {
                    facture.TypePVC = cmbNbrCantoPvc.Text;
                    facture.CheckPVC = chSeulPVC.Checked;
                    facture.TailleCanto = txtTaillePVC.Text;
                    facture.PrixMitresLinear = double.Parse(txtPrixMetreLPVC.Text);
                    facture.TotalTaillPVC = double.Parse(txtTotaleTaillPVC.Text);
                    facture.PrixTotalPVC = double.Parse(txtPrixTotalPVC.Text);
                }
                else
                {
                    facture.TypePVC = "---";
                    facture.CheckPVC = chSeulPVC.Checked;
                    facture.TailleCanto = "0.0";
                    facture.PrixMitresLinear = 0.0;
                    facture.TotalTaillPVC = 0.0;
                    facture.PrixTotalPVC = 0.0;
                }

                factures.Add(facture);
                btnDeterminClick = "add";

                MessageBox.Show("facture ajouter avec succes", "Ajouter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                idFacture();

                if (!String.IsNullOrEmpty(txtPrixTotalPVC.Text))
                    prix_total_client += double.Parse(txtPrixTotalMesure.Text) + double.Parse(txtPrixTotalPVC.Text);
                else
                    prix_total_client += double.Parse(txtPrixTotalMesure.Text);

                txtPrixTotalClient.Text = prix_total_client.ToString("F2");

                if (double.Parse(txtPrixTotalClient.Text) == double.Parse(txtPrixRestClient.Text))
                    txtPrixRestClient.Text = "0.00";
                ViderTxtBox();
            }
        }

        /// <summary>
        /// c'est party de remplir est definer PVC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        /// <summary>
        /// c'est method qui activer ou desactiver PVC
        /// </summary>
        /// <param name="b"></param>
        private void enabledTxtBxPvc(bool b)
        {
            dtGridPvc.Enabled = b;
            btnImportPvc.Enabled = b;

            chSeulPVC.Enabled = b;

            btnDeleteSeulPVC.Enabled = b;
            txtTotaleTaillPVC.Enabled = false;
            txtTaillePVC.Enabled = b;
            txtPrixMetreLPVC.Enabled = b;
            txtPrixTotalPVC.Enabled = false;

            txtQtePVC.Enabled = false;
            txtLargPVC.Enabled = false;
            txtLongPVC.Enabled = false;
            cmbOrtnPVC.Enabled = false;

            btnDeleteSeulPVC.Enabled = false;
            btnAddSeulPVC.Enabled = false;
        }

        /// <summary>
        /// c'est method pour activer ou desactiver les champs
        /// </summary>
        /// <param name="ft"></param>
        private void checkSeulPVC(bool ft)
        {
            txtPrixMetreMesure.Enabled = ft;
            txtMetrageDeFeuille.Enabled = ft;
            txtCategorie.Enabled = ft;
            cmbTypeDuMetres.Enabled = ft;
            btnCmbCategorie.Enabled = ft;
            cmbTypeDeBois.Enabled = ft;
            txtSearch.Enabled = ft;
            lstTypeBois.Enabled = ft;

            btnImportPvc.Enabled = ft;
            btnSavePvc.Enabled = ft;

            btnAddSeulPVC.Enabled = !ft;
            btnDeleteSeulPVC.Enabled = !ft;

            txtQtePVC.Enabled = !ft;
            txtLargPVC.Enabled = !ft;
            txtLongPVC.Enabled = !ft;
            cmbOrtnPVC.Enabled = !ft;

            txtQuantite.Enabled = ft;
            txtLargeur.Enabled = ft;
            txtLongueur.Enabled = ft;
            txtEpaisseur.Enabled = ft;
            btnAddMesure.Enabled = ft;
            btnDeleteMesure.Enabled = ft;
        }

        /// <summary>
        /// quand le check sure le button check seul PVC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chSeulPVC_CheckedChanged(object sender, EventArgs e)
        {
            if (cmbNbrCantoPvc.SelectedIndex > 0)
            {
                if (chSeulPVC.Checked)
                {
                    checkSeulPVC(false);
                    txtTypeDeBois.Text = "---";
                    txtPrixMetreMesure.Text = "0.00";
                    txtTotalMesure.Text = "0.00";
                    txtPrixTotalMesure.Text = "0.00";
                    txtMetrageDeFeuille.Text = "---";
                    txtCategorie.Text = "---";
                }
                else
                {
                    checkSeulPVC(true);

                    dtGMesure.Rows.Clear();
                    mesures.Clear();
                    dtGridPvc.Rows.Clear();
                    txtTypeDeBois.Clear();
                    txtPrixMetreMesure.Clear();
                    txtTotalMesure.Clear();
                    txtPrixTotalMesure.Text = "0.00";
                    txtMetrageDeFeuille.Clear();
                    txtCategorie.Clear();
                }
            }
        }

        /// <summary>
        /// button ajouter pvc si le checkbox true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSeulPVC_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtQtePVC.Text))
            {
                MessageBox.Show(txtQtePVC.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQtePVC.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtLargPVC.Text))
            {
                MessageBox.Show(txtLargPVC.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLargPVC.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtLongPVC.Text))
            {
                MessageBox.Show(txtLongPVC.Tag + " est vide", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLongPVC.Focus();
                return;
            }

            for (int i = 0; i < dtGridPvc.Rows.Count; i++)
            {
                if (dtGridPvc.Rows[i].Cells[0].Value.ToString() == txtQtePVC.Text &&
                    dtGridPvc.Rows[i].Cells[1].Value.ToString() == txtLargPVC.Text &&
                    dtGridPvc.Rows[i].Cells[2].Value.ToString() == txtLongPVC.Text &&
                    dtGridPvc.Rows[i].Cells[3].Value.ToString() == cmbOrtnPVC.Text)
                {
                    MessageBox.Show("les valeur est le meme deja saisir", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQtePVC.Focus();
                    return;
                }
            }

            dtGridPvc.Rows.Add(txtQtePVC.Text, txtLargPVC.Text, txtLongPVC.Text, cmbOrtnPVC.Text);
            btnSaveCalculPvc();

            txtQtePVC.Clear();
            txtLargPVC.Clear();
            txtLongPVC.Clear();
            txtQtePVC.Focus();
        }

        /// <summary>
        /// quand la selection de combobox pour choisizer type PVC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbNbrCantoPvc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNbrCantoPvc.SelectedIndex < 0)
            {
                enabledTxtBxPvc(false);
            }
            else
            {
                enabledTxtBxPvc(true);
            }
        }

        /// <summary>
        /// c'est le button qui ajouter les valeur de qte, largeur et longueur
        /// a le data grid view pvc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportPvc_Click(object sender, EventArgs e)
        {
            dtGridPvc.Rows.Clear();
            for (int i = 0; i < dtGMesure.Rows.Count; i++)
            {
                dtGridPvc.Rows.Add(
                    dtGMesure.Rows[i].Cells[0].Value,
                    dtGMesure.Rows[i].Cells[1].Value,
                    dtGMesure.Rows[i].Cells[2].Value);
            }
        }

        /// <summary>
        /// le traitmenet pour calculer les donnees du datagrid pvc
        /// </summary>
        public void btnSaveCalculPvc()
        {
            if (dtGridPvc.Rows.Count == 0)
            {
                MessageBox.Show("Importer les valeurs des mesures", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            pvcs.Clear();
            double total = 0;
            for (int i = 0; i < dtGridPvc.Rows.Count; i++)
            {
                Pvc pvc = new Pvc();
                String ss = (dtGridPvc.Rows[i].Cells[3] as DataGridViewComboBoxCell).FormattedValue.ToString();
                if (String.IsNullOrEmpty(ss))
                    ss = "0";
                double qt = double.Parse(dtGridPvc.Rows[i].Cells[0].Value.ToString());
                double lar = double.Parse(dtGridPvc.Rows[i].Cells[1].Value.ToString());
                double lon = double.Parse(dtGridPvc.Rows[i].Cells[2].Value.ToString());

                /*
                 * horizontal
                 * largeur
                 * 
                 * vertical
                 * longueur
                 */
                switch (ss)
                {
                    /*horizontal = 0, vertical = 0*/
                    case "0":
                        total += (lar / 100) * 0 * qt + (lon / 100) * 0 * qt;
                        break;
                    /*horizontal = 1, vertical = 0*/
                    case "h*1":
                        total += (lar / 100) * 1 * qt + (lon / 100) * 0 * qt;
                        break;
                    /*horizontal = 0, vertical = 1*/
                    case "v*1":
                        total += (lar / 100) * 0 * qt + (lon / 100) * 1 * qt;
                        break;
                    /*horizontal = 2, vertical = 0*/
                    case "h*2":
                        total += (lar / 100) * 2 * qt + (lon / 100) * 0 * qt;
                        break;
                    /*horizontal = 0, vertical = 2*/
                    case "v*2":
                        total += (lar / 100) * 0 * qt + (lon / 100) * 2 * qt;
                        break;
                    /*horizontal = 2, vertical = 0*/
                    case "4":
                        total += (lar / 100) * 2 * qt + (lon / 100) * 2 * qt;
                        break;
                }

                pvc.Qte = qt;
                pvc.Longr = lar;
                pvc.Largr = lon;
                pvc.Ortn = ss;

                /*ajouter a la liste des Pvcs*/
                pvcs.Add(pvc);
            }
            txtTotaleTaillPVC.Text = total.ToString("F2");
            total = 0;
        }

        /// <summary>
        /// c'est le button pour calculer les donnees du datagrid pvc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPvc_Click(object sender, EventArgs e)
        {
            btnSaveCalculPvc();
        }

        /// <summary>
        /// c'est le button pour supprimer la ligne selectionnes du data grid pvc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteSeulPVC_Click(object sender, EventArgs e)
        {
            if (dtGridPvc.SelectedRows.Count <= 0)
            {
                MessageBox.Show("selectionnes une ligne pour supprimer");
                return;
            }

            dtGridPvc.Rows.RemoveAt(dtGridPvc.CurrentRow.Index);
            btnSaveCalculPvc();
            if (dtGridPvc.Rows.Count == 0)
            {
                txtTotaleTaillPVC.Text = "0.00";
            }
        }

        /// <summary>
        /// quand la forme fermer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormAjoutFactures_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnDeterminClick == "add")
            {
                /*clients.Add(new Client(txtNomClient.Text, dtDateClient.Value, checkAvance.Checked, factures.Count, double.Parse(txtMontantTotal.Text), double.Parse(txtAvance.Text), double.Parse(txtRestMontant.Text), factures));*/
                try
                {
                    connectionClient.Open();
                    int idCLIENT = 0;
                    int idFACTURE = 0;
                    OleDbCommand commandClient = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = "insert into client (nomClient, dateClient, nbFacture, chAvance, prixTotalAvance, prixTotalRest, prixTotalClient) values('" + txtNomClient.Text + "','" + dtDateClient.Value + "','" + factures.Count + "'," + checkAvance.Checked.ToString() + ",'" + Convert.ToDouble(txtPrixAvanceClient.Text) + "','" + Convert.ToDouble(txtPrixRestClient.Text) + "','" + Convert.ToDouble(txtPrixTotalClient.Text) + "')"
                    };
                    commandClient.ExecuteNonQuery();

                    /*get idClient from database*/
                    OleDbCommand commandIdClient = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = "select top 1 idClient from client order by idClient desc"
                    };
                    OleDbDataReader readerIdClient = commandIdClient.ExecuteReader();
                    while (readerIdClient.Read())
                    {
                        idCLIENT = Convert.ToInt32(readerIdClient["idClient"]);
                    }

                    /*facture*/
                    foreach (Facture fct in factures)
                    {
                        OleDbCommand commandFacture = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "insert into facture (idClient, typeDeBois, metrage, categorie, totalMesure, prixMetres, typePVC, checkPVC, tailleCanto, totalTaillPVC, prixMitresLinear, prixTotalPVC, prixTotalMesure)" +
                                          " values ('" + (idCLIENT) + "', '" + fct.TypeDeBois + "', '" + fct.Metrage + "','" + fct.Categorie + "', '" + fct.TotalMesure + "', '" + fct.PrixMetres + "', '" + fct.TypePVC + "', " + fct.CheckPVC.ToString() + ",'" + fct.TailleCanto + "', '"
                                          + fct.TotalTaillPVC + "', '" + fct.PrixMitresLinear + "', '" + fct.PrixTotalPVC + "', '" + fct.PrixTotalMesure + "')"
                        };
                        commandFacture.ExecuteNonQuery();

                        /*get idClient from database*/
                        OleDbCommand commandIdFacture = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "select top 1 idFacture from facture order by idFacture desc"
                        };
                        OleDbDataReader readerIdFacture = commandIdFacture.ExecuteReader();
                        while (readerIdFacture.Read())
                        {
                            idFACTURE = Convert.ToInt32(readerIdFacture["idFacture"]);
                        }
                        foreach (Mesure msr in fct.Mesures)
                        {
                            OleDbCommand commandMesure = new OleDbCommand
                            {
                                Connection = connectionClient,
                                CommandText = "insert into mesure (idFacture, quantite, largeur, longueur, eppaiseur, type) values('" + (idFACTURE) + "','" + msr.Quantite + "','" + msr.Largeur + "','" + msr.Longueur + "','" + msr.Epaisseur + "','" + msr.Type + "')"
                            };
                            commandMesure.ExecuteNonQuery();
                        }

                        foreach (Pvc pvc in fct.Pvcs)
                        {
                            OleDbCommand commandMesure = new OleDbCommand
                            {
                                Connection = connectionClient,
                                CommandText = "insert into pvc (idFacture, quantite, largeur, longueur, orientation) values('" + (idFACTURE) + "','" + pvc.Qte + "','" + pvc.Largr + "','" + pvc.Longr + "','" + pvc.Ortn + "')"
                            };
                            commandMesure.ExecuteNonQuery();
                        }
                    }
                    connectionClient.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur:: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        /// <summary>
        /// quand ecrire sure le text box txtPrixMetreMesure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private double prix_total_client = 0;
        private void txtPrixMetreMesure_TextChanged(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(txtPrixMetreMesure.Text) || String.IsNullOrEmpty(txtTotalMesure.Text))
                return;

            txtPrixTotalMesure.Text = (double.Parse(txtPrixMetreMesure.Text) * double.Parse(txtTotalMesure.Text)).ToString("F2");

        }

        /// <summary>
        /// quand ecrire sure le text box txtPrixMetreLPVC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrixMetreLPVC_TextChanged(object sender, EventArgs e)
        {
            double prix_mettres_pvc = 0;
            if (String.IsNullOrEmpty(txtPrixMetreLPVC.Text) || String.IsNullOrEmpty(txtTotaleTaillPVC.Text))
                return;
            prix_mettres_pvc = double.Parse(txtPrixMetreLPVC.Text) * double.Parse(txtTotaleTaillPVC.Text);
            txtPrixTotalPVC.Text = prix_mettres_pvc.ToString("F2");
        }

        /// <summary>
        /// quand ecrire sure le text box txtPrixRestClient
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrixRestClient_TextChanged(object sender, EventArgs e)
        {
            double total = 0;
            double avance = 0;

            if (String.IsNullOrEmpty(txtPrixTotalClient.Text) || String.IsNullOrEmpty(txtPrixAvanceClient.Text))
                return;

            total = double.Parse(txtPrixTotalClient.Text);

            avance = double.Parse(txtPrixAvanceClient.Text);


            if (avance >= total)
            {
                MessageBox.Show("le prix d'avance est grand a prix total", "Attension", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrixAvanceClient.Text = "0.00";
                txtPrixRestClient.Text = "0.00";
                checkAvance.Checked = false;
            }
            else
                txtPrixRestClient.Text = (total - avance).ToString("F2");

        }

        /// <summary>
        /// c'est le button pour sauvegarder les donnes qui ajouter dans le formule quand femiture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveFacture_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
