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
        public bool Adulte { get; set; }
        private bool adulte;
        public DateTime DernierRepas { get => dernierRepas; set => dernierRepas = value; }
        public int Faim { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int ID { get; set; }
        private bool enGestation;
        public int Type { get; set; }
        public int Enclos { get; set; } // 1:Haut-Gauche, 2:Haut-Droite, 3:Bas-Gauche, 4:Bas-Droite.
        public int MovingX { get => movingX; set => movingX = value; }
        public int MovingY { get => movingY; set => movingY = value; }
        public int Gestation { get; set; }
        public int Croissance { get; set; }
        public int CurrentRow { get; set; }
        private int gestation;
        private int croissance;
        public int CurrentColumn { get; set; }
        public Genre genre;
        public enum Genre
        {
            Male,
            Femelle
        }

        public Image image { get; set; }
        public bool EnGestation { get => enGestation; set => enGestation = value; }
        public DateTime DateNaissance { get => dateNaissance; set => dateNaissance = value; }
        public DateTime DebutGestation { get => debutGestation; set => debutGestation = value; }
        public int JrsDepuitDebGest { get => jrsDepuitDebGest; set => jrsDepuitDebGest = value; }
        public int Age { get => age; set => age = value; }
        public int JrsDepuitManger { get => jrsDepuitManger; set => jrsDepuitManger = value; }

        private DateTime dernierRepas;
        private DateTime dateNaissance;
        private DateTime debutGestation;

        private int jrsDepuitDebGest;
        private int age;
        private int jrsDepuitManger;

        public Animal(int X, int Y, Random rand)
        {
            this.dernierRepas = DateTime.Now;
            this.DateNaissance = DateTime.Now;
            this.enGestation = false;

            this.X = X;
            this.Y = Y;

            this.jrsDepuitDebGest = 0;
            this.age = 0;
            this.JrsDepuitManger = 0;

            Adulte = false;

            this.Enclos = TrouverEnclos();

            if (rand.Next(2) == 0)
            {
                genre = Genre.Femelle;
            }
            else
            {
                genre = Genre.Male;
            }
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
