using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WannabeFarmVille
{
    class Tuile
    {
        private int type;
        Bitmap image;

        public Tuile()
        {
            EstUnObstacle = false;
            Ligne = 0;
            Colonne = 0;
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
        public int Ligne { get; set; }
        public int Colonne { get; set; }
    }
}
