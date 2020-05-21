using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WannabeFarmVille.Animaux
{
    class Mouton : Animal
    {
        public static int Nombre_Moutons = 0;

        private const int MS = 1000;
        // Toutes les durées sont en "jours"

        private Timer CompteARebours { get; set; }

        private const int Jour = MS; // En millisecondes

        // Commence le timer et assigne un id à l'animal
        public Mouton(int X, int Y) : base(X, Y)
        {
            this.DernierRepas = DateTime.Now;
            Nombre_Moutons++;
            this.Faim = 120;
            Commencer_Timer(CompteARebours, Jour);
            this.X = X;
            this.Y = Y;
            this.image = Properties.Resources.moutonLeftDown;
            this.Type = 1;
            this.Gestation = 150;
            this.Croissance = 150;
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
            timer.Elapsed += OnTimedEvent;
        }

        /**
         * À chaque coup de timer (chaque jour donc),
         * chaque variable est réduite de 1.
         * Quand la variable arrive à 0, l'event associé se déclenche
         * et la variable est remise à sa valeur initiale.
         */
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Gestation--;
            Croissance--;
            Faim--;
            if (Gestation == 0)
            {
                // A un bébé
                Gestation = 110;
                Console.WriteLine("Fin de la Gestation");
            }
            if (Croissance == 0)
            {
                // Atteint la maturité
                Croissance = 110;
                Console.WriteLine("Fin de la Croissance");
            }
            if (Faim == 0)
            {
                // Contravention
                Faim = 120;
                Console.WriteLine("Fin de la Faim");
            }
        }

        internal override void ReloadImages()
        {
            if (MovingX == 1 && MovingY == 0)
            {
                this.image = Properties.Resources.moutonRightDown;
            }
            else if (MovingX == -1 && MovingY == 0)
            {
                this.image = Properties.Resources.moutonLeftDown;
            }

            else if (MovingX == 0 && MovingY == 1)
            {
                this.image = Properties.Resources.moutonRightUp;
            }
            else if (MovingX == 0 && MovingY == -1)
            {
                this.image = Properties.Resources.moutonLeftUp;
            }
        }
    }
}
