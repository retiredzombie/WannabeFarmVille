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
        private int Gestation { get; set; } = 110;
        private int Croissance { get; set; } = 110;
        private int Faim { get; set; } = 120;
        private int Genre { get; set; }
        private int ID { get; set; }
        private Timer CompteARebours { get; set; }

        private const int Jour = MS; // En millisecondes

        // Commence le timer et assigne un id à l'animal
        public Lion(int id)
        {
            Nombre_Lions++;
            this.ID = id;
            Commencer_Timer(CompteARebours, Jour);
            
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
    }
}