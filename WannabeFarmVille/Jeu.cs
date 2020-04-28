using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            visiteursPicBox = new List<PictureBox>();
            for (int i = 0; i < 50; i++)
            {
               AjouterVisiteurSpawn();
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
        }


        /* Tout ce qui est dessiner sur l'écran.
         */
        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
        }

        /* Logique du jeu (1x par tick).
         */
        private void Logic()
        {
            PicUpRight.Size = new Size(Player.Width, Player.Height);
            PicUpRight.Location = new Point(Player.X, Player.Y);

            Thread threadLogiqueVisiteurs = new Thread(LogicVisiteurs);
            threadLogiqueVisiteurs.Start();
        }

        /*
         * Fait bouger les visiteurs.
         */
        private void LogicVisiteurs()
        {
            try
            {
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
                    string visiteurPBName = "visiteurPB" + i.ToString().Trim();
                    Control[] foundVisiteurs = Controls.Find(visiteurPBName, true);
                    PictureBox visiteurPB = (PictureBox) foundVisiteurs.First();
                    if (randX == 0) visiteurPB.Location = new Point(visiteurPB.Location.X - tuile.Width, visiteurPB.Location.Y);
                    else if (randX == 1) visiteurPB.Location = new Point(visiteurPB.Location.X + tuile.Width, visiteurPB.Location.Y);

                    if (randY == 0) visiteurPB.Location = new Point(visiteurPB.Location.X, visiteurPB.Location.Y - tuile.Height);
                    else if (randY == 1) visiteurPB.Location = new Point(visiteurPB.Location.X, visiteurPB.Location.Y + tuile.Height);
                }
            } catch (InvalidOperationException)
            {

            }
        }

        private void Jeu_Load(object sender, EventArgs e)
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
        }

        private void embaucherToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Jeu_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.S)
            {
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
            }
            if (e.KeyCode == Keys.W)
            {
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
            }
            if (e.KeyCode == Keys.D)
            {
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
            }
            if (e.KeyCode == Keys.A)
            {
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
            }
        }

        private void Jeu_FormClosing(object sender, FormClosingEventArgs e)
        {
            menuDepart.Dispose();
        }
    }
}
