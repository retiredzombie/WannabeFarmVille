using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WannabeFarmVille
{
    /// <summary>
    /// Cette classe représente le joueur
    /// </summary>
    class Joueur: Movable
    {
        private double profitTotal;

        public Joueur(PictureBox PicUpLeft, PictureBox PicUpRight, PictureBox PicDownLeft, PictureBox PicDownRight,
            PictureBox PicLeftLeft, PictureBox PicLeftRight, PictureBox PicRightLeft, PictureBox PicRightRight, int CurrentRow,
            int CurrentColumn, int X, int Y, Tuile[,] carte)       
               : base(PicUpLeft, PicUpRight, PicDownLeft, PicDownRight, PicLeftLeft, PicLeftRight, PicRightLeft,
                       PicRightRight, CurrentRow, CurrentColumn, X, Y, carte)
        {
            this.Argent = 100;
            this.profitTotal = this.Argent;
            X = 0;
            Y = 0;
            CurrentRow = 0;
            CurrentColumn = 0;
            Width = 50;
            Height = 20;
            PeutNourrir = false;
        }
        public double Argent { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PictureBox JoeUpLeft { get; set; }
        public PictureBox JoeUpRight { get; set; }
        public Point JoeUpRightLocation { get; set; }
        public PictureBox JoeDownRight { get; set; }
        public Point JoeDownRightLocation { get; set; }
        public PictureBox JoeDownLeft { get; set; }
        public Point JoeDownLeftLocation { get; set; }
        public PictureBox JoeLeftLeft { get; set; }
        public Point JoeLeftLeftLocation { get; set; }
        public PictureBox JoeLeftRight { get; set; }
        public Point JoeLeftRightLocation { get; set; }
        public PictureBox JoeRightRight { get; set; }
        public Point JoeRightRightLocation { get; set; }
        public PictureBox JoeRightLeft { get; set; }
        public Enclo EncloChoisi { get; set; } = Enclo.PasEnclo;
        public bool PeutNourrir { get; set; }
        public double ProfitTotal { get => profitTotal; set => profitTotal = value; }

        public void RetirerArgent(int cout)
        {
            this.Argent -= cout;

            if (this.Argent < 0)
            {
                this.Argent = 0;
            }

            this.profitTotal = this.Argent;
        }

        public void AjouterArgent(double cout)
        {
            this.Argent += cout;
        }
    }
}
