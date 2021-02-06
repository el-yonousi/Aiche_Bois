using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;

namespace Aiche_Bois
{
    public partial class FormClient : Form
    {
        /// <summary>
        /// this code form move panel
        /// </summary>
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg,
        int wparam, int lparam);

        /*
         * ================ Panel Add ================
         */
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
        /// this instance from form message to show message error
        /// </summary>
        private FormMessage message;

        /// <summary>
        /// verify les donnée si enregistre ou non
        /// </summary>
        bool verifyForm = false;

        /// <summary>
        /// prendre id client de data grid a partir de ligne selecionnees
        /// </summary>
        private String[] idClient;

        /// <summary>
        /// stocker les client a liste des clients
        /// </summary>
        DataTable tb = new DataTable();

        /*
         * =========================== Panel Add Edit Method ===========================
         */
        /// <summary>
        /// method qui charger la base de donnees
        /// </summary>
        /// <param name="idFacture"></param>
        private void loadDataEdit(long idFacture)
        {
            try
            {
                connectionClient.Open();
                ViderTxtBox();
                //client
                var commandClient = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText =
                    "SELECT count(idFacture) AS nbFacture, " +
                    "(sum(f.prixtotalmesure) + sum(prixtotalpvc)) AS total, " +
                    "IIF((sum(f.prixtotalmesure) + sum(prixtotalpvc)) = prixTotalAvance, 0, (sum(f.prixtotalmesure) + sum(prixtotalpvc)) - prixTotalAvance) AS rest, " +
                    "IIF(ROUND((sum(f.prixtotalmesure) + sum(prixtotalpvc)), 2) = ROUND((prixTotalAvance), 2), 'true', 'false') AS cavance, " +
                    "c.nomClient, dateClient, c.prixTotalAvance, c.idClient " +
                    "FROM client AS c INNER JOIN facture AS f ON f.idClient = c.idClient " +
                    "WHERE c.idClient = " + long.Parse(idClient[1]) +
                    " GROUP BY nomClient, dateClient, prixTotalAvance, c.idClient"
                };
                OleDbDataReader readerClient = commandClient.ExecuteReader();
                while (readerClient.Read())
                {
                    txtNomClient.Text = readerClient["nomClient"].ToString();
                    txtPrixTotalClient.Text = readerClient["total"].ToString();
                    txtPrixAvanceClient.Text = readerClient["prixTotalAvance"].ToString();
                    txtPrixRestClient.Text = readerClient["rest"].ToString();
                }

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
                        cmbTypeDuMetres.SelectedItem = readerFacture["typeMetres"].ToString();
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

                if (!cmbTypeDuMetres.SelectedItem.Equals("m"))
                {
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
                            if (cmbTypeDuMetres.SelectedItem.Equals("m3"))
                            {
                                dtGMesure.Rows.Add(
                                readerMesure["quantite"].ToString(),
                                readerMesure["largeur"].ToString(),
                                readerMesure["longueur"].ToString(),
                                readerMesure["eppaiseur"].ToString());
                            }
                            else
                            {
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
                }
                else
                {
                    dtGridPvc.Rows.Clear();
                    dtGMesure.Rows.Clear();
                }

                connectionClient.Close();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionClient.Close();
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
                dtGMesure.Rows.Clear();
            }
        }

        /// <summary>
        /// remplir listeBox MDF, LATTE, STD de dataBase
        /// </summary>
        /// <param name="typeBois"></param>
        DataTable tb_Type = new DataTable();
        private void remplirListe(string typeBois)
        {
            lstTypeBois.Items.Clear();
            tb_Type.Rows.Clear();
            try
            {
                connectionType.Open();
                OleDbCommand command = new OleDbCommand
                {
                    Connection = connectionType,
                    CommandText = "select Libelle from " + typeBois
                };

                tb_Type.Load(command.ExecuteReader());

                connectionType.Close();

                for (int i = 0; i < tb_Type.Rows.Count; i++)
                {
                    lstTypeBois.Items.Add(tb_Type.Rows[i][0].ToString());
                }
                lstTypeBois.SelectedIndex = lstTypeBois.Items.Count - 1;

            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionType.Close();
            }
        }

