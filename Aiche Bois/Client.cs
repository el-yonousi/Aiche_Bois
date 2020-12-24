using System;
using System.Collections.Generic;

namespace Aiche_Bois
{
    class Client
    {
        /*attribut*/
        private int idClient;
        private string nomClient;
        private DateTime dateClient;
        private int nbFacture;
        private bool checkAvance;
        private double prixTotalClient;
        private double prixTotalAvance;
        private double prixTotalRest;
        private List<Facture> factureClients = new List<Facture>();

        public Client()
        {
        }

        public Client(string nomClient, DateTime dateClient, List<Facture> factureClients, int nbFacture)
        {
            NomClient = nomClient;
            DateClient = dateClient;
            FactureClients = factureClients;
            NbFacture = nbFacture;
        }

        public Client(string nomClient, DateTime dateClient, int nbFacture, List<Facture> factureClients, bool checkAvance, double prixTotalAvance, double prixTotalRest, double prixTotalClient)
        {
            NbFacture = nbFacture;
            CheckAvance = checkAvance;
            PrixTotalAvance = prixTotalAvance;
            PrixTotalRest = prixTotalRest;
            PrixTotalClient = prixTotalClient;
        }

        public Client(string nomClient, DateTime dateClient, bool checkAvance, double prixTotalAvance, double prixTotalRest, double prixTotalClient, int nbFacture)
        {
            NomClient = nomClient;
            DateClient = dateClient;
            CheckAvance = checkAvance;
            PrixTotalAvance = prixTotalAvance;
            PrixTotalRest = prixTotalRest;
            PrixTotalClient = prixTotalClient;
            NbFacture = nbFacture;
        }

        /// <summary>
        /// c'est constrecteur pour remplir datagride view client
        /// </summary>
        /// <param name="nomClient"></param>
        /// <param name="dateClient"></param>
        /// <param name="checkAvance"></param>
        /// <param name="nbFacture"></param>
        /// <param name="prixTotalClient"></param>
        /// <param name="prixTotalAvance"></param>
        /// <param name="prixTotalRest"></param>
        /// <param name="factures"></param>
        public Client(string nomClient, DateTime dateClient, bool checkAvance, int nbFacture, double prixTotalClient, double prixTotalAvance, double prixTotalRest, List<Facture> factures)
        {
            NomClient = nomClient;
            DateClient = dateClient;
            CheckAvance = checkAvance;
            NbFacture = nbFacture;
            PrixTotalClient = prixTotalClient;
            PrixTotalAvance = prixTotalAvance;
            PrixTotalRest = prixTotalRest;
            FactureClients = factures;
        }


        /*les accesseurs*/
        public int IdClient { get => idClient; set => idClient = value; }
        public string NomClient { get => nomClient; set => nomClient = value; }
        public DateTime DateClient { get => dateClient; set => dateClient = value; }
        public List<Facture> FactureClients { get => factureClients; set => factureClients = value; }
        public bool CheckAvance { get => checkAvance; set => checkAvance = value; }
        public double PrixTotalAvance { get => prixTotalAvance; set => prixTotalAvance = value; }
        public double PrixTotalRest { get => prixTotalRest; set => prixTotalRest = value; }
        public double PrixTotalClient { get => prixTotalClient; set => prixTotalClient = value; }
        public int NbFacture { get => nbFacture; set => nbFacture = value; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is Client client && IdClient == client.IdClient;
        }

        public override int GetHashCode()
        {
            return 66531687 + idClient.GetHashCode();
        }
    }
}
