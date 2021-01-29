using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

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
                clients.Clear();

                OleDbCommand commandClient = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText =
                    "SELECT count(idFacture) AS nbFacture, " +
                    "(sum(f.prixtotalmesure) + sum(prixtotalpvc)) AS total, " +
                    "IIF((sum(f.prixtotalmesure) + sum(prixtotalpvc)) = prixTotalAvance, 0, (sum(f.prixtotalmesure) + sum(prixtotalpvc)) - prixTotalAvance) AS rest, " +
                    "IIF(ROUND((sum(f.prixtotalmesure) + sum(prixtotalpvc)), 2) = ROUND((prixTotalAvance), 2), 'true', 'false') AS cavance, " +
                    "c.nomClient, " +
                    "dateClient, " +
                    "c.prixTotalAvance, " +
                    "c.idClient " +
                    "FROM client AS c INNER JOIN facture AS f ON f.idClient = c.idClient " +
                    "GROUP BY nomClient, " +
                    "dateClient, " +
                    "prixTotalAvance, " +
                    "c.idClient " +
                    "ORDER BY c.idClient DESC;"
                };

                //OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(commandClient);
                //DataTable dataTable = new DataTable();
                //dbDataAdapter.Fill(dataTable);
                //dtGridFacture.DataSource = dbDataAdapter;

                OleDbDataReader readerClient = commandClient.ExecuteReader();
                while (readerClient.Read())
                {
                    Client client = new Client
                    {
                        IdClient = Convert.ToInt32(readerClient["idClient"]),
                        NomClient = readerClient["nomClient"].ToString(),
                        DateClient = Convert.ToDateTime(readerClient["dateClient"]),
                        NbFacture = Convert.ToInt32(readerClient["nbFacture"]),
                        CheckAvance = Convert.ToBoolean(readerClient["cavance"]),
                        PrixTotalAvance = Convert.ToDouble(readerClient["prixTotalAvance"]),
                        PrixTotalRest = Convert.ToDouble(readerClient["rest"]),
                        PrixTotalClient = Convert.ToDouble(readerClient["total"])
                    };

                    /*remplir a la liste client*/
                    clients.Add(client);
                }
                connectionClient.Close();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connectionClient.Close();
                return;
            }

            dtGridFacture.Rows.Clear();
            foreach (Client ct in clients)
            {
                dtGridFacture.Rows.Add("N" + ct.IdClient.ToString("D4"),
                    ct.NomClient, ct.DateClient.ToString("dd/MM/yyyy"),
                    ct.NbFacture, "", ct.CheckAvance,
                    ct.PrixTotalAvance.ToString("F2"),
                    ct.PrixTotalRest.ToString("F2"),
                    ct.PrixTotalClient.ToString("F2"));
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
                Program.indxFacture = dtGridFacture.Rows[e.RowIndex].Index;

                FormAvance avance = new FormAvance(idClient[1]);
                avance.ShowDialog();
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
                var strArr = new string[4] { "one", "Two", "Three", "Four" };
                Console.WriteLine($"Hello, {idClient}! Today is {DateTime.Today}, it's {DateTime.Today:HH:mm} now.");
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
                foreach (Client ct in clients)
                {
                    String id = "N" + ct.IdClient.ToString("D4");
                    String dt = ct.DateClient.ToString("dd/MM/yyyy");
                    if (ct.NomClient.Contains(value: txtSearch.Text.ToUpper()) ||
                        id.Contains(value: txtSearch.Text) ||
                        dt.Contains(value: txtSearch.Text))
                        dtGridFacture.Rows.Add(id,
                            ct.NomClient,
                            dt, ct.NbFacture,
                            "",
                            ct.CheckAvance,
                            ct.PrixTotalAvance.ToString("F2"),
                            ct.PrixTotalRest.ToString("F2"),
                            ct.PrixTotalClient.ToString("F2"));
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

        private void btnResult_Click(object sender, EventArgs e)
        {
            double a = 0;
            double b = 0;
            double c = 0;
            foreach (DataGridViewRow row in dtGridFacture.Rows)
            {
                if (row.Cells[6].Value == null)
                {
                    break;
                }

                a += double.Parse(row.Cells[6].Value.ToString());
                b += double.Parse(row.Cells[7].Value.ToString());
                c += double.Parse(row.Cells[8].Value.ToString());
            }

            SaveFileDialog save = new SaveFileDialog();
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/aiche bois/";
            save.InitialDirectory = path;
            save.FileName = "information";
            save.Title = "Information sur les montants totaux de la société Aiche Bois";
            save.DefaultExt = "txt";
            save.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            save.FilterIndex = 1;

            if (save.ShowDialog() == DialogResult.OK)
            {
                TextWriter txt = new StreamWriter(save.FileName);

                String t = "******" + save.Title + "******\n\n" +
                    "======= Date: " + DateTime.Today.ToString("dddd-MMMM-yyyy") + " =======\n\n" +
                    "==========================================================================\n" +
                    "======= Total de Prix Payée pour tous les clients: " + a + " dh =======\n" +
                    "======= Reste total pour tous les clients: " + b + " dh =======\n" +
                    "======= Montant Total pour Tous les Clients: " + c + " dh =======";
                txt.Write(t);
                txt.Close();

                FileInfo fileInfo = new FileInfo(save.FileName);
                fileInfo.IsReadOnly = true;
            }
        }
    }
}
