using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WannabeFarmVille
{
    /// <summary>
    /// Cette classe représente le joueur
    /// </summary>
    class Joueur
    {
        public Joueur()
        {
            Argent = 100;
            X = 50;
            Y = 50;
        }
        public int Argent { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
