using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestTilesetMario;
using WannabeFarmVille.Properties;

namespace WannabeFarmVille
{

    public partial class Jeu : Form
    {
        private static Tuile[,] Carte = new Tuile[28, 40];
        private Map map;
        private Joueur Player;
        private List<Visiteur> visiteurs;
        private Bitmap ImgJoe = new Bitmap(Properties.Resources.joeExotic);
        private Graphics g;
        Bitmap tuile;
        List<PictureBox> visiteursPicBox;
        MenuDepart menuDepart;

        public Jeu(MenuDepart menuDepart)
        {
            InitializeComponent();

            this.menuDepart = menuDepart;
            Init();
        }

        /*
         * Initialiser les composants du jeu/
         */
        private void Init()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
            map = new Map(this.Width, this.Height, TilesetImageGenerator.GetTile(0));
            tuile = TilesetImageGenerator.GetTile(0);
            for (int row = 0; row < 28; row++)
            {
                for (int column = 0; column < 40; column++)
                {
                    Carte[row, column] = new Tuile();
                }
            }
            RendreClotureSolide(2, 4);
            RendreClotureSolide(14, 4);
            RendreClotureSolide(14, 25);
            RendreClotureSolide(2, 25);
            PicUpRight.Size = new Size(32, 32);
            PicUpRight.Location = new Point(0, 32);
            Player = new Joueur(PicUpLeft, PicUpRight, PicDownLeft, PicDownRight, 
                PicLeftLeft, PicLeftRight, PicRightLeft, PicRightRight, 0, 0, 0, 32, Carte);
            Player.Y += tuile.Height;
            visiteurs = new List<Visiteur>();
            Player.Width = tuile.Width;
            Player.Height = tuile.Height;
            Player.JoeDownLeft = PicDownLeft;
            Player.JoeDownRight = PicDownRight;
            Player.JoeUpLeft = PicUpLeft;
            Player.JoeUpRight = PicUpRight;
            Player.JoeRightLeft = PicRightLeft;
            Player.JoeRightRight = PicRightRight;
            Player.JoeLeftLeft = PicLeftLeft;
            Player.JoeLeftRight = PicLeftRight;
            Player.CurrentSprite = Player.JoeUpRight;
            //Stream str = Properties.Resources.rd2;
            //System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            //snd.Play();
            Player.CurrentSprite = Player.JoeUpRight;

            visiteursPicBox = new List<PictureBox>();
            for (int i = 0; i < 10; i++)
            {
               AjouterVisiteurSpawn();
            }
        }

