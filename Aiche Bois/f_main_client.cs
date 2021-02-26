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

namespace Aiche_Bois
{
    public partial class f_main_client : Form
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
        /// this instance from form message to show message error
        /// </summary>
        private f_message message;

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
        DataTable dtb_client = new DataTable();

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
                    "SELECT (sum(f.prixtotalmesure) + sum(prixtotalpvc)) AS total, " +
                    "IIF((sum(f.prixtotalmesure) + sum(prixtotalpvc)) = prixTotalAvance, 0, (sum(f.prixtotalmesure) + sum(prixtotalpvc)) - prixTotalAvance) AS rest, " +
                    "c.nomClient, c.prixTotalAvance, c.idClient " +
                    "FROM client AS c INNER JOIN facture AS f ON f.idClient = c.idClient " +
                    "WHERE c.idClient = " + long.Parse(idClient[1]) +
                    " GROUP BY nomClient, prixTotalAvance, c.idClient"
                };
                OleDbDataReader readerClient = commandClient.ExecuteReader();
                while (readerClient.Read())
                {
                    t_nom_client.Text = readerClient["nomClient"].ToString();
                    t_prix_total_client.Text = readerClient["total"].ToString();
                    t_prix_avance_client.Text = readerClient["prixTotalAvance"].ToString();
                    t_prix_rest_client.Text = readerClient["rest"].ToString();
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
                    dp_date_facture.Value = Convert.ToDateTime(readerFacture["dtDateFacture"]);

                    ck_seul_pvc.Checked = Convert.ToBoolean(readerFacture["checkPVC"].ToString());


                    if (!ck_seul_pvc.Checked)
                    {
                        t_categorie.Text = readerFacture["categorie"].ToString();
                        t_metrage_feuille.Text = readerFacture["metrage"].ToString();
                        cb_type_metres.SelectedItem = readerFacture["typeMetres"].ToString();
                        t_prix_metre_mesure.Text = readerFacture["prixMetres"].ToString();
                        t_total_size_mesure.Text = readerFacture["totalMesure"].ToString();
                        txtPrixTotalMesure.Text = readerFacture["prixTotalMesure"].ToString();
                    }

                    //pvc
                    pvc = readerFacture["typePVC"].ToString();
                    dp_date_facture.Value = DateTime.Parse(readerFacture["dtDateFacture"].ToString());
                    t_type_Bois.Text = readerFacture["typeDeBois"].ToString();
                    cb_type_pvc.SelectedItem = (pvc == "---" ? null : readerFacture["typePVC"].ToString());
                    t_size_pvc.Text = readerFacture["tailleCanto"].ToString();
                    t_total_size_pvc.Text = readerFacture["totalTaillPVC"].ToString();
                    t_prix_metre_linear_pvc.Text = readerFacture["prixMitresLinear"].ToString();
                    t_prix_total_pvc.Text = readerFacture["prixTotalPVC"].ToString();
                }

                if (!cb_type_metres.SelectedItem.Equals("m"))
                {
                    //Mesure
                    if (!ck_seul_pvc.Checked)
                    {
                        var commandMesure = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "SELECT quantite, largeur, longueur, eppaiseur FROM MESURE WHERE IDFACTURE = " + idFacture
                        };
                        OleDbDataReader readerMesure = commandMesure.ExecuteReader();
                        // clear data grid mesure
                        dg_mesure.Rows.Clear();
                        while (readerMesure.Read())
                        {
                            if (cb_type_metres.SelectedItem.Equals("m3"))
                            {
                                dg_mesure.Rows.Add(
                                readerMesure["quantite"].ToString(),
                                readerMesure["largeur"].ToString(),
                                readerMesure["longueur"].ToString(),
                                readerMesure["eppaiseur"].ToString());
                            }
                            else
                            {
                                dg_mesure.Rows.Add(
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
                            CommandText = "SELECT quantite, largeur, longueur, orientation FROM PVC WHERE IDFACTURE = " + idFacture
                        };
                        OleDbDataReader readerPVC = commandPVC.ExecuteReader();
                        dg_pvc.Rows.Clear();
                        while (readerPVC.Read())
                        {
                            dg_pvc.Rows.Add(
                                readerPVC["quantite"].ToString(),
                                readerPVC["largeur"].ToString(),
                                readerPVC["longueur"].ToString(),
                                readerPVC["orientation"].ToString());
                        }
                    }
                    else
                    {
                        dg_pvc.Rows.Clear();
                    }
                    // desable field depend by check seul
                    checkSeulPVC(ck_seul_pvc.Checked);
                }
                else
                {
                    dg_pvc.Rows.Clear();
                    dg_mesure.Rows.Clear();
                }

                // close connection
                connectionClient.Close();
            }
            catch (Exception ex)
            {

                connectionClient.Close();
                LogFile.Message(ex);
            }
        }

        /// <summary>
        /// desabled field text depend on checked button seul
        /// </summary>
        /// <param name="ft"></param>
        private void checkSeulPVC(bool ft)
        {
            t_prix_metre_mesure.Enabled = t_metrage_feuille.Enabled = t_categorie.Enabled =
            t_search_client.Enabled = t_quantity_mesure.Enabled = t_largeur_mesure.Enabled = t_longueur_mesure.Enabled =
            t_epaisseur_mesure.Enabled = b_add_mesure.Enabled = b_delete_mesure.Enabled = b_export_csv.Enabled =
            b_import_pvc.Enabled = !ft;

            b_add_seul_pvc.Enabled = b_delete_seul_pvc.Enabled = t_quantity_pvc.Enabled = t_largeur_pvc.Enabled =
            t_longueur_pvc.Enabled = cb_orientation_pvc.Enabled = ft;

            if (ft)
            {
                t_prix_metre_mesure.Text = t_total_size_mesure.Text = txtPrixTotalMesure.Text = "0.00";
                t_metrage_feuille.Text = t_categorie.Text = "---";
                dg_mesure.Rows.Clear();
            }
        }

        /// <summary>
        /// remplir listeBox MDF, LATTE, STD de dataBase
        /// </summary>
        /// <param name="typeBois"></param>
        DataTable tb_Type = new DataTable();
        private void remplirListe(string typeBois)
        {
            lt_type_bois.Items.Clear();
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
                    lt_type_bois.Items.Add(tb_Type.Rows[i][0].ToString());
                }
                lt_type_bois.SelectedIndex = lt_type_bois.Items.Count - 1;

            }
            catch (Exception ex)
            {
                connectionType.Close();
                LogFile.Message(ex);
            }
        }

        /// <summary>
        /// clear textBox and datagrid
        /// </summary>
        private void ViderTxtBox()
        {
            /*vider les objet*/
            t_total_size_pvc.Text = t_type_Bois.Text = "";
            t_quantity_mesure.Clear();
            t_largeur_mesure.Clear();
            t_longueur_mesure.Clear();
            t_epaisseur_mesure.Clear();
            t_total_size_mesure.Clear();
            t_prix_total_pvc.Text = txtPrixTotalMesure.Text = "0.00";
            t_metrage_feuille.Clear();
            cb_type_pvc.SelectedItem = null;
            cb_type_metres.SelectedItem = "feuille";
            t_size_pvc.Clear();
            t_prix_metre_linear_pvc.Clear();
            mesures.Clear();
            pvcs.Clear();
            dg_mesure.Rows.Clear();
            dg_pvc.Rows.Clear();
            ck_seul_pvc.Checked = false;
            checkSeulPVC(ck_seul_pvc.Checked);
            lt_type_bois.Focus();
        }

        /// <summary>
        /// calculate datagride when click on button save
        /// </summary>
        public void btnSaveCalculPvc()
        {
            if (dg_pvc.Rows.Count == 0)
            {
                message = new f_message("Importer les valeurs des mesures", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            // check if already there this numbers
            for (int i = 0; i < dg_pvc.Rows.Count; i++)
            {
                if (dg_pvc.Rows[i].Cells[3].Value == null)
                {
                    message = new f_message("sélection l'orientation de ligne " + (i + 1), "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    return;
                }
            }

            pvcs.Clear();
            double total = 0;
            for (int i = 0; i < dg_pvc.Rows.Count; i++)
            {
                String ss = (dg_pvc.Rows[i].Cells[3] as DataGridViewComboBoxCell).FormattedValue.ToString();
                if (String.IsNullOrEmpty(ss))
                    ss = "0";
                double qt = double.Parse(dg_pvc.Rows[i].Cells[0].Value.ToString());
                double lar = double.Parse(dg_pvc.Rows[i].Cells[1].Value.ToString());
                double lon = double.Parse(dg_pvc.Rows[i].Cells[2].Value.ToString());

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
                    /*horizontal = 2, vertical = 2*/
                    case "4":
                        total += (lar / 100) * 2 * qt + (lon / 100) * 2 * qt;
                        break;

                    /*horizontal = 1, vertical = 2*/
                    case "h*1+v*2":
                        total += (lar / 100) * 1 * qt + (lon / 100) * 2 * qt;
                        break;
                    /*horizontal = 2, vertical = 1*/
                    case "h*2+v*1":
                        total += (lar / 100) * 2 * qt + (lon / 100) * 1 * qt;
                        break;
                    /*horizontal = 1, vertical = 1*/
                    case "h*1+v*1":
                        total += (lar / 100) * 1 * qt + (lon / 100) * 1 * qt;
                        break;
                }
            }
            t_total_size_pvc.Text = total.ToString("F2");
            total = 0;
        }

        /// <summary>
        /// this method return true or false for Empty textBox
        /// </summary>
        /// <returns></returns>
        private bool checkIsNullOrEmpty()
        {
            if (string.IsNullOrEmpty(t_nom_client.Text))
            {
                message = new f_message(t_nom_client.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_nom_client.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(t_type_Bois.Text))
            {
                message = new f_message(t_type_Bois.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                lt_type_bois.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(t_prix_metre_mesure.Text))
            {
                message = new f_message(t_prix_metre_mesure.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_prix_metre_mesure.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(t_total_size_mesure.Text))
            {
                message = new f_message(t_total_size_mesure.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_total_size_mesure.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPrixTotalMesure.Text))
            {
                message = new f_message(txtPrixTotalMesure.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                txtPrixTotalMesure.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(t_metrage_feuille.Text))
            {
                message = new f_message(t_metrage_feuille.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_metrage_feuille.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(t_categorie.Text))
            {
                message = new f_message(t_categorie.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_categorie.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(t_prix_total_client.Text))
            {
                message = new f_message(t_prix_total_client.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_prix_total_client.Focus();
                return false;
            }
            if (ck_avance_client.Checked)
                if (string.IsNullOrEmpty(t_prix_avance_client.Text))
                {
                    message = new f_message(t_prix_avance_client.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    t_prix_avance_client.Focus();
                    return false;
                }
            if (!string.IsNullOrEmpty(cb_type_pvc.Text))
            {
                if (string.IsNullOrEmpty(t_total_size_pvc.Text))
                {
                    message = new f_message(t_total_size_pvc.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    t_total_size_pvc.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(t_size_pvc.Text))
                {
                    message = new f_message(t_size_pvc.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    t_size_pvc.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(t_prix_metre_linear_pvc.Text))
                {
                    message = new f_message(t_prix_metre_linear_pvc.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog(); t_prix_metre_linear_pvc.Focus();
                    return false;
                }
                if (dg_mesure.Rows.Count != dg_pvc.Rows.Count && !ck_seul_pvc.Checked && cb_type_pvc.SelectedText != null)
                {
                    message = new f_message("Exporter les mesures vers le menu Pvc", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
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
            t_quantity_mesure.Enabled = t_largeur_mesure.Enabled = t_longueur_mesure.Enabled = t_epaisseur_mesure.Enabled = b;
        }

        /// <summary>
        /// initialise l'identificateur de facture
        /// </summary>
        private void idFacture()
        {
            cb_id_facture.Text = "Facture Numéro: " + (factures.Count + 1).ToString("D2");
        }

        /// <summary>
        /// remplir comboBox PVC de dataBase
        /// </summary>
        private void RemplirComboBxPvc()
        {
            cb_type_pvc.Items.Clear();
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
                    cb_type_pvc.Items.Add(reader["Libelle"].ToString());
                }
                connectionType.Close();
            }
            catch (Exception ex)
            {
                connectionType.Close();
                LogFile.Message(ex);
            }
        }

        /// <summary>
        /// method qui calcul data grid view mesure
        /// </summary>
        /// <param name="mesures"></param>
        private void RemplirDataMesure()
        {
            double totale = 0;
            for (int i = 0; i < dg_mesure.Rows.Count; i++)
            {
                if (cb_type_metres.SelectedItem.Equals("m3"))
                {
                    totale +=
                        ((Convert.ToDouble(dg_mesure.Rows[i].Cells[1].Value) / 100) *
                        (Convert.ToDouble(dg_mesure.Rows[i].Cells[2].Value) / 100) *
                        (Convert.ToDouble(dg_mesure.Rows[i].Cells[3].Value) / 1000)) *
                        Convert.ToDouble(dg_mesure.Rows[i].Cells[0].Value);
                }
                else
                {
                    totale +=
                        ((Convert.ToDouble(dg_mesure.Rows[i].Cells[1].Value) / 100) *
                        (Convert.ToDouble(dg_mesure.Rows[i].Cells[2].Value) / 100)) *
                        Convert.ToDouble(dg_mesure.Rows[i].Cells[0].Value);
                }
            }

            if (!cb_type_metres.SelectedItem.Equals("feuille"))
            {
                t_total_size_mesure.Text = totale.ToString();
            }
        }

        /// <summary>
        /// validate data on database mode desconnect
        /// </summary>
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
                    "values('" + t_nom_client.Text + "', '" + DateTime.Now + "', '" +
                    Convert.ToDouble(t_prix_avance_client.Text) + "')"
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

                    // insert datagride pvc
                    foreach (Pvc pvc in fct.Pvcs)
                    {
                        var commandPvc = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "insert into pvc (idFacture, quantite, largeur, longueur, orientation) values('" + idFACTURE + "','" + pvc.Qte + "','" + pvc.Largr + "','" + pvc.Longr + "','" + pvc.Ortn + "')"
                        };
                        commandPvc.ExecuteNonQuery();
                    }

                    // insert datagride mesure
                    foreach (Mesure msr in fct.Mesures)
                    {
                        OleDbCommand commandMesure = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "insert into mesure (idFacture, quantite, largeur, longueur, eppaiseur) values('" + idFACTURE + "','" + msr.Quantite + "','" + msr.Largeur + "','" + msr.Longueur + "','" + msr.Epaisseur + "')"
                        };
                        commandMesure.ExecuteNonQuery();
                    }
                }
                // close connection and desplay message
                connectionClient.Close();
            }
            catch (Exception ex)
            {
                connectionClient.Close();
                LogFile.Message(ex);
            }
        }
        /*
         * =========================== End Panel Add Edit Method ===========================
         */

        /// <summary>
        /// c'est le design du formulaire et l'initialisation de connecter a la base de donnees
        /// </summary>
        public f_main_client()
        {
            connectionClient.ConnectionString = Program.Path;
            connectionType.ConnectionString = Program.PathType;

            InitializeComponent();
        }

        /// <summary>
        /// fiil data gride clients
        /// </summary>
        private void fill_dt_grid_client(DataTable dataTable)
        {
            dg_client.Rows.Clear();
            // fill datagride
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dg_client.Rows.Add(
                    String.Format("N{0:D4}", long.Parse(dtb_client.Rows[i][0].ToString())),
                    dtb_client.Rows[i][1].ToString(),
                    String.Format("{0}", dtb_client.Rows[i][2].ToString()),
                    dtb_client.Rows[i][3].ToString(),
                    "",
                    dtb_client.Rows[i][4].ToString(),
                    dtb_client.Rows[i][5].ToString(),
                    dtb_client.Rows[i][6].ToString(),
                    dtb_client.Rows[i][7].ToString());
            }
        }
        /// <summary>
        /// fill datagride client from database
        /// </summary>
        private void remplissageDtGridClient()
        {
            try
            {
                connectionClient.Open();

                dtb_client.Rows.Clear();
                dg_client.Rows.Clear();
                //clients.Clear();

                OleDbCommand commandClient = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText =
                    "SELECT c.idClient, c.nomClient, dateClient, count(idFacture) AS nbFacture, " +
                    "IIF(ROUND((sum(f.prixtotalmesure) + sum(prixtotalpvc)), 2) = ROUND((prixTotalAvance), 2), 'true', 'false') AS cavance, " +
                    "c.prixTotalAvance, " +
                    "IIF(ROUND((sum(f.prixtotalmesure) + sum(prixtotalpvc)), 2) = ROUND(prixTotalAvance, 2), 0, ROUND((sum(f.prixtotalmesure) + sum(prixtotalpvc)), 2) - ROUND(prixTotalAvance, 2)) AS rest, " +
                    "ROUND((sum(f.prixtotalmesure) + sum(prixtotalpvc)), 2) AS total " +
                    "FROM client AS c INNER JOIN facture AS f ON f.idClient = c.idClient " +
                    "GROUP BY c.idClient, nomClient, dateClient, prixTotalAvance " +
                    "ORDER BY c.idClient DESC;"
                };

                dtb_client.Load(commandClient.ExecuteReader());

                connectionClient.Close();

                // fill datagride
                fill_dt_grid_client(dtb_client);

                // remove selected row
                if (dg_client.Rows.Count != 0)
                    dg_client.Rows[0].Selected = false;
            }
            catch (Exception ex)
            {
                connectionClient.Close();
                LogFile.Message(ex);
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
            if (dg_client.Rows.Count <= 0 || e.RowIndex < 0)
                return;

            idClient = dg_client.Rows[e.RowIndex].Cells[0].Value.ToString().Split('N');

            if (e.ColumnIndex == 4 && e.RowIndex < dg_client.Rows.Count)
            {
                f_avance avance = new f_avance(idClient[1]);
                avance.ShowDialog();
                // fill datagride
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
            if (dg_client.Rows.Count <= 0 || idClient == null)
            {
                message = new f_message("sélectionner une ligne š'il vous plaît", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            //pvc_mesure(idClient[1], indxFacture);
            f_print print = new f_print(idClient[1], "null", "btnPrintClient", false);
            print.ShowDialog();

            // initilize id client
            idClient = null;

            // remove select row
            dg_client.Rows[dg_client.CurrentRow.Index].Selected = false;
        }

        /// <summary>
        /// c'est event chercher a client a partir du date, nom ou id client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //txtSearch.CharacterCasing = CharacterCasing.Upper;
            if (!string.IsNullOrEmpty(t_search_client.Text))
            {
                dg_client.Rows.Clear();
                for (int i = 0; i < dtb_client.Rows.Count; i++)
                {
                    String id = String.Format("N{0:D4}", long.Parse(dtb_client.Rows[i][0].ToString()));
                    String dt = String.Format("{0}", dtb_client.Rows[i][2].ToString());
                    if (dtb_client.Rows[i][1].ToString().Contains(value: t_search_client.Text.ToUpper()) ||
                        id.Contains(value: t_search_client.Text) ||
                        dt.Contains(value: t_search_client.Text))
                        dg_client.Rows.Add(
                        String.Format("N{0:D4}", long.Parse(dtb_client.Rows[i][0].ToString())),
                        dtb_client.Rows[i][1].ToString(),
                        String.Format("{0}", dtb_client.Rows[i][2].ToString()),
                        dtb_client.Rows[i][3].ToString(),
                        "",
                        dtb_client.Rows[i][4].ToString(),
                        dtb_client.Rows[i][5].ToString(),
                        dtb_client.Rows[i][6].ToString(),
                        dtb_client.Rows[i][7].ToString());
                }
            }
            else
            {
                // fill datagride
                fill_dt_grid_client(dtb_client);
            }
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
                            " typeDeBois = '" + t_type_Bois.Text + "'" +
                            ", metrage = '" + t_metrage_feuille.Text + "'" +
                            ", categorie = '" + t_categorie.Text + "'" +
                            ", totalMesure = " + (string.IsNullOrEmpty(t_total_size_mesure.Text) ? 0 : double.Parse(t_total_size_mesure.Text)) +
                            ", typeMetres = '" + cb_type_metres.SelectedItem.ToString() + "'" +
                            ", prixMetres = " + (string.IsNullOrEmpty(t_prix_metre_mesure.Text) ? 0 : double.Parse(t_prix_metre_mesure.Text)) +
                            ", typePVC = '" + (cb_type_pvc.SelectedItem == null ? "---" : cb_type_pvc.SelectedItem) + "'" +
                            ", checkPVC = " + ck_seul_pvc.Checked +
                            ", tailleCanto = " + (string.IsNullOrEmpty(t_size_pvc.Text) ? 0 : double.Parse(t_size_pvc.Text)) +
                            ", totalTaillPVC = " + (string.IsNullOrEmpty(t_total_size_pvc.Text) ? 0 : double.Parse(t_total_size_pvc.Text)) +
                            ", prixMitresLinear = " + (string.IsNullOrEmpty(t_prix_metre_linear_pvc.Text) ? 0 : double.Parse(t_prix_metre_linear_pvc.Text)) +
                            ", prixTotalPVC = " + (string.IsNullOrEmpty(t_prix_total_pvc.Text) ? 0 : double.Parse(t_prix_total_pvc.Text)) +
                            ", prixTotalMesure = " + (string.IsNullOrEmpty(txtPrixTotalMesure.Text) ? 0 : double.Parse(txtPrixTotalMesure.Text)) +
                            " WHERE idFacture = " + long.Parse(cb_id_facture.Text)
                    };
                    commandF.ExecuteNonQuery();

                    if (!ck_seul_pvc.Checked && dg_mesure.Rows.Count > 0)
                    {
                        OleDbCommand commandMD = new OleDbCommand();
                        commandMD.Connection = connectionClient;
                        commandMD.CommandText = "DELETE * from mesure WHERE idFacture = " + long.Parse(cb_id_facture.Text);
                        commandMD.ExecuteNonQuery();

                        for (int i = 0; i < dg_mesure.Rows.Count; i++)
                        {
                            OleDbCommand commandM = new OleDbCommand();
                            commandM.Connection = connectionClient;
                            commandM.CommandText = "INSERT INTO mesure(idFacture, quantite, largeur, longueur, eppaiseur) " +
                                "VALUES('" + long.Parse(cb_id_facture.Text) + "','" +
                                double.Parse(dg_mesure.Rows[i].Cells[0].Value.ToString()) + "','" +
                                double.Parse(dg_mesure.Rows[i].Cells[1].Value.ToString()) + "','" +
                                double.Parse(dg_mesure.Rows[i].Cells[2].Value.ToString()) + "','" +
                                (cb_type_metres.SelectedItem.Equals("m3") ? double.Parse(dg_mesure.Rows[i].Cells[3].Value.ToString()) : 0) + "')";
                            commandM.ExecuteNonQuery();
                        }
                    }

                    if (!String.IsNullOrEmpty(cb_type_pvc.Text))
                    {
                        OleDbCommand commandPD = new OleDbCommand();
                        commandPD.Connection = connectionClient;
                        commandPD.CommandText = "DELETE * from pvc WHERE idFacture = " + long.Parse(cb_id_facture.Text);
                        commandPD.ExecuteNonQuery();
                        for (int i = 0; i < dg_pvc.Rows.Count; i++)
                        {
                            OleDbCommand commandP = new OleDbCommand();
                            commandP.Connection = connectionClient;
                            commandP.CommandText = "INSERT INTO pvc (idFacture, quantite, largeur, longueur, orientation) " +
                                "values('" + long.Parse(cb_id_facture.Text) + "','" +
                                double.Parse(dg_pvc.Rows[i].Cells[0].Value.ToString()) + "','" +
                                double.Parse(dg_pvc.Rows[i].Cells[1].Value.ToString()) + "','" +
                                double.Parse(dg_pvc.Rows[i].Cells[2].Value.ToString()) + "','" +
                                dg_pvc.Rows[i].Cells[3].Value.ToString() + "')";
                            commandP.ExecuteNonQuery();
                        }
                    }

                    connectionClient.Close();

                    // fill edit panel by id facture
                    loadDataEdit(long.Parse(cb_id_facture.Text));

                }
                catch (Exception ex)
                {
                    connectionClient.Close();
                    LogFile.Message(ex);
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
                message = new f_message("Vous avez des données non enregistrées, si vous souhaitez les enregistrer, cliquez sur le bouton Sauvgarder les factures", "Attention", true, true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                if (DialogResult.Yes == message.ShowDialog())
                {
                    b_save_client.PerformClick();
                    Environment.Exit(0);
                }
                else
                    Environment.Exit(0);
            }
        }

        /// <summary>
        /// statistical for year filtering by date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Obsolete]
        private void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                // get data from database access
                connectionClient.Open();
                OleDbCommand fctr = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText = "SELECT typeDeBois, dtDateFacture FROM facture order by typeDeBois"
                };
                // table to store select item from database
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

                // fill list for calc repeat items
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

                // dictionary to store item double
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

                // calculate items
                for (int i = 0; i < dtb_client.Rows.Count; i++)
                {
                    if (dtb_client.Rows[i][6].ToString() == null)
                    {
                        break;
                    }
                    if (DateTime.Parse(dtb_client.Rows[i][2].ToString()).Year.Equals(DateTime.Today.Year))
                    {
                        a += double.Parse(dtb_client.Rows[i][5].ToString());
                        b += double.Parse(dtb_client.Rows[i][6].ToString());
                        c += double.Parse(dtb_client.Rows[i][7].ToString());
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
                    // html worker to v=convert code above to web than pdf document convert it to pdf
                    HTMLWorker htmlWorker = new HTMLWorker(pdfDoc);
                    htmlWorker.Parse(new StringReader(strHTML));
                    pdfWriter.CloseStream = true;
                    pdfDoc.Close();
                    FileInfo fileInfo = new FileInfo(save.FileName);
                    fileInfo.IsReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                connectionClient.Close();
                LogFile.Message(ex);
            }
        }

        /// <summary>
        /// style for selected row on mouse move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGridFacture_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                dg_client.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(47, 47, 47);
                dg_client.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(255, 244, 228);
            }
        }

        /// <summary>
        /// style for selected row on mouse leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGridFacture_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                dg_client.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 244, 228);
                dg_client.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(47, 47, 47);
            }
        }

        /// <summary>
        /// exit button to shutdown application from tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// button miximezid to open application as maximized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// button minimize application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// top tablelayout for move application by mouse down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            // verify if data not saved
            if (verifyForm)
            {
                message = new f_message("Vous avez des données non enregistrées, si vous souhaitez les enregistrer, cliquez sur le bouton Sauvgarder les factures", "Attention", true, true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                if (DialogResult.Yes == message.ShowDialog())
                {
                    b_save_client.PerformClick();
                    verifyForm = false;
                }
                else
                    factures.Clear();
            }

            // initilize idClient
            idClient = null;

            // refrech main datagrid
            remplissageDtGridClient();

            //enabled buttons
            b_add_client.Enabled = b_delete_client.Enabled = b_edit_client.Enabled = b_print_client.Enabled = cb_id_facture.Enabled = true;

            // remove event for button save facture
            b_add_facture.Click -= new EventHandler(this.e_Add_New_Facture_Client_Click);
            b_add_facture.Click -= new EventHandler(this.e_Edit_Factures_Click);
            b_add_facture.Click -= new EventHandler(this.e_Add_New_Client_Click);

            // change event for text changed on this texts boxes
            t_prix_avance_client.TextChanged -= new EventHandler(this.txtPrixAvanceClient_TextChanged);
            t_prix_total_client.TextChanged -= new EventHandler(this.txtPrixAvanceClient_TextChanged);

            // bring home panel
            tblp_add_edit.Visible = false;
            p_home.Visible = true;
        }

        /// <summary>
        /// this is button edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dg_client.Rows.Count <= 0 || idClient == null)
            {
                message = new f_message("sélectionner une ligne š'il vous plaît", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            try
            {
                /*remple comboBox PVC*/
                RemplirComboBxPvc();

                b_add_client.Enabled = b_delete_client.Enabled = b_print_client.Enabled =
                    b_edit_client.Enabled = b_save_client.Visible = false;

                /*desable buttons*/
                b_print_facture.Visible = b_delete_facture.Visible = b_add_new_facture.Visible = true;



                cb_type_metres.SelectedItem = "m2";
                cb_type_bois.SelectedIndex = 0;
                dp_date_facture.Value = DateTime.Today;
                ck_avance_client.Checked = t_prix_avance_client.Enabled = false;
                t_prix_avance_client.Text = "0.00";
                cb_orientation_pvc.SelectedIndex = 0;

                cb_id_facture.DropDownStyle = ComboBoxStyle.DropDownList;
                lblNumeroFacture.Text = "Selectionner la Facture";
                b_add_facture.IconChar = FontAwesome.Sharp.IconChar.Edit;
                b_add_facture.Text = "Modifier";

                t_nom_client.Enabled = dp_date_facture.Enabled = ck_avance_client.Enabled = false;
                lt_type_bois.SelectedItem = null;

                //desactiver le champ de pre avance
                t_prix_avance_client.Enabled = false;

                // change event for btn save
                b_add_facture.Click -= new EventHandler(this.e_Add_New_Facture_Client_Click);
                b_add_facture.Click -= new EventHandler(this.e_Add_New_Client_Click);
                b_add_facture.Click += new EventHandler(this.e_Edit_Factures_Click);

                // change event for text changed on this texts boxes
                t_prix_avance_client.TextChanged -= new EventHandler(this.txtPrixAvanceClient_TextChanged);
                t_prix_total_client.TextChanged -= new EventHandler(this.txtPrixAvanceClient_TextChanged);

                // reset facture count
                cb_id_facture.Items.Clear();

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
                    cb_id_facture.Items.Add(long.Parse(readerFacture["IDFACTURE"].ToString()));
                }
                connectionClient.Close();
            }
            catch (Exception ex)
            {
                connectionClient.Close();
                LogFile.Message(ex);
            }

            // select the first item on list
            if (cb_id_facture.Items.Count > 0)
                cb_id_facture.SelectedIndex = 0;

            //visible panel and bring to front
            p_home.Visible = false;
            tblp_add_edit.Visible = true;
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
            t_prix_avance_client.Text = t_total_size_pvc.Text = t_prix_total_pvc.Text =
            txtPrixTotalMesure.Text = t_prix_total_client.Text = t_prix_rest_client.Text = "0.00";
            t_prix_metre_mesure.Clear();
            t_categorie.Clear();
            t_metrage_feuille.Clear();

            t_nom_client.Clear();
            t_nom_client.Enabled = ck_avance_client.Enabled = true;
            cb_type_metres.SelectedItem = "m2";
            cb_type_bois.SelectedIndex = 0;
            dp_date_facture.Value = DateTime.Today;

            t_prix_avance_client.Enabled = ck_avance_client.Checked = false;
            cb_orientation_pvc.SelectedIndex = 0;
            lt_type_bois.SelectedIndex = lt_type_bois.Items.Count > 0 ? lt_type_bois.SelectedIndex = 0 : lt_type_bois.SelectedIndex = -1;

            // invisible button delete and print facture
            b_print_facture.Visible = b_delete_facture.Visible = b_add_new_facture.Visible = false;

            b_save_client.Visible = true;

            b_add_client.Enabled = b_delete_client.Enabled = b_print_client.Enabled = b_edit_client.Enabled = false;

            cb_id_facture.DropDownStyle = ComboBoxStyle.Simple;
            lblNumeroFacture.Text = "Selectionner la Facture";
            idFacture();

            b_add_facture.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            b_add_facture.Text = "Ajouter";

            // change event for btn save
            b_add_facture.Click -= new EventHandler(this.e_Add_New_Facture_Client_Click);
            b_add_facture.Click -= new EventHandler(this.e_Edit_Factures_Click);
            b_add_facture.Click += new EventHandler(this.e_Add_New_Client_Click);

            // change event for text changed on this texts boxes
            t_prix_avance_client.TextChanged += new EventHandler(this.txtPrixAvanceClient_TextChanged);
            t_prix_total_client.TextChanged += new EventHandler(this.txtPrixAvanceClient_TextChanged);

            //visible panel and bring to frot
            p_home.Visible = false;
            tblp_add_edit.Visible = true;

            t_nom_client.Focus();
        }

        /*
         * ======================== Panel Add ========================
         */
        private void cmbNumeroFacture_SelectedIndexChanged(object sender, EventArgs e)
        {
            // fill data by id facture
            loadDataEdit(long.Parse(cb_id_facture.SelectedItem.ToString()));
        }

        /// <summary>
        /// fill list box 'type bois'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTypeDeBois_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.btnAddTypeClick = cb_type_bois.Text;
            // fill list type bois by combobox select
            remplirListe(cb_type_bois.Text);
            if (lt_type_bois.Items.Count <= 0)
            {
                t_type_Bois.Text = "";
                return;
            }
            lt_type_bois.SelectedIndex = 0;
            t_type_Bois.Text = lt_type_bois.SelectedItem.ToString();
        }

        /// <summary>
        /// button add titem by combobox select item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCmbCategorie_Click(object sender, EventArgs e)
        {
            Program.btnAddTypeClick = cb_type_bois.Text.ToUpper();

            frm_ajout ajout = new frm_ajout();
            ajout.ShowDialog();

            /*remplirLstTypeBois();*/
            remplirListe(Program.btnAddTypeClick);
        }

        /// <summary>
        /// search for type bois by clicking enter from keybord
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void t_search_type_bois_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                /*si le textBox est vide, remplir tous les items*/
                if (string.IsNullOrEmpty(t_search_type_bois.Text))
                {
                    remplirListe(cb_type_bois.Text);
                }
                else
                {
                    lt_type_bois.Items.Clear();
                    for (int i = 0; i < tb_Type.Rows.Count; i++)
                    {
                        if (tb_Type.Rows[i][0].ToString().Contains(t_search_type_bois.Text.ToUpper()))
                        {
                            lt_type_bois.Items.Add(tb_Type.Rows[i][0].ToString());
                        }
                    }
                }

                /*selectionner le premiere ligne*/
                if (lt_type_bois.Items.Count != 0)
                {
                    lt_type_bois.SelectedIndex = 0;
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// fill txttypebois by select item from list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstTypeBois_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lt_type_bois.Items.Count <= 0 || lt_type_bois.SelectedIndex == -1)
            {
                t_type_Bois.Text = "";
                return;
            }
            t_type_Bois.Text = lt_type_bois.SelectedItem.ToString();
        }

        /// <summary>
        /// quand ecrire sure le text box txtPrixMetreMesure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private double prix_total_client = 0;
        private void txtPrixMetreMesure_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(t_prix_metre_mesure.Text) || String.IsNullOrEmpty(t_total_size_mesure.Text))
                return;

            txtPrixTotalMesure.Text = (double.Parse(t_prix_metre_mesure.Text) * double.Parse(t_total_size_mesure.Text)).ToString("F2");
        }

        /// <summary>
        /// key press for all field accept just integer or double
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// select item from combobox type metres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTypeDuMetres_SelectedIndexChanged(object sender, EventArgs e)
        {
            t_total_size_mesure.Enabled = false;
            b_add_mesure.Enabled = true;
            b_delete_mesure.Enabled = true;
            b_export_csv.Enabled = true;

            cb_type_pvc.Enabled = b_cmb_pvc.Enabled = true;

            // item = feuille
            if (cb_type_metres.SelectedItem.Equals("feuille"))
            {
                lblTypeDuMetres.Text = "Prix ​​au feuille";
                lblMesure.Text = "Nombre de Feuilles";
                lblEpaisseur.Visible = false;
                t_epaisseur_mesure.Visible = false;
                if (dg_mesure.ColumnCount > 3)
                {
                    dg_mesure.ColumnCount -= 1;
                }
                enabledMesure(true);
                t_total_size_mesure.Enabled = true;
                t_total_size_mesure.Focus();
            }
            // item = m
            else if (cb_type_metres.SelectedItem.Equals("m"))
            {
                lblTypeDuMetres.Text = "Prix ​​au Mètres Linéaires";
                lblMesure.Text = "Taille en mètres linéaires";
                lblEpaisseur.Visible = false;
                t_epaisseur_mesure.Visible = false;
                if (dg_mesure.ColumnCount > 3)
                {
                    dg_mesure.ColumnCount -= 1;
                }
                enabledMesure(false);

                cb_type_pvc.Enabled = b_cmb_pvc.Enabled = false;

                dg_mesure.Rows.Clear();
                b_add_mesure.Enabled = false;
                b_delete_mesure.Enabled = false;
                b_export_csv.Enabled = false;
                t_total_size_mesure.Enabled = true;
                t_total_size_mesure.Focus();
            }
            //item = m2
            else if (cb_type_metres.SelectedItem.Equals("m2"))
            {
                lblTypeDuMetres.Text = "Prix ​​au mètre carré";
                lblMesure.Text = "Volume Total de la Mesure de mètre carré";
                lblEpaisseur.Visible = false;
                t_epaisseur_mesure.Visible = false;
                if (dg_mesure.ColumnCount > 3)
                {
                    dg_mesure.ColumnCount -= 1;
                }
                enabledMesure(true);
                t_quantity_mesure.Focus();
            }
            // item = m3
            else if (cb_type_metres.SelectedItem.Equals("m3"))
            {
                dg_mesure.Rows.Clear();
                mesures.Clear();
                lblTypeDuMetres.Text = "Prix ​​au mètre cube";
                lblMesure.Text = "Volume Total de la Mesure de mètre cube";
                lblEpaisseur.Visible = true;
                t_epaisseur_mesure.Visible = true;
                if (dg_mesure.ColumnCount == 3)
                {
                    dg_mesure.ColumnCount += 1;
                    dg_mesure.Columns[3].HeaderText = "Epssr";
                }
                enabledMesure(true);
                t_quantity_mesure.Focus();
            }
        }

        /// <summary>
        /// button add mesure to datagride mesure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddMesure_Click(object sender, EventArgs e)
        {
            //verifier si le champs contain un chifre ou vide 
            if (string.IsNullOrEmpty(t_quantity_mesure.Text) || double.Parse(t_quantity_mesure.Text) == 0)
            {
                message = new f_message("n'accepte pas 0, saisir la valeur de " + t_quantity_mesure.Tag, "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_quantity_mesure.Focus();
                return;
            }
            if (string.IsNullOrEmpty(t_largeur_mesure.Text) || double.Parse(t_largeur_mesure.Text) == 0)
            {
                message = new f_message("n'accepte pas 0, saisir la valeur de " + t_largeur_mesure.Tag, "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_largeur_mesure.Focus();
                return;
            }
            if (string.IsNullOrEmpty(t_longueur_mesure.Text) || double.Parse(t_longueur_mesure.Text) == 0)
            {
                message = new f_message("n'accepte pas 0, saisir la valeur de " + t_longueur_mesure.Tag, "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_longueur_mesure.Focus();
                return;
            }

            // check if already had this mesure when datagride has 3 column
            for (int i = 0; i < dg_mesure.Rows.Count; i++)
            {
                if (cb_type_metres.SelectedItem.Equals("m3"))
                {
                    if (double.Parse(t_quantity_mesure.Text) == double.Parse(dg_mesure.Rows[i].Cells[0].Value.ToString()) &&
                        double.Parse(t_largeur_mesure.Text) == double.Parse(dg_mesure.Rows[i].Cells[1].Value.ToString()) &&
                        double.Parse(t_longueur_mesure.Text) == double.Parse(dg_mesure.Rows[i].Cells[2].Value.ToString()) &&
                        double.Parse(t_epaisseur_mesure.Text) == double.Parse(dg_mesure.Rows[i].Cells[3].Value.ToString()))
                    {
                        message = new f_message("ç'est mesure exist déjà", "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                        message.ShowDialog();
                        return;
                    }
                }
                // check if already had this mesure when datagride has 4 column
                else
                {
                    if (double.Parse(t_quantity_mesure.Text) == double.Parse(dg_mesure.Rows[i].Cells[0].Value.ToString()) &&
                      double.Parse(t_largeur_mesure.Text) == double.Parse(dg_mesure.Rows[i].Cells[1].Value.ToString()) &&
                      double.Parse(t_longueur_mesure.Text) == double.Parse(dg_mesure.Rows[i].Cells[2].Value.ToString()))
                    {
                        message = new f_message("ç'est mesure exist déjà", "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                        message.ShowDialog();
                        return;
                    }
                }
            }
            // test on select item = m3 to add new column eppaiseur
            if (cb_type_metres.SelectedItem.Equals("m3"))
            {
                //verifier si le champs contain un chifre ou vide
                if (string.IsNullOrEmpty(t_epaisseur_mesure.Text) || double.Parse(t_epaisseur_mesure.Text) == 0)
                {
                    message = new f_message("n'accepte pas 0, saisir la valeur de " + t_epaisseur_mesure.Tag, "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    t_epaisseur_mesure.Focus();
                    return;
                }
                // add field text to datagride
                dg_mesure.Rows.Add(t_quantity_mesure.Text, t_largeur_mesure.Text, t_longueur_mesure.Text, t_epaisseur_mesure.Text);
                t_epaisseur_mesure.Clear();
            }
            else
            {
                dg_mesure.Rows.Add(t_quantity_mesure.Text, t_largeur_mesure.Text, t_longueur_mesure.Text);
            }

            t_quantity_mesure.Clear();
            t_largeur_mesure.Clear();
            t_longueur_mesure.Clear();
            t_quantity_mesure.Focus();

            // calculate datagride rows
            RemplirDataMesure();
        }

        /// <summary>
        /// click enter from key bord to add mesure on datagride
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void t_quantity_mesure_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                b_add_mesure.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// click enter from key bord to add pvc on datagride
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void t_quantity_pvc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                b_add_seul_pvc.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        /// <summary>
        /// button delete mesure by select row from data gride mesure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteMesure_Click(object sender, EventArgs e)
        {
            if (dg_mesure.SelectedRows.Count <= 0)
            {
                message = new f_message("sélectionner une ligne pour supprimer", "Attention !!!", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            dg_mesure.Rows.RemoveAt(dg_mesure.CurrentRow.Index);

            RemplirDataMesure();
        }

        /// <summary>
        /// button export datagride mesure to csv file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (dg_mesure.Rows.Count > 0)
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
                            message = new f_message("Il n'était pas possible d'écrire les données sur le disque" + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                            message.ShowDialog();
                            LogFile.Message(ex);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dg_mesure.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dg_mesure.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dg_mesure.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dg_mesure.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dg_mesure.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, System.Text.Encoding.UTF8);
                        }
                        catch (Exception ex)
                        {
                            LogFile.Message(ex);
                        }
                    }
                }
            }
            else
            {
                message = new f_message("La List est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }
        }

        /// <summary>
        /// combobox type pvc selected item to enabled field text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTypePvc_SelectedIndexChanged(object sender, EventArgs e)
        {
            dg_pvc.Enabled = b_import_pvc.Enabled = b_save_pvc.Enabled = ck_seul_pvc.Enabled =
            t_size_pvc.Enabled = t_prix_metre_linear_pvc.Enabled = cb_type_pvc.SelectedIndex < 0 ? false : true;
        }

        /// <summary>
        /// add new item to combobox on database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btncmbNbrCanto_Click(object sender, EventArgs e)
        {
            Program.btnAddTypeClick = "PVC";

            /*ouvrier de fenétre*/
            frm_ajout ajout = new frm_ajout();
            ajout.ShowDialog();

            /*vider le comboBox PVC*/
            cb_type_pvc.Items.Clear();

            /*remplir comboBox par les donneesde dataBase*/
            RemplirComboBxPvc();
            cb_type_pvc.SelectedIndex = cb_type_pvc.Items.Count - 1;
        }

        /// <summary>
        /// enabled or desable field text depend on checked changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chSeulPVC_CheckedChanged(object sender, EventArgs e)
        {
            checkSeulPVC(ck_seul_pvc.Checked);
        }

        /// <summary>
        /// button to add new row to datagride from field text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSeulPVC_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(t_quantity_pvc.Text))
            {
                message = new f_message(t_quantity_pvc.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_quantity_pvc.Focus();
                return;
            }
            if (String.IsNullOrEmpty(t_largeur_pvc.Text))
            {
                message = new f_message(t_largeur_pvc.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_largeur_pvc.Focus();
                return;
            }
            if (String.IsNullOrEmpty(t_longueur_pvc.Text))
            {
                message = new f_message(t_longueur_pvc.Tag + " est vide", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_longueur_pvc.Focus();
                return;
            }

            //
            for (int i = 0; i < dg_pvc.Rows.Count; i++)
            {
                if (dg_pvc.Rows[i].Cells[0].Value.ToString() == t_quantity_pvc.Text &&
                    dg_pvc.Rows[i].Cells[1].Value.ToString() == t_largeur_pvc.Text &&
                    dg_pvc.Rows[i].Cells[2].Value.ToString() == t_longueur_pvc.Text &&
                    dg_pvc.Rows[i].Cells[3].Value.ToString() == cb_orientation_pvc.Text)
                {
                    message = new f_message("la valeur est le même déjà saisie", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    t_quantity_pvc.Focus();
                    return;
                }
            }
            // add to datagride
            dg_pvc.Rows.Add(t_quantity_pvc.Text, t_largeur_pvc.Text, t_longueur_pvc.Text, cb_orientation_pvc.Text);
            // calculate datagride rows
            btnSaveCalculPvc();

            t_quantity_pvc.Clear();
            t_largeur_pvc.Clear();
            t_longueur_pvc.Clear();
            t_quantity_pvc.Focus();
        }

        /// <summary>
        /// button delete pvc from datagride by selected row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteSeulPVC_Click(object sender, EventArgs e)
        {
            if (dg_pvc.SelectedRows.Count <= 0)
            {
                message = new f_message("sélectionner une ligne pour supprimer", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            dg_pvc.Rows.RemoveAt(dg_pvc.CurrentRow.Index);
            // calculate when remove row
            btnSaveCalculPvc();
            if (dg_pvc.Rows.Count == 0)
                t_total_size_pvc.Text = "0.00";
        }

        /// <summary>
        /// button to import data from datagride mesure to datagride pvc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportPvc_Click(object sender, EventArgs e)
        {
            dg_pvc.Rows.Clear();
            for (int i = 0; i < dg_mesure.Rows.Count; i++)
            {
                dg_pvc.Rows.Add(
                    dg_mesure.Rows[i].Cells[0].Value,
                    dg_mesure.Rows[i].Cells[1].Value,
                    dg_mesure.Rows[i].Cells[2].Value);
                t_total_size_pvc.Text = "";
            }
        }

        /// <summary>
        /// button to calculate datagride rows pvc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSavePvc_Click(object sender, EventArgs e)
        {
            btnSaveCalculPvc();
        }

        /// <summary>
        /// calculate txtPrixMetreLPvc * txtTotaleTaillPVC to txtPrixTotalPVC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrixMetreLPVC_TextChanged(object sender, EventArgs e)
        {
            double prix_mettres_pvc = 0;
            if (String.IsNullOrEmpty(t_prix_metre_linear_pvc.Text) || String.IsNullOrEmpty(t_total_size_pvc.Text))
                return;
            prix_mettres_pvc = double.Parse(t_prix_metre_linear_pvc.Text) * double.Parse(t_total_size_pvc.Text);
            t_prix_total_pvc.Text = prix_mettres_pvc.ToString("F2");
        }

        /// <summary>
        /// event to add new client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void e_Add_New_Client_Click(object sender, EventArgs e)
        {
            // check if field text fill
            if (checkIsNullOrEmpty())
            {
                Facture facture = new Facture();

                List<Mesure> vs = new List<Mesure>();

                List<Pvc> vspvc = new List<Pvc>();

                // add datagride to new list vs
                for (int i = 0; i < dg_mesure.Rows.Count; i++)
                {
                    vs.Add(new Mesure(Convert.ToDouble(dg_mesure.Rows[i].Cells[0].Value), Convert.ToDouble(dg_mesure.Rows[i].Cells[1].Value), Convert.ToDouble(dg_mesure.Rows[i].Cells[2].Value), cb_type_metres.SelectedItem.Equals("m3") ? Convert.ToDouble(dg_mesure.Rows[i].Cells[3].Value) : 0));
                }

                facture.Mesures = vs;
                facture.DateFacture = dp_date_facture.Value;
                facture.TypeDeBois = t_type_Bois.Text;
                facture.Categorie = t_categorie.Text;
                facture.Metrage = t_metrage_feuille.Text;
                facture.TypeMetres = cb_type_metres.SelectedItem.ToString();
                facture.PrixMetres = double.Parse(t_prix_metre_mesure.Text);
                facture.TotalMesure = double.Parse(t_total_size_mesure.Text);
                facture.PrixTotalMesure = double.Parse(txtPrixTotalMesure.Text);

                // check if combobox pvc is not null
                if (!string.IsNullOrEmpty(cb_type_pvc.Text))
                {


                    for (int i = 0; i < dg_pvc.Rows.Count; i++)
                    {
                        vspvc.Add(new Pvc(Convert.ToDouble(dg_pvc.Rows[i].Cells[0].Value),
                            Convert.ToDouble(dg_pvc.Rows[i].Cells[1].Value),
                            Convert.ToDouble(dg_pvc.Rows[i].Cells[2].Value),
                            (dg_pvc.Rows[i].Cells[3] as DataGridViewComboBoxCell).FormattedValue.ToString()));
                    }

                    facture.Pvcs = vspvc;
                    facture.TypePVC = cb_type_pvc.Text;
                    facture.CheckPVC = ck_seul_pvc.Checked;
                    facture.TailleCanto = double.Parse(t_size_pvc.Text);
                    facture.PrixMitresLinear = double.Parse(t_prix_metre_linear_pvc.Text);
                    facture.TotalTaillPVC = double.Parse(t_total_size_pvc.Text);
                    facture.PrixTotalPVC = double.Parse(t_prix_total_pvc.Text);
                }
                else
                {
                    vspvc.Clear();
                    facture.Pvcs = vspvc;
                    facture.TypePVC = "---";
                    facture.CheckPVC = ck_seul_pvc.Checked;
                    facture.TailleCanto = 0.0;
                    facture.PrixMitresLinear = 0.0;
                    facture.TotalTaillPVC = 0.0;
                    facture.PrixTotalPVC = 0.0;
                }

                // add all to class facture
                factures.Add(facture);

                // miss a jour l'id
                idFacture();

                if (!String.IsNullOrEmpty(t_prix_total_pvc.Text))
                    prix_total_client += double.Parse(txtPrixTotalMesure.Text) + double.Parse(t_prix_total_pvc.Text);
                else
                    prix_total_client += double.Parse(txtPrixTotalMesure.Text);

                t_prix_total_client.Text = prix_total_client.ToString("F2");

                if (double.Parse(t_prix_total_client.Text) == double.Parse(t_prix_rest_client.Text))
                    t_prix_rest_client.Text = "0.00";

                // vider les champs
                ViderTxtBox();

                verifyForm = true;
                b_save_client.Enabled = true;
            }
        }

        /// <summary>
        /// button add new facture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddNewFacture_Click(object sender, EventArgs e)
        {
            ViderTxtBox();
            cb_id_facture.Enabled = false;
            dp_date_facture.Enabled = true;
            dp_date_facture.Value = DateTime.Now;
            b_add_facture.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            b_add_facture.Text = "Ajouter";

            // change event for btn save
            b_add_facture.Click -= new EventHandler(this.e_Add_New_Client_Click);
            b_add_facture.Click -= new EventHandler(this.e_Edit_Factures_Click);
            b_add_facture.Click += new EventHandler(this.e_Add_New_Facture_Client_Click);

            // change event for text changed on this texts boxes
            t_prix_avance_client.TextChanged += new EventHandler(this.txtPrixAvanceClient_TextChanged);
            t_prix_total_client.TextChanged += new EventHandler(this.txtPrixAvanceClient_TextChanged);
        }

        /// <summary>
        /// event to add new facture to client already on database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void e_Add_New_Facture_Client_Click(object sender, EventArgs e)
        {
            try
            {
                // check if field was fill
                if (checkIsNullOrEmpty())
                {
                    connectionClient.Open();

                    // when facture have just pvc
                    if (ck_seul_pvc.Checked && cb_type_pvc.SelectedItem != null)
                    {
                        var mnd1 = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "INSERT INTO facture (idClient, dtDateFacture, typeDeBois, " +
                            "metrage, categorie, totalMesure, typeMetres, prixMetres, typePVC, checkPVC, tailleCanto, " +
                            "totalTaillPVC, prixMitresLinear, prixTotalPVC, prixTotalMesure) " +
                                          "values ('" + long.Parse(idClient[1]) + "', '" + DateTime.Today + "', '" +
                                          t_type_Bois.Text + "', '" + "---" + "','" + "---" + "', '" +
                                          0.0 + "', '" + cb_type_metres.SelectedItem.ToString() + "','" + 0.0 + "', '" +
                                          cb_type_pvc.Text + "', " + ck_seul_pvc.Checked + ", '" + double.Parse(t_size_pvc.Text) + "', '" +
                                          double.Parse(t_total_size_pvc.Text) + "', '" + double.Parse(t_prix_metre_linear_pvc.Text) + "', '" +
                                          double.Parse(t_prix_total_pvc.Text) + "', '" + 0.0 + "')"
                        };
                        mnd1.ExecuteNonQuery();

                        long idFS = 0;
                        /*get last idFacture from database*/
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
                        // insert facture where idFacture
                        for (int i = 0; i < dg_pvc.Rows.Count; i++)
                        {
                            OleDbCommand commandP = new OleDbCommand();
                            commandP.Connection = connectionClient;
                            commandP.CommandText = "INSERT INTO pvc (idFacture, quantite, largeur, longueur, orientation) " +
                                "values('" + idFS + "','" +
                                double.Parse(dg_pvc.Rows[i].Cells[0].Value.ToString()) + "','" +
                                double.Parse(dg_pvc.Rows[i].Cells[1].Value.ToString()) + "','" +
                                double.Parse(dg_pvc.Rows[i].Cells[2].Value.ToString()) + "','" +
                                dg_pvc.Rows[i].Cells[3].Value.ToString() + "')";
                            commandP.ExecuteNonQuery();
                        }
                    }
                    // when facture has just mesure
                    else if (cb_type_pvc.SelectedItem == null)
                    {
                        var mnd = new OleDbCommand
                        {
                            Connection = connectionClient,
                            CommandText = "INSERT INTO facture (idClient, dtDateFacture, typeDeBois, " +
                            "metrage, categorie, totalMesure, typeMetres, prixMetres, typePVC, checkPVC, tailleCanto, " +
                            "totalTaillPVC, prixMitresLinear, prixTotalPVC, prixTotalMesure) " +
                                          "values ('" + long.Parse(idClient[1]) + "', '" + DateTime.Today + "', '" +
                                          t_type_Bois.Text + "', '" + t_metrage_feuille.Text + "','" + t_categorie.Text + "', '" +
                                          double.Parse(t_total_size_mesure.Text) + "','" + cb_type_metres.SelectedItem.ToString() + "', '" +
                                          double.Parse(t_prix_metre_mesure.Text) + "', '" +
                                          "---" + "', " + ck_seul_pvc.Checked + ", '" + 0.0 + "', '" +
                                          0.0 + "', '" + 0.0 + "', '" +
                                          0.0 + "', '" + double.Parse(txtPrixTotalMesure.Text) + "')"
                        };
                        mnd.ExecuteNonQuery();


                        // mesure
                        if (!ck_seul_pvc.Checked && dg_mesure.Rows.Count > 0 && !cb_type_metres.SelectedItem.Equals("m"))
                        {
                            long idFM = 0;
                            /*get idFacture from database*/
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

                            for (int i = 0; i < dg_mesure.Rows.Count; i++)
                            {
                                OleDbCommand commandM = new OleDbCommand();
                                commandM.Connection = connectionClient;
                                commandM.CommandText = "INSERT INTO mesure(idFacture, quantite, largeur, longueur, eppaiseur)  " +
                                    "VALUES('" + idFM + "', '" +
                                    double.Parse(dg_mesure.Rows[i].Cells[0].Value.ToString()) + "', '" +
                                    double.Parse(dg_mesure.Rows[i].Cells[1].Value.ToString()) + "', '" +
                                    double.Parse(dg_mesure.Rows[i].Cells[2].Value.ToString()) + "', '" +
                                    (cb_type_metres.SelectedItem.Equals("m3") ? double.Parse(dg_mesure.Rows[i].Cells[3].Value.ToString()) : 0) + "')";
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
                                          t_type_Bois.Text + "', '" + t_metrage_feuille.Text + "','" + t_categorie.Text + "', '" +
                                          double.Parse(t_total_size_mesure.Text) + "','" + cb_type_metres.SelectedItem.ToString() + "', '" + double.Parse(t_prix_metre_mesure.Text) + "', '" +
                                          cb_type_pvc.Text + "', " + ck_seul_pvc.Checked + ", '" + double.Parse(t_size_pvc.Text) + "', '" +
                                          double.Parse(t_total_size_pvc.Text) + "', '" + double.Parse(t_prix_metre_linear_pvc.Text) + "', '" +
                                          double.Parse(t_prix_total_pvc.Text) + "', '" + double.Parse(txtPrixTotalMesure.Text) + "')"
                        };
                        mnd.ExecuteNonQuery();

                        long idFMS = 0;
                        /*get idFacture from database*/
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
                        if (!ck_seul_pvc.Checked && dg_mesure.Rows.Count > 0 && !cb_type_metres.SelectedItem.Equals("m"))
                        {
                            for (int i = 0; i < dg_mesure.Rows.Count; i++)
                            {
                                OleDbCommand commandM = new OleDbCommand();
                                commandM.Connection = connectionClient;
                                commandM.CommandText = "INSERT INTO mesure(idFacture, quantite, largeur, longueur, eppaiseur)  " +
                                    "VALUES('" + idFMS + "','" +
                                    double.Parse(dg_mesure.Rows[i].Cells[0].Value.ToString()) + "','" +
                                    double.Parse(dg_mesure.Rows[i].Cells[1].Value.ToString()) + "','" +
                                    double.Parse(dg_mesure.Rows[i].Cells[2].Value.ToString()) + "','" +
                                    (cb_type_metres.Text == "m3" ? double.Parse(dg_mesure.Rows[i].Cells[3].Value.ToString()) : 0) + "')";
                                commandM.ExecuteNonQuery();
                            }
                        }

                        for (int i = 0; i < dg_pvc.Rows.Count; i++)
                        {
                            OleDbCommand commandP = new OleDbCommand();
                            commandP.Connection = connectionClient;
                            commandP.CommandText = "INSERT INTO pvc (idFacture, quantite, largeur, longueur, orientation) " +
                                "values('" + idFMS + "','" +
                                double.Parse(dg_pvc.Rows[i].Cells[0].Value.ToString()) + "','" +
                                double.Parse(dg_pvc.Rows[i].Cells[1].Value.ToString()) + "','" +
                                double.Parse(dg_pvc.Rows[i].Cells[2].Value.ToString()) + "','" +
                                dg_pvc.Rows[i].Cells[3].Value.ToString() + "')";
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
                    cb_id_facture.Enabled = true;
                    cb_id_facture.Items.Clear();
                    while (readerFe.Read())
                    {
                        // get list of factures
                        cb_id_facture.Items.Add(long.Parse(readerFe["IDFACTURE"].ToString()));
                    }

                    connectionClient.Close();

                    cb_id_facture.SelectedIndex = cb_id_facture.Items.Count - 1;
                    b_add_facture.IconChar = FontAwesome.Sharp.IconChar.Edit;
                    b_add_facture.Text = "Modifier";

                    // change event for btn save
                    b_add_facture.Click -= new EventHandler(this.e_Add_New_Facture_Client_Click);
                    b_add_facture.Click -= new EventHandler(this.e_Add_New_Client_Click);
                    b_add_facture.Click += new EventHandler(this.e_Edit_Factures_Click);

                    // change event for text changed on this texts boxes
                    t_prix_avance_client.TextChanged += new EventHandler(this.txtPrixAvanceClient_TextChanged);
                    t_prix_total_client.TextChanged += new EventHandler(this.txtPrixAvanceClient_TextChanged);
                }
            }
            catch (Exception ex)
            {
                connectionClient.Close();
                LogFile.Message(ex);
            }
        }

        /// <summary>
        /// clear field text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ViderTxtBox();
        }

        /// <summary>
        /// button delete facture by id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteFacture_Click(object sender, EventArgs e)
        {
            try
            {
                if (cb_id_facture.Items.Count <= 0)
                {
                    message = new f_message("le client a aucun facture", "Erreur !!!", true, FontAwesome.Sharp.IconChar.Ban);
                    message.ShowDialog();
                    return;
                }

                // verifier que la liste des facture contient plus d'un un facture
                if (cb_id_facture.Items.Count <= 1)
                {
                    message = new f_message("Vous ne pouvez pas supprimer cette facture car le client n'a qu'une seule facture, vous devez supprimer le client.", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                    message.ShowDialog();
                    return;
                }

                message = new f_message("Voulez-vous vraiment supprimer cette facture", "Question", true, true, FontAwesome.Sharp.IconChar.ExclamationTriangle);

                if (message.ShowDialog() == DialogResult.No)
                    return;

                connectionClient.Open();

                OleDbCommand commandFacture = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText = "DELETE * FROM FACTURE WHERE idFacture = " + cb_id_facture.SelectedItem + "and idClient = " + long.Parse(idClient[1])
                };
                commandFacture.ExecuteNonQuery();

                connectionClient.Close();

                cb_id_facture.Items.Remove(cb_id_facture.SelectedItem);

                if (cb_id_facture.Items.Count >= 0)
                    cb_id_facture.SelectedIndex = 0;
                else
                    ViderTxtBox();
                connectionClient.Close();
            }
            catch (Exception ex)
            {
                connectionClient.Close();
                LogFile.Message(ex);
            }
        }

        /// <summary>
        /// button print facture and mesure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimerFacture_Click(object sender, EventArgs e)
        {
            if (cb_id_facture.Items.Count == 0)
                return;
            f_print printPdf = new f_print(idClient[1], cb_id_facture.SelectedItem.ToString(), "btnPrintFacture", ck_seul_pvc.Checked);
            printPdf.ShowDialog();
        }

        /// <summary>
        /// check price paying less than price total
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrixAvanceClient_TextChanged(object sender, EventArgs e)
        {
            double total = 0;
            double avance = 0;

            if (String.IsNullOrEmpty(t_prix_total_client.Text) || String.IsNullOrEmpty(t_prix_avance_client.Text))
                return;

            total = double.Parse(t_prix_total_client.Text);

            avance = double.Parse(t_prix_avance_client.Text);


            if (avance > total)
            {
                message = new f_message("le prix d'avance est grand que le prix total", "Attension", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                t_prix_avance_client.Text = "0.00";
                t_prix_rest_client.Text = "0.00";
                ck_avance_client.Checked = false;
            }
            else
                t_prix_rest_client.Text = (total - avance).ToString("F2");

        }

        /// <summary>
        /// enabled field text price paying
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAvance_CheckedChanged(object sender, EventArgs e)
        {
            t_prix_avance_client.Enabled = ck_avance_client.Checked;
            t_prix_avance_client.Text = "0.00";
            t_prix_avance_client.Focus();

        }

        /// <summary>
        /// button save facture to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveFacture_Click(object sender, EventArgs e)
        {
            verifyForm = false;
            saveDataClient();

            // fill datagride client
            remplissageDtGridClient();

            //visible panel and bring to frot
            b_add_client.Enabled = b_delete_client.Enabled = b_edit_client.Enabled = b_print_client.Enabled = true;

            //visible panel and bring to frot
            tblp_add_edit.Visible = false;
            p_home.Visible = true;
        }

        /// <summary>
        /// style for field text enter to change background color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNomClient_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(255, 170, 0);
            ((TextBox)sender).ForeColor = System.Drawing.Color.FromArgb(47, 47, 47);
        }

        /// <summary>
        /// style for field text leave to change background color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNomClient_Leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(47, 47, 47);
            ((TextBox)sender).ForeColor = System.Drawing.Color.FromArgb(255, 170, 0);
        }

        /// <summary>
        /// button delete client by id client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteClient_Click(object sender, EventArgs e)
        {
            if (dg_client.Rows.Count <= 0 || idClient == null)
            {
                message = new f_message("sélectionner une ligne š'il vous plaît", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            message = new f_message("Voulez-vous vraiment supprimer définitivement " + $"{dg_client.CurrentRow.Cells[1].Value}" + " de la liste?", "Attention", true, true, FontAwesome.Sharp.IconChar.ExclamationTriangle);

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
                }
                catch (Exception ex)
                {
                    connectionClient.Close();
                    LogFile.Message(ex);
                }

                /*
                 * fill datagride client
                 */
                remplissageDtGridClient();

                // initialize idClient
                idClient = null;

                // remove selected row
                if (dg_client.Rows.Count != 0)
                    dg_client.Rows[dg_client.CurrentRow.Index].Selected = false;
            }
        }
    }
}
