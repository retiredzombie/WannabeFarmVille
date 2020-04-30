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
    /// Cette classe est une entitée qui permet à un object de bouger
    /// Pour la faire fonctionner: 
    ///     Faite ne sorte que votre classe hérite de Movable puis créer un contructeur
    ///     qui prend les paramètres du constructeur de Movable (voir la classe Joueur pour un exemple)
    ///     et vous pourez utiliser les méthodes Move pour faire bouger votre object dans la direction que
    ///     vous voulez.
    ///     NOTE: Les picture box correspondent chacun à un sprite Ex: PicUpLeft représente 
    ///           la sprites ou le peronnage oun l'animal regarde vers le haut en faisant
    ///           un pas vers la gauche.
    ///     IMPORTANT: Assurez-vous que les éléments du tableaux Carte sont instanciés avant de l'envoyer
    ///                dans le constructeur, sinon ça va planter.
    /// </summary>
    class Movable
    {
        protected static Tuile[,] Carte = new Tuile[28, 40];
        public PictureBox PicUpLeft { get; set; }
        public PictureBox PicUpRight { get; set; }
        public PictureBox PicDownRight { get; set; }
        public PictureBox PicDownLeft { get; set; }
        public PictureBox PicLeftLeft { get; set; }
        public PictureBox PicLeftRight { get; set; }
        public PictureBox PicRightRight { get; set; }
        public PictureBox PicRightLeft { get; set; }
        public PictureBox CurrentSprite { get; set; }
        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        
        protected Movable(PictureBox PicUpLeft, PictureBox PicUpRight, PictureBox PicDownLeft, PictureBox PicDownRight,
            PictureBox PicLeftLeft, PictureBox PicLeftRight, PictureBox PicRightLeft, PictureBox PicRightRight, int CurrentRow,
            int CurrentColumn, int X, int Y, Tuile[,] carte)
        {
            this.PicUpLeft = PicUpLeft;
            this.PicUpRight = PicUpRight;
            this.PicDownLeft = PicDownLeft;
            this.PicDownRight = PicDownRight;
            this.PicLeftLeft = PicLeftLeft;
            this.PicLeftRight = PicLeftRight;
            this.PicRightLeft = PicRightLeft;
            this.PicRightRight = PicRightRight;
            this.CurrentRow = CurrentRow;
            this.CurrentColumn = CurrentColumn;
            this.X = X;
            this.Y = Y;
            Carte = carte;
            CurrentSprite = this.PicUpRight;            
        }
        public void MoveDown()
        {
            if (CurrentRow != 27)
            {
                if (!Carte[CurrentRow + 1, CurrentColumn].EstUnObstacle)
                {
                    CurrentRow++;
                    if (PicDownLeft.Visible == false)
                    {
                        PicDownLeft.Location = new Point(X, Y);
                        PicDownLeft.Visible = true;
                        if (CurrentSprite != PicDownLeft)
                        {
                            CurrentSprite.Visible = false;
                        }
                        CurrentSprite = PicDownLeft;
                    }
                    else
                    {
                        PicDownRight.Location = new Point(X, Y);
                        PicDownRight.Visible = true;
                        if (CurrentSprite != PicDownRight)
                        {
                            CurrentSprite.Visible = false;
                        }
                        CurrentSprite = PicDownRight;
                    }
                    Y += 32;
                    CurrentSprite.Location = new Point(X, Y);
                }
            }
        }
        public void MoveUp()
        {
            if (CurrentRow != 0)
            {
                if (!Carte[CurrentRow - 1, CurrentColumn].EstUnObstacle)
                {
                    CurrentRow--;
                    if (PicUpLeft.Visible == false)
                    {
                        PicUpLeft.Location = new Point(X, Y);
                        PicUpLeft.Visible = true;
                        if (CurrentSprite != PicUpLeft)
                        {
                            CurrentSprite.Visible = false;
                        }
                        CurrentSprite = PicUpLeft;
                    }
                    else
                    {
                        PicUpRight.Location = new Point(X, Y);
                        PicUpRight.Visible = true;
                        if (CurrentSprite != PicUpRight)
                        {
                            CurrentSprite.Visible = false;
                        }
                        CurrentSprite = PicUpRight;
                    }
                    Y -= 32;
                    CurrentSprite.Location = new Point(X, Y);
                }
            }
        }
        public void MoveRight()
        {
            if (CurrentColumn != 39)
            {
                if (!Carte[CurrentRow, CurrentColumn + 1].EstUnObstacle)
                {
                    CurrentColumn++;
                    if (PicRightLeft.Visible == false)
                    {
                        PicRightLeft.Location = new Point(X, Y);
                        PicRightLeft.Visible = true;
                        if (CurrentSprite != PicRightLeft)
                        {
                            CurrentSprite.Visible = false;
                        }
                        CurrentSprite = PicRightLeft;
                    }
                    else
                    {
                        PicRightRight.Location = new Point(X, Y);
                        PicRightRight.Visible = true;
                        if (CurrentSprite != PicRightRight)
                        {
                            CurrentSprite.Visible = false;
                        }
                        CurrentSprite = PicRightRight;
                    }
                    X += 32;
                    CurrentSprite.Location = new Point(X, Y);
                }
            }
        }
        public void MoveLeft()
        {
            if (CurrentColumn != 0)
            {
                if (!Carte[CurrentRow, CurrentColumn - 1].EstUnObstacle)
                {
                    CurrentColumn--;
                    if (PicLeftLeft.Visible == false)
                    {
                        PicLeftLeft.Location = new Point(X, Y);
                        PicLeftLeft.Visible = true;
                        if (CurrentSprite != PicLeftLeft)
                        {
                            CurrentSprite.Visible = false;
                        }
                        CurrentSprite = PicLeftLeft;
                    }
                    else
                    {
                        PicLeftRight.Location = new Point(X, Y);
                        PicLeftRight.Visible = true;
                        if (CurrentSprite != PicLeftRight)
                        {
                            CurrentSprite.Visible = false;
                        }
                        CurrentSprite = PicLeftRight;
                    }
                    X -= 32;
                    CurrentSprite.Location = new Point(X, Y);
                }
            }
        }
    }
}
