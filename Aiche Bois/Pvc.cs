namespace Aiche_Bois
{
    class Pvc
    {

        /*les constrecteurs*/
        public Pvc()
        {
        }

        public Pvc(double qte, double largr, double longr, string ortn)
        {
            Qte = qte;
            Largr = largr;
            Longr = longr;
            Ortn = ortn;
        }

        /*les accesseurs*/
        public double Qte { get; set; }
        public double Largr { get; set; }
        public double Longr { get; set; }
        public string Ortn { get; set; }
        public long IdFacture { get; set; }
    }
}
