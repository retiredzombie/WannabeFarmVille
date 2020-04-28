using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WannabeFarmVille
{
    class Lion
    {
        private const int MS = 1000;

        // Toutes les durées sont en "jours"
        public int Gestation { get; set; } = 110;
        public int Croissance { get; set; } = 110;
        public int Faim { get; set; } = 120;
        public int Genre { get; private set; }
        public int ID { get; private set; }
        public Timer JoursGestation { get; private set; }

        public const int Jour = MS; // En millisecondes

        // Commence le timer qui décrémente les jours de chaque variable et assigne un id à l'animal
        public Lion(int id)
        {
            this.ID = id;
            Commencer_Timer(JoursGestation, Jour);
            
        }

        private void Commencer_Timer(Timer timer, int temps)
        {
            timer = new Timer(temps);
            //timer = new Timer(MS);
            timer.AutoReset = false;
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Gestation--;
            Croissance--;
            Faim--;
            if (Gestation == 0)
            {
                // A un bébé
                Gestation = 110;
            }
            if (Croissance == 0)
            {
                // Atteint la maturité
                Croissance = 110;
            }
            if (Faim == 0)
            {
                // Contravention
                Faim = 120;
            }
        }
    }
}