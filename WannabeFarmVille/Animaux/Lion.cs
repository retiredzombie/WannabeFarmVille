using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WannabeFarmVille
{
    class Lion : Animal
    {
        public static int Nombre_Lions = 0;

        private const int MS = 1000;
        // Toutes les durées sont en "jours"
       
        private Timer CompteARebours { get; set; }

        private const int Jour = MS; // En millisecondes

        // Commence le timer et assigne un id à l'animal
        public Lion(int X, int Y, Random rand) : base (X, Y, rand)
        {
            this.DernierRepas = DateTime.Now;
            Nombre_Lions++;
            this.Faim = 120;
            Commencer_Timer(CompteARebours, Jour);
            this.X = X;
            this.Y = Y;
            this.image = Properties.Resources.lionLeftDown;
            this.Type = 3;
            this.Gestation = 110;
            this.Croissance = 110;
        }

        /**
         * Commence le timer et setup ses paramètres.
         */
        private void Commencer_Timer(Timer timer, int temps)
        {
            timer = new Timer(temps);
            //timer = new Timer(MS);
            timer.AutoReset = true;
            timer.Start();
        }


        internal override void ReloadImages()
        {
            if (MovingX == 1 && MovingY == 0)
            {
                this.image = Properties.Resources.lionRightDown;
            }
            else if (MovingX == -1 && MovingY == 0)
            {
                this.image = Properties.Resources.lionLeftDown;
            }

            else if (MovingX == 0 && MovingY == 1)
            {
                this.image = Properties.Resources.lionRightUp;
            }
            else if (MovingX == 0 && MovingY == -1)
            {
                this.image = Properties.Resources.lionLeftUp;
            }
        }
    }
}