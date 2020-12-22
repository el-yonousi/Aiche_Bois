using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using CrystalDecisions.CrystalReports.Engine;

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

            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM CLIENT WHERE IDCLIENT = " + long.Parse(idClient), connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "client");
            //report.Load(Application.StartupPath + @"CrystalReport1.rpt");
            report.Load(@"CrystalReportClient.rpt");

            report.SetDataSource(dataSet);
            crystalRepClient.ReportSource = report;
            dataSet.Dispose();
            connection.Close();
        }
    }
}
