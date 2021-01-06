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
        private string orientation;

        /*les constrecteurs*/
        public Mesure()
        {
            idMesure = ++cp;
        }

        public Mesure(long idFacture, double quantite, double largeur, double longueur, string type, string orienatation)
        {
            idMesure = ++cp;
            IdFacture = idFacture;
            Quantite = quantite;
            Largeur = largeur;
            Longueur = longueur;
            Type = type;
            Orientation = orienatation;
        }

        public Mesure(long idFacture, double quantite, double largeur, double longueur, double epaisseur, string type, string orienatation)
        {
            IdFacture = IdFacture;
            Quantite = quantite;
            Largeur = largeur;
            Longueur = longueur;
            Epaisseur = epaisseur;
            Type = type;
            Orientation = orienatation;
        }

        /*les accesseurs*/
        public double Quantite { get => quantite; set => quantite = value; }
        public double Largeur { get => largeur; set => largeur = value; }
        public double Longueur { get => longueur; set => longueur = value; }
        public double Epaisseur { get => epaisseur; set => epaisseur = value; }
        public string Type { get => type; set => type = value; }
        public long IdFacture { get => idFacture; set => idFacture = value; }
        public int IdMesure { get => idMesure; }
        public string Orientation { get => orientation; set => orientation = value; }
    }
}
