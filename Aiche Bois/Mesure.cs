namespace Aiche_Bois
{
    class Mesure
    {
        /*les constrecteurs*/
        public Mesure()
        {
        }

        public Mesure(double quantite, double largeur, double longueur, double epaisseur)
        {
            Quantite = quantite;
            Largeur = largeur;
            Longueur = longueur;
            Epaisseur = epaisseur;
        }

        /*les accesseurs*/
        public double Quantite { get; set; }
        public double Largeur { get; set; }
        public double Longueur { get; set; }
        public double Epaisseur { get; set; }
        public long IdFacture { get; set; }
    }
}
