using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WannabeFarmVille.Animaux
{
    class Rhino : Animal
    {
        public static int Nombre_Rhinos = 0;

        private const int MS = 1000;
        // Toutes les durées sont en "jours"

        private Timer CompteARebours { get; set; }

        private const int Jour = MS; // En millisecondes

        // Commence le timer et assigne un id à l'animal
        public Rhino(int X, int Y, Random rand) : base(X, Y, rand)
        {
            this.DernierRepas = DateTime.Now;
            Nombre_Rhinos++;
            this.Faim = 120;
            Commencer_Timer(CompteARebours, Jour);
            this.X = X;
            this.Y = Y;
            this.image = Properties.Resources.rhinoLeftDown;
            this.Type = 5;
            this.Gestation = 480;
            this.Croissance = 480;
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
                this.image = Properties.Resources.rhinoRightDown;
            }
            else if (MovingX == -1 && MovingY == 0)
            {
                this.image = Properties.Resources.rhinoLeftDown;
            }

            else if (MovingX == 0 && MovingY == 1)
            {
                this.image = Properties.Resources.rhinoRightUp;
            }
            else if (MovingX == 0 && MovingY == -1)
            {
                this.image = Properties.Resources.rhinoLeftUp;
            }
        }
    }
}
