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
        private Tuile[,] Carte = new Tuile[28, 40];
        private Map map;
        private bool backDrawn = false;
        private Joueur Player = new Joueur();
        private List<Visiteur> visiteurs;
        private Bitmap ImgJoe = new Bitmap(Properties.Resources.joeExotic);
        private Graphics g;
        private System.Windows.Forms.PictureBox Joe;
        Bitmap tuile;
        bool gameStarted;
        List<PictureBox> visiteursPicBox;
        MenuDepart menuDepart;
        Thread threadTimer;
        bool joueurBouge;
        bool jeuBoucle;
        Thread visiteursThread;
        List<Thread> visiteursThreads;

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
            joueurBouge = false;
            jeuBoucle = true;
            //Stream str = Properties.Resources.rd2;
            //System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            //snd.Play();
            for (int row = 0; row < 28; row++)
            {
                for(int column = 0; column < 40; column++)
                {
                    Carte[row, column] = new Tuile();
                }
            }
            AjouterObstacle(2, 4);
            AjouterObstacle(14, 4);
            AjouterObstacle(14, 25);
            AjouterObstacle(2, 25);
            visiteursPicBox = new List<PictureBox>();
            PicUpRight.Size = new Size(Player.Width, Player.Height);
            PicUpRight.Location = new Point(Player.X, Player.Y);
            for (int i = 0; i < 10; i++)
            {
               AjouterVisiteurSpawn();
            }
        }

        private void AjouterObstacle(int row, int column)
        {   
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                column++;
            }
            column++;
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                row++;
            }
            row++;
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                column--;
            }
            column--;
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
                    } 
                    else if (randX == 1)
                    { 
                        visiteurs[i].X += tuile.Width;
                    }

                    if (randY == 0)
                    {
                        visiteurs[i].Y -= tuile.Height;
                    }
                    else if (randY == 1)
                    {
                        visiteurs[i].Y += tuile.Height;
                    }

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
                if (Player.CurrentRow != 27)
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
                    Player.Location = new Point(Player.X, Player.Y);
                    Player.CurrentSprite.Location = Player.Location;
                }
            }
            if (e.KeyCode == Keys.W)
            {
                if (Player.CurrentRow != 0)
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
                    Player.Location = new Point(Player.X, Player.Y);
                    Player.CurrentSprite.Location = Player.Location;
                }
            }
            if (e.KeyCode == Keys.D)
            {
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
                    Player.Location = new Point(Player.X, Player.Y);
                    Player.CurrentSprite.Location = Player.Location;
                }
            }
            if (e.KeyCode == Keys.A)
            {
                if (Player.CurrentColumn != 0)
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
                    Player.Location = new Point(Player.X, Player.Y);
                    Player.CurrentSprite.Location = Player.Location;
                }
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
    }
}
