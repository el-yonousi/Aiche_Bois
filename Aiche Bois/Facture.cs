using System;
using System.Collections.Generic;


namespace Aiche_Bois
{
    internal class Facture
    {
        /*les attributs*/
        private List<Mesure> mesure = new List<Mesure>();
        private List<Pvc> pvcs = new List<Pvc>();

        private int idFacture;
        private int idClient;
        
        private string typeDeBois;
        private string categorie;
        private string metrage;
        private double prixMetres;
        private double totalMesure;
        private double prixTotalMesure;

        private string typePVC;
        private string tailleCanto;
        private double totalTaillPVC;
        private double prixMitresLinear;
        private double prixTotalPVC;

        private double prixAvance;

        /// <summary>
        /// getter and setter for class factur
        /// </summary>
        public int IDFacture { get => idFacture; set => idFacture = value; }
        public string TypeDeBois { get => typeDeBois; set => typeDeBois = value; }
        public string Metrage { get => metrage; set => metrage = value; }
        public double PrixMetres { get => prixMetres; set => prixMetres = value; }
        public double TotalMesure { get => totalMesure; set => totalMesure = value; }
        public string TypePVC { get => typePVC; set => typePVC = value; }
        public string TailleCanto { get => tailleCanto; set => tailleCanto = value; }
        public double TotalTaillPVC { get => totalTaillPVC; set => totalTaillPVC = value; }
        public double PrixMitresLinear { get => prixMitresLinear; set => prixMitresLinear = value; }
        public double PrixTotalPVC { get => prixTotalPVC; set => prixTotalPVC = value; }
        public double PrixAvance { get => prixAvance; set => prixAvance = value; }
        public double PrixTotalMesure { get => prixTotalMesure; set => prixTotalMesure = value; }
        public List<Mesure> Mesures { get => mesure; set => mesure = value; }
        public int IdClient { get => idClient; set => idClient = value; }
        public string Categorie { get => categorie; set => categorie = value; }
        public List<Pvc> Pvcs { get => pvcs; set => pvcs = value; }

        public Facture()
        {
        }

        public Facture(int idClient, string typeDeBois, string metrage, string categorie, double prixMetres, double totalMesure, string typePVC, string tailleCanto, double totalTaillPVC, double prixMitresLinear, double prixTotalPVC, double prixAvance, double prixTotalMesure, List<Mesure> mesures, List<Pvc> pvcs)
        {
            IdClient = idClient;
            TypeDeBois = typeDeBois;
            Metrage = metrage;
            PrixMetres = prixMetres;
            Categorie = categorie;
            TotalMesure = totalMesure;
            TypePVC = typePVC;
            TailleCanto = tailleCanto;
            TotalTaillPVC = totalTaillPVC;
            PrixMitresLinear = prixMitresLinear;
            PrixTotalPVC = prixTotalPVC;
            PrixAvance = prixAvance;
            PrixTotalMesure = prixTotalMesure;
            Mesures = mesures;
            Pvcs = pvcs;
        }

        /*les methods*/
        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            return IDFacture == ((Facture)obj).IDFacture;
        }

        /// <summary>
        /// returner la taille total de la mesure
        /// </summary>
        /// <returns></returns>
        public double getMesure()
        {
            double totale = 0;
            foreach (Mesure msr in Mesures)
            {
                if (msr.Epaisseur == 0 || double.IsNaN(msr.Epaisseur))
                {
                    totale += ((msr.Largeur * Math.Pow(10, -2)) * (msr.Longueur * Math.Pow(10, -2))) * msr.Quantite;
                }
                else
                {
                    totale += ((msr.Largeur * Math.Pow(10, -2)) * (msr.Longueur * Math.Pow(10, -2)) * (msr.Epaisseur * Math.Pow(10, -2))) * msr.Quantite;
                }
            }
            return totale;
        }

        /// <summary>
        /// returner le rest de prix total avance
        /// </summary>
        /// <returns></returns>
        public double getResteFacture()
        {
            if (getTotalFacture() - PrixAvance == getTotalFacture())
            {
                return 0;
            }

            return getTotalFacture() - PrixAvance;
        }

        /// <summary>
        /// returner le prix de prix total du facture
        /// </summary>
        /// <returns></returns>
        public double getTotalFacture()
        {
            return (PrixTotalMesure) + (PrixTotalPVC);
        }

        public override int GetHashCode()
        {
            return 66531687 + idFacture.GetHashCode();
        }
    }
}
