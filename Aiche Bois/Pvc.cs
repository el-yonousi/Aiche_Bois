using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiche_Bois
{
    class Pvc
    {
        /*les attribus*/
        public static int cp = 0;
        private int idFacture;
        private int idPvc;
        private string ortn;
        private double qte;
        private double largr;
        private double longr;

        /*les constrecteurs*/
        public Pvc() {
            idPvc = ++cp;
        }

        public Pvc(string ortn, double qte, double largr, double longr)
        {
            idPvc = ++cp;
            Qte = qte;
            Largr = largr;
            Longr = longr;
            Ortn = ortn;
        }

        /*les accesseurs*/
        public double Qte { get => qte; set => qte = value; }
        public double Largr { get => largr; set => largr = value; }
        public double Longr { get => longr; set => longr = value; }
        public string Ortn { get => ortn; set => ortn = value; }
        public int IdFacture { get => idFacture; set => idFacture = value; }
        public int IdPvc { get => idPvc;}
    }
}
