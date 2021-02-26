using System;
using System.Collections.Generic;


namespace Aiche_Bois
{
    internal class Facture
    {
        /// <summary>
        /// getter and setter for class factur
        /// </summary>
        public long IDFacture { get; set; }
        public string TypeDeBois { get; set; }
        public string Metrage { get; set; }
        public double PrixMetres { get; set; }
        public double TotalMesure { get; set; }
        public string TypePVC { get; set; }
        public double TailleCanto { get; set; }
        public double TotalTaillPVC { get; set; }
        public double PrixMitresLinear { get; set; }
        public double PrixTotalPVC { get; set; }
        public double PrixTotalMesure { get; set; }
        public List<Mesure> Mesures { get; set; }
        public long IdClient { get; set; }
        public string Categorie { get; set; }
        public List<Pvc> Pvcs { get; set; }
        public bool CheckPVC { get; set; }
        public DateTime DateFacture { get; set; }
        public String TypeMetres { get; set; }

        public Facture()
        {
        }
    }
}
