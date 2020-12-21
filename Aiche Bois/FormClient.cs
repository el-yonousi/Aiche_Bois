using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class FormClient : Form
    {
        /// <summary>
        /// c'est l'access a extereiur Client data base
        /// </summary>
        private readonly OleDbConnection connectionClient = new OleDbConnection();

        /// <summary>
        /// prendre id client de data grid a partir de ligne selecionnees
        /// </summary>
        private String[] idClient;

        /// <summary>
        /// c'est le design du formulaire et l'initialisation de connecter a la base de donnees
        /// </summary>
        public FormClient()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/factures";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            connectionClient.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + path + "/aicheBois.accdb";

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
                Program.Clients.Clear();

                OleDbCommand commandClient = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText = "select * from client order by idClient"
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
                        CheckAvance = Convert.ToBoolean(readerClient["chAvance"]),
                        PrixTotalAvance = Convert.ToDouble(readerClient["prixTotalAvance"]),
                        PrixTotalRest = Convert.ToDouble(readerClient["prixTotalRest"]),
                        PrixTotalClient = Convert.ToDouble(readerClient["prixTotalClient"])
                    };

                    /*remplir a la liste client*/
                    Program.Clients.Add(client);
                }
                connectionClient.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dtGridFacture.Rows.Clear();
            foreach (Client ct in Program.Clients)
            {
                dtGridFacture.Rows.Add("N" + ct.IdClient.ToString("D4"), ct.NomClient, ct.DateClient.ToString("dd/MM/yyyy"), ct.NbFacture, "", ct.CheckAvance, ct.PrixTotalAvance.ToString("F2"), ct.PrixTotalRest.ToString("F2"), ct.PrixTotalClient.ToString("F2"));
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
                MessageBox.Show("selectionner une ligne s'il vous plait");
                return;
            }
            indxFacture = dtGridFacture.CurrentRow.Index;
        }

        /// <summary>
        /// c'est button pour imprimer les factures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintFacture_Click(object sender, EventArgs e)
        {
            if (indxFacture <= -1 || dtGridFacture.Rows.Count <= 0)
            {
                MessageBox.Show("la list est vide ou Selectionner une ligne", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ((Form)(prntPrevDiag)).WindowState = FormWindowState.Maximized;
            if (prntPrevDiag.ShowDialog() == DialogResult.OK)
            {
                prntDoc.Print();
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
            if (dtGridFacture.Rows.Count <= 0)
            {
                MessageBox.Show("selectionner une ligne s'il vous plait");
                return;
            }
            indxFacture = dtGridFacture.CurrentRow.Index;
        }

        /// <summary>
        /// c'est le traimtment pour imprimer les facture comme pdf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prntDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font fontBol = new Font("Nirmala UI", 14, FontStyle.Bold);
            Font fontReg = new Font("Nirmala UI", 12, FontStyle.Regular);

            string titre = "AICHE BOIS";
            SizeF fSTitre = e.Graphics.MeasureString(titre, new Font("Nirmala UI", 25, FontStyle.Bold));

            string idString = "Numéro du Client";
            SizeF fSIdString = e.Graphics.MeasureString(idString, fontBol);
            string idFacture = "N" + Program.Clients[indxFacture].IdClient.ToString("D4");

            float margin = 40;
            float heightString = fSIdString.Height;
            float changeHeight = margin + fSTitre.Height + 100;

            string nomString = "Nom du Client";
            string nom = Program.Clients[indxFacture].NomClient.ToString().ToUpper();

            string date = "Tanger le " + Program.Clients[indxFacture].DateClient.ToString("dd/MM/yyyy");

            string categorie = "Categorie";


            string couleurString = "Couleur";
            string couleur = "*****";

            string cantoString = "Type_canto";
            string nbrCanto = "*****";

            e.Graphics.DrawImage(Properties.Resources.logo, margin, margin, 100, 100);
            e.Graphics.DrawString(titre, new Font("Nirmala UI", 25, FontStyle.Bold), Brushes.Black, (e.PageBounds.Width - fSTitre.Width) / 2, margin);
            e.Graphics.DrawImage(Properties.Resources.logo, (e.PageBounds.Width - margin - 100), margin, 100, 100);

            e.Graphics.DrawString(date, fontBol, Brushes.Black, e.PageBounds.Width - fSTitre.Width - 20, changeHeight);

            e.Graphics.DrawString(idString, fontBol, Brushes.Black, margin, changeHeight);
            e.Graphics.DrawString(idFacture, fontReg, Brushes.Black, e.MarginBounds.Width / 2, changeHeight);

            changeHeight += heightString;
            e.Graphics.DrawString(nomString, fontBol, Brushes.Black, margin, changeHeight);
            e.Graphics.DrawString(nom, fontReg, Brushes.Black, e.MarginBounds.Width / 2, changeHeight);

            changeHeight += heightString + 20;
            e.Graphics.DrawString(categorie, fontBol, Brushes.Black, margin, changeHeight);

            changeHeight += heightString;
            e.Graphics.DrawString(couleurString, fontBol, Brushes.Black, margin, changeHeight);
            e.Graphics.DrawString(couleur, fontReg, Brushes.Black, e.MarginBounds.Width / 2, changeHeight);

            changeHeight += heightString;
            e.Graphics.DrawString(cantoString, fontBol, Brushes.Black, margin, changeHeight);
            e.Graphics.DrawString(nbrCanto, fontReg, Brushes.Black, e.MarginBounds.Width / 2, changeHeight);

            /*draw rectangle data*/
            changeHeight += heightString + 20;

            float colHeight = 40;
            float col1Width = 130;
            float col2Width = 400 + col1Width;
            float col3Width = 125 + col2Width;
            float col4Width = 125 + col3Width;
            float stringCenter = 5;
            /*draw Rectangle Facture
            e.Graphics.DrawRectangle(Pens.Black, margin, changeHeight, e.PageBounds.Width - margin * 2, e.PageBounds.Height - changeHeight - colHeight * 8);

            /*draw row1*/
            e.Graphics.DrawLine(Pens.Black, margin, changeHeight + colHeight, e.PageBounds.Width - margin, changeHeight + colHeight);
            changeHeight += 5;
            /*draw column 1*/
            e.Graphics.DrawString("QUANTITE", fontBol, Brushes.Black, margin + 15, changeHeight + stringCenter);
            e.Graphics.DrawLine(Pens.Black, margin + col1Width, changeHeight, margin + col1Width, changeHeight + colHeight);

            /*draw column 2*/
            SizeF fsDestination = e.Graphics.MeasureString("DESIGNATION", fontBol);
            e.Graphics.DrawString("DESIGNATION", fontBol, Brushes.Black, (e.PageBounds.Width - margin - col1Width - fsDestination.Width) / 2, changeHeight + stringCenter);
            e.Graphics.DrawLine(Pens.Black, margin + col2Width, changeHeight, margin + col2Width, changeHeight + colHeight);
            /*draw column 3*/

            e.Graphics.DrawString("P . U", fontBol, Brushes.Black, margin * 2 + col2Width - 5, changeHeight + stringCenter);
            e.Graphics.DrawLine(Pens.Black, margin + col3Width, changeHeight, margin + col3Width, changeHeight + colHeight);
            /*draw column 4*/

            e.Graphics.DrawLine(Pens.Black, margin, changeHeight, e.PageBounds.Width - margin, changeHeight);
            e.Graphics.DrawString("Total", fontBol, Brushes.Black, margin * 2 + col3Width - 10, changeHeight + stringCenter);

            /*draw row2*/
            foreach (Client facture in Program.Clients)
            {
                e.Graphics.DrawString(facture.PrixTotalClient.ToString("F2"), fontBol, Brushes.Black, margin * 2 + col3Width - 10, changeHeight + colHeight + stringCenter);
                e.Graphics.DrawString(facture.getNbFacture().ToString(), fontBol, Brushes.Black, margin * 2 + col2Width - 10, changeHeight + colHeight + stringCenter);
                e.Graphics.DrawString(facture.NomClient, fontBol, Brushes.Black, margin * 2 + col1Width - 10, changeHeight + colHeight + stringCenter);
                e.Graphics.DrawString(facture.IdClient.ToString("D4"), fontBol, Brushes.Black, margin * 2, changeHeight + colHeight + stringCenter);
                colHeight += 40;
                /*draw rows*/
                e.Graphics.DrawLine(Pens.Black, margin, changeHeight + colHeight, e.PageBounds.Width - margin, changeHeight + colHeight);
                /*draw column 4*/
                e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - margin, changeHeight, e.PageBounds.Width - margin, changeHeight + colHeight);
                /*draw column 3*/
                e.Graphics.DrawLine(Pens.Black, margin + col3Width, changeHeight, margin + col3Width, changeHeight + colHeight);
                /*draw column 2*/
                e.Graphics.DrawLine(Pens.Black, margin + col2Width, changeHeight, margin + col2Width, changeHeight + colHeight);
                /*draw column 1*/
                e.Graphics.DrawLine(Pens.Black, margin + col1Width, changeHeight, margin + col1Width, changeHeight + colHeight);
                /*draw column 0*/
                e.Graphics.DrawLine(Pens.Black, margin, changeHeight, margin, changeHeight + colHeight);
            }

            /*draw Rectangle Avance*/
            colHeight += 40;
            e.Graphics.DrawLine(Pens.Black, margin, changeHeight + colHeight, margin, changeHeight + colHeight + 100);
            e.Graphics.DrawLine(Pens.Black, margin, changeHeight + colHeight, e.PageBounds.Width - margin, changeHeight + colHeight);

            e.Graphics.DrawString("DATE", fontBol, Brushes.Black, margin + col1Width, changeHeight + colHeight + stringCenter);
            e.Graphics.DrawString("AVANCE", fontBol, Brushes.Black, margin * 2 + col2Width - 20, e.PageBounds.Height - (colHeight * 6) - 20);
            e.Graphics.DrawString("REST", fontBol, Brushes.Black, margin * 2 + col3Width - 10, e.PageBounds.Height - (colHeight * 6) - 20);
            /*draw row*/
            e.Graphics.DrawString(Program.Clients[indxFacture].DateClient.ToString("D"), fontReg, Brushes.Black, col1Width, e.PageBounds.Height - (colHeight * 5));
            e.Graphics.DrawString(Program.Clients[indxFacture].PrixTotalAvance.ToString("F2"), fontReg, Brushes.Black, margin + 5 + col2Width, e.PageBounds.Height - (colHeight * 5));
            e.Graphics.DrawString(Program.Clients[indxFacture].PrixTotalRest.ToString("F2"), fontReg, Brushes.Black, margin + 5 + col3Width, e.PageBounds.Height - (colHeight * 5));

            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - margin, changeHeight + colHeight, e.PageBounds.Width - margin, changeHeight + colHeight + 100);
            colHeight += 100;
            e.Graphics.DrawLine(Pens.Black, margin, changeHeight + colHeight, e.PageBounds.Width - margin, changeHeight + colHeight);

            /*string signature*/
            string signature1 = "MODE DE PAIMENT";
            SizeF fsSign1 = e.Graphics.MeasureString(signature1, fontReg);
            string signature2 = "50% : à la commande\t50% : à la finition";
            SizeF fsSign2 = e.Graphics.MeasureString(signature2, fontReg);
            string signature3 = "Validité de l'offre 1 mois";
            SizeF fsSign3 = e.Graphics.MeasureString(signature3, fontReg);
            e.Graphics.DrawString(signature1, fontReg, Brushes.Black, (e.PageBounds.Width - fsSign1.Width) / 2, e.PageBounds.Height - 80);
            e.Graphics.DrawString(signature2, fontReg, Brushes.Black, (e.PageBounds.Width - fsSign2.Width) / 2, e.PageBounds.Height - 60);
            e.Graphics.DrawString(signature3, fontReg, Brushes.Black, (e.PageBounds.Width - fsSign3.Width) / 2, e.PageBounds.Height - margin);
        }

        /// <summary>
        /// c'est button afficher la formulaire pour ajouter des factures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFacture_Click(object sender, EventArgs e)
        {
            FormAjoutFactures ajouterFacture = new FormAjoutFactures("0000", "add");
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
                foreach (Client ct in Program.Clients)
                {
                    String id = "N" + ct.IdClient.ToString("D4");
                    String dt = ct.DateClient.ToString("dd/MM/yyyy");
                    if (ct.NomClient.Contains(value: txtSearch.Text.ToUpper()) || id.Contains(value: txtSearch.Text) || dt.Contains(value: txtSearch.Text))
                        dtGridFacture.Rows.Add(id, ct.NomClient, dt, ct.NbFacture, "", ct.CheckAvance, ct.PrixTotalAvance.ToString("F2"), ct.PrixTotalRest.ToString("F2"), ct.PrixTotalClient.ToString("F2"));
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
            if (indxFacture <= -1 || dtGridFacture.Rows.Count <= 0 || idClient == null)
            {
                MessageBox.Show("la list est vide ou Selectionner une ligne", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                connectionClient.Open();
                OleDbCommand commandDelete = new OleDbCommand
                {
                    Connection = connectionClient,
                    CommandText = "delete * from client where idClient = " + Convert.ToInt64(idClient[1])
                };

                commandDelete.ExecuteNonQuery();
                connectionClient.Close();
                MessageBox.Show("le client " + "N" + idClient + " est supprimer avec success", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            /*
             * refrecher les donness du client
             */
            remplissageDtGridClient();
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
                MessageBox.Show("la list est vide ou Selectionner une ligne", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //send button click name
            FormAjoutFactures ajoutFactures = new FormAjoutFactures(idClient[1], "edit");
            ajoutFactures.ShowDialog();
            remplissageDtGridClient();
        }
    }
}
