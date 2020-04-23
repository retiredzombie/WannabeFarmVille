using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            X = 0;
            Y = 0;
            Width = 50;
            Height = 20;
        }
        public int Argent { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public PictureBox JoeUpLeft { get; set; }
        public PictureBox JoeUpRight { get; set; }
        public PictureBox JoeDownRight { get; set; }
        public PictureBox JoeDownLeft { get; set; }
        public PictureBox JoeLeftLeft { get; set; }
        public PictureBox JoeLeftRight { get; set; }
        public PictureBox JoeRightRight { get; set; }
        public PictureBox JoeRightLeft { get; set; }

    }
}
