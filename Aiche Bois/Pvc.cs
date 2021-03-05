namespace Aiche_Bois
{
    class Pvc
    {

        /// <summary>
        /// Constructor:: by default
        /// </summary>
        public Pvc()
        {
        }

        /// <summary>
        /// Constructor:: double qte, double largr, double longr, string ortn
        /// </summary>
        /// <param name="qte"></param>
        /// <param name="largr"></param>
        /// <param name="longr"></param>
        /// <param name="ortn"></param>
        public Pvc(double qte, double largr, double longr, string ortn)
        {
            Qte = qte;
            Largr = largr;
            Longr = longr;
            Ortn = ortn;
        }

        /*Attributs*/
        public double Qte { get; set; }
        public double Largr { get; set; }
        public double Longr { get; set; }
        public string Ortn { get; set; }
        public long IdFacture { get; set; }
    }
}
