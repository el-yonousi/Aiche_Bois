using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;

namespace Aiche_Bois
{
    public partial class FormClient : Form
    {
        /// <summary>
        /// this form show message informations
        /// </summary>
        FormMessage message;
        /// <summary>
        /// c'est l'access a extereiur Client data base
        /// </summary>
        private readonly OleDbConnection connectionClient = new OleDbConnection();

        /// <summary>
        /// prendre id client de data grid a partir de ligne selecionnees
        /// </summary>
        private String[] idClient;

        /// <summary>
        /// stocker les client a liste des clients
        /// </summary>
        private List<Client> clients = new List<Client>();
        DataTable tb = new DataTable();

        /// <summary>
        /// c'est le design du formulaire et l'initialisation de connecter a la base de donnees
        /// </summary>
        public FormClient()
        {
            connectionClient.ConnectionString = Program.Path;

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

                //dtGridFacture.DataSource = tb;

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


                //for (int i = 0; i < tb.Rows.Count; i++)
                //{
                //    Client client = new Client
                //    {
                //        NbFacture = Convert.ToInt32(tb.Rows[i][0]),
                //        PrixTotalClient = Convert.ToDouble(tb.Rows[i][1]),
                //        PrixTotalRest = Convert.ToDouble(tb.Rows[i][2]),
                //        CheckAvance = Convert.ToBoolean(tb.Rows[i][3]),
                //        NomClient = tb.Rows[i][4].ToString(),
                //        DateClient = Convert.ToDateTime(tb.Rows[i][5]),
                //        PrixTotalAvance = Convert.ToDouble(tb.Rows[i][6]),
                //        IdClient = Convert.ToInt32(tb.Rows[i][7]),
                //    };
                //    clients.Add(client);
                //}


                //OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(commandClient);
                //DataTable dataTable = new DataTable();
                //dbDataAdapter.Fill(dataTable);
                //dtGridFacture.DataSource = dbDataAdapter;

                //OleDbDataReader readerClient = commandClient.ExecuteReader();
                //while (readerClient.Read())
                //{
                //    Client client = new Client
                //    {
                //        IdClient = Convert.ToInt32(readerClient["idClient"]),
                //        NomClient = readerClient["nomClient"].ToString(),
                //        DateClient = Convert.ToDateTime(readerClient["dateClient"]),
                //        NbFacture = Convert.ToInt32(readerClient["nbFacture"]),
                //        CheckAvance = Convert.ToBoolean(readerClient["cavance"]),
                //        PrixTotalAvance = Convert.ToDouble(readerClient["prixTotalAvance"]),
                //        PrixTotalRest = Convert.ToDouble(readerClient["rest"]),
                //        PrixTotalClient = Convert.ToDouble(readerClient["total"])
                //    };

                //    /*remplir a la liste client*/
                //    clients.Add(client);
                //}
                connectionClient.Close();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionClient.Close();
                return;
            }

            //dtGridFacture.Rows.Clear();
            //foreach (Client ct in clients)
            //{
            //    dtGridFacture.Rows.Add("N" + ct.IdClient.ToString("D4"),
            //        ct.NomClient, ct.DateClient.ToString("dd/MM/yyyy"),
            //        ct.NbFacture, "", ct.CheckAvance,
            //        ct.PrixTotalAvance.ToString("F2"),
            //        ct.PrixTotalRest.ToString("F2"),
            //        ct.PrixTotalClient.ToString("F2"));
            //}
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
        /// this method create pdf by id
        /// </summary>
        /// <param name="idClient"></param>
        /// <param name="idFacture"></param>
        [Obsolete]
        private void pvc_mesure(String idClient, long idFacture)
        {

            Document pdfDoc = new Document(PageSize.A4);

            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Properties.Resources.logo, System.Drawing.Imaging.ImageFormat.Png);

            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\aiche bois\\";
            String file = "facture.pdf";

            FileStream os = new FileStream(path + file, FileMode.Create);
            using (os)
            {

                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, os);
                pdfDoc.Open();
                String typeBois = "";
                string nom = "";
                //String mes = "";

                for (int i = 0; i < clients.Count; i++)
                {
                    nom = clients[i].NomClient;
                    typeBois += @"<tr><td align='center' colSpan='5'>" + $"{clients[i].PrixTotalRest}" + @"</td></tr>";
                    typeBois +=
                    @"<td align ='center'> " + $"{clients[i].IdClient}" + @"</td>
                                    <td align ='center'> " + $"{clients[i].NbFacture}" + @"</td>
                                    <td align ='center'> " + $"{clients[i].NomClient}" + @"</td>
                                    <td align ='center'> " + $"{ clients[i].PrixTotalClient}" + @"</td>
                               <tr >
                                    <td align ='center'> " + $"{clients[i]}" + @"</td>
                                 </tr > ";
                }
                string strHTML = @"<!DOCTYPE html>  
                        <html xmlns='http://www.w3.org/1999/xhtml'>  
                        <head>  
                            <title></title>  
                        </head>  
                        <body>  
                              
                             <table border='1' width ='100%' height ='100%'>
                                <!-- header -->
                                <tr style='font-weight: bold;'>  
                                    <td align='center'>Aiche Bois</td>
                                    <td rowSpan='2' align='center' style='font-size: 15pt;'>Etat Mesure</td>
                                    <td rowSpan='2' align='center'>Tanger le</td>           
                               </tr>
                               <tr>
                                    <td align='center'>Nom du Client</td>
                               </tr>
                                <
                               <tr>  
                                    <td align='center'>" + $"{nom}" + @"</td>  
                                    <td align='center'>" + String.Format("N{0:D4}", idClient) + @"</td>              
                                    <td align='center'>" + DateTime.Today.ToString("dd-MMMM-yyyy") + @"</td>              
                               </tr>
                        </table>
                        <br>
                        <table border='1' width ='100%' height ='100%'>
                                <!-- Quantite -->
                               <tr>  
                                    <td align='center' style='font-weight: bold; color: #7BA63C;'>Quantite</td>  
                                    <td align='center' style='font-weight: bold; color: #7BA63C;' colSpan='3'>" + "Mesure/Pvc" + @"</td>
                                    <td align='center' style='font-weight: bold; color: #7BA63C;'>Nbr_Canto</td>
                               </tr>
                               <!-- Couleur -->

                               " + $"{typeBois}" + @"
                            </table>
                        </body>  
                        </html>";
                HTMLWorker htmlWorker = new HTMLWorker(pdfDoc);
                htmlWorker.Parse(new StringReader(strHTML));
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                // to open pdf dynamique
                System.Diagnostics.Process.Start(path + file);
            }
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
        /// c'est button afficher la formulaire pour ajouter des factures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFacture_Click(object sender, EventArgs e)
        {
            FormAjoutFactures ajouterFacture = new FormAjoutFactures("", "");
            ajouterFacture.ShowDialog();
            remplissageDtGridClient();
        }

        /// <summary>
        /// c'est event chercher a client a partir du date, nom ou id client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
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

        /// <summary>
        /// c'est button supprimer client a partir de la ligne selectionnees
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteFacture_Click(object sender, EventArgs e)
        {
            if (indxFacture <= -1 ||
                dtGridFacture.Rows.Count <= 0 ||
                idClient == null)
            {
                message = new FormMessage("la list est vide ou Selectionner une ligne", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }

            message = new FormMessage("voulez vous vraiment supprimer c'est client", "Attention", true, true, FontAwesome.Sharp.IconChar.ExclamationTriangle);

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
                }

                /*
                 * refrecher les donness du client
                 */
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
    }
}
