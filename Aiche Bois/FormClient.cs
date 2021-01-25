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
        /// stocker les client a liste des clients
        /// </summary>
        private List<Client> clients = new List<Client>();

        /// <summary>
        /// c'est le design du formulaire et l'initialisation de connecter a la base de donnees
        /// </summary>
        public FormClient()
        {
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/factures";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            connectionClient.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = aicheBois.accdb";

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
                    "IIF((sum(f.prixtotalmesure) + sum(prixtotalpvc)) = prixTotalAvance, 'true', 'false') AS cavance, " +
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
                FormMessage message = new FormMessage("Erreur:: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
        message.ShowDialog();
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
        var message = new FormMessage("selectionner une ligne s'il vous plait", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
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
private void btnPrintFacture_Click(object sender, EventArgs e)
{
    if (indxFacture <= -1 || dtGridFacture.Rows.Count <= 0 || idClient == null)
    {
        var message = new FormMessage("selectionner une ligne s'il vous plait", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
        message.ShowDialog();
        return;
    }

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
        FormMessage message = new FormMessage("la list est vide ou Selectionner une ligne", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
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
        FormMessage mge = new FormMessage("la list est vide ou Selectionner une ligne", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
        mge.ShowDialog();
        return;
    }

    FormMessage message = new FormMessage("voulez vous vraiment supprimer c'est client", "Attention", true, true, FontAwesome.Sharp.IconChar.ExclamationTriangle);

    DialogResult dialog = message.ShowDialog();

    if (DialogResult.Yes == dialog)
    {
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
        }
        catch (Exception ex)
        {
            FormMessage mg = new FormMessage("Error: " + ex.Message, "Erreur", true, FontAwesome.Sharp.IconChar.RadiationAlt);
            mg.ShowDialog();
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
        FormMessage message = new FormMessage("la list est vide ou Selectionner une ligne", "Attention", true, FontAwesome.Sharp.IconChar.ExclamationTriangle);
        message.ShowDialog();
        return;
    }

    //send button click name
    FormAjoutFactures ajoutFactures = new FormAjoutFactures(idClient[1], "edit");
    ajoutFactures.ShowDialog();
    remplissageDtGridClient();
}
    }
}
