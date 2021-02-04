using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;

namespace Aiche_Bois
{
    public partial class FormClient : Form
    {
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
        /// c'est variable determiner si le click on button ajouter ou modifier
        /// </summary>
        private String btnDeterminClick = "";

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
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionType.Close();
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
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionType.Close();
            }
            return vs;
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
                dtGridFacture.Rows.Clear();
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
                    dtGridFacture.Rows.Add(
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
                dtGridFacture.Rows[0].Selected = false;
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
            if (dtGridFacture.Rows.Count <= 0 || e.RowIndex < 0)
                return;

            idClient = dtGridFacture.Rows[e.RowIndex].Cells[0].Value.ToString().Split('N');

            if (e.ColumnIndex == 4 && e.RowIndex < dtGridFacture.Rows.Count)
            {
                FormAvance avance = new FormAvance(idClient[1]);
                avance.ShowDialog();
                remplissageDtGridClient();
            }

            if (e.ColumnIndex == 9 && e.RowIndex < dtGridFacture.Rows.Count)
            {
                //send button click name
                FormAjoutFactures ajoutFactures = new FormAjoutFactures(idClient[1], "edit");
                ajoutFactures.ShowDialog();
                remplissageDtGridClient();
            }
            if (e.ColumnIndex == 10 && e.RowIndex < dtGridFacture.Rows.Count)
            {
                message = new FormMessage("Voulez-vous vraiment supprimer définitivement " + $"{dtGridFacture.CurrentRow.Cells[1].Value}" + " de la liste?", "Attention", true, true, FontAwesome.Sharp.IconChar.ExclamationTriangle);

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
                }
            }
        }

        /// <summary>
        /// c'est event prendre l'index de la ligne selectionnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGridFacture_SelectionChanged(object sender, EventArgs e)
        {
            if (dtGridFacture.Rows.Count <= 0)
            {
                message = new FormMessage("selectionner une ligne s'il vous plait", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }
            indxFacture = dtGridFacture.CurrentRow.Index;
        }

        /// <summary>
        /// c'est button pour imprimer les factures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Obsolete]
        private void btnPrintFacture_Click(object sender, EventArgs e)
        {
            if (indxFacture <= -1 || dtGridFacture.Rows.Count <= 0 || idClient == null)
            {
                message = new FormMessage("selectionner une ligne s'il vous plait", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }
            //pvc_mesure(idClient[1], indxFacture);
            PrintPdf print = new PrintPdf(idClient[1], "null", "btnPrintClient", false);
            print.ShowDialog();
        }

        /// <summary>
        /// c'est event pour prendre l'index de la ligne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private int indxFacture;
        private void dtGridFacture_DoubleClick(object sender, EventArgs e)
        {
            if (indxFacture <= -1 || dtGridFacture.Rows.Count <= 0 || idClient == null)
            {
                message = new FormMessage("la list est vide ou Selectionner une ligne", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }
            indxFacture = dtGridFacture.CurrentRow.Index;
            FormAjoutFactures formShow = new FormAjoutFactures(idClient[1], "edit");
            formShow.ShowDialog();
            remplissageDtGridClient();
        }

        /// <summary>
        /// c'est event chercher a client a partir du date, nom ou id client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text != "Rechercher le client par nom, identifiant ou date".ToLower())
            {
                //txtSearch.CharacterCasing = CharacterCasing.Upper;
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    dtGridFacture.Rows.Clear();
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        String id = String.Format("N{0:D4}", long.Parse(tb.Rows[i][0].ToString()));
                        String dt = String.Format("{0:dd/MM/yyyy}", tb.Rows[i][2].ToString());
                        if (tb.Rows[i][1].ToString().Contains(value: txtSearch.Text.ToUpper()) ||
                            id.Contains(value: txtSearch.Text) ||
                            dt.Contains(value: txtSearch.Text))
                            dtGridFacture.Rows.Add(
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
        }

        /// <summary>
        /// c'est le button qui modifier les factures du client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditFacture_Click(object sender, EventArgs e)
        {
            if (indxFacture <= -1 || dtGridFacture.Rows.Count <= 0 || idClient == null)
            {
                message = new FormMessage("la list est vide ou Selectionner une ligne", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            //send button click name
            FormAjoutFactures ajoutFactures = new FormAjoutFactures(idClient[1], "edit");
            ajoutFactures.ShowDialog();
            remplissageDtGridClient();
        }

        /// <summary>
        /// closing the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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
                dtGridFacture.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(47, 47, 47);
                dtGridFacture.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(255, 244, 228);
            }
        }

        private void dtGridFacture_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                dtGridFacture.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 244, 228);
                dtGridFacture.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(47, 47, 47);
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
            //U_Add_Edit uc = new U_Add_Edit("", "");
            //uc.Dock = DockStyle.Fill;
            //uc.Visible = false;
            //p_home.Controls.Add(uc);
            p_Add_Edit.Visible = false;
            t_home.Visible = true;
        }

        /// <summary>
        /// this is button edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (indxFacture <= -1 || dtGridFacture.Rows.Count <= 0 || idClient == null)
            {
                message = new FormMessage("selectionner une ligne s'il vous plait", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            //send button click name
            t_home.Visible = false;
            p_Add_Edit.Visible = true;
            
            //remplissageDtGridClient();
        }

        /// <summary>
        /// c'est button afficher la formulaire pour ajouter des factures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFacture_Click(object sender, EventArgs e)
        {
            //send button click name
            t_home.Visible = false;
            p_Add_Edit.Visible = true;
            //remplissageDtGridClient();
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
    }
}
