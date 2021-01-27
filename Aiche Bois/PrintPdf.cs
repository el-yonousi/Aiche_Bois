using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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

            // Check the file
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/factures";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            connection.ConnectionString = Program.Path;

            InitializeComponent();
        }

        /// <summary>
        /// When the page loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintPdf_Load(object sender, EventArgs e)
        {
            // تحميل جميع معرفات الفاتورة من خلال معرف العميل
            try
            {
                connection.Open();

                OleDbCommand commandIdFacture = new OleDbCommand
                {
                    Connection = connection,
                    CommandText = "select idFacture, typeDeBois from facture where idClient = " + long.Parse(idClient)
                };

                OleDbDataReader readerIdFacture = commandIdFacture.ExecuteReader();
                while (readerIdFacture.Read())
                {
                    //تسجيل المعرفات في القائمة
                    cmbShoosePrint.Items.Add(readerIdFacture["idFacture"].ToString());
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
            cmbShoosePrint.Items.Add("Toutes Les Mesures");
            cmbShoosePrint.Items.Add("Toutes Les factures");

            if (btnClick == "btnPrintFacture")
            {
                cmbShoosePrint.SelectedItem = idFacture;
            }
            else
            {
                cmbShoosePrint.SelectedItem = "Toutes Les factures";
            }
        }

        /// <summary>
        /// Delete the selection of items in the Invoice knowledge list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbShoosePrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                //تحديد الطباعة حسب الزر المضغوط

                if (cmbShoosePrint.Text == "Toutes Les factures")
                {
                    btnCorrespondFacture.Visible = false;

                    //طباعة صفحة العميل، كل الفواتر
                    OleDbDataAdapter adapterClient = new OleDbDataAdapter("select distinct * from client where idClient = " + long.Parse(idClient), connection);
                    ReportDocument reportClient = new ReportDocument();
                    DataSet dataSetClient = new DataSet();
                    adapterClient.Fill(dataSetClient, "client");
                    //report.Load(Application.StartupPath + @"CrystalReport1.rpt");
                    reportClient.Load(@"CrystalReportClient.rpt");
                    reportClient.SetDataSource(dataSetClient);
                    crystalRepClient.ReportSource = reportClient;
                    dataSetClient.Dispose();
                }
                else if (cmbShoosePrint.Text == "Toutes Les Mesures")
                {
                    btnCorrespondFacture.Visible = false;

                    //طباعة صفحة القياسات من خلال زر نافدذة العميل
                    OleDbDataAdapter adapterFacturesMesure = new OleDbDataAdapter("SELECT distinct * FROM facture WHERE IDCLIENT = " + long.Parse(idClient), connection);
                    ReportDocument reportFacturesMesure = new ReportDocument();
                    DataSet dataSetFactures = new DataSet();
                    adapterFacturesMesure.Fill(dataSetFactures, "facture");
                    reportFacturesMesure.Load(@"CrystalReportMesure.rpt");
                    reportFacturesMesure.SetDataSource(dataSetFactures);
                    crystalRepClient.ReportSource = reportFacturesMesure;
                    dataSetFactures.Dispose();
                }
                else
                {
                    btnCorrespondFacture.Visible = true;

                    //طباعة صفحة العميل، من خلال رقم الفاتورة المحددة
                    OleDbDataAdapter adapterClientFacture = new OleDbDataAdapter("select distinct * from facture  where idFacture = " + long.Parse(cmbShoosePrint.Text), connection);
                    ReportDocument reportClientFacture = new ReportDocument();
                    DataSet dataSetClientFacture = new DataSet();
                    adapterClientFacture.Fill(dataSetClientFacture, "facture");
                    reportClientFacture.Load(@"CrystalReportClient.rpt");
                    reportClientFacture.SetDataSource(dataSetClientFacture);
                    crystalRepClient.ReportSource = reportClientFacture;
                    dataSetClientFacture.Dispose();
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
        }

        private void btnCorrespondFacture_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT checkPVC FROM facture Where idFacture = " + long.Parse(cmbShoosePrint.SelectedItem.ToString());// + " and idClient = " + long.Parse(idClient);
                OleDbDataReader readerIdClient = command.ExecuteReader();
                while (readerIdClient.Read())
                {
                    btnCheck = Convert.ToBoolean(readerIdClient["checkPVC"].ToString());
                }

                if (!btnCheck)
                {
                    // طباعة القياسات من خلال رقم الفاتورة
                    OleDbDataAdapter adapterMesure = new OleDbDataAdapter("SELECT distinct * FROM facture WHERE idFacture = " + long.Parse(cmbShoosePrint.Text), connection);
                    ReportDocument reportMesure = new ReportDocument();
                    DataSet dataSetMesure = new DataSet();
                    adapterMesure.Fill(dataSetMesure, "facture");
                    reportMesure.Load(@"CrystalReportMesure.rpt");
                    reportMesure.SetDataSource(dataSetMesure);
                    crystalRepClient.ReportSource = reportMesure;
                    dataSetMesure.Dispose();
                }
                else
                {
                    // طباعة ا من خلال رقم الفاتورة
                    OleDbDataAdapter adapterPvc = new OleDbDataAdapter("SELECT distinct * FROM facture WHERE idFacture = " + long.Parse(cmbShoosePrint.Text), connection);
                    ReportDocument reportPvc = new ReportDocument();
                    DataSet dataSetPvc = new DataSet();
                    adapterPvc.Fill(dataSetPvc, "facture");
                    reportPvc.Load(@"CrystalReportPvc.rpt");
                    reportPvc.SetDataSource(dataSetPvc);
                    crystalRepClient.ReportSource = reportPvc;
                    dataSetPvc.Dispose();
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
        }
    }
}