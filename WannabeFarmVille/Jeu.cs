using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        // OPTIONS

        int FPS = 1;


        // VARIABLES
        private static Tuile[,] Carte = new Tuile[28, 40];
        private Map map;
        private Joueur Player;
        private List<Visiteur> visiteurs;
        private Bitmap ImgJoe = new Bitmap(Properties.Resources.joeExotic);
        private Graphics g;
        ThreadStart thStart; 
        Bitmap tuile;
        List<PictureBox> visiteursPicBox;
        MenuDepart menuDepart;
        Random rand;
        private bool gameover;
        Stopwatch stopWatch;
        DelegateRefresh refreshFormDelegate;

        public Jeu(MenuDepart menuDepart)
        {
            InitializeComponent();

            this.menuDepart = menuDepart;
            Init();

            Thread bouclePrincipale = new Thread(BouclePrincipaleDuJeu);
            bouclePrincipale.Start();
        }

        /*
         * Initialiser les composants du jeu/
         */
        private void Init()
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
            rand = new Random();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
            map = new Map(this.Width, this.Height, TilesetImageGenerator.GetTile(0));
            tuile = TilesetImageGenerator.GetTile(0);
            for (int row = 0; row < 28; row++)
            {
                for (int column = 0; column < 40; column++)
                {
                    Carte[row, column] = new Tuile();
                    Carte[row, column].Ligne = row;
                    Carte[row, column].Colonne = column;
                }
            }
            refreshFormDelegate = new DelegateRefresh(Refresh);
            FPS = 1 / FPS * 1000;
            RendreClotureSolide(2, 4);
            RendreClotureSolide(15, 4);
            RendreClotureSolide(15, 25);
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
            gameover = false;
            //Stream str = Properties.Resources.rd2;
            //System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            //snd.Play();
            Stream str = Properties.Resources.rd2;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
            Player.CurrentSprite = Player.JoeUpRight;
            thStart = delegate { this.VisiteurThread(); };
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
            this.visiteurs.Add(new Visiteur(tuile.Width * x, tuile.Height * y, rand));
        }

        /*
         * Ajoute un visiteur directemnet au spawn de visiteur.
         */
        public void AjouterVisiteurSpawn()
        {
            this.visiteurs.Add(new Visiteur(tuile.Width * 19, tuile.Height * 25, rand));
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

                Font font = new Font("Arial", 8);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                int nomX = visiteurs[i].X - visiteurs[i].Width / 2;
                int nomY = visiteurs[i].Y - 20;
                string nom = visiteurs[i].Nom;

                g.DrawString(nom, font, drawBrush, new Point(nomX, nomY));
            }
            
        }

        /* Logique du jeu (1x par tick).
         */
        public void Logic()
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
                    int randX = rand.Next(3);
                    int randY = rand.Next(3);
                while ((randX == randY) ||
                        (randY == 0 && visiteurs[i].Y - tuile.Height <= 0 + tuile.Height) ||
                        (randY == 1 && visiteurs[i].Y + tuile.Height >= this.Height - tuile.Height) ||
                        (randX == 0 && visiteurs[i].X - tuile.Width <= 0 + tuile.Width) ||
                        (randX == 1 && visiteurs[i].X + tuile.Width >= this.Width - tuile.Height) ||
                        (randX != 2 && randY != 2) ||
                        (IsColliding(randX, randY, visiteurs[i]))
                      )
                    {
                        randX = rand.Next(3);
                        randY = rand.Next(3);
                    }

                    if (randX == 0)
                    { 
                        visiteurs[i].X -= tuile.Width;
                        visiteurs[i].MovingX = -1;
                        visiteurs[i].MovingY = 0;
                    } 
                    else if (randX == 1)
                    { 
                        visiteurs[i].X += tuile.Width;
                        visiteurs[i].MovingX = 1;
                        visiteurs[i].MovingY = 0;
                    }

                    if (randY == 0)
                    {
                        visiteurs[i].Y -= tuile.Height;
                        visiteurs[i].MovingY = 1;
                        visiteurs[i].MovingX = -0;
                    }
                    else if (randY == 1)
                    {
                        visiteurs[i].Y += tuile.Height;
                        visiteurs[i].MovingY = -1;
                        visiteurs[i].MovingX = 0;
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

        private bool IsColliding(int randX, int randY, Visiteur visiteur)
        {
            bool colliding = false;

            if (randX == 0)
            {
                visiteur.X -= tuile.Width;
                visiteur.MovingX = -1;
                visiteur.MovingY = 0;
            }
            else if (randX == 1)
            {
                visiteur.X += tuile.Width;
                visiteur.MovingX = 1;
                visiteur.MovingY = 0;
            }

            if (randY == 0)
            {
                visiteur.Y -= tuile.Height;
                visiteur.MovingY = 1;
                visiteur.MovingX = -0;
            }
            else if (randY == 1)
            {
                visiteur.Y += tuile.Height;
                visiteur.MovingY = -1;
                visiteur.MovingX = 0;
            }


            for (int i = 0; i < Carte.GetLength(1); i++)
            {
                for (int o = 0; o < Carte.GetLength(0); o++)
                {
                    int vX = visiteur.X;
                    int vY = visiteur.Y;
                    int vW = visiteur.Width;
                    int vH = visiteur.Height;
                    int tS = 32;
                    int tX = Carte[o, i].Colonne * tS;
                    int tY = Carte[o, i].Ligne * tS;


                    if (vX > tX && vX < tX + tS && vY > tY && vY < tY + tS)
                    {
                        colliding = true;
                    }
                }
            }

            return colliding;
        }


        private void Jeu_Load(object sender, EventArgs e)
        {
            /// FPS timer
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = (FPS); // FPS
            timer.Tick += new EventHandler(TickTick);
            timer.Start();
        }

        public delegate void DelegateRefresh();
        

        private void BouclePrincipaleDuJeu()
        {
            

            while (!gameover)
            {
                if (stopWatch.ElapsedMilliseconds >= FPS)
                {
                    Logic();
                    stopWatch.Restart();
                }
                
            }
        }

        // Roule à chaque fois que le timer tick.
        private void TickTick(object sender, EventArgs e)
        {
             Refresh();
        /*    Thread th = new Thread(thStart);
            th.Start();*/
        }

        private void VisiteurThread()
        {

        }

        private void embaucherToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
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
            }
            if (e.KeyCode == Keys.W)
            {
                Player.MoveUp();
            }
            if (e.KeyCode == Keys.D)
            {
                Player.MoveRight();
            }
            if (e.KeyCode == Keys.A)
            {
                Player.MoveLeft();
            }
        }

        // Déduit 35$ du joueur et crée un nouveau Lion
        // NE FONCTIONNE PAS
        private void lion35ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String[] TextArray = affichageArgent.Text.Split('$');
            String TextArgent = TextArray[0];
            int IntArgent = Int32.Parse(TextArgent);

            Player.Argent -= 35;
            IntArgent = Player.Argent;
            TextArgent = IntArgent.ToString();
            
            affichageArgent.Text = TextArgent + "$";
            
            Lion lion = new Lion(0);
            Console.WriteLine("Un lion a été ajouté");
        }
        private void Jeu_FormClosing(object sender, FormClosingEventArgs e)
        {
            menuDepart.Dispose();
        }

        private void PicUpRight_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

        }
    }
}
