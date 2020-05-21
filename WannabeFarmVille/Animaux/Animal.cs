using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WannabeFarmVille
{
    abstract class Animal
    {
        private int movingX;
        private int movingY;

        public bool AFaim { get; set; } = false;
        public DateTime DernierRepas { get => dernierRepas; set => dernierRepas = value; }
        public int Faim { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int ID { get; set; }
        public int Type { get; set; }
        public int Enclos { get; set; } // 1:Haut-Gauche, 2:Haut-Droite, 3:Bas-Gauche, 4:Bas-Droite.
        public int MovingX { get => movingX; set => movingX = value; }
        public int MovingY { get => movingY; set => movingY = value; }
        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }

        public Image image { get; set; }

        private DateTime dernierRepas;

        public Animal(int X, int Y)
        {
            this.dernierRepas = DateTime.Now;

            this.X = X;
            this.Y = Y;

            this.Enclos = TrouverEnclos();
        }

        private int TrouverEnclos()
        {
            int enclosNum = 0;

            int vX = X / 32;
            int vY = Y / 32;

            //HAUT-GAUCHE
            if ((vX >= 5 && vX <= 12) && (vY >= 4 && vY <= 13))
            {
                enclosNum = 1;
            }
            //HAUT-DROITE
            if ((vX >= 25 && vX <= 33) && (vY >= 4 && vY <= 13))
            {
                enclosNum = 2;
            }

            //BAS-GAUCHE
            if ((vY >= 16 && vY <= 25) && (vX >= 5 && vX <= 12))
            {
                enclosNum = 3;
            }
            //BAS-DROITE
            if ((vX >= 25 && vX <= 33) && (vY >= 16 && vY <= 25))
            {
                enclosNum = 4;
            }

            return enclosNum;
        }

        internal void NourrirDoublePrix(Joueur Player)
        {
            Player.RetirerArgent(2);
            AFaim = false;
            dernierRepas = DateTime.Now;
        }

        internal abstract void ReloadImages();
    }
}