        private void RendreClotureSolide(int row, int column)
        {   
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                column++;
            }
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                row++;
            }
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                column--;
            }
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                row--;
            }
        }

        /*
         * Ajoute un visiteur à la tuile position X, Y (en tuiles pas en pixels).
         * Le spawn des visiteurs est à (19, 28).
         */
        public void AjouterVisiteur(Genre genre, int x, int y)
        {
            visiteurs.Add(new Visiteur(tuile.Width * x, tuile.Height * y));
        }

        /*
         * Ajoute un visiteur directemnet au spawn de visiteur.
         */
        public void AjouterVisiteurSpawn()
        {
            visiteurs.Add(new Visiteur(tuile.Width * 19, tuile.Height * 25));
            /*
            PictureBox newVisiteur = new PictureBox();
            newVisiteur.BackgroundImage = visiteurs[visiteurs.Count - 1].imageVisiteur;
            newVisiteur.Location = new Point(visiteurs[visiteurs.Count - 1].X, visiteurs[visiteurs.Count - 1].Y);
            newVisiteur.Width = visiteurs[visiteurs.Count - 1].Width;
            newVisiteur.Height = visiteurs[visiteurs.Count - 1].Height;
            newVisiteur.BringToFront();
            newVisiteur.Name = "visiteurPB" + visiteursPicBox.Count.ToString().Trim();
            newVisiteur.BackgroundImageLayout = ImageLayout.Stretch;
            
            this.Controls.Add(newVisiteur);

            visiteursPicBox.Add(newVisiteur);
            */
        }


        /* Tout ce qui est dessiner sur l'écran.
         */
        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;

            g.DrawImage(Properties.Resources.Background_game, 0, 0, this.Width, this.Height);

            for (int i = 0; i < visiteurs.Count; i++)
            {
                g.DrawImage(visiteurs[i].imageVisiteur, visiteurs[i].X, visiteurs[i].Y, 32, 32);
            }
            
        }

        /* Logique du jeu (1x par tick).
         */
        private void Logic()
        {
            LogicVisiteurs();
        }

        /*
         * Fait bouger les visiteurs.
         */
        private void LogicVisiteurs()
        {
            Console.WriteLine("LogicVisiteurs.");

            for (int i = 0; i < visiteurs.Count; i++)
            {
                int randX = new Random().Next(3);
                int randY = new Random().Next(3);
                while ((randX == randY) ||
                        (randY == 0 && visiteurs[i].Y - tuile.Height <= 0 + tuile.Height) ||
                        (randY == 1 && visiteurs[i].Y + tuile.Height >= this.Height - tuile.Height) ||
                        (randX == 0 && visiteurs[i].X - tuile.Width <= 0 + tuile.Width) ||
                        (randX == 1 && visiteurs[i].X + tuile.Width >= this.Width - tuile.Height)
                        )
                {
                    randX = new Random().Next(3);
                    randY = new Random().Next(3);
                }

                    if (randX == 0)
                    { 
                        visiteurs[i].X -= tuile.Width;
                        visiteurs[i].MovingX = -1;
                    } 
                    else if (randX == 1)
                    { 
                        visiteurs[i].X += tuile.Width;
                        visiteurs[i].MovingX = 1;
                    }

                    if (randY == 0)
                    {
                        visiteurs[i].Y -= tuile.Height;
                        visiteurs[i].MovingY = -1;
                    }
                    else if (randY == 1)
                    {
                        visiteurs[i].Y += tuile.Height;
                        visiteurs[i].MovingY = 1;
                    }

                    visiteurs[i].ReloadImages();

                    /*
                    string visiteurPBName = "visiteurPB" + i.ToString().Trim();
                    Control[] foundVisiteurs = Controls.Find(visiteurPBName, true);
                    PictureBox visiteurPB = (PictureBox)foundVisiteurs.First();
                    if (randX == 0) visiteurPB.Location = new Point(visiteurPB.Location.X - tuile.Width, visiteurPB.Location.Y);
                    else if (randX == 1) visiteurPB.Location = new Point(visiteurPB.Location.X + tuile.Width, visiteurPB.Location.Y);

                    if (randY == 0) visiteurPB.Location = new Point(visiteurPB.Location.X, visiteurPB.Location.Y - tuile.Height);
                    else if (randY == 1) visiteurPB.Location = new Point(visiteurPB.Location.X, visiteurPB.Location.Y + tuile.Height);
                    */
                }
            Console.WriteLine("LogicVisiteurs Fin.");
        }
        
        private void Jeu_Load(object sender, EventArgs e)
        {
            BouclePrincipaleDuJeu();
        }

        private void BouclePrincipaleDuJeu()
        {
            // FPS timer
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = (1 * 1000); // FPS
            timer.Tick += new EventHandler(TickTick);
            timer.Start();
        }

        // Roule à chaque fois que le timer tick.
        private void TickTick(object sender, EventArgs e)
        {
            Logic();
            Refresh();
        }


        private void embaucherToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void DeplacerJoueur()
        {

        }

        private void Jeu_KeyDown(object sender, KeyEventArgs e)
        {
            BougerJoueur(e);
        }

        private void BougerJoueur(KeyEventArgs e)
        {
            Console.WriteLine("KeyDown");
            if (e.KeyCode == Keys.U)
            {
                MessageBox.Show("(" + Player.CurrentRow + "," + Player.CurrentColumn + ")");
            }
            if (e.KeyCode == Keys.S)
            {
                Player.MoveDown();
                /*if (Player.CurrentRow != 27)
                {
                    Player.CurrentRow++;
                    if (Player.JoeDownLeft.Visible == false)
                    {
                        Player.JoeDownLeft.Location = new Point(Player.X, Player.Y);
                        Player.JoeDownLeft.Visible = true;
                        if (Player.CurrentSprite != Player.JoeDownLeft)
                        {
                            Player.CurrentSprite.Visible = false;
                        }
                        Player.CurrentSprite = Player.JoeDownLeft;
                    }
                    else
                    {
                        Player.JoeDownRight.Location = new Point(Player.X, Player.Y);
                        Player.JoeDownRight.Visible = true;
                        if (Player.CurrentSprite != Player.JoeDownRight)
                        {
                            Player.CurrentSprite.Visible = false;
                        }
                        Player.CurrentSprite = Player.JoeDownRight;
                    }
                    Player.Y += tuile.Height;
                    Player.CurrentSprite.Location = new Point(Player.X, Player.Y);
                }*/
            }
            if (e.KeyCode == Keys.W)
            {
                Player.MoveUp();
                /*if (Player.CurrentRow != 0)
                {
                    Player.CurrentRow--;
                    if (Player.JoeUpLeft.Visible == false)
                    {
                        Player.JoeUpLeft.Location = new Point(Player.X, Player.Y);
                        Player.JoeUpLeft.Visible = true;
                        if (Player.CurrentSprite != Player.JoeUpLeft)
                        {
                            Player.CurrentSprite.Visible = false;
                        }
                        Player.CurrentSprite = Player.JoeUpLeft;
                    }
                    else
                    {
                        Player.JoeUpRight.Location = new Point(Player.X, Player.Y);
                        Player.JoeUpRight.Visible = true;
                        if (Player.CurrentSprite != Player.JoeUpRight)
                        {
                            Player.CurrentSprite.Visible = false;
                        }
                        Player.CurrentSprite = Player.JoeUpRight;
                    }
                    Player.Y -= tuile.Height;
                    Player.CurrentSprite.Location = new Point(Player.X, Player.Y);
                    
                }*/
            }
            if (e.KeyCode == Keys.D)
            {
                Player.MoveRight();
                /*
                if (Player.CurrentColumn != 39)
                {
                    Player.CurrentColumn++;
                    if (Player.JoeRightLeft.Visible == false)
                    {
                        Player.JoeRightLeft.Location = new Point(Player.X, Player.Y);
                        Player.JoeRightLeft.Visible = true;
                        if (Player.CurrentSprite != Player.JoeRightLeft)
                        {
                            Player.CurrentSprite.Visible = false;
                        }
                        Player.CurrentSprite = Player.JoeRightLeft;
                    }
                    else
                    {
                        Player.JoeRightRight.Location = new Point(Player.X, Player.Y);
                        Player.JoeRightRight.Visible = true;
                        if (Player.CurrentSprite != Player.JoeRightRight)
                        {
                            Player.CurrentSprite.Visible = false;
                        }
                        Player.CurrentSprite = Player.JoeRightRight;
                    }
                    Player.X += tuile.Width;
                    Player.CurrentSprite.Location = new Point(Player.X, Player.Y);
                }*/
            }
            if (e.KeyCode == Keys.A)
            {
                Player.MoveLeft();
             /*   if (Player.CurrentColumn != 0)
                {
                    Player.CurrentColumn--;
                    if (Player.JoeLeftLeft.Visible == false)
                    {
                        Player.JoeLeftLeft.Location = new Point(Player.X, Player.Y);
                        Player.JoeLeftLeft.Visible = true;
                        if (Player.CurrentSprite != Player.JoeLeftLeft)
                        {
                            Player.CurrentSprite.Visible = false;
                        }
                        Player.CurrentSprite = Player.JoeLeftLeft;
                    }
                    else
                    {
                        Player.JoeLeftRight.Location = new Point(Player.X, Player.Y);
                        Player.JoeLeftRight.Visible = true;
                        if (Player.CurrentSprite != Player.JoeLeftRight)
                        {
                            Player.CurrentSprite.Visible = false;
                        }
                        Player.CurrentSprite = Player.JoeLeftRight;
                    }
                    Player.X -= tuile.Width;
                    Player.CurrentSprite.Location = new Point(Player.X, Player.Y);
                    
                }*/
            }
        }

        /**
         * Déduit 35$ du joueur et instancie un nouveau Lion
         */
        private void lion35ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(35, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                Lion lion = new Lion(Lion.Nombre_Lions);
                Console.WriteLine(Lion.Nombre_Lions);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter un lion.");
            }
            
        }

        /**
         * Ajoute ou retire de l'argent du joueur selon le modificateur indiqué.
         * Met à jour l'affichage.
         * 
         * @param Montant - Quantité d'argent
         * @param Modificateur - True => Ajouter / False => Retirer
         */
        private bool Modifier_Argent(int Montant, bool Modificateur)
        {
            String[] TextArray = affichageArgent.Text.Split('$');
            String TextArgent = TextArray[0];
            int IntArgent;
            bool AjoutReussi = true;

            if (Modificateur)
            {
                Player.Argent += Montant;
            }
            else
            {
                if (Player.Argent - Montant >= 0)
                {
                    Player.Argent -= Montant;
                }
                else
                {
                    AjoutReussi = false;
                }
            }

            IntArgent = Player.Argent;
            TextArgent = IntArgent.ToString();

            affichageArgent.Text = TextArgent + " $";

            return AjoutReussi;
        }

        /**
         * Ajoute un animal au compteur visuel.
         */
        private void Ajouter_Animal()
        {
            String[] TextArray = animauxToolStripMenuItem.Text.Split(' ');
            String TextAnimaux = TextArray[0];
            int NombreAnimaux = Int32.Parse(TextAnimaux);

            TextAnimaux = (++NombreAnimaux).ToString();

            animauxToolStripMenuItem.Text = NombreAnimaux + " Animaux";
        }

        private void Jeu_FormClosing(object sender, FormClosingEventArgs e)
        {
            menuDepart.Dispose();
        }
    }
}
