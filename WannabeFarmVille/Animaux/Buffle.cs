using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WannabeFarmVille.Animaux
{
    class Buffle : Animal
    {
        public static int Nombre_Buffles = 0;

        private const int MS = 1000;
        // Toutes les durées sont en "jours"

        private Timer CompteARebours { get; set; }

        private const int Jour = MS; // En millisecondes

        // Commence le timer et assigne un id à l'animal
        public Buffle(int X, int Y, Random rand) : base(X, Y, rand)
        {
            this.DernierRepas = DateTime.Now;
            Nombre_Buffles++;
            this.Faim = 120;
            Commencer_Timer(CompteARebours, Jour);
            this.X = X;
            this.Y = Y;
            this.image = Properties.Resources.buffleLeftDown;
            this.Type = 6;
            this.Gestation = 340;
            this.Croissance = 340;
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
                this.image = Properties.Resources.buffleRightDown;
            }
            else if (MovingX == -1 && MovingY == 0)
            {
                this.image = Properties.Resources.buffleLeftDown;
            }

            else if (MovingX == 0 && MovingY == 1)
            {
                this.image = Properties.Resources.buffleRightUp;
            }
            else if (MovingX == 0 && MovingY == -1)
            {
                this.image = Properties.Resources.buffleLeftUp;
            }
        }
    }
}
