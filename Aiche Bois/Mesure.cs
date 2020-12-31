namespace Aiche_Bois
{
    class Mesure
    {
        /*les attribus*/
        public static int cp = 0;
        private int idMesure;
        private long idFacture;
        private string type;
        private double quantite;
        private double largeur;
        private double longueur;
        private double epaisseur;

        /*les constrecteurs*/
        public Mesure() {
            idMesure = ++cp;
        }

        public Mesure(long idFacture, double quantite, double largeur, double longueur, string type)
        {
            idMesure = ++cp;
            IdFacture = idFacture;
            Quantite = quantite;
            Largeur = largeur;
            Longueur = longueur;
            Type = type;
        }

        public Mesure(long idFacture, double quantite, double largeur, double longueur, double epaisseur, string type)
        {
            IdFacture = IdFacture;
            Quantite = quantite;
            Largeur = largeur;
            Longueur = longueur;
            Epaisseur = epaisseur;
            Type = type;
        }

        /*les accesseurs*/
        public double Quantite { get => quantite; set => quantite = value; }
        public double Largeur { get => largeur; set => largeur = value; }
        public double Longueur { get => longueur; set => longueur = value; }
        public double Epaisseur { get => epaisseur; set => epaisseur = value; }
        public string Type { get => type; set => type = value; }
        public long IdFacture { get => idFacture; set => idFacture = value; }
        public int IdMesure { get => idMesure;}
    }
}