        /// <summary>
        /// method qui vider les text box
        /// </summary>
        private void ViderTxtBox()
        {
            /*vider les objet*/
            txtTypeDeBois.Text = "";
            txtQuantite.Clear();
            txtLargeur.Clear();
            txtLongueur.Clear();
            txtEpaisseur.Clear();
            txtTotalMesure.Clear();
            txtPrixTotalMesure.Text = "0.00";
            txtMetrageDeFeuille.Clear();
            cmbTypePvc.SelectedItem = null;
            cmbTypeDuMetres.SelectedItem = "feuille";
            txtTaillePVC.Clear();
            txtPrixMetreLPVC.Clear();
            txtTotaleTaillPVC.Text = "";
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
        /// le traitmenet pour calculer les donnees du datagrid pvc
        /// </summary>
        public void btnSaveCalculPvc()
        {
            if (dtGridPvc.Rows.Count == 0)
            {
                message = new FormMessage("Importer les valeurs des mesures", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
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
                    message = new FormMessage("Exporter les Mesures vers Pvc", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// this method disable/enable mesures
        /// </summary>
        /// <param name="b"></param>
        private void enabledMesure(bool b)
        {
            txtQuantite.Enabled = txtLargeur.Enabled = txtLongueur.Enabled = txtEpaisseur.Enabled = b;
        }

        /// <summary>
        /// initialise l'identificateur de facture
        /// </summary>
        private void idFacture()
        {
            cmbNumeroFacture.Text = "Facture Numéro: " + (factures.Count + 1).ToString("D2");
        }

        /// <summary>
        /// remplir comboBox PVC de dataBase
        /// </summary>
        private void RemplirComboBxPvc()
        {
            cmbTypePvc.Items.Clear();
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
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionType.Close();
            }
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
                if (cmbTypeDuMetres.SelectedItem.Equals("m3"))
                {
                    totale +=
                        ((Convert.ToDouble(dtGMesure.Rows[i].Cells[1].Value) / 100) *
                        (Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value) / 100) *
                        (Convert.ToDouble(dtGMesure.Rows[i].Cells[3].Value) / 1000)) *
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

            if (!cmbTypeDuMetres.SelectedItem.Equals("feuille"))
            {
                txtTotalMesure.Text = totale.ToString();
            }
        }
        private void saveDataClient()
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
                        "metrage, categorie, totalMesure, typeMetres, prixMetres, typePVC, checkPVC, tailleCanto, " +
                        "totalTaillPVC, prixMitresLinear, prixTotalPVC, prixTotalMesure) " +
                                      "values ('" + (idCLIENT) + "', '" + fct.DateFacture + "', '" +
                                      fct.TypeDeBois + "', '" + fct.Metrage + "','" + fct.Categorie + "', '" +
                                      fct.TotalMesure + "', '" + fct.TypeMetres + "', '" + fct.PrixMetres + "', '" + fct.TypePVC + "', " +
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
                            CommandText = "insert into mesure (idFacture, quantite, largeur, longueur, eppaiseur) values('" + (idFACTURE) + "','" + msr.Quantite + "','" + msr.Largeur + "','" + msr.Longueur + "','" + msr.Epaisseur + "')"
                        };
                        commandMesure.ExecuteNonQuery();
                    }
                }
                connectionClient.Close();
                message = new FormMessage("Les données ont été enregistrées", "Succés", true, FontAwesome.Sharp.IconChar.CheckCircle);
                message.ShowDialog();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionClient.Close();
            }
        }
        /*
         * =========================== End Panel Add Edit Method ===========================
         */

        /// <summary>
        /// c'est le design du formulaire et l'initialisation de connecter a la base de donnees
        /// </summary>
        public FormClient()
        {
            connectionClient.ConnectionString = Program.Path;
            connectionType.ConnectionString = Program.PathType;

            InitializeComponent();
        }

        /// <summary>
        /// c'est method remplir data gride view de client
        /// </summary>
        private void remplissageDtGridClient()
        {
            try
            {
                connectionClient.Open();
                tb.Rows.Clear();
                dtGridClient.Rows.Clear();
                //clients.Clear();

                OleDbCommand commandClient = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText =
                    "SELECT c.idClient, " +
                    "c.nomClient, " +
                    "dateClient, " +
                    "count(idFacture) AS nbFacture, " +
                    "IIF(ROUND((sum(f.prixtotalmesure) + sum(prixtotalpvc)), 2) = ROUND((prixTotalAvance), 2), 'true', 'false') AS cavance, " +
                    "c.prixTotalAvance, " +
                    "IIF((sum(f.prixtotalmesure) + sum(prixtotalpvc)) = prixTotalAvance, 0, (sum(f.prixtotalmesure) + sum(prixtotalpvc)) - prixTotalAvance) AS rest, " +
                    "(sum(f.prixtotalmesure) + sum(prixtotalpvc)) AS total " +
                    "FROM client AS c INNER JOIN facture AS f ON f.idClient = c.idClient " +
                    "GROUP BY " +
                    "c.idClient, " +
                    "nomClient, " +
                    "dateClient, " +
                    "prixTotalAvance " +
                    "ORDER BY c.idClient DESC;"
                };

                tb.Load(commandClient.ExecuteReader());

                connectionClient.Close();

                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    dtGridClient.Rows.Add(
                        String.Format("N{0:D4}", long.Parse(tb.Rows[i][0].ToString())),
                        tb.Rows[i][1].ToString(),
                        String.Format("{0:dd/MM/yyyy}", tb.Rows[i][2].ToString()),
                        tb.Rows[i][3].ToString(),
                        "",
                        tb.Rows[i][4].ToString(),
                        tb.Rows[i][5].ToString(),
                        tb.Rows[i][6].ToString(),
                        tb.Rows[i][7].ToString(),
                        "");
                }
                dtGridClient.Rows[0].Selected = false;
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionClient.Close();
                return;
            }
        }

        /// <summary>
        /// c'est l'affichage du formulaire client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            /*remplissage data grid view*/
            remplissageDtGridClient();
        }

        /// <summary>
        /// c'est event traitment du button click id pour envoyer ca a l'autre formulaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGridFacture_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtGridClient.Rows.Count <= 0 || e.RowIndex < 0)
                return;

            idClient = dtGridClient.Rows[e.RowIndex].Cells[0].Value.ToString().Split('N');

            if (e.ColumnIndex == 4 && e.RowIndex < dtGridClient.Rows.Count)
            {
                FormAvance avance = new FormAvance(idClient[1]);
                avance.ShowDialog();
                remplissageDtGridClient();
                idClient = null;
            }
        }


        /// <summary>
        /// c'est button pour imprimer les factures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Obsolete]
        private void btnPrintFacture_Click(object sender, EventArgs e)
        {
            if (dtGridClient.Rows.Count <= 0 || idClient == null)
            {
                message = new FormMessage("selectionner une ligne s'il vous plait", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            //pvc_mesure(idClient[1], indxFacture);
            FormPrint print = new FormPrint(idClient[1], "null", "btnPrintClient", false);
            print.ShowDialog();

            // initilize id client
            idClient = null;

            // remove select row
            dtGridClient.Rows[dtGridClient.CurrentRow.Index].Selected = false;
        }

        /// <summary>
        /// c'est event chercher a client a partir du date, nom ou id client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //txtSearch.CharacterCasing = CharacterCasing.Upper;
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                dtGridClient.Rows.Clear();
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    String id = String.Format("N{0:D4}", long.Parse(tb.Rows[i][0].ToString()));
                    String dt = String.Format("{0:dd/MM/yyyy}", tb.Rows[i][2].ToString());
                    if (tb.Rows[i][1].ToString().Contains(value: txtSearch.Text.ToUpper()) ||
                        id.Contains(value: txtSearch.Text) ||
                        dt.Contains(value: txtSearch.Text))
                        dtGridClient.Rows.Add(
                        String.Format("N{0:D4}", long.Parse(tb.Rows[i][0].ToString())),
                        tb.Rows[i][1].ToString(),
                        String.Format("{0:dd/MM/yyyy}", tb.Rows[i][2].ToString()),
                        tb.Rows[i][3].ToString(),
                        "",
                        tb.Rows[i][4].ToString(),
                        tb.Rows[i][5].ToString(),
                        tb.Rows[i][6].ToString(),
                        tb.Rows[i][7].ToString());
                }
            }
            else
                remplissageDtGridClient();
        }

        /// <summary>
        /// c'est le button qui modifier les factures du client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void e_Edit_Factures_Click(object sender, EventArgs e)
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
                            ", typeMetres = '" + cmbTypeDuMetres.SelectedItem.ToString() + "'" +
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
                            commandM.CommandText = "INSERT INTO mesure(idFacture, quantite, largeur, longueur, eppaiseur) " +
                                "VALUES('" + long.Parse(cmbNumeroFacture.Text) + "','" +
                                double.Parse(dtGMesure.Rows[i].Cells[0].Value.ToString()) + "','" +
                                double.Parse(dtGMesure.Rows[i].Cells[1].Value.ToString()) + "','" +
                                double.Parse(dtGMesure.Rows[i].Cells[2].Value.ToString()) + "','" +
                                (cmbTypeDuMetres.SelectedItem.Equals("m3") ? double.Parse(dtGMesure.Rows[i].Cells[3].Value.ToString()) : 0) + "')";
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

                    message = new FormMessage("Modifier Avec Succès", "Succès", true, FontAwesome.Sharp.IconChar.CheckCircle);
                    message.ShowDialog();

                    connectionClient.Close();

                    loadDataEdit(long.Parse(cmbNumeroFacture.Text));

                }
                catch (Exception ex)
                {
                    message = new FormMessage("Erreur: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                    message.ShowDialog();
                    connectionClient.Close();
                }
            }

        }

        /// <summary>
        /// closing the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (verifyForm)
            {
                message = new FormMessage("Vous avez des données non enregistrées, si vous souhaitez les enregistrer, cliquez sur le bouton Sauvgarder les factures", "Attention", true, true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                if (DialogResult.Yes == message.ShowDialog())
                {
                    btnSaveClient.PerformClick();
                    Application.Exit();
                }
                else Application.Exit();
            }
        }

        [Obsolete]
        private void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                connectionClient.Open();
                OleDbCommand fctr = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText = "SELECT typeDeBois, dtDateFacture FROM facture order by typeDeBois"
                };
                DataTable tbf = new DataTable();
                tbf.Load(fctr.ExecuteReader());
                connectionClient.Close();

                String s_mdf = "";
                String s_latte = "";
                String s_std = "";
                String s_bois_divirs = "";
                var mdf = new List<String>();
                var latte = new List<String>();
                var std = new List<String>();
                var bois = new List<String>();


                for (int i = 0; i < tbf.Rows.Count; i++)
                {
                    if (DateTime.Parse(tbf.Rows[i][1].ToString()).Year.Equals(DateTime.Today.Year))
                    {
                        if (tbf.Rows[i][0].ToString().Contains("MDF"))
                        {
                            mdf.Add(tbf.Rows[i][0].ToString());
                        }
                        if (tbf.Rows[i][0].ToString().Contains("LATTE"))
                        {
                            latte.Add(tbf.Rows[i][0].ToString());
                        }
                        if (tbf.Rows[i][0].ToString().Contains("STD"))
                        {
                            std.Add(tbf.Rows[i][0].ToString());
                        }
                        if (tbf.Rows[i][0].ToString().Contains("BOIS DIVERS"))
                        {
                            bois.Add(tbf.Rows[i][0].ToString());
                        }
                    }
                }

                var dict_mdf = new Dictionary<String, int>();
                // mdf
                foreach (var value in mdf)
                {
                    if (dict_mdf.ContainsKey(value))
                        dict_mdf[value]++;
                    else
                        dict_mdf[value] = 1;
                }
                foreach (var s in dict_mdf)
                    s_mdf += @"<tr><td colSpan='2' align='center'>" + $"{s.Key}" + $" ({s.Value})" + @"</td><tr>";
                // latte
                var dict_latte = new Dictionary<String, int>();
                foreach (var value in latte)
                {
                    if (dict_latte.ContainsKey(value))
                        dict_latte[value]++;
                    else
                        dict_latte[value] = 1;
                }
                foreach (var s in dict_latte)
                    s_latte += @"<tr><td colSpan='2' align='center'>" + $"{s.Key}" + $" ({s.Value})" + @"</td><tr>";
                // std
                var dict_std = new Dictionary<String, int>();
                foreach (var value in std)
                {
                    if (dict_std.ContainsKey(value))
                        dict_std[value]++;
                    else
                        dict_std[value] = 1;
                }
                foreach (var s in dict_std)
                    s_std += @"<tr><td colSpan='2' align='center'>" + $"{s.Key}" + $" ({s.Value})" + @"</td><tr>";
                // bois divers
                var dict_bois = new Dictionary<String, int>();
                foreach (var value in bois)
                {
                    if (dict_bois.ContainsKey(value))
                        dict_bois[value]++;
                    else
                        dict_bois[value] = 1;
                }
                foreach (var s in dict_bois)
                    s_bois_divirs += @"<tr><td colSpan='2' align='center'>" + $"{s.Key}" + $" ({s.Value})" + @"</td><tr>";

                double a = 0;
                double b = 0;
                double c = 0;

                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    if (tb.Rows[i][6].ToString() == null)
                    {
                        break;
                    }
                    if (DateTime.Parse(tb.Rows[i][2].ToString()).Year.Equals(DateTime.Today.Year))
                    {
                        a += double.Parse(tb.Rows[i][5].ToString());
                        b += double.Parse(tb.Rows[i][6].ToString());
                        c += double.Parse(tb.Rows[i][7].ToString());
                    }
                }

                SaveFileDialog save = new SaveFileDialog();
                String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\aiche bois\\Information annuelle\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                save.InitialDirectory = path;
                save.FileName = DateTime.Today.ToString("dd-MM-yyyy");
                save.Title = "Information sur les montants totaux de la société Aiche Bois";
                save.DefaultExt = "pdf";
                save.Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
                save.FilterIndex = 1;

                if (save.ShowDialog() == DialogResult.OK)
                {
                    Document pdfDoc = new Document(PageSize.A4);

                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(save.FileName, FileMode.Create));
                    pdfDoc.Open();
                    PdfPTable tab1 = new PdfPTable(1);
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(@"Resources\\header.jpg");
                    tab1.WidthPercentage = 100;
                    PdfPCell cel1 = new PdfPCell(image, true);
                    cel1.BorderColor = iTextSharp.text.BaseColor.WHITE;

                    tab1.SpacingAfter = 30;
                    tab1.AddCell(cel1);
                    pdfDoc.Add(tab1);

                    string strHTML = @"<!DOCTYPE html>  
                        <html xmlns='http://www.w3.org/1999/xhtml'>  
                        <head>  
                            <title></title>  
                        </head>  
                        <body>  
                           <table width ='100%' style='font-family: 'Ubuntu', sans-serif;'>
                                <!-- header -->
                                <tr align='center' style='font-weight: bold;'>  
                                    <td align='center'>Les Statistique de la Societe Aiche Bois</td>
                               </tr>
                               <tr style='font-weight: bold;'>  
                                    <td align='center'>" + DateTime.Today.ToString("yyyy") + @"</td><br>
                               </tr>
                            </table>
                            <table>
                                <tr><td align='center'>Montant Total/Client</td></tr>
                            </table>
                            <table border='1' style='font-family: 'Ubuntu', sans-serif;'>
                               <tr>
                                    <td align='left'>Prix ​​Total Payé</td>
                                    <td align='center'>" + $"{a} dh" + @"</td>  
                               </tr>
                               <tr>
                                    <td align='left'>Total Restant</td>
                                    <td align='center'>" + $"{b} dh" + @"</td>  
                               </tr>
                               <tr>
                                    <td align='left'>Montant Total</td>
                                    <td align='center'>" + $"{c} dh" + @"</td>              
                               </tr>
                            </table>
                            <!-- type du bois --><br><br>
                            <table>
                                <tr><td align='center'>Nombre total de bois vendu par Type/Facture</td></tr>
                            </table>
                            <table border='1' style='font-family: 'Ubuntu', sans-serif;'>
                               <tr>
                                    <td align='left'>MDF</td>
                                    <td align='center'>" + $"{mdf.Count}" + @"</td>  
                               </tr>
                                " + $"{s_mdf}" + @"
                            </table>
                            <br>    
                            <table border='1' style='font-family: 'Ubuntu', sans-serif;'>
                               <tr>
                                    <td align='left'>LATTE</td>
                                    <td align='center'>" + $"{latte.Count}" + @"</td>  
                               </tr>
                               " + $"{s_latte}" + @"
                            </table>
                            <br>
                            <table border='1' style='font-family: 'Ubuntu', sans-serif;'>
                               <tr>
                                    <td align='left'>STD</td>
                                    <td align='center'>" + $"{std.Count}" + @"</td>              
                               </tr>
                                " + $"{s_std}" + @"
                            </table>
                            <br>
                            <table border='1' style='font-family: 'Ubuntu', sans-serif;'>
                               <tr>
                                    <td align='left'>BOIS DIVERS</td>
                                    <td align='center'>" + $"{bois.Count}" + @"</td>              
                               </tr>
                                " + $"{s_bois_divirs}" + @"
                            </table>
                        </body>  
                        </html>";
                    HTMLWorker htmlWorker = new HTMLWorker(pdfDoc);
                    htmlWorker.Parse(new StringReader(strHTML));
                    pdfWriter.CloseStream = true;
                    pdfDoc.Close();
                    FileInfo fileInfo = new FileInfo(save.FileName);
                    fileInfo.IsReadOnly = true;
                    message = new FormMessage("Les statistiques ont été enregistrées dans " + save.FileName, "Succès", true, FontAwesome.Sharp.IconChar.CheckCircle);
                    message.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                connectionClient.Close();
            }
        }

        private void dtGridFacture_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                dtGridClient.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(47, 47, 47);
                dtGridClient.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(255, 244, 228);
            }
        }

        private void dtGridFacture_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                dtGridClient.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 244, 228);
                dtGridClient.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(47, 47, 47);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void tableLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture(); /*function*/
            SendMessage(this.Handle, 0x112, 0xf012, 0);  /*function*/
        }

        /// <summary>
        /// home pannel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_home_Click(object sender, EventArgs e)
        {
            if (verifyForm)
            {
                message = new FormMessage("Vous avez des données non enregistrées, si vous souhaitez les enregistrer, cliquez sur le bouton Sauvgarder les factures", "Attention", true, true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                if (DialogResult.Yes == message.ShowDialog())
                {
                    btnSaveClient.PerformClick();
                    verifyForm = false;
                }
                else
                {
                    factures.Clear();
                }
            }

            // initilize idClient
            idClient = null;
            
            // refrech main datagrid
            remplissageDtGridClient();

            //visible panel and bring to frot
            btnAddClient.Enabled = true;
            btnDeleteClient.Enabled = true;
            btnEditClient.Enabled = true;
            btnPrintClient.Enabled = true;

            // remove event for button save facture
            btnAddFacture.Click -= new EventHandler(this.e_Add_New_Facture_Client_Click);
            btnAddFacture.Click -= new EventHandler(this.e_Edit_Factures_Click);
            btnAddFacture.Click -= new EventHandler(this.e_Add_New_Client_Click);

            // change event for text changed on this texts boxes
            txtPrixAvanceClient.TextChanged -= new EventHandler(this.txtPrixRestClient_TextChanged);
            txtPrixTotalClient.TextChanged -= new EventHandler(this.txtPrixRestClient_TextChanged);

            // bring home panel
            p_Add_Edit.SendToBack();
            p_home.BringToFront();

        }

        /// <summary>
        /// this is button edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dtGridClient.Rows.Count <= 0 || idClient == null)
            {
                message = new FormMessage("selectionner une ligne s'il vous plait", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            try
            {
                /*remple comboBox PVC*/
                RemplirComboBxPvc();

                btnAddClient.Enabled = false;
                btnDeleteClient.Enabled = false;
                btnPrintClient.Enabled = false;
                btnEditClient.Enabled = false;

                /*initialise combo box*/
                btnImprimerFacture.Visible = true;
                btnDeleteFacture.Visible = true;
                btnAddNewFacture.Visible = true;

                btnSaveClient.Visible = false;

                cmbTypeDuMetres.SelectedItem = "m2";
                cmbTypeDeBois.SelectedIndex = 0;
                dtDateFacture.Value = DateTime.Today;
                checkAvance.Checked = false;
                txtPrixAvanceClient.Enabled = false;
                txtPrixAvanceClient.Text = "0.00";
                cmbOrtnPVC.SelectedIndex = 0;

                cmbNumeroFacture.DropDownStyle = ComboBoxStyle.DropDownList;
                lblNumeroFacture.Text = "Selectionner la Facture";
                btnAddFacture.IconChar = FontAwesome.Sharp.IconChar.Edit;
                btnAddFacture.Text = "Modifier";

                txtNomClient.Enabled = false;
                dtDateFacture.Enabled = false;
                checkAvance.Enabled = false;
                lstTypeBois.SelectedItem = null;

                //desactiver le champ de pre avance
                txtPrixAvanceClient.Enabled = false;

                // change event for btn save
                btnAddFacture.Click -= new EventHandler(this.e_Add_New_Facture_Client_Click);
                btnAddFacture.Click -= new EventHandler(this.e_Add_New_Client_Click);
                btnAddFacture.Click += new EventHandler(this.e_Edit_Factures_Click);

                // change event for text changed on this texts boxes
                txtPrixAvanceClient.TextChanged -= new EventHandler(this.txtPrixRestClient_TextChanged);
                txtPrixTotalClient.TextChanged -= new EventHandler(this.txtPrixRestClient_TextChanged);

                // reset facture count
                cmbNumeroFacture.Items.Clear();

                // opne connection
                connectionClient.Open();

                //facture
                var commandFacture = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText = "SELECT idFacture FROM FACTURE WHERE IDCLIENT = " + long.Parse(idClient[1])
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
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionClient.Close();
            }

            // select the first item on list
            if (cmbNumeroFacture.Items.Count > 0)
                cmbNumeroFacture.SelectedIndex = 0;

            //visible panel and bring to frot
            p_home.SendToBack();
            p_Add_Edit.BringToFront();
        }

        /// <summary>
        /// c'est button afficher la formulaire pour ajouter des factures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddClient_Click(object sender, EventArgs e)
        {
            /*remple comboBox PVC*/
            RemplirComboBxPvc();
            ViderTxtBox();
            txtPrixAvanceClient.Text = "0.00";
            txtTotaleTaillPVC.Text = "0.00";
            txtPrixTotalPVC.Text = "0.00";
            txtPrixTotalMesure.Text = "0.00";
            txtPrixTotalClient.Text = "0.00";
            txtPrixRestClient.Text = "0.00";
            txtPrixMetreMesure.Clear();
            txtCategorie.Clear();
            txtMetrageDeFeuille.Clear();

            txtNomClient.Clear();
            txtNomClient.Enabled = true;
            cmbTypeDuMetres.SelectedItem = "m2";
            cmbTypeDeBois.SelectedIndex = 0;
            dtDateFacture.Value = DateTime.Today;
            checkAvance.Checked = false;
            checkAvance.Enabled = true;
            txtPrixAvanceClient.Enabled = false;
            cmbOrtnPVC.SelectedIndex = 0;
            lstTypeBois.SelectedIndex = lstTypeBois.Items.Count > 0 ? lstTypeBois.SelectedIndex = 0 : lstTypeBois.SelectedIndex = -1;

            // invisible button delete and print facture
            btnImprimerFacture.Visible = false;
            btnDeleteFacture.Visible = false;
            btnAddNewFacture.Visible = false;

            btnSaveClient.Visible = true;

            btnAddClient.Enabled = false;
            btnDeleteClient.Enabled = false;
            btnPrintClient.Enabled = false;
            btnEditClient.Enabled = false;

            cmbNumeroFacture.DropDownStyle = ComboBoxStyle.Simple;
            lblNumeroFacture.Text = "Selectionner la Facture";
            idFacture();

            btnAddFacture.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            btnAddFacture.Text = "Ajouter";

            // change event for btn save
            btnAddFacture.Click -= new EventHandler(this.e_Add_New_Facture_Client_Click);
            btnAddFacture.Click -= new EventHandler(this.e_Edit_Factures_Click);
            btnAddFacture.Click += new EventHandler(this.e_Add_New_Client_Click);

            // change event for text changed on this texts boxes
            txtPrixAvanceClient.TextChanged += new EventHandler(this.txtPrixRestClient_TextChanged);
            txtPrixTotalClient.TextChanged += new EventHandler(this.txtPrixRestClient_TextChanged);

            //visible panel and bring to frot
            p_home.SendToBack();
            p_Add_Edit.BringToFront();

            txtNomClient.TabIndex = 0;
            lstTypeBois.TabIndex = 1;
            txtPrixMetreMesure.TabIndex = 2;
            cmbTypeDuMetres.TabIndex = 3;
            txtQuantite.TabIndex = 4;
            txtLargeur.TabIndex = 5;
            txtLongueur.TabIndex = 6;
            txtEpaisseur.TabIndex = 7;
            btnAddMesure.TabIndex = 8;
        }
        /**
         * ======================== Panel Add ========================
         */
        private void cmbNumeroFacture_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDataEdit(long.Parse(cmbNumeroFacture.SelectedItem.ToString()));
        }

        private void cmbTypeDeBois_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.btnAddTypeClick = cmbTypeDeBois.Text;
            remplirListe(cmbTypeDeBois.Text);
            /*remplirLstTypeBois();*/
            if (lstTypeBois.Items.Count <= 0)
            {
                txtTypeDeBois.Text = "";
                return;
            }
            lstTypeBois.SelectedIndex = 0;
            txtTypeDeBois.Text = lstTypeBois.SelectedItem.ToString();
        }

        private void btnCmbCategorie_Click(object sender, EventArgs e)
        {
            Program.btnAddTypeClick = cmbTypeDeBois.Text.ToUpper();

            FormAjout ajout = new FormAjout();
            ajout.ShowDialog();

            /*remplirLstTypeBois();*/
            remplirListe(Program.btnAddTypeClick);
        }

        private void txtSearchFacture_TextChanged(object sender, EventArgs e)
        {
            /*si le textBox est vide, remplir tous les items*/
            if (string.IsNullOrEmpty(txtSearchFacture.Text))
            {
                remplirListe(cmbTypeDeBois.Text);
            }
            else
            {
                lstTypeBois.Items.Clear();
                for (int i = 0; i < tb_Type.Rows.Count; i++)
                {
                    if (tb_Type.Rows[i][0].ToString().Contains(txtSearchFacture.Text.ToUpper()))
                    {
                        lstTypeBois.Items.Add(tb_Type.Rows[i][0].ToString());
                    }
                }
            }

            /*selectionner le premiere ligne*/
            if (lstTypeBois.Items.Count != 0)
            {
                lstTypeBois.SelectedIndex = 0;
            }
        }

        private void lstTypeBois_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTypeBois.Items.Count <= 0 || lstTypeBois.SelectedIndex == -1)
            {
                txtTypeDeBois.Text = "";
                return;
            }
            txtTypeDeBois.Text = lstTypeBois.SelectedItem.ToString();
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

        private void txtPrixMetreMesure_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cmbTypeDuMetres_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTotalMesure.Enabled = false;
            btnAddMesure.Enabled = true;
            btnDeleteMesure.Enabled = true;
            btnExportCsv.Enabled = true;

            txtMetrageDeFeuille.Enabled = txtCategorie.Enabled = cmbTypePvc.Enabled = btncmbNbrCanto.Enabled =
                btnCmbCategorie.Enabled = cmbTypeDeBois.Enabled = txtSearch.Enabled = lstTypeBois.Enabled = true;

            //txtTypeDeBois.Text = "";
            //txtMetrageDeFeuille.Clear();
            //txtCategorie.Clear();

            if (cmbTypeDuMetres.SelectedItem.Equals("feuille"))
            {
                lblTypeDuMetres.Text = "Prix ​​au feuille";
                lblMesure.Text = "Nombre de Feuilles";
                lblEpaisseur.Visible = false;
                txtEpaisseur.Visible = false;
                if (dtGMesure.ColumnCount > 3)
                {
                    dtGMesure.ColumnCount -= 1;
                }
                enabledMesure(true);
                txtTotalMesure.Enabled = true;
                txtTotalMesure.Focus();
            }
            else if (cmbTypeDuMetres.SelectedItem.Equals("m"))
            {
                lblTypeDuMetres.Text = "Prix ​​au Mètres Linéaires";
                lblMesure.Text = "Taille en mètres linéaires";
                lblEpaisseur.Visible = false;
                txtEpaisseur.Visible = false;
                if (dtGMesure.ColumnCount > 3)
                {
                    dtGMesure.ColumnCount -= 1;
                }
                enabledMesure(false);

                txtMetrageDeFeuille.Enabled = txtCategorie.Enabled = cmbTypePvc.Enabled = btncmbNbrCanto.Enabled =
                btnCmbCategorie.Enabled = cmbTypeDeBois.Enabled = txtSearch.Enabled = lstTypeBois.Enabled = false;

                txtTypeDeBois.Text = "---m---";
                txtMetrageDeFeuille.Text = "---m---";
                txtCategorie.Text = "---m---";

                btnAddMesure.Enabled = false;
                btnDeleteMesure.Enabled = false;
                btnExportCsv.Enabled = false;
                txtTotalMesure.Enabled = true;
                txtTotalMesure.Focus();
            }
            else if (cmbTypeDuMetres.SelectedItem.Equals("m2"))
            {
                lblTypeDuMetres.Text = "Prix ​​au mètre carré";
                lblMesure.Text = "Volume Total de la Mesure de mètre carré";
                lblEpaisseur.Visible = false;
                txtEpaisseur.Visible = false;
                if (dtGMesure.ColumnCount > 3)
                {
                    dtGMesure.ColumnCount -= 1;
                }
                enabledMesure(true);
                txtQuantite.Focus();
            }
            else if (cmbTypeDuMetres.SelectedItem.Equals("m3"))
            {
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
                enabledMesure(true);
                txtQuantite.Focus();
            }
        }

        private void btnAddMesure_Click(object sender, EventArgs e)
        {
            //verifier si le champs contain un chifre ou vide 
            if (string.IsNullOrEmpty(txtQuantite.Text) || double.Parse(txtQuantite.Text) == 0)
            {
                message = new FormMessage("n'accept pas 0, saisir la valeur de " + txtQuantite.Tag, "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtQuantite.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtLargeur.Text) || double.Parse(txtLargeur.Text) == 0)
            {
                message = new FormMessage("n'accept pas 0, saisir la valeur de " + txtLargeur.Tag, "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtLargeur.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtLongueur.Text) || double.Parse(txtLongueur.Text) == 0)
            {
                message = new FormMessage("n'accept pas 0, saisir la valeur de " + txtLongueur.Tag, "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtLongueur.Focus();
                return;
            }

            for (int i = 0; i < dtGMesure.Rows.Count; i++)
            {
                if (cmbTypeDuMetres.SelectedItem.Equals("m3"))
                {
                    if (double.Parse(txtQuantite.Text) == double.Parse(dtGMesure.Rows[i].Cells[0].Value.ToString()) &&
                        double.Parse(txtLargeur.Text) == double.Parse(dtGMesure.Rows[i].Cells[1].Value.ToString()) &&
                        double.Parse(txtLongueur.Text) == double.Parse(dtGMesure.Rows[i].Cells[2].Value.ToString()) &&
                        double.Parse(txtEpaisseur.Text) == double.Parse(dtGMesure.Rows[i].Cells[3].Value.ToString()))
                    {
                        message = new FormMessage("c'est mesure exist deja", "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                        message.ShowDialog();
                        return;
                    }
                }
                else
                {
                    if (double.Parse(txtQuantite.Text) == double.Parse(dtGMesure.Rows[i].Cells[0].Value.ToString()) &&
                      double.Parse(txtLargeur.Text) == double.Parse(dtGMesure.Rows[i].Cells[1].Value.ToString()) &&
                      double.Parse(txtLongueur.Text) == double.Parse(dtGMesure.Rows[i].Cells[2].Value.ToString()))
                    {
                        message = new FormMessage("c'est mesure exist deja", "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                        message.ShowDialog();
                        return;
                    }
                }
            }

            if (cmbTypeDuMetres.SelectedItem.Equals("m3"))
            {
                //verifier si le champs contain un chifre ou vide
                if (string.IsNullOrEmpty(txtEpaisseur.Text) || double.Parse(txtEpaisseur.Text) == 0)
                {
                    message = new FormMessage("n'accept pas 0, saisir la valeur de " + txtEpaisseur.Tag, "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
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

        private void btnDeleteMesure_Click(object sender, EventArgs e)
        {
            if (dtGMesure.SelectedRows.Count <= 0)
            {
                message = new FormMessage("selectionnes une ligne pour supprimer", "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            dtGMesure.Rows.RemoveAt(dtGMesure.CurrentRow.Index);

            RemplirDataMesure();
            message = new FormMessage("supprimer avec succes", "Supprimer !!!", true, FontAwesome.Sharp.IconChar.CheckCircle);
            message.ShowDialog();
        }

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
                            message = new FormMessage("It wasn't possible to write the data to the disk" + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                            message.ShowDialog();
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

                            File.WriteAllLines(sfd.FileName, outputCsv, System.Text.Encoding.UTF8);
                            message = new FormMessage("Données exportées avec succès !!!", "Succès !!!", true, FontAwesome.Sharp.IconChar.CheckCircle);
                            message.ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            message = new FormMessage("Error :" + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                            message.ShowDialog();
                        }
                    }
                }
            }
            else
            {
                message = new FormMessage("La List est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }
        }

        private void cmbTypePvc_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtGridPvc.Enabled = btnImportPvc.Enabled = btnSavePvc.Enabled = chSeulPVC.Enabled =
            txtTaillePVC.Enabled = txtPrixMetreLPVC.Enabled = cmbTypePvc.SelectedIndex < 0 ? false : true;
        }

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
            cmbTypePvc.SelectedIndex = cmbTypePvc.Items.Count - 1;
        }

        private void chSeulPVC_CheckedChanged(object sender, EventArgs e)
        {
            checkSeulPVC(chSeulPVC.Checked);
        }

        private void btnAddSeulPVC_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtQtePVC.Text))
            {
                message = new FormMessage(txtQtePVC.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtQtePVC.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtLargPVC.Text))
            {
                message = new FormMessage(txtLargPVC.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtLargPVC.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtLongPVC.Text))
            {
                message = new FormMessage(txtLongPVC.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
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
                    message = new FormMessage("les valeur est le meme deja saisir", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
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

        private void btnDeleteSeulPVC_Click(object sender, EventArgs e)
        {
            if (dtGridPvc.SelectedRows.Count <= 0)
            {
                message = new FormMessage("selectionnes une ligne pour supprimer", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            dtGridPvc.Rows.RemoveAt(dtGridPvc.CurrentRow.Index);
            btnSaveCalculPvc();
            if (dtGridPvc.Rows.Count == 0)
            {
                txtTotaleTaillPVC.Text = "0.00";
            }
        }

        private void btnImportPvc_Click(object sender, EventArgs e)
        {
            dtGridPvc.Rows.Clear();
            for (int i = 0; i < dtGMesure.Rows.Count; i++)
            {
                dtGridPvc.Rows.Add(
                    dtGMesure.Rows[i].Cells[0].Value,
                    dtGMesure.Rows[i].Cells[1].Value,
                    dtGMesure.Rows[i].Cells[2].Value);
                txtTotaleTaillPVC.Text = "";
            }
        }

        private void btnSavePvc_Click(object sender, EventArgs e)
        {
            btnSaveCalculPvc();
        }

        private void txtTotaleTaillPVC_Click(object sender, EventArgs e)
        {

        }

        private void txtPrixMetreLPVC_TextChanged(object sender, EventArgs e)
        {
            double prix_mettres_pvc = 0;
            if (String.IsNullOrEmpty(txtPrixMetreLPVC.Text) || String.IsNullOrEmpty(txtTotaleTaillPVC.Text))
                return;
            prix_mettres_pvc = double.Parse(txtPrixMetreLPVC.Text) * double.Parse(txtTotaleTaillPVC.Text);
            txtPrixTotalPVC.Text = prix_mettres_pvc.ToString("F2");
        }

        private void e_Add_New_Client_Click(object sender, EventArgs e)
        {
            if (checkIsNullOrEmpty())
            {
                Facture facture = new Facture();

                List<Mesure> vs = new List<Mesure>();

                for (int i = 0; i < dtGMesure.Rows.Count; i++)
                {
                    vs.Add(new Mesure(facture.IDFacture, Convert.ToDouble(dtGMesure.Rows[i].Cells[0].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[1].Value), Convert.ToDouble(dtGMesure.Rows[i].Cells[2].Value), cmbTypeDuMetres.SelectedItem.Equals("m3") ? Convert.ToDouble(dtGMesure.Rows[i].Cells[3].Value) : 0));
                }

                facture.Mesures = vs;
                facture.DateFacture = dtDateFacture.Value;
                facture.TypeDeBois = txtTypeDeBois.Text;
                facture.Categorie = txtCategorie.Text;
                facture.Metrage = txtMetrageDeFeuille.Text;
                facture.TypeMetres = cmbTypeDuMetres.SelectedItem.ToString();
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
                    facture.TailleCanto = double.Parse(txtTaillePVC.Text);
                    facture.PrixMitresLinear = double.Parse(txtPrixMetreLPVC.Text);
                    facture.TotalTaillPVC = double.Parse(txtTotaleTaillPVC.Text);
                    facture.PrixTotalPVC = double.Parse(txtPrixTotalPVC.Text);
                }
                else
                {
                    facture.TypePVC = "---";
                    facture.CheckPVC = chSeulPVC.Checked;
                    facture.TailleCanto = 0.0;
                    facture.PrixMitresLinear = 0.0;
                    facture.TotalTaillPVC = 0.0;
                    facture.PrixTotalPVC = 0.0;
                }

                factures.Add(facture);

                message = new FormMessage("facture ajouter avec succès", "Succès", true, FontAwesome.Sharp.IconChar.CheckCircle);
                message.ShowDialog();

                // miss a jour l'id
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

                verifyForm = true;
                btnSaveClient.Enabled = true;
            }
        }


        private void btnAddNewFacture_Click(object sender, EventArgs e)
        {
            ViderTxtBox();
            cmbNumeroFacture.Enabled = false;
            btnAddFacture.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            btnAddFacture.Text = "Ajouter";

            // change event for btn save
            btnAddFacture.Click -= new EventHandler(this.e_Add_New_Client_Click);
            btnAddFacture.Click -= new EventHandler(this.e_Edit_Factures_Click);
            btnAddFacture.Click += new EventHandler(this.e_Add_New_Facture_Client_Click);

            // change event for text changed on this texts boxes
            txtPrixAvanceClient.TextChanged += new EventHandler(this.txtPrixRestClient_TextChanged);
            txtPrixTotalClient.TextChanged += new EventHandler(this.txtPrixRestClient_TextChanged);
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
                message = new FormMessage("le prix d'avance est grand que le prix total", "Attension", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtPrixAvanceClient.Text = "0.00";
                txtPrixRestClient.Text = "0.00";
                checkAvance.Checked = false;
            }
            else
                txtPrixRestClient.Text = (total - avance).ToString("F2");

        }

        private void e_Add_New_Facture_Client_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkIsNullOrEmpty())
                {
                    connectionClient.Open();

                    if (chSeulPVC.Checked && cmbTypePvc.SelectedItem != null)
                    {
                        var mnd1 = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "INSERT INTO facture (idClient, dtDateFacture, typeDeBois, " +
                            "metrage, categorie, totalMesure, typeMetres, prixMetres, typePVC, checkPVC, tailleCanto, " +
                            "totalTaillPVC, prixMitresLinear, prixTotalPVC, prixTotalMesure) " +
                                          "values ('" + long.Parse(idClient[1]) + "', '" + DateTime.Today + "', '" +
                                          "---" + "', '" + "---" + "','" + "---" + "', '" +
                                          0.0 + "', '" + cmbTypeDuMetres.SelectedItem.ToString() + "','" + 0.0 + "', '" +
                                          cmbTypePvc.Text + "', " + chSeulPVC.Checked + ", '" + double.Parse(txtTaillePVC.Text) + "', '" +
                                          double.Parse(txtTotaleTaillPVC.Text) + "', '" + double.Parse(txtPrixMetreLPVC.Text) + "', '" +
                                          double.Parse(txtPrixTotalPVC.Text) + "', '" + 0.0 + "')"
                        };
                        mnd1.ExecuteNonQuery();

                        long idFS = 0;
                        /*get idClient from database*/
                        OleDbCommand commF = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "SELECT TOP 1 idFacture FROM facture WHERE idClient = " + long.Parse(idClient[1]) + " ORDER BY idFacture DESC"
                        };
                        OleDbDataReader readF = commF.ExecuteReader();
                        while (readF.Read())
                        {
                            idFS = Convert.ToInt64(readF["idFacture"]);
                        }

                        for (int i = 0; i < dtGridPvc.Rows.Count; i++)
                        {
                            OleDbCommand commandP = new OleDbCommand();
                            commandP.Connection = connectionClient;
                            commandP.CommandText = "INSERT INTO pvc (idFacture, quantite, largeur, longueur, orientation) " +
                                "values('" + idFS + "','" +
                                double.Parse(dtGridPvc.Rows[i].Cells[0].Value.ToString()) + "','" +
                                double.Parse(dtGridPvc.Rows[i].Cells[1].Value.ToString()) + "','" +
                                double.Parse(dtGridPvc.Rows[i].Cells[2].Value.ToString()) + "','" +
                                dtGridPvc.Rows[i].Cells[3].Value.ToString() + "')";
                            commandP.ExecuteNonQuery();
                        }
                    }
                    else if (cmbTypePvc.SelectedItem == null)
                    {
                        var mnd = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "INSERT INTO facture (idClient, dtDateFacture, typeDeBois, " +
                            "metrage, categorie, totalMesure, typeMetres, prixMetres, typePVC, checkPVC, tailleCanto, " +
                            "totalTaillPVC, prixMitresLinear, prixTotalPVC, prixTotalMesure) " +
                                          "values ('" + long.Parse(idClient[1]) + "', '" + DateTime.Today + "', '" +
                                          txtTypeDeBois.Text + "', '" + txtMetrageDeFeuille.Text + "','" + txtCategorie.Text + "', '" +
                                          double.Parse(txtTotalMesure.Text) + "','" + cmbTypeDuMetres.SelectedItem.ToString() + "', '" +
                                          double.Parse(txtPrixMetreMesure.Text) + "', '" +
                                          "---" + "', " + chSeulPVC.Checked + ", '" + 0.0 + "', '" +
                                          0.0 + "', '" + 0.0 + "', '" +
                                          0.0 + "', '" + double.Parse(txtPrixTotalMesure.Text) + "')"
                        };
                        mnd.ExecuteNonQuery();


                        // mesure
                        if (!chSeulPVC.Checked && dtGMesure.Rows.Count > 0 && !cmbTypeDuMetres.SelectedItem.Equals("m"))
                        {
                            long idFM = 0;
                            /*get idClient from database*/
                            OleDbCommand commandIdFacture = new OleDbCommand
                            {
                                Connection = connectionClient,
                                CommandText = "SELECT TOP 1 idFacture FROM facture WHERE idClient = " + long.Parse(idClient[1]) + " ORDER BY idFacture DESC"
                            };
                            OleDbDataReader readerIdFacture = commandIdFacture.ExecuteReader();
                            while (readerIdFacture.Read())
                            {
                                idFM = Convert.ToInt64(readerIdFacture["idFacture"]);
                            }

                            for (int i = 0; i < dtGMesure.Rows.Count; i++)
                            {
                                OleDbCommand commandM = new OleDbCommand();
                                commandM.Connection = connectionClient;
                                commandM.CommandText = "INSERT INTO mesure(idFacture, quantite, largeur, longueur, eppaiseur)  " +
                                    "VALUES('" + idFM + "', '" +
                                    double.Parse(dtGMesure.Rows[i].Cells[0].Value.ToString()) + "', '" +
                                    double.Parse(dtGMesure.Rows[i].Cells[1].Value.ToString()) + "', '" +
                                    double.Parse(dtGMesure.Rows[i].Cells[2].Value.ToString()) + "', '" +
                                    (cmbTypeDuMetres.SelectedItem.Equals("m3") ? double.Parse(dtGMesure.Rows[i].Cells[3].Value.ToString()) : 0) + "')";
                                commandM.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        var mnd = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "insert into facture (idClient, dtDateFacture, typeDeBois, " +
                            "metrage, categorie, totalMesure, typeMetres, prixMetres, typePVC, checkPVC, tailleCanto, " +
                            "totalTaillPVC, prixMitresLinear, prixTotalPVC, prixTotalMesure) " +
                                          "values ('" + long.Parse(idClient[1]) + "', '" + DateTime.Today + "', '" +
                                          txtTypeDeBois.Text + "', '" + txtMetrageDeFeuille.Text + "','" + txtCategorie.Text + "', '" +
                                          double.Parse(txtTotalMesure.Text) + "','" + cmbTypeDuMetres.SelectedItem.ToString() + "', '" + double.Parse(txtPrixMetreMesure.Text) + "', '" +
                                          cmbTypePvc.Text + "', " + chSeulPVC.Checked + ", '" + double.Parse(txtTaillePVC.Text) + "', '" +
                                          double.Parse(txtTotaleTaillPVC.Text) + "', '" + double.Parse(txtPrixMetreLPVC.Text) + "', '" +
                                          double.Parse(txtPrixTotalPVC.Text) + "', '" + double.Parse(txtPrixTotalMesure.Text) + "')"
                        };
                        mnd.ExecuteNonQuery();

                        long idFMS = 0;
                        /*get idClient from database*/
                        OleDbCommand commandIdFacture = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "SELECT TOP 1 idFacture FROM facture WHERE idClient = " + long.Parse(idClient[1]) + " ORDER BY idFacture DESC"
                        };
                        OleDbDataReader readerIdFacture = commandIdFacture.ExecuteReader();
                        while (readerIdFacture.Read())
                        {
                            idFMS = Convert.ToInt64(readerIdFacture["idFacture"]);
                        }

                        // mesure
                        if (!chSeulPVC.Checked && dtGMesure.Rows.Count > 0 && !cmbTypeDuMetres.SelectedItem.Equals("m"))
                        {
                            for (int i = 0; i < dtGMesure.Rows.Count; i++)
                            {
                                OleDbCommand commandM = new OleDbCommand();
                                commandM.Connection = connectionClient;
                                commandM.CommandText = "INSERT INTO mesure(idFacture, quantite, largeur, longueur, eppaiseur)  " +
                                    "VALUES('" + idFMS + "','" +
                                    double.Parse(dtGMesure.Rows[i].Cells[0].Value.ToString()) + "','" +
                                    double.Parse(dtGMesure.Rows[i].Cells[1].Value.ToString()) + "','" +
                                    double.Parse(dtGMesure.Rows[i].Cells[2].Value.ToString()) + "','" +
                                    (cmbTypeDuMetres.Text == "m3" ? double.Parse(dtGMesure.Rows[i].Cells[3].Value.ToString()) : 0) + "')";
                                commandM.ExecuteNonQuery();
                            }
                        }

                        for (int i = 0; i < dtGridPvc.Rows.Count; i++)
                        {
                            OleDbCommand commandP = new OleDbCommand();
                            commandP.Connection = connectionClient;
                            commandP.CommandText = "INSERT INTO pvc (idFacture, quantite, largeur, longueur, orientation) " +
                                "values('" + idFMS + "','" +
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
                        CommandText = "SELECT idFacture FROM FACTURE WHERE IDCLIENT = " + long.Parse(idClient[1])
                    };
                    OleDbDataReader readerFe = commandFe.ExecuteReader();
                    cmbNumeroFacture.Enabled = true;
                    cmbNumeroFacture.Items.Clear();
                    while (readerFe.Read())
                    {
                        // get list of factures
                        cmbNumeroFacture.Items.Add(long.Parse(readerFe["IDFACTURE"].ToString()));
                    }

                    connectionClient.Close();

                    cmbNumeroFacture.SelectedIndex = cmbNumeroFacture.Items.Count - 1;
                    btnAddFacture.IconChar = FontAwesome.Sharp.IconChar.Edit;
                    btnAddFacture.Text = "Modifier";

                    // change event for btn save
                    btnAddFacture.Click -= new EventHandler(this.e_Add_New_Facture_Client_Click);
                    btnAddFacture.Click -= new EventHandler(this.e_Add_New_Client_Click);
                    btnAddFacture.Click += new EventHandler(this.e_Edit_Factures_Click);

                    // change event for text changed on this texts boxes
                    txtPrixAvanceClient.TextChanged += new EventHandler(this.txtPrixRestClient_TextChanged);
                    txtPrixTotalClient.TextChanged += new EventHandler(this.txtPrixRestClient_TextChanged);

                    message = new FormMessage("Ajouté Avec Succès", "Succès", true, FontAwesome.Sharp.IconChar.CheckCircle);
                    message.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionClient.Close();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ViderTxtBox();
        }

        private void btnDeleteFacture_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbNumeroFacture.Items.Count <= 0)
                {
                    message = new FormMessage("le client a aucun facture", "Erreur !!!", true, FontAwesome.Sharp.IconChar.Ban);
                    message.ShowDialog();
                    return;
                }

                // verifier que la liste des facture contient plus d'un un facture
                if (cmbNumeroFacture.Items.Count <= 1)
                {
                    message = new FormMessage("Vous ne pouvez pas supprimer cette facture car le client n'a qu'une seule facture, vous devez supprimer le client.", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    return;
                }

                message = new FormMessage("Voulez-vous vraiment supprimer cette facture", "Question", true, true, FontAwesome.Sharp.IconChar.Question);

                if (message.ShowDialog() == DialogResult.No)
                    return;

                connectionClient.Open();

                double result = double.Parse(txtPrixTotalMesure.Text) + double.Parse(txtPrixTotalPVC.Text);

                if (result >= double.Parse(txtPrixAvanceClient.Text))
                {
                    result = 0;
                }

                //if (result >= double.Parse(txtPrixRestClient.Text))
                //{
                //    result -= double.Parse(txtPrixRestClient.Text);
                //}

                OleDbCommand commandFacture = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText = "DELETE * FROM FACTURE WHERE idFacture = " + cmbNumeroFacture.SelectedItem
                };
                commandFacture.ExecuteNonQuery();

                OleDbCommand commandFt = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText = "UPDATE client SET prixTotalAvance = prixTotalAvance - " + result + " WHERE idClient = " + long.Parse(idClient[1])
                };
                commandFt.ExecuteNonQuery();

                connectionClient.Close();

                cmbNumeroFacture.Items.Remove(cmbNumeroFacture.SelectedItem);
                message = new FormMessage("La facture a été supprimée avec succès", "Succès !!!", true, FontAwesome.Sharp.IconChar.CheckCircle);
                message.ShowDialog();

                if (cmbNumeroFacture.Items.Count >= 0)
                    cmbNumeroFacture.SelectedIndex = 0;
                else
                    ViderTxtBox();
                connectionClient.Close();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur !!!", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionClient.Close();
            }
        }

        private void btnImprimerFacture_Click(object sender, EventArgs e)
        {
            if (cmbNumeroFacture.Items.Count == 0)
                return;
            FormPrint printPdf = new FormPrint(idClient[1], cmbNumeroFacture.SelectedItem.ToString(), "btnPrintFacture", chSeulPVC.Checked);
            printPdf.ShowDialog();
        }

        private void txtPrixAvanceClient_TextChanged(object sender, EventArgs e)
        {
            double total = 0;
            double avance = 0;

            if (String.IsNullOrEmpty(txtPrixTotalClient.Text) || String.IsNullOrEmpty(txtPrixAvanceClient.Text))
                return;

            total = double.Parse(txtPrixTotalClient.Text);

            avance = double.Parse(txtPrixAvanceClient.Text);


            if (avance > total)
            {
                message = new FormMessage("le prix d'avance est grand que le prix total", "Attension", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtPrixAvanceClient.Text = "0.00";
                txtPrixRestClient.Text = "0.00";
                checkAvance.Checked = false;
            }
            else
                txtPrixRestClient.Text = (total - avance).ToString("F2");
        }

        private void checkAvance_CheckedChanged(object sender, EventArgs e)
        {
            txtPrixAvanceClient.Enabled = checkAvance.Checked;
            txtPrixAvanceClient.Text = "0.00";
            txtPrixAvanceClient.Focus();

        }

        private void btnSaveFacture_Click(object sender, EventArgs e)
        {
            verifyForm = false;
            saveDataClient();

            remplissageDtGridClient();

            //visible panel and bring to frot
            btnAddClient.Enabled = true;
            btnDeleteClient.Enabled = true;
            btnEditClient.Enabled = true;
            btnPrintClient.Enabled = true;
            p_Add_Edit.SendToBack();
            p_home.BringToFront();
        }

        private void txtNomClient_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(255, 170, 0);
            ((TextBox)sender).ForeColor = System.Drawing.Color.FromArgb(47, 47, 47);

            if (((TextBox)sender) == txtPrixAvanceClient)
            {
                ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(255, 244, 228);
                ((TextBox)sender).ForeColor = System.Drawing.Color.FromArgb(47, 47, 47);
            }
        }

        private void txtNomClient_Leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(47, 47, 47);
            ((TextBox)sender).ForeColor = System.Drawing.Color.FromArgb(255, 170, 0);

            if (((TextBox)sender) == txtPrixAvanceClient)
            {
                ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(47, 47, 47);
                ((TextBox)sender).ForeColor = System.Drawing.Color.FromArgb(255, 244, 228);
            }
        }

        private void btnSaveFacture_MouseHover(object sender, EventArgs e)
        {
            ((Button)sender).ForeColor = System.Drawing.Color.FromArgb(47, 47, 47);
            ((Button)sender).BackColor = System.Drawing.Color.FromArgb(255, 244, 228);
        }

        private void btnSaveFacture_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).ForeColor = System.Drawing.Color.FromArgb(255, 244, 228);
            ((Button)sender).BackColor = System.Drawing.Color.FromArgb(47, 47, 47);
        }

        private void btnDeleteClient_Click(object sender, EventArgs e)
        {
            if (dtGridClient.Rows.Count <= 0 || idClient == null)
            {
                message = new FormMessage("selectionner une ligne s'il vous plait", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            message = new FormMessage("Voulez-vous vraiment supprimer définitivement " + $"{dtGridClient.CurrentRow.Cells[1].Value}" + " de la liste?", "Attention", true, true, FontAwesome.Sharp.IconChar.ExclamationTriangle);

            if (DialogResult.Yes == message.ShowDialog())
            {
                try
                {
                    connectionClient.Open();
                    OleDbCommand commandDelete = new OleDbCommand
                    {
                        Connection = connectionClient,
                        CommandText = "delete * from client where idClient = " + long.Parse(idClient[1])
                    };
                    commandDelete.ExecuteNonQuery();

                    connectionClient.Close();
                    message = new FormMessage("le Client a ete Supprimé avec Succès", "Succès", true, FontAwesome.Sharp.IconChar.CheckCircle);
                    message.ShowDialog();
                }
                catch (Exception ex)
                {
                    message = new FormMessage("Error: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                    message.ShowDialog();
                    connectionClient.Close();
                }

                /*
                 * refrecher les donness du client
                 */
                remplissageDtGridClient();

                // initialize idClient
                idClient = null;

                // remove select row
                dtGridClient.Rows[dtGridClient.CurrentRow.Index].Selected = false;
            }
        }
    }
}
