using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WannabeFarmVille
{
    public enum Enclo
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
        PasEnclo
    }
    class Tuile
    {
        private int type;
        Bitmap image;
        public Tuile()
        {
            EstUnObstacle = false;
            EstDansUnEnclo = false;
            Ligne = 0;
            Colonne = 0;
            Bouton = null;
        }
        public Tuile(int type)
        {
            this.type = type;
            EstUnObstacle = false;
        }

       public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public int getWitdth()
        {
            return image.Width;
        }
        public int getHeight()
        {
            return image.Height;
        }
        public bool EstUnObstacle { get; set; }
        public bool EstDansUnEnclo { get; set; }
        public bool EstAdjacente { get; set; } = false;
        public Dechet DechetSurTuile { get; set; } = null;
        public int Ligne { get; set; }
        public int Colonne { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Animal AnimalSurLaCase { get; set; }
        public Enclo PositionEnclo { get; set; } = Enclo.PasEnclo;
    }
}
