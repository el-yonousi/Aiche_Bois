using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class PrintPdf : Form
    {
        OleDbConnection connection = new OleDbConnection();

        /// <summary>
        /// Customer ID number
        /// </summary>
        String idClient;

        /// <summary>
        /// Invoice ID number
        /// </summary>
        String idFacture;

        /// <summary>
        /// Select the push button
        /// </summary>
        String btnClick;

        /// <summary>
        /// Check the PVC
        /// </summary>
        bool btnCheck;

        /// <summary>
        /// this list to stock data reader
        /// </summary>
        List<Client> clients = new List<Client>();
        List<Facture> factures = new List<Facture>();
        List<Mesure> mesures = new List<Mesure>();
        List<Pvc> pvcs = new List<Pvc>();

        /// <summary>
        /// this is a error form
        /// </summary>
        FormMessage message;

        /// <summary>
        /// this is a path to save factures
        /// </summary>
        String path = "";
        String file = "";

        /// <summary>
        /// Design page
        /// </summary>
        /// <param name="idClient"></param>
        /// <param name="idFacture"></param>
        /// <param name="btn"></param>
        public PrintPdf(String idClient, String idFacture, String btn, bool check)
        {
            this.idClient = idClient;
            this.idFacture = idFacture;
            this.btnClick = btn;
            this.btnCheck = check;

            // set connetion initialise
            connection.ConnectionString = Program.Path;

            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\aiche bois\\les factures\\";
            // If directory does not exist, don't even try   
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            InitializeComponent();
        }

        /// <summary>
        /// c'est method remplir data gride view de client
        /// </summary>
        private void remplissageDtGridClient()
        {
            try
            {
                connection.Open();
                // clear lists
                clients.Clear();
                factures.Clear();
                mesures.Clear();
                pvcs.Clear();
                long idf = 0;

                OleDbCommand commandClient = new OleDbCommand
                {
                    Connection = connection,
                    CommandText =
                    "SELECT count(idFacture) AS nbFacture, " +
                    "(sum(f.prixtotalmesure) + sum(prixtotalpvc)) AS total, " +
                    "IIF((sum(f.prixtotalmesure) + sum(prixtotalpvc)) = prixTotalAvance, 0, (sum(f.prixtotalmesure) + sum(prixtotalpvc)) - prixTotalAvance) AS rest, " +
                    "IIF(ROUND((sum(f.prixtotalmesure) + sum(prixtotalpvc)), 2) = ROUND((prixTotalAvance), 2), 'true', 'false') AS cavance, " +
                    "c.nomClient, " +
                    "dateClient, " +
                    "c.prixTotalAvance, " +
                    "c.idClient " +
                    "FROM client c INNER JOIN facture f ON f.idClient = c.idClient " +
                    "WHERE c.idClient = " + long.Parse(idClient) +
                    " GROUP BY nomClient, " +
                    "dateClient, " +
                    "prixTotalAvance, " +
                    "c.idClient " +
                    "ORDER BY c.idClient DESC;"
                };

                OleDbDataReader readerClient = commandClient.ExecuteReader();
                while (readerClient.Read())
                {
                    Client client = new Client();
                    client.IdClient = Convert.ToInt32(readerClient["idClient"]);
                    client.NomClient = readerClient["nomClient"].ToString();
                    client.DateClient = Convert.ToDateTime(readerClient["dateClient"]);
                    client.NbFacture = Convert.ToInt32(readerClient["nbFacture"]);
                    client.CheckAvance = Convert.ToBoolean(readerClient["cavance"]);
                    client.PrixTotalAvance = Convert.ToDouble(readerClient["prixTotalAvance"]);
                    client.PrixTotalRest = Convert.ToDouble(readerClient["rest"]);
                    client.PrixTotalClient = Convert.ToDouble(readerClient["total"]);

                    /*remplir a la liste client*/
                    clients.Add(client);
                }

                OleDbCommand commandFacture = new OleDbCommand
                {
                    Connection = connection,
                    CommandText = "SELECT * FROM facture WHERE idClient = " + long.Parse(idClient)
                };
                OleDbDataReader readerFacture = commandFacture.ExecuteReader();
                while (readerFacture.Read())
                {
                    Facture facture = new Facture();
                    idf = long.Parse(readerFacture["idFacture"].ToString());
                    facture.IdClient = long.Parse(readerFacture["idClient"].ToString());
                    facture.IDFacture = long.Parse(readerFacture["idFacture"].ToString());
                    facture.DateFacture = DateTime.Parse(readerFacture["dtdateFacture"].ToString());
                    facture.TypeDeBois = readerFacture["typeDeBois"].ToString();
                    facture.Metrage = readerFacture["metrage"].ToString();
                    facture.PrixMetres = double.Parse(readerFacture["prixMetres"].ToString());
                    facture.Categorie = readerFacture["categorie"].ToString();
                    facture.TotalMesure = double.Parse(readerFacture["totalMesure"].ToString());
                    facture.TypePVC = readerFacture["typePVC"].ToString();
                    facture.CheckPVC = Boolean.Parse(readerFacture["checkPVC"].ToString());
                    facture.TailleCanto = double.Parse(readerFacture["tailleCanto"].ToString());
                    facture.TotalTaillPVC = double.Parse(readerFacture["totalTaillPVC"].ToString());
                    facture.PrixMitresLinear = double.Parse(readerFacture["prixMitresLinear"].ToString());
                    facture.PrixTotalPVC = double.Parse(readerFacture["prixTotalPVC"].ToString());
                    facture.PrixTotalMesure = double.Parse(readerFacture["prixTotalMesure"].ToString());


                    OleDbCommand commandMesure = new OleDbCommand
                    {
                        Connection = connection,
                        CommandText = "SELECT * FROM Mesure WHERE idFacture = " + idf
                    };

                    OleDbDataReader readerMesure = commandMesure.ExecuteReader();
                    while (readerMesure.Read())
                    {
                        Mesure mesure = new Mesure
                        {
                            IdFacture = long.Parse(readerMesure["IdFacture"].ToString()),
                            Quantite = double.Parse(readerMesure["quantite"].ToString()),
                            Largeur = double.Parse(readerMesure["largeur"].ToString()),
                            Longueur = double.Parse(readerMesure["longueur"].ToString()),
                            Epaisseur = double.Parse(readerMesure["eppaiseur"].ToString())
                        };
                        mesures.Add(mesure);
                    }

                    OleDbCommand commandPvc = new OleDbCommand
                    {
                        Connection = connection,
                        CommandText = "SELECT * FROM Pvc WHERE idFacture = " + idf
                    };

                    OleDbDataReader readerPvc = commandPvc.ExecuteReader();
                    while (readerPvc.Read())
                    {
                        Pvc pvc = new Pvc
                        {
                            IdFacture = long.Parse(readerPvc["IdFacture"].ToString()),
                            Qte = double.Parse(readerPvc["quantite"].ToString()),
                            Largr = double.Parse(readerPvc["largeur"].ToString()),
                            Longr = double.Parse(readerPvc["longueur"].ToString()),
                            Ortn = readerPvc["orientation"].ToString()
                        };
                        pvcs.Add(pvc);
                    }

                    factures.Add(facture);
                }

                file = $"{clients[0].NomClient + String.Format("-N{0:D4}", idClient.ToString())}.pdf";

                connection.Close();
            }
            catch (Exception ex)
            {
                message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.Ban);
                message.ShowDialog();
                connection.Close();
                return;
            }
        }

        /// <summary>
        /// print client by id facture with id client
        /// </summary>
        /// <param name="idClient"></param>
        /// <param name="idFacture"></param>
        [Obsolete]
        private void client(long idFacture)
        {
            Document pdfDoc = new Document(PageSize.A4);

            FileStream os = new FileStream(path + file, FileMode.Create);
            using (os)
            {
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, os);
                pdfDoc.Open();
                PdfPTable tab1 = new PdfPTable(1);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(@"Resources\\header.jpg");
                tab1.WidthPercentage = 100;
                PdfPCell cel1 = new PdfPCell(image, true);
                cel1.BorderColor = iTextSharp.text.BaseColor.WHITE;
                
                tab1.SpacingAfter = 20;
                tab1.AddCell(cel1);
                pdfDoc.Add(tab1);

                // set query
                String str = "";
                int index = 0;
                for (int i = 0; i < factures.Count; i++)
                    if (factures[i].IDFacture == idFacture)
                    {
                        index = i;
                        break;
                    }

                if (factures[index].TypeDeBois != "---")
                {
                    str +=
                        @"<tr align='center'>
                				<td rowSpan='2' width='100'>" + $"{factures[index].TotalMesure}" + @"</td>
                				<td>" + $"{factures[index].TypeDeBois}" + @"</td>
                				<td rowSpan='2'>" + $"{factures[index].PrixMetres}" + @"</td>
                				<td rowSpan='2'>" + $"{factures[index].PrixTotalMesure}" + @"</td>
                			</tr>
                			<tr align='center'>
                				<td>" + $"{factures[index].Metrage}" + @"</td>
                			</tr>";
                };
                if (factures[index].TypePVC != "---")
                {
                    str += @"
                            <tr align='center'>
                				<td rowSpan='2'>" + $"{factures[index].TotalTaillPVC}" + @"</td>
                				<td>" + $"{factures[index].TypePVC}" + @"</td>
                				<td rowSpan='2'>" + $"{factures[index].PrixMitresLinear}" + @"</td>
                				<td rowSpan='2'>" + $"{factures[index].PrixTotalPVC}" + @"</td>
                			</tr>
                			<tr align='center'>
                				<td>" + $"{factures[index].TailleCanto}" + @"</td>
                			</tr>";
                }

                str += @"
                        <tr>
                            <td colSpan='3'>Total De facture</td>
                            <td align='center'>" + $"{(factures[index].PrixTotalPVC + factures[index].PrixTotalMesure)}" + @"</td>
                        </tr>
                        ";

                string strHTML = @"<!DOCTYPE html>  
                <html xmlns='http://www.w3.org/1999/xhtml'>  
                <head>  
                	<title></title>  
                </head>  
                <body>  
                	<!-- sub header -->
                	<table width = '100%' height = '100%'>
                		<!-- Numéro ID -->
                		<tr style='font-weight: bold;'>  
                			<td align='left'>Numéro Client: </td>
                			<td>" + String.Format("N{0:D4}", idClient) + @"</td>
                			<td align='right'>" + "Tanger le: " + DateTime.Today.ToString("dd MMMM yyyy") + @"</td>           
                		</tr> 
                		<!-- Nom -->
                		<tr>  
                			<td align='left'>Nom du Client</td>  
                			<td colspan=2>" + $"{clients[0].NomClient}" + @"</td>              
                		</tr>
                		<!-- Catègorie -->
                		<tr>  
                			<td align='left'>Catègorie</td>  
                			<td colspan=2>" + $"{factures[index].Categorie}" + @"</td>              
                		</tr>
                		<!-- Couleur -->
                		<tr>  
                			<td align='left'>Couleur</td>  
                			<td colspan=2>" + $"{factures[index].TypeDeBois}" + @"</td>              
                		</tr>
                		<!-- Couleur -->
                		<tr>  
                			<td align='left'>Type Canto</td>  
                			<td colspan=2>" + $"{factures[index].TypePVC}" + @"</td>              
                		</tr>
                	</table>
                	<br><br>
                	<!-- main header -->
                	<table border='1' align='center' width ='100%' height ='100%'>
                		<!-- this is a header for main -->
                		<thead>
                			<tr align='center' style='color: #7BA63C; padding: 5px; text-align: center; font-weight: bold;'>
                				<td>Quantite</td>
                				<td>Designation</td>
                				<td>P.U</td>
                				<td>Total</td>
                			</tr>
                		</thead>
                
                		<!-- this is a body for main -->
                		<tbody>
                			" + $"{str}" + @"
                		</tbody>
                
                		<!-- this is footer table -->
                		<tfoot>
                			<tr style='font-weight: bold;'>
                				<td colSpan='3'>Total</td>
                				<td align='center'>" + $"{clients[0].PrixTotalClient} dh" + @"</td>
                			</tr>
                		</tfoot>
                	</table>
                
                	<br>
                	<!-- sub body -->
                	<table border='1' align='center' width ='100%' height ='100%'>
                		<tr align='center' style='font-weight: bold;'>
                			<td colSpan='2'>Date</td>
                			<td>Prix Payé</td>
                			<td>Rest</td>
                		</tr>
                		<tr align='center'>
                			<td colSpan='2'>" + $"{clients[0].DateClient.ToString("dd MMMM yyyy")}" + @"</td>
                			<td>" + $"{clients[0].PrixTotalAvance} dh" + @"</td>
                			<td>" + $"{clients[0].PrixTotalRest} dh" + @"</td>
                		</tr>
                	</table>
                
                	<!-- this is footer page -->
                	<br>
                	<div style='font-weight: initial; font-size: 8pt;'>
                        <p>Numéro de téléphone: +212 55 5555 555</p>
                        <p>Adresse: Tanger 90060, Maroc</p>
                		<p>
                			MODE DE PAIMENT: 50% à la commande, 50% à la finition (Validité de l'offre 1 mois)
                		</p>
                    </div>
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
        /// this method create pdf by id
        /// </summary>
        /// <param name="idClient"></param>
        /// <param name="idFacture"></param>
        [Obsolete]
        private void client()
        {
            //طباعة صفحة العميل، كل الفواتر


            Document pdfDoc = new Document(PageSize.A4);

            FileStream os = new FileStream(path + file, FileMode.Create);
            using (os)
            {
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, os);
                pdfDoc.Open();
                PdfPTable tab1 = new PdfPTable(1);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(@"Resources\\header.jpg");
                tab1.WidthPercentage = 100;
                PdfPCell cel1 = new PdfPCell(image, true);
                cel1.BorderColor = iTextSharp.text.BaseColor.WHITE;

                tab1.SpacingAfter = 20;
                tab1.AddCell(cel1);
                pdfDoc.Add(tab1);

                // set query
                String str = "";
                String typeBois = "";
                String typePvc = "";
                foreach (Facture f in factures)
                {
                    if (f.TypeDeBois != "---")
                    {
                        typeBois = f.TypeDeBois + "/..";
                        str +=
                            @"<tr align='center'>
                				<td rowSpan='2' width='100'>" + $"{f.TotalMesure}" + @"</td>
                				<td>" + $"{f.TypeDeBois}" + @"</td>
                				<td rowSpan='2'>" + $"{f.PrixMetres}" + @"</td>
                				<td rowSpan='2'>" + $"{f.PrixTotalMesure}" + @"</td>
                			</tr>
                			<tr align='center'>
                				<td>" + $"{f.Metrage}" + @"</td>
                			</tr>";
                    };
                    if (f.TypePVC != "---")
                    {
                        typePvc = f.TypePVC + "/..";
                        str += @"
                            <tr align='center'>
                				<td rowSpan='2'>" + $"{f.TotalTaillPVC}" + @"</td>
                				<td>" + $"{f.TypePVC}" + @"</td>
                				<td rowSpan='2'>" + $"{f.PrixMitresLinear}" + @"</td>
                				<td rowSpan='2'>" + $"{f.PrixTotalPVC}" + @"</td>
                			</tr>
                			<tr align='center'>
                				<td>" + $"{f.TailleCanto}" + @"</td>
                			</tr>";
                    }
                }


                string strHTML = @"<!DOCTYPE html>  
                <html xmlns='http://www.w3.org/1999/xhtml'>  
                <head>  
                	<title></title>  
                </head>  
                <body>  
                	<!-- sub header -->
                	<table width = '100%' height = '100%'>
                		<!-- Numéro ID -->
                		<tr style='font-weight: bold;'>  
                			<td align='left'>Numéro Client: </td>
                			<td>" + String.Format("N{0:D4}", idClient) + @"</td>
                			<td align='right'>" + "Tanger le: " + DateTime.Today.ToString("dd MMMM yyyy") + @"</td>           
                		</tr> 
                		<!-- Nom -->
                		<tr>  
                			<td align='left'>Nom du Client</td>  
                			<td colspan=2>" + $"{clients[0].NomClient}" + @"</td>              
                		</tr>
                		<!-- Catègorie -->
                		<tr>  
                			<td align='left'>Catègorie</td>  
                			<td colspan=2>" + $"{factures[0].Categorie}/.." + @"</td>              
                		</tr>
                		<!-- Couleur -->
                		<tr>  
                			<td align='left'>Couleur</td>  
                			<td colspan=2>" + $"{typeBois}" + @"</td>              
                		</tr>
                		<!-- Couleur -->
                		<tr>  
                			<td align='left'>Type Canto</td>  
                			<td colspan=2>" + $"{typePvc}" + @"</td>              
                		</tr>
                	</table>
                	<br><br>
                	<!-- main header -->
                	<table border='1' align='center' width ='100%' height ='100%'>
                		<!-- this is a header for main -->
                		<thead>
                			<tr align='center' style='color: #7BA63C; padding: 5px; text-align: center; font-weight: bold;'>
                				<td>Quantite</td>
                				<td>Designation</td>
                				<td>P.U</td>
                				<td>Total</td>
                			</tr>
                		</thead>
                
                		<!-- this is a body for main -->
                		<tbody>
                			" + $"{str}" + @"
                		</tbody>
                
                		<!-- this is footer table -->
                		<tfoot>
                			<tr style='font-weight: bold;'>
                				<td colSpan='3'>Total</td>
                				<td align='center'>" + $"{clients[0].PrixTotalClient} dh" + @"</td>
                			</tr>
                		</tfoot>
                	</table>
                
                	<br>
                	<!-- sub body -->
                	<table border='1' align='center' width ='100%' height ='100%'>
                		<tr align='center' style='font-weight: bold;'>
                			<td colSpan='2'>Date</td>
                			<td>Prix Payé</td>
                			<td>Rest</td>
                		</tr>
                		<tr align='center'>
                			<td colSpan='2'>" + $"{clients[0].DateClient.ToString("dd MMMM yyyy")}" + @"</td>
                			<td>" + $"{clients[0].PrixTotalAvance} dh" + @"</td>
                			<td>" + $"{clients[0].PrixTotalRest} dh" + @"</td>
                		</tr>
                	</table>
                
                	<!-- this is footer page -->
                	<br>
                	<div style='font-weight: initial; font-size: 8pt;'>
                        <p>Numéro de téléphone: +212 700 48 31 86</p>
                        <p>Adresse: Tanger 90060, Maroc</p>
                		<p>
                			MODE DE PAIMENT: 50% à la commande, 50% à la finition (Validité de l'offre 1 mois)
                		</p>
                    </div>
                </body>  
                </html>";
                HTMLWorker htmlWorker = new HTMLWorker(pdfDoc);
                htmlWorker.Parse(new StringReader(strHTML));
                pdfWriter.CloseStream = true;
                pdfDoc.Close();

                // to open pdf dynamique
                System.Diagnostics.Process.Start(path + file);
            }
        }

        /// <summary>
        /// this method create pdf by id
        /// </summary>
        /// <param name="idClient"></param>
        /// <param name="idFacture"></param>
        [Obsolete]
        private void pvc_mesure(long idFacture)
        {
            Document pdfDoc = new Document(PageSize.A4);

            FileStream os = new FileStream(path + file, FileMode.Create);
            using (os)
            {

                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, os);
                pdfDoc.Open();
                PdfPTable tab1 = new PdfPTable(1);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(@"Resources\\sub-header.jpg");
                tab1.WidthPercentage = 100;
                PdfPCell cel1 = new PdfPCell(image, true);
                cel1.BorderColor = iTextSharp.text.BaseColor.WHITE;

                tab1.SpacingAfter = 20;
                tab1.AddCell(cel1);
                pdfDoc.Add(tab1);

                String typeBois = "";
                String type = "";
                String symbole = "";

                int index = 0;
                for (int i = 0; i < factures.Count; i++)
                    if (factures[i].IDFacture == idFacture)
                    {
                        index = i;
                        break;
                    }

                bool c_seul = factures[index].CheckPVC;

                if (!c_seul)
                {
                    type = "Mesure";
                    if (factures[index].TypeDeBois != "---")
                        typeBois += @"<tr><td align='center' colSpan='5'>" + $"{factures[index].TypeDeBois}" + @"</td></tr>";
                    for (int i = 0; i < mesures.Count; i++)
                    {
                        if (mesures[i].IdFacture == idFacture)
                        {
                            switch (pvcs.Count > 0 ? pvcs[i].Ortn : "0")
                            {
                                case "0": symbole = $"0"; break;
                                case "h*1": symbole = $"___"; break;
                                case "v*1": symbole = $"|"; break;
                                case "h*2": symbole = $"==="; break;
                                case "v*2": symbole = $"| |"; break;
                                case "4": symbole = $"[]"; break;
                            }

                            if (factures[index].TypeMetres != "m3")
                            {
                                typeBois +=
                               @"<tr>
                                    <td align = 'center'>" + $"{mesures[i].Quantite}" + @"</td>
                                    <td align = 'center'>" + $"{mesures[i].Largeur}" + @"</td>
                                    <td align = 'center'>" + $"{mesures[i].Longueur}" + @"</td>
                                    <td align = 'center' colSpan='2'>" + $"{symbole}" + @"</td>
                                 </tr> ";
                            }
                            else
                            {
                                typeBois +=
                               @"<tr>
                                    <td align = 'center'>" + $"{mesures[i].Quantite}" + @"</td>
                                    <td align = 'center'>" + $"{mesures[i].Largeur}" + @"</td>
                                    <td align = 'center'>" + $"{mesures[i].Longueur}" + @"</td>
                                    <td align = 'center'>" + $"{mesures[i].Epaisseur}" + @"</td>
                                    <td align = 'center'>" + $"{symbole}" + @"</td>
                                 </tr>";
                            }
                        }
                    }
                }
                else
                {
                    type = "Pvc";
                    if (factures[index].TypePVC != "---")
                        typeBois += @"<tr><td align='center' colSpan='5'>" + $"{factures[index].TypePVC}" + @"</td></tr>";
                    foreach (Pvc p in pvcs)
                    {
                        if (p.IdFacture == idFacture)
                        {
                            switch (p.Ortn)
                            {
                                case "0": symbole = $"0"; break;
                                case "h*1": symbole = $"___"; break;
                                case "v*1": symbole = $"|"; break;
                                case "h*2": symbole = $"==="; break;
                                case "v*2": symbole = $"| |"; break;
                                case "4": symbole = $"[]"; break;
                            }
                            typeBois +=
                           @"<tr>
                                <td align = 'center'>" + $"{p.Qte}" + @"</td>
                                <td align = 'center'>" + $"{p.Largr}" + @"</td>
                                <td align = 'center'>" + $"{p.Longr}" + @"</td>
                                <td align = 'center' colSpan='2'>" + $"{symbole}" + @"</td>
                            </tr> ";
                        }
                    }
                }

                string strHTML = @"<!DOCTYPE html>  
                        <html xmlns='http://www.w3.org/1999/xhtml'>  
                        <head>  
                            <title></title>  
                        </head>  
                        <body>  
                              
                             <table border='1' width ='100%' height ='100%'>
                               <tr>  
                                    <td align='center'>" + $"{clients[0].NomClient}" + @"</td>  
                                    <td align='center'>" + String.Format("N{0:D4}", idClient) + @"</td>              
                                    <td align='center'>" + DateTime.Today.ToString("dd MMMM yyyy") + @"</td>              
                               </tr>
                        </table>
                        <br>
                        <table border='1' width ='100%' height ='100%'>
                                <!-- Quantite -->
                               <tr>  
                                    <td align='center' style='font-weight: bold; color: #7BA63C;'>Quantite</td>  
                                    <td align='center' style='font-weight: bold; color: #7BA63C;' colSpan='3'>" + $"{type}" + @"</td>
                                    <td align='center' style='font-weight: bold; color: #7BA63C;'>Nbr_Canto</td>
                               </tr>
                               <!-- this is a query -->

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
        /// print all mesure
        /// </summary>
        [Obsolete]
        private void pvc_mesure()
        {
            Document pdfDoc = new Document(PageSize.A4);

            FileStream os = new FileStream(path + file, FileMode.Create);
            using (os)
            {

                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, os);
                pdfDoc.Open();
                PdfPTable tab1 = new PdfPTable(1);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(@"Resources\\sub-header.jpg");
                tab1.WidthPercentage = 100;
                PdfPCell cel1 = new PdfPCell(image, true);
                cel1.BorderColor = iTextSharp.text.BaseColor.WHITE;

                tab1.SpacingAfter = 20;
                tab1.AddCell(cel1);
                pdfDoc.Add(tab1);

                String typeBois = "";
                String type = "";
                String symbole = "";

                foreach (Facture f in factures)
                {
                    type = "Mesure";
                    if (f.TypeDeBois != "---")
                        typeBois += @"<tr><td align='center' colSpan='5'>" + $"{f.TypeDeBois}" + @"</td></tr>";
                    for (int i = 0; i < mesures.Count; i++)
                    {
                        if (mesures[i].IdFacture == f.IDFacture)
                        {
                            switch (pvcs.Count > 0 ? pvcs[i].Ortn : "0")
                            {
                                case "0": symbole = $"0"; break;
                                case "h*1": symbole = $"___"; break;
                                case "v*1": symbole = $"|"; break;
                                case "h*2": symbole = $"==="; break;
                                case "v*2": symbole = $"| |"; break;
                                case "4": symbole = $"[]"; break;
                            }

                            if (f.TypeMetres != "m3")
                            {
                                typeBois +=
                               @"<tr>
                                    <td align = 'center'>" + $"{mesures[i].Quantite}" + @"</td>
                                    <td align = 'center'>" + $"{mesures[i].Largeur}" + @"</td>
                                    <td align = 'center'>" + $"{mesures[i].Longueur}" + @"</td>
                                    <td align = 'center' colSpan='2'>" + $"{symbole}" + @"</td>
                                 </tr> ";
                            }
                            else
                            {
                                typeBois +=
                               @"<tr>
                                    <td align = 'center'>" + $"{mesures[i].Quantite}" + @"</td>
                                    <td align = 'center'>" + $"{mesures[i].Largeur}" + @"</td>
                                    <td align = 'center'>" + $"{mesures[i].Longueur}" + @"</td>
                                    <td align = 'center'>" + $"{mesures[i].Epaisseur}" + @"</td>
                                    <td align = 'center'>" + $"{symbole}" + @"</td>
                                 </tr>";
                            }
                        }
                    }
                }
                string strHTML = @"<!DOCTYPE html>  
                        <html xmlns='http://www.w3.org/1999/xhtml'>  
                        <head>  
                            <title></title>  
                        </head>  
                        <body>  
                              
                             <table border='1' width ='100%' height ='100%'>
                               <tr>  
                                    <td align='center'>" + $"{clients[0].NomClient}" + @"</td>  
                                    <td align='center'>" + String.Format("N{0:D4}", idClient) + @"</td>              
                                    <td align='center'>" + DateTime.Today.ToString("dd MMMM yyyy") + @"</td>              
                               </tr>
                        </table>
                        <br>
                        <table border='1' width ='100%' height ='100%'>
                                <!-- Quantite -->
                               <tr>  
                                    <td align='center' style='font-weight: bold; color: #7BA63C;'>Quantite</td>  
                                    <td align='center' style='font-weight: bold; color: #7BA63C;' colSpan='3'>" + $"{type}" + @"</td>
                                    <td align='center' style='font-weight: bold; color: #7BA63C;'>Nbr_Canto</td>
                               </tr>
                               <!-- this is a query -->

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
        /// When the page loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintPdf_Load(object sender, EventArgs e)
        {
            // تحميل جميع معرفات الفاتورة من خلال معرف العميل

            // load data
            remplissageDtGridClient();

            try
            {
                connection.Open();

                OleDbCommand commandIdFacture = new OleDbCommand
                {
                    Connection = connection,
                    CommandText = "select idFacture from facture where idClient = " + long.Parse(idClient)
                };

                OleDbDataReader readerIdFacture = commandIdFacture.ExecuteReader();
                while (readerIdFacture.Read())
                {
                    //تسجيل المعرفات في القائمة
                    lstFacture.Items.Add(readerIdFacture["idFacture"].ToString());
                    lstMesure.Items.Add(readerIdFacture["idFacture"].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                FormMessage message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                connection.Close();
                return;
            }

            //إضافة عنصر في قائمة المعرفات
            lstFacture.Items.Add("Toutes Les factures");
            lstMesure.Items.Add("Toutes Les Mesures");

            if (btnClick == "btnPrintFacture")
            {
                lstFacture.SelectedItem = idFacture;
            }
            else
            {
                lstFacture.SelectedItem = "Toutes Les factures";
            }
        }


        [Obsolete]
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //    //تحديد الطباعة حسب الزر المضغوط
                if (lstFacture.SelectedIndex > -1 && !lstFacture.SelectedItem.Equals("aucune"))
                {
                    if (lstFacture.SelectedItem.Equals("Toutes Les factures"))
                    {
                        ////طباعة صفحة العميل، كل الفواتر
                        file = $"Factures-{clients[0].NomClient + String.Format("-N{0:D4}", idClient)}.pdf";
                        client();
                    }
                    else
                    {
                        btnPrint.Visible = true;
                        file = $"{clients[0].NomClient + String.Format("-N{0:D4}-Facture-Numero-{1}", idClient, lstFacture.SelectedItem)}.pdf";
                        //طباعة صفحة العميل، من خلال رقم الفاتورة المحددة
                        client(long.Parse(lstFacture.SelectedItem.ToString()));

                    }
                }
                if (lstMesure.SelectedIndex > -1 && !lstMesure.SelectedItem.Equals("aucune"))
                {
                    if (lstMesure.Text == "Toutes Les Mesures")
                    {
                        file = $"Mesures-{clients[0].NomClient + String.Format("-N{0:D4}", idClient)}.pdf";

                        //طباعة صفحة القياسات من خلال زر نافدذة العميل
                        pvc_mesure();
                    }
                    else
                    {
                        file = $"{clients[0].NomClient + String.Format("-N{0:D4}-Facture-Numero-{1}", idClient, lstMesure.SelectedItem)}.pdf";

                        // طباعة القياسات من خلال رقم الفاتورة
                        pvc_mesure(long.Parse(lstMesure.Text));
                    }
                }
            }
            catch (Exception ex)
            {
                FormMessage message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
                message.ShowDialog();
                return;
            }
        }

        /// <summary>
        /// this is selected style
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstMesure_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            //if the item state is selected them change the back color 
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          Color.FromArgb(255, 170, 0));//Choose the color

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Draw the current item text
            e.Graphics.DrawString(lstMesure.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }
    }
}