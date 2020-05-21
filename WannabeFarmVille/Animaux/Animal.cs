using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
