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
        public Joueur(PictureBox PicUpLeft, PictureBox PicUpRight, PictureBox PicDownLeft, PictureBox PicDownRight,
            PictureBox PicLeftLeft, PictureBox PicLeftRight, PictureBox PicRightLeft, PictureBox PicRightRight, int CurrentRow,
            int CurrentColumn, int X, int Y, Tuile[,] carte)       
               : base(PicUpLeft, PicUpRight, PicDownLeft, PicDownRight, PicLeftLeft, PicLeftRight, PicRightLeft,
                       PicRightRight, CurrentRow, CurrentColumn, X, Y, carte)
        {
            Argent = 100;
            X = 0;
            Y = 0;
            CurrentRow = 0;
            CurrentColumn = 0;
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
        public PictureBox CurrentSprite { get; set; }

    }
}
