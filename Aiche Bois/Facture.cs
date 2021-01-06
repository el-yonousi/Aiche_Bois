using System;
using System.Collections.Generic;


namespace Aiche_Bois
{
    internal class Facture
    {
        /*les attributs*/
        private List<Mesure> mesure = new List<Mesure>();
        private List<Pvc> pvcs = new List<Pvc>();

        private long idFacture;
        private long idClient;

        private DateTime dateFacture;
        private string typeDeBois;
        private string categorie;
        private string metrage;
        private double prixMetres;
        private double totalMesure;
        private double prixTotalMesure;

        private string typePVC;
        private bool checkPVC;
        private string tailleCanto;
        private double totalTaillPVC;
        private double prixMitresLinear;
        private double prixTotalPVC;

        /// <summary>
        /// getter and setter for class factur
        /// </summary>
        public long IDFacture { get => idFacture; set => idFacture = value; }
        public string TypeDeBois { get => typeDeBois; set => typeDeBois = value; }
        public string Metrage { get => metrage; set => metrage = value; }
        public double PrixMetres { get => prixMetres; set => prixMetres = value; }
        public double TotalMesure { get => totalMesure; set => totalMesure = value; }
        public string TypePVC { get => typePVC; set => typePVC = value; }
        public string TailleCanto { get => tailleCanto; set => tailleCanto = value; }
        public double TotalTaillPVC { get => totalTaillPVC; set => totalTaillPVC = value; }
        public double PrixMitresLinear { get => prixMitresLinear; set => prixMitresLinear = value; }
        public double PrixTotalPVC { get => prixTotalPVC; set => prixTotalPVC = value; }
        public double PrixTotalMesure { get => prixTotalMesure; set => prixTotalMesure = value; }
        public List<Mesure> Mesures { get => mesure; set => mesure = value; }
        public long IdClient { get => idClient; set => idClient = value; }
        public string Categorie { get => categorie; set => categorie = value; }
        public List<Pvc> Pvcs { get => pvcs; set => pvcs = value; }
        public bool CheckPVC { get => checkPVC; set => checkPVC = value; }
        public DateTime DateFacture { get => dateFacture; set => dateFacture = value; }

        public Facture()
        {
        }

        public Facture(int idClient, DateTime dateFacture, string typeDeBois, string metrage, string categorie, double prixMetres, double totalMesure, string typePVC, bool checkPVC, string tailleCanto, double totalTaillPVC, double prixMitresLinear, double prixTotalPVC, double prixTotalMesure, List<Mesure> mesures, List<Pvc> pvcs)
        {
            DateFacture = dateFacture;
            IdClient = idClient;
            TypeDeBois = typeDeBois;
            Metrage = metrage;
            PrixMetres = prixMetres;
            Categorie = categorie;
            TotalMesure = totalMesure;
            TypePVC = typePVC;
            CheckPVC = checkPVC;
            TailleCanto = tailleCanto;
            TotalTaillPVC = totalTaillPVC;
            PrixMitresLinear = prixMitresLinear;
            PrixTotalPVC = prixTotalPVC;
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

        public override int GetHashCode()
        {
            return 66531687 + idFacture.GetHashCode();
        }
    }
}
