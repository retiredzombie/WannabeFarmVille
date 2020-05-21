using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WannabeFarmVille.Animaux
{
    class Grizzly : Animal
    {
        public static int Nombre_Grizzlys = 0;

        private const int MS = 1000;
        // Toutes les durées sont en "jours"

        private Timer CompteARebours { get; set; }

        private const int Jour = MS; // En millisecondes

        // Commence le timer et assigne un id à l'animal
        public Grizzly(int X, int Y, Random rand) : base(X, Y, rand)
        {
            this.DernierRepas = DateTime.Now;
            Nombre_Grizzlys++;
            this.Faim = 120;
            Commencer_Timer(CompteARebours, Jour);
            this.X = X;
            this.Y = Y;
            this.image = Properties.Resources.grizzlyLeftDown;
            this.Type = 2;
            this.Gestation = 220;
            this.Croissance = 220;
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
                this.image = Properties.Resources.grizzlyRightDown;
            }
            else if (MovingX == -1 && MovingY == 0)
            {
                this.image = Properties.Resources.grizzlyLeftDown;
            }

            else if (MovingX == 0 && MovingY == 1)
            {
                this.image = Properties.Resources.grizzlyRightUp;
            }
            else if (MovingX == 0 && MovingY == -1)
            {
                this.image = Properties.Resources.grizzlyLeftUp;
            }
        }
    }
}
