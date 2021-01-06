using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class PrintPdf : Form
    {
        String idClient;
        public PrintPdf(String idClient)
        {
            this.idClient = idClient;
            InitializeComponent();
        }

        private void PrintPdf_Load(object sender, EventArgs e)
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/factures";
            ReportDocument report = new ReportDocument();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + path + "/aicheBois.accdb";

            cmbShoosePrint.SelectedIndex = 0;

            //Client
            //OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM CLIENT WHERE IDCLIENT = " + long.Parse(idClient), connection);
            //DataSet dataSet = new DataSet();
            //adapter.Fill(dataSet, "client");
            ////report.Load(Application.StartupPath + @"CrystalReport1.rpt");
            //report.Load(@"CrystalReportClient.rpt");

            //report.SetDataSource(dataSet);
            //crystalRepClient.ReportSource = report;
            //dataSet.Dispose();

            // Facture
            OleDbDataAdapter adapterMesure = new OleDbDataAdapter("SELECT distinct * FROM facture WHERE IDCLIENT = " + long.Parse(idClient), connection);
            DataSet dataSetMesure = new DataSet();
            ReportDocument reportMesure = new ReportDocument();
            adapterMesure.Fill(dataSetMesure, "facture");
            //report.Load(Application.StartupPath + @"CrystalReport1.rpt");
            reportMesure.Load(@"CrystalReportMesure.rpt");

            //// Facture Numero, imprimer seul la facture selectionner
            //OleDbDataAdapter adapterMesure = new OleDbDataAdapter("select distinct * from facture where idFacture = " + long.Parse(idClient), connection);
            //DataSet dataSetMesure = new DataSet();
            //ReportDocument reportMesure = new ReportDocument();
            //adapterMesure.Fill(dataSetMesure, "facture");
            ////report.Load(Application.StartupPath + @"CrystalReport1.rpt");
            //reportMesure.Load(@cmbShoosePrint.SelectedItem.ToString());

            reportMesure.SetDataSource(dataSetMesure);
            crystalRepClient.ReportSource = reportMesure;
            dataSetMesure.Dispose();
            connection.Close();
        }
    }
}
