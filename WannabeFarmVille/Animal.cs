using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WannabeFarmVille
{
    abstract class Animal
    {
        public bool AFaim { get; set; } = false;
        public DateTime DernierRepas { get => dernierRepas; set => dernierRepas = value; }
        public int Faim { get; set; }

        private DateTime dernierRepas;

        public Animal()
        {
            this.dernierRepas = DateTime.Now;
        }

        internal void NourrirDoublePrix(Joueur Player)
        {
            Player.RetirerArgent(2);
            AFaim = false;
            dernierRepas = DateTime.Now;
        }
    }
}
