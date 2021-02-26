using System;
using System.Collections.Generic;

namespace Aiche_Bois
{
    class Client
    {
        /*les accesseurs*/
        public int IdClient { get; set; }
        public string NomClient { get; set; }
        public DateTime DateClient { get; set; }
        public List<Facture> FactureClients { get; set; }
        public bool CheckAvance { get; set; }
        public double PrixTotalAvance { get; set; }
        public double PrixTotalRest { get; set; }
        public double PrixTotalClient { get; set; }
        public int NbFacture { get; set; }
    }
}
