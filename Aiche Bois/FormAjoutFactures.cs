using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class FormAjoutFactures : Form
    {
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
        private String btnDeterminClick = "";
        private String idClient = "";

        /// <summary>
        /// this instance from form message to show message error
        /// </summary>
        private FormMessage message;

        public FormAjoutFactures(String idClient, String btnClick)
        {
            /*
             * difiner la connnection vers la base de donnee
             */
            //String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/factures";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            connectionClient.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = aicheBois.accdb";
            connectionType.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = Type.accdb";

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
            cmbTypePvc.SelectedItem = null;
            cmbTypeDuMetres.SelectedIndex = 1;
            txtTaillePVC.Clear();
            txtPrixMetreLPVC.Clear();
            txtTotaleTaillPVC.Clear();
            txtPrixTotalPVC.Text = "0.00";
            mesures.Clear();
            pvcs.Clear();
            dtGMesure.Rows.Clear();
            dtGridPvc.Rows.Clear();
            chSeulPVC.Checked = false;
            checkSeulPVC(chSeulPVC.Checked);
            lstTypeBois.Focus();
        }

        /// <summary>
        /// method qui calcul data grid view mesure
        /// </summary>
        /// <param name="mesures"></param>
        private void RemplirDataMesure()
        {
            double totale = 0;
            for (int i = 0; i < dtGMesure.Rows.Count; i++)
            {
                if (cmbTypeDuMetres.SelectedIndex == 2)
                {
                    totale +=
                        ((Convert.ToDouble(dtGMesure.Rows[i].Cells[1].Value) / 100) *
                        (Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value) / 100) *
                        (Convert.ToDouble(dtGMesure.Rows[i].Cells[3].Value) / 100)) *
                        Convert.ToDouble(dtGMesure.Rows[i].Cells[0].Value);
                }
                else
                {
                    totale +=
                        ((Convert.ToDouble(dtGMesure.Rows[i].Cells[1].Value) / 100) *
                        (Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value) / 100)) *
                        Convert.ToDouble(dtGMesure.Rows[i].Cells[0].Value);
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
                    cmbTypePvc.Items.Add(reader["Libelle"].ToString());
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
            lblFactureNumero.Text = "Facture Numéro: " + (factures.Count + 1).ToString("D2");
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
            dtDateFacture.Value = DateTime.Today;
            checkAvance.Checked = false;
            txtPrixAvanceClient.Enabled = false;
            txtPrixAvanceClient.Text = "0.00";
            cmbOrtnPVC.SelectedIndex = 0;

            /*remple comboBox PVC*/
            RemplirComboBxPvc();

            // si le click a button modifier
            if (btnDeterminClick == "edit")
            {
                try
                {
                    lblFactureNumero.Visible = false;
                    cmbNumeroFacture.Visible = true;
                    lblFactureNumero.Text = "Selectionner la Facture";
                    btnAddFacture.IconChar = FontAwesome.Sharp.IconChar.Edit;
                    btnClear.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;

                    txtNomClient.Enabled = false;
                    dtDateFacture.Enabled = false;
                    checkAvance.Enabled = false;
                    lstTypeBois.SelectedItem = null;
                    btnPrintFacture.Visible = true;
                    btnDeleteFacture.Visible = true;

                    // change event for btn save
                    btnAddFacture.Click -= new EventHandler(this.btnSave_Click);
                    btnAddFacture.Click += new EventHandler(this.btnEditFacture_Click);

                    txtPrixAvanceClient.TextChanged -= new EventHandler(this.txtPrixRestClient_TextChanged);
                    txtPrixTotalClient.TextChanged -= new EventHandler(this.txtPrixRestClient_TextChanged);

                    connectionClient.Open();

                    //client
                    var commandClient = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = "SELECT * FROM QUERY WHERE IDCLIENT = " + long.Parse(idClient)
                    };
                    OleDbDataReader readerClient = commandClient.ExecuteReader();
                    while (readerClient.Read())
                    {
                        txtNomClient.Text = readerClient["nomClient"].ToString();
                        txtPrixTotalClient.Text = readerClient["total"].ToString();
                        txtPrixAvanceClient.Text = readerClient["prixTotalAvance"].ToString();
                        txtPrixRestClient.Text = readerClient["rest"].ToString();
                    }

                    //facture
                    var commandFacture = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = "SELECT idFacture FROM FACTURE WHERE IDCLIENT = " + long.Parse(idClient)
                    };
                    OleDbDataReader readerFacture = commandFacture.ExecuteReader();
                    while (readerFacture.Read())
                    {
                        // get list of factures
                        cmbNumeroFacture.Items.Add(long.Parse(readerFacture["IDFACTURE"].ToString()));
                    }
                    connectionClient.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //desactiver le champ de pre avance
                txtPrixAvanceClient.Enabled = false;

                // select the first item on list
                if (cmbNumeroFacture.Items.Count > 0)
                    cmbNumeroFacture.SelectedIndex = 0;
                // verifier que la liste des facture contient plus d'un un facture
                //if (cmbNumeroFacture.Items.Count <= 1)
                //    btnDeleteFacture.Enabled = false;
            }
            /*initialise IdFacture, IdClient*/
            idFacture();
        }

        /// <summary>
        /// method qui charger la base de donnees
        /// </summary>
        /// <param name="idFacture"></param>
        private void loadDataEdit(long idFacture)
        {
            try
            {
                connectionClient.Open();

                String pvc = "";

                //facture
                var commandFacture = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText = "SELECT * FROM FACTURE WHERE idFacture = " + idFacture
                };
                OleDbDataReader readerFacture = commandFacture.ExecuteReader();
                while (readerFacture.Read())
                {
                    dtDateFacture.Value = Convert.ToDateTime(readerFacture["dtDateFacture"]);

                    chSeulPVC.Checked = Convert.ToBoolean(readerFacture["checkPVC"].ToString());


                    if (!chSeulPVC.Checked)
                    {
                        txtTypeDeBois.Text = readerFacture["typeDeBois"].ToString();
                        txtCategorie.Text = readerFacture["categorie"].ToString();
                        txtMetrageDeFeuille.Text = readerFacture["metrage"].ToString();
                        txtPrixMetreMesure.Text = readerFacture["prixMetres"].ToString();
                        txtTotalMesure.Text = readerFacture["totalMesure"].ToString();
                        txtPrixTotalMesure.Text = readerFacture["prixTotalMesure"].ToString();
                    }

                    //pvc
                    pvc = readerFacture["typePVC"].ToString();
                    cmbTypePvc.SelectedItem = (pvc == "---" ? null : readerFacture["typePVC"].ToString());
                    txtTaillePVC.Text = readerFacture["tailleCanto"].ToString();
                    txtTotaleTaillPVC.Text = readerFacture["totalTaillPVC"].ToString();
                    txtPrixMetreLPVC.Text = readerFacture["prixMitresLinear"].ToString();
                    txtPrixTotalPVC.Text = readerFacture["prixTotalPVC"].ToString();
                }


                //Mesure
                if (!chSeulPVC.Checked)
                {
                    var commandMesure = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = "SELECT * FROM MESURE WHERE IDFACTURE = " + idFacture
                    };
                    OleDbDataReader readerMesure = commandMesure.ExecuteReader();
                    // clear data grid mesure
                    dtGMesure.Rows.Clear();
                    while (readerMesure.Read())
                    {
                        if (readerMesure["type"].ToString() == "m3")
                        {
                            cmbTypeDuMetres.SelectedIndex = 2;
                            dtGMesure.Rows.Add(
                            readerMesure["quantite"].ToString(),
                            readerMesure["largeur"].ToString(),
                            readerMesure["longueur"].ToString(),
                            readerMesure["eppaiseur"].ToString());
                        }
                        else if (readerMesure["type"].ToString() == "m2")
                        {
                            cmbTypeDuMetres.SelectedIndex = 1;
                            dtGMesure.Rows.Add(
                            readerMesure["quantite"].ToString(),
                            readerMesure["largeur"].ToString(),
                            readerMesure["longueur"].ToString());
                        }
                        else
                        {
                            cmbTypeDuMetres.SelectedIndex = 0;
                            dtGMesure.Rows.Add(
                            readerMesure["quantite"].ToString(),
                            readerMesure["largeur"].ToString(),
                            readerMesure["longueur"].ToString());
                        }
                    }
                }

                // pvc
                if (pvc != "---")
                {
                    var commandPVC = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = "SELECT * FROM PVC WHERE IDFACTURE = " + idFacture
                    };
                    OleDbDataReader readerPVC = commandPVC.ExecuteReader();
                    dtGridPvc.Rows.Clear();
                    while (readerPVC.Read())
                    {
                        dtGridPvc.Rows.Add(
                            readerPVC["quantite"].ToString(),
                            readerPVC["largeur"].ToString(),
                            readerPVC["longueur"].ToString(),
                            readerPVC["orientation"].ToString());
                    }
                }
                else
                {
                    dtGridPvc.Rows.Clear();
                }

                checkSeulPVC(chSeulPVC.Checked);

                connectionClient.Close();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.ExclamationCircle);
                message.ShowDialog();
            }
        }

        /// <summary>
        /// c'est la selection du facture a partir du button modifier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFactureNumero_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDataEdit(long.Parse(cmbNumeroFacture.SelectedItem.ToString()));
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
            cmbTypePvc.Items.Clear();

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
                List<string> list = remplirRechercheListe(cmbTypeDeBois.Text);
                for (int i = 0; i < list.Count; i++)
                {
                    string st1 = list[i];
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
        /// cancel:: c'est le button qui returner a la fomr presedent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, EventArgs e)
        {
            btnDeterminClick = "";
            Close();
        }

        /// <summary>
        /// c'est button qui vider les champs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (btnDeterminClick == "edit" && btnClear.IconChar != FontAwesome.Sharp.IconChar.Backspace)
            {
                ViderTxtBox();
                btnAddFacture.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
                btnClear.IconChar = FontAwesome.Sharp.IconChar.Backspace;
                // change event for btn save
                btnAddFacture.Click -= new EventHandler(this.btnEditFacture_Click);
                btnAddFacture.Click += new EventHandler(this.AddFactureClient_Click);
            }
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
                message = new FormMessage(txtNomClient.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtNomClient.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtTypeDeBois.Text))
            {
                message = new FormMessage(txtTypeDeBois.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                lstTypeBois.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPrixMetreMesure.Text))
            {
                message = new FormMessage(txtPrixMetreMesure.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtPrixMetreMesure.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtTotalMesure.Text))
            {
                message = new FormMessage(txtTotalMesure.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtTotalMesure.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPrixTotalMesure.Text))
            {
                message = new FormMessage(txtPrixTotalMesure.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtPrixTotalMesure.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtMetrageDeFeuille.Text))
            {
                message = new FormMessage(txtMetrageDeFeuille.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtMetrageDeFeuille.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCategorie.Text))
            {
                message = new FormMessage(txtCategorie.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtCategorie.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPrixTotalClient.Text))
            {
                message = new FormMessage(txtPrixTotalClient.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtPrixTotalClient.Focus();
                return false;
            }
            if (checkAvance.Checked)
                if (string.IsNullOrEmpty(txtPrixAvanceClient.Text))
                {
                    message = new FormMessage(txtPrixAvanceClient.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    txtPrixAvanceClient.Focus();
                    return false;
                }
            if (!string.IsNullOrEmpty(cmbTypePvc.Text))
            {
                if (string.IsNullOrEmpty(txtTotaleTaillPVC.Text))
                {
                    message = new FormMessage(txtTotaleTaillPVC.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    txtTotaleTaillPVC.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(txtTaillePVC.Text))
                {
                    message = new FormMessage(txtTaillePVC.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    txtTaillePVC.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(txtPrixMetreLPVC.Text))
                {
                    message = new FormMessage(txtPrixMetreLPVC.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog(); txtPrixMetreLPVC.Focus();
                    return false;
                }
                if (dtGMesure.Rows.Count != dtGridPvc.Rows.Count && !chSeulPVC.Checked && cmbTypePvc.SelectedText != null)
                {
                    message = new FormMessage("Exporter les Mesures vers Pvc", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationCircle);
                    message.ShowDialog();
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
                if (btnDeterminClick != "edit")
                {
                    dtGMesure.Rows.Clear();
                    mesures.Clear();
                }
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
                if (btnDeterminClick != "edit")
                {
                    dtGMesure.Rows.Clear();
                    mesures.Clear();
                    txtTotalMesure.Clear();
                }
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
                if (btnDeterminClick != "edit")
                {
                    dtGMesure.Rows.Clear();
                    mesures.Clear();
                    txtTotalMesure.Clear();
                }
                dtGMesure.Rows.Clear();
                mesures.Clear();
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
            //verifier si le champs contain un chifre ou vide 
            if (string.IsNullOrEmpty(txtQuantite.Text) || double.Parse(txtQuantite.Text) == 0)
            {
                MessageBox.Show("n'accept pas 0, saisir la valeur de " + txtQuantite.Tag, "attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantite.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtLargeur.Text) || double.Parse(txtLargeur.Text) == 0)
            {
                MessageBox.Show("n'accept pas 0, saisir la valeur de " + txtLargeur.Tag, "attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLargeur.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtLongueur.Text) || double.Parse(txtLongueur.Text) == 0)
            {
                MessageBox.Show("n'accept pas 0, saisir la valeur de " + txtLongueur.Tag, "attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLongueur.Focus();
                return;
            }


            for (int i = 0; i < dtGMesure.Rows.Count; i++)
            {
                if (cmbTypeDuMetres.SelectedIndex == 2)
                {
                    if (double.Parse(txtQuantite.Text) == double.Parse(dtGMesure.Rows[i].Cells[0].Value.ToString()) &&
                        double.Parse(txtLargeur.Text) == double.Parse(dtGMesure.Rows[i].Cells[1].Value.ToString()) &&
                        double.Parse(txtLongueur.Text) == double.Parse(dtGMesure.Rows[i].Cells[2].Value.ToString()) &&
                        double.Parse(txtEpaisseur.Text) == double.Parse(dtGMesure.Rows[i].Cells[3].Value.ToString()))
                    {
                        MessageBox.Show("c'est mesure exist deja", "attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    if (double.Parse(txtQuantite.Text) == double.Parse(dtGMesure.Rows[i].Cells[0].Value.ToString()) &&
                      double.Parse(txtLargeur.Text) == double.Parse(dtGMesure.Rows[i].Cells[1].Value.ToString()) &&
                      double.Parse(txtLongueur.Text) == double.Parse(dtGMesure.Rows[i].Cells[2].Value.ToString()))
                    {
                        MessageBox.Show("c'est mesure exist deja", "attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            if (cmbTypeDuMetres.SelectedIndex == 2)
            {
                //verifier si le champs contain un chifre ou vide
                if (string.IsNullOrEmpty(txtEpaisseur.Text) || double.Parse(txtEpaisseur.Text) == 0)
                {
                    MessageBox.Show("n'accept pas 0, saisir la valeur de " + txtEpaisseur.Tag, "attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEpaisseur.Focus();
                    return;
                }

                dtGMesure.Rows.Add(txtQuantite.Text, txtLargeur.Text, txtLongueur.Text, txtEpaisseur.Text);
                txtEpaisseur.Clear();
            }
            else
            {
                dtGMesure.Rows.Add(txtQuantite.Text, txtLargeur.Text, txtLongueur.Text);
            }
            RemplirDataMesure();
            txtQuantite.Clear();
            txtLargeur.Clear();
            txtLongueur.Clear();
            txtQuantite.Focus();
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

            dtGMesure.Rows.RemoveAt(dtGMesure.CurrentRow.Index);

            RemplirDataMesure();
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
        /// this button delete facture from database by idFacture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteFacture_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbNumeroFacture.Items.Count <= 0)
                {
                    message = new FormMessage("le client a aucun facture", "Succès !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    return;
                }
                connectionClient.Open();

                OleDbCommand commandFacture = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText = "DELETE * FROM FACTURE WHERE idFacture = " + cmbNumeroFacture.SelectedItem
                };
                commandFacture.ExecuteNonQuery();

                connectionClient.Close();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Succès !!!", true, FontAwesome.Sharp.IconChar.ExclamationCircle);
                message.ShowDialog();
            }
            cmbNumeroFacture.Items.Remove(cmbNumeroFacture.SelectedItem);
            message = new FormMessage("La facture a été supprimée avec succès", "Succès !!!", true, FontAwesome.Sharp.IconChar.ThumbsUp);
            message.ShowDialog();

            if (cmbNumeroFacture.Items.Count >= 1)
                cmbNumeroFacture.SelectedIndex = 0;
            else
                ViderTxtBox();

        }

        /// <summary>
        /// this button print facture by id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintFacture_Click(object sender, EventArgs e)
        {
            PrintPdf printPdf = new PrintPdf(idClient, cmbNumeroFacture.SelectedItem.ToString(), "btnPrintFacture", chSeulPVC.Checked);
            printPdf.ShowDialog();
        }

        /// <summary>
        /// this button export dataMesure as csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (dtGMesure.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Mesure.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dtGMesure.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dtGMesure.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dtGMesure.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dtGMesure.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dtGMesure.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            message = new FormMessage("Données exportées avec succès !!!", "Succès !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                            message.ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            message = new FormMessage("Error :" + ex.Message, "Erreur", true);
                            message.ShowDialog();
                        }
                    }
                }
            }
            else
            {
                message = new FormMessage("Spécifiez une ligne !!!", "Warning", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
            }
        }

        private void btnEditFacture_Click(object sender, EventArgs e)
        {
            if (checkIsNullOrEmpty())
            {
                try
                {
                    connectionClient.Open();

                    var commandF = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = "UPDATE facture SET " +
                            " typeDeBois = '" + txtTypeDeBois.Text + "'" +
                            ", metrage = '" + txtMetrageDeFeuille.Text + "'" +
                            ", categorie = '" + txtCategorie.Text + "'" +
                            ", totalMesure = " + (string.IsNullOrEmpty(txtTotalMesure.Text) ? 0 : double.Parse(txtTotalMesure.Text)) +
                            ", prixMetres = " + (string.IsNullOrEmpty(txtPrixMetreMesure.Text) ? 0 : double.Parse(txtPrixMetreMesure.Text)) +
                            ", typePVC = '" + (cmbTypePvc.SelectedItem == null ? "---" : cmbTypePvc.SelectedItem) + "'" +
                            ", checkPVC = " + chSeulPVC.Checked +
                            ", tailleCanto = " + (string.IsNullOrEmpty(txtTaillePVC.Text) ? 0 : double.Parse(txtTaillePVC.Text)) +
                            ", totalTaillPVC = " + (string.IsNullOrEmpty(txtTotaleTaillPVC.Text) ? 0 : double.Parse(txtTotaleTaillPVC.Text)) +
                            ", prixMitresLinear = " + (string.IsNullOrEmpty(txtPrixMetreLPVC.Text) ? 0 : double.Parse(txtPrixMetreLPVC.Text)) +
                            ", prixTotalPVC = " + (string.IsNullOrEmpty(txtPrixTotalPVC.Text) ? 0 : double.Parse(txtPrixTotalPVC.Text)) +
                            ", prixTotalMesure = " + (string.IsNullOrEmpty(txtPrixTotalMesure.Text) ? 0 : double.Parse(txtPrixTotalMesure.Text)) +
                            " WHERE idFacture = " + long.Parse(cmbNumeroFacture.Text)
                    };
                    commandF.ExecuteNonQuery();

                    if (!chSeulPVC.Checked && dtGMesure.Rows.Count > 0)
                    {
                        OleDbCommand commandMD = new OleDbCommand();
                        commandMD.Connection = connectionClient;
                        commandMD.CommandText = "DELETE * from mesure WHERE idFacture = " + long.Parse(cmbNumeroFacture.Text);
                        commandMD.ExecuteNonQuery();

                        for (int i = 0; i < dtGMesure.Rows.Count; i++)
                        {
                            OleDbCommand commandM = new OleDbCommand();
                            commandM.Connection = connectionClient;
                            commandM.CommandText = "INSERT INTO mesure(idFacture, quantite, largeur, longueur, eppaiseur, type, orientation)  " +
                                "VALUES('" + long.Parse(cmbNumeroFacture.Text) + "','" +
                                double.Parse(dtGMesure.Rows[i].Cells[0].Value.ToString()) + "','" +
                                double.Parse(dtGMesure.Rows[i].Cells[1].Value.ToString()) + "','" +
                                double.Parse(dtGMesure.Rows[i].Cells[2].Value.ToString()) + "','" +
                                (cmbTypeDuMetres.Text == "m3" ? double.Parse(dtGMesure.Rows[i].Cells[3].Value.ToString()) : 0) + "','" +
                                cmbTypeDuMetres.Text + "','" +
                                ((dtGridPvc.Rows.Count <= 0) ? "0" : dtGridPvc.Rows[i].Cells[3].Value.ToString()) + "')";
                            commandM.ExecuteNonQuery();
                        }
                    }

                    if (!String.IsNullOrEmpty(cmbTypePvc.Text))
                    {
                        OleDbCommand commandPD = new OleDbCommand();
                        commandPD.Connection = connectionClient;
                        commandPD.CommandText = "DELETE * from pvc WHERE idFacture = " + long.Parse(cmbNumeroFacture.Text);
                        commandPD.ExecuteNonQuery();
                        for (int i = 0; i < dtGridPvc.Rows.Count; i++)
                        {
                            OleDbCommand commandP = new OleDbCommand();
                            commandP.Connection = connectionClient;
                            commandP.CommandText = "INSERT INTO pvc (idFacture, quantite, largeur, longueur, orientation) " +
                                "values('" + long.Parse(cmbNumeroFacture.Text) + "','" +
                                double.Parse(dtGridPvc.Rows[i].Cells[0].Value.ToString()) + "','" +
                                double.Parse(dtGridPvc.Rows[i].Cells[1].Value.ToString()) + "','" +
                                double.Parse(dtGridPvc.Rows[i].Cells[2].Value.ToString()) + "','" +
                                dtGridPvc.Rows[i].Cells[3].Value.ToString() + "')";
                            commandP.ExecuteNonQuery();
                        }
                    }

                    message = new FormMessage("Modifier Avec Succès", "Succès", true, FontAwesome.Sharp.IconChar.ExclamationCircle);
                    message.ShowDialog();

                    connectionClient.Close();

                    loadDataEdit(long.Parse(cmbNumeroFacture.Text));

                }
                catch (Exception ex)
                {
                    message = new FormMessage("Erreur: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.ExclamationCircle);
                    message.ShowDialog();
                    connectionClient.Close();
                }
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
                        vs.Add(new Mesure(facture.IDFacture, Convert.ToDouble(dtGMesure.Rows[i].Cells[0].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[1].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value), cmbTypeDuMetres.Text, dtGridPvc.Rows.Count > 0 ? dtGridPvc.Rows[i].Cells[3].Value.ToString() : "0"));
                    }
                }
                else
                {
                    for (int i = 0; i < dtGMesure.Rows.Count; i++)
                    {
                        vs.Add(new Mesure(facture.IDFacture, Convert.ToDouble(dtGMesure.Rows[i].Cells[0].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[1].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[3].Value), cmbTypeDuMetres.Text, dtGridPvc.Rows.Count > 0 ? dtGridPvc.Rows[i].Cells[3].Value.ToString() : "0"));
                    }
                }

                facture.Mesures = vs;
                facture.DateFacture = dtDateFacture.Value;
                facture.TypeDeBois = txtTypeDeBois.Text;
                facture.Categorie = txtCategorie.Text;
                facture.Metrage = txtMetrageDeFeuille.Text;
                facture.PrixMetres = double.Parse(txtPrixMetreMesure.Text);
                facture.TotalMesure = double.Parse(txtTotalMesure.Text);
                facture.PrixTotalMesure = double.Parse(txtPrixTotalMesure.Text);

                if (!string.IsNullOrEmpty(cmbTypePvc.Text))
                {
                    List<Pvc> vspvc = new List<Pvc>();

                    for (int i = 0; i < dtGridPvc.Rows.Count; i++)
                    {
                        vspvc.Add(new Pvc(Convert.ToDouble(dtGridPvc.Rows[i].Cells[0].Value),
                            Convert.ToDouble(dtGridPvc.Rows[i].Cells[1].Value),
                            Convert.ToDouble(dtGridPvc.Rows[i].Cells[2].Value),
                            (dtGridPvc.Rows[i].Cells[3] as DataGridViewComboBoxCell).FormattedValue.ToString()));
                    }

                    facture.Pvcs = vspvc;
                    facture.TypePVC = cmbTypePvc.Text;
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

                // vider les champs
                ViderTxtBox();
                btnSaveFacture.Enabled = true;
            }
        }

        /// <summary>
        /// c'est method pour activer ou desactiver les champs
        /// </summary>
        /// <param name="ft"></param>
        private void checkSeulPVC(bool ft)
        {
            txtPrixMetreMesure.Enabled = txtMetrageDeFeuille.Enabled = txtCategorie.Enabled = cmbTypeDuMetres.Enabled =
            btnCmbCategorie.Enabled = cmbTypeDeBois.Enabled = txtSearch.Enabled = lstTypeBois.Enabled =
            txtQuantite.Enabled = txtLargeur.Enabled = txtLongueur.Enabled = txtEpaisseur.Enabled =
            btnAddMesure.Enabled = btnDeleteMesure.Enabled = btnExportCsv.Enabled =
            btnImportPvc.Enabled = btnSavePvc.Enabled = !ft;

            btnAddSeulPVC.Enabled = btnDeleteSeulPVC.Enabled = txtQtePVC.Enabled = txtLargPVC.Enabled =
            txtLongPVC.Enabled = cmbOrtnPVC.Enabled = ft;

            if (ft)
            {
                txtTypeDeBois.Text = "---";
                txtPrixMetreMesure.Text = "0.00";
                txtTotalMesure.Text = "0.00";
                txtPrixTotalMesure.Text = "0.00";
                txtMetrageDeFeuille.Text = "---";
                txtCategorie.Text = "---";
            }
        }

        /// <summary>
        /// quand le check sure le button check seul PVC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chSeulPVC_CheckedChanged(object sender, EventArgs e)
        {
            checkSeulPVC(chSeulPVC.Checked);
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
            dtGridPvc.Enabled = btnImportPvc.Enabled = btnSavePvc.Enabled = chSeulPVC.Enabled =
            txtTaillePVC.Enabled = txtPrixMetreLPVC.Enabled = cmbTypePvc.SelectedIndex < 0 ? false : true;
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
                txtTotaleTaillPVC.Clear();
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

            for (int i = 0; i < dtGridPvc.Rows.Count; i++)
            {
                if (dtGridPvc.Rows[i].Cells[3].Value == null)
                {
                    message = new FormMessage("selection l'orientation de ligne " + (i + 1), "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    return;
                }
            }

            pvcs.Clear();
            double total = 0;
            for (int i = 0; i < dtGridPvc.Rows.Count; i++)
            {
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


            if (avance > total)
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
        /// c'est le button pour sauvegarder les donnes qui ajouter dans le formule quand fermiture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveFacture_Click(object sender, EventArgs e)
        {
            this.Close();
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
                try
                {
                    connectionClient.Open();
                    int idCLIENT = 0;
                    int idFACTURE = 0;
                    OleDbCommand commandClient = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = "insert into client (nomClient, dateClient, prixTotalAvance) " +
                        "values('" + txtNomClient.Text + "', '" + DateTime.Today + "', '" +
                        Convert.ToDouble(txtPrixAvanceClient.Text) + "')"
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
                            CommandText = "insert into facture (idClient, dtDateFacture, typeDeBois, " +
                            "metrage, categorie, totalMesure, prixMetres, typePVC, checkPVC, tailleCanto, " +
                            "totalTaillPVC, prixMitresLinear, prixTotalPVC, prixTotalMesure) " +
                                          "values ('" + (idCLIENT) + "', '" + fct.DateFacture + "', '" +
                                          fct.TypeDeBois + "', '" + fct.Metrage + "','" + fct.Categorie + "', '" +
                                          fct.TotalMesure + "', '" + fct.PrixMetres + "', '" + fct.TypePVC + "', " +
                                          fct.CheckPVC.ToString() + ", '" + fct.TailleCanto + "', '" +
                                          fct.TotalTaillPVC + "', '" + fct.PrixMitresLinear + "', '" +
                                          fct.PrixTotalPVC + "', '" + fct.PrixTotalMesure + "')"
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

                        foreach (Pvc pvc in fct.Pvcs)
                        {
                            var commandPvc = new OleDbCommand
                            {
                                Connection = connectionClient,
                                CommandText = "insert into pvc (idFacture, quantite, largeur, longueur, orientation) values('" + idFACTURE + "','" + pvc.Qte + "','" + pvc.Largr + "','" + pvc.Longr + "','" + pvc.Ortn + "')"
                            };
                            commandPvc.ExecuteNonQuery();
                        }

                        foreach (Mesure msr in fct.Mesures)
                        {
                            OleDbCommand commandMesure = new OleDbCommand
                            {
                                Connection = connectionClient,
                                CommandText = "insert into mesure (idFacture, quantite, largeur, longueur, eppaiseur, type, orientation) values('" + (idFACTURE) + "','" + msr.Quantite + "','" + msr.Largeur + "','" + msr.Longueur + "','" + msr.Epaisseur + "','" + msr.Type + "','" + msr.Orientation + "')"
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

        private void btnAddFactureClient_Click(object sender, EventArgs e)
        {
            ViderTxtBox();
            btnAddFacture.IconChar = FontAwesome.Sharp.IconChar.Edit;

            // change event for btn save
            btnAddFacture.Click -= new EventHandler(this.btnSave_Click);
            btnAddFacture.Click += new EventHandler(this.btnEditFacture_Click);
        }

        private void AddFactureClient_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkIsNullOrEmpty())
                {
                    connectionClient.Open();
                    var mnd = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText =  "insert into facture (idClient, dtDateFacture, typeDeBois, " +
                            "metrage, categorie, totalMesure, prixMetres, typePVC, checkPVC, tailleCanto, " +
                            "totalTaillPVC, prixMitresLinear, prixTotalPVC, prixTotalMesure) " +
                                          "values ('" + long.Parse(idClient) + "', '" + DateTime.Today + "', '" +
                                          txtTypeDeBois.Text + "', '" + txtMetrageDeFeuille.Text + "','" + txtCategorie.Text + "', '" +
                                          double.Parse(txtTotalMesure.Text) + "', '" + double.Parse(txtPrixMetreMesure.Text) + "', '" + 
                                          cmbTypePvc.Text + "', " + chSeulPVC.Checked + ", '" + double.Parse(txtTaillePVC.Text) + "', '" +
                                          double.Parse(txtPrixTotalPVC.Text) + "', '" + double.Parse(txtPrixMetreLPVC.Text) + "', '" +
                                          double.Parse(txtPrixTotalPVC.Text) + "', '" + double.Parse(txtPrixTotalMesure.Text) + "')"
                    };
                    mnd.ExecuteNonQuery();

                    long idF = 0;
                    /*get idClient from database*/
                    OleDbCommand commandIdFacture = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = "select top 1 idFacture from facture order by idFacture desc"
                    };
                    OleDbDataReader readerIdFacture = commandIdFacture.ExecuteReader();
                    while (readerIdFacture.Read())
                    {
                        idF = Convert.ToInt64(readerIdFacture["idFacture"]);
                    }

                    if (!chSeulPVC.Checked && dtGMesure.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtGMesure.Rows.Count; i++)
                        {
                            OleDbCommand commandM = new OleDbCommand();
                            commandM.Connection = connectionClient;
                            commandM.CommandText = "INSERT INTO mesure(idFacture, quantite, largeur, longueur, eppaiseur, type, orientation)  " +
                                "VALUES('" + idF + "','" +
                                double.Parse(dtGMesure.Rows[i].Cells[0].Value.ToString()) + "','" +
                                double.Parse(dtGMesure.Rows[i].Cells[1].Value.ToString()) + "','" +
                                double.Parse(dtGMesure.Rows[i].Cells[2].Value.ToString()) + "','" +
                                (cmbTypeDuMetres.Text == "m3" ? double.Parse(dtGMesure.Rows[i].Cells[3].Value.ToString()) : 0) + "','" +
                                cmbTypeDuMetres.Text + "','" +
                                ((dtGridPvc.Rows.Count <= 0) ? "0" : dtGridPvc.Rows[i].Cells[3].Value.ToString()) + "')";
                            commandM.ExecuteNonQuery();
                        }
                    }

                    if (!String.IsNullOrEmpty(cmbTypePvc.Text))
                    {
                        for (int i = 0; i < dtGridPvc.Rows.Count; i++)
                        {
                            OleDbCommand commandP = new OleDbCommand();
                            commandP.Connection = connectionClient;
                            commandP.CommandText = "INSERT INTO pvc (idFacture, quantite, largeur, longueur, orientation) " +
                                "values('" + idF + "','" +
                                double.Parse(dtGridPvc.Rows[i].Cells[0].Value.ToString()) + "','" +
                                double.Parse(dtGridPvc.Rows[i].Cells[1].Value.ToString()) + "','" +
                                double.Parse(dtGridPvc.Rows[i].Cells[2].Value.ToString()) + "','" +
                                dtGridPvc.Rows[i].Cells[3].Value.ToString() + "')";
                            commandP.ExecuteNonQuery();
                        }
                    }

                    //facture
                    var commandFe = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = "SELECT idFacture FROM FACTURE WHERE IDCLIENT = " + long.Parse(idClient)
                    };
                    OleDbDataReader readerFe = commandFe.ExecuteReader();
                    cmbNumeroFacture.Items.Clear();
                    while (readerFe.Read())
                    {
                        // get list of factures
                        cmbNumeroFacture.Items.Add(long.Parse(readerFe["IDFACTURE"].ToString()));
                    }

                    connectionClient.Close();

                    btnClear.IconChar = FontAwesome.Sharp.IconChar.Backspace;
                    cmbNumeroFacture.SelectedItem = cmbNumeroFacture.TabIndex;
                    btnAddFacture.IconChar = FontAwesome.Sharp.IconChar.Edit;
                    btnAddFacture.Click -= new EventHandler(this.AddFactureClient_Click);
                    btnAddFacture.Click += new EventHandler(this.btnEditFacture_Click);
                    message = new FormMessage("Ajouté Avec Succès", "Succès", true, FontAwesome.Sharp.IconChar.ThumbsUp);
                    message.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.ExclamationCircle);
                message.ShowDialog();
                connectionClient.Close();
            }
        }
    }
}
