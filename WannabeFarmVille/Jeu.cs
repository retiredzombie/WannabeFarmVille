﻿using System;
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
        private Joueur Player = new Joueur();
        private List<Visiteur> visiteurs;
        private Bitmap ImgJoe = new Bitmap(Properties.Resources.joeExotic);
        private Graphics g;
        private System.Windows.Forms.PictureBox Joe;
        Bitmap tuile;
        bool gameStarted;


        public Jeu()
        {
            InitializeComponent();

            Init();
        }

        /*
         * Initialiser les composants du jeu/
         */
        private void Init()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            map = new Map(this.Width, this.Height, TilesetImageGenerator.GetTile(0));
            tuile = TilesetImageGenerator.GetTile(0);
            Player.Y += tuile.Height;
            visiteurs = new List<Visiteur>();
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
            visiteurs.Add(new Visiteur(tuile.Width * 19, tuile.Height * 28));
        }


        /* Tout ce qui est dessiner sur l'écran.
         */
        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            
            DrawTiles(g);

            DrawVisiteurs(g);
        }

        /*
         * Dessine les visiteurs.
         */
        private void DrawVisiteurs(Graphics g)
        {
            if (visiteurs.Count > 0)
            {
                for (int i = 0; i < visiteurs.Count; i++)
                {
                    g.DrawImage(visiteurs[i].imageVisiteur, visiteurs[i].X, visiteurs[i].Y, tuile.Width, tuile.Height);
                }
            }
        }

        /* Logique du jeu (1x par tick).
         */
        private void Logic()
        {
            PicJoe.Size = new Size(Player.Width, Player.Height);
            PicJoe.Location = new Point(Player.X, Player.Y);

            Thread threadLogiqueVisiteurs = new Thread(LogicVisiteurs);
            threadLogiqueVisiteurs.Start();
        }

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

                    if (randX == 0) visiteurs[i].X -= tuile.Width;
                    else if (randX == 1) visiteurs[i].X += tuile.Width;

                    if (randY == 0) visiteurs[i].Y -= tuile.Height;
                    else if (randY == 1) visiteurs[i].Y += tuile.Height;
                }
            } catch (InvalidOperationException)
            {

            }
        }

        /*
         * Dessiner les tuiles.
         */
        private void DrawTiles(Graphics g)
        {
            Bitmap bitmap = new Bitmap(Properties.Resources.zoo_tileset);

            Bitmap tuile = TilesetImageGenerator.GetTile(0);
            
            int tuileWidth = tuile.Width;
            int tuileHeight = tuile.Height;

            Player.Width = tuileWidth;
            Player.Height = tuileHeight;

            for (int i = 0; i < this.Height; i += tuileHeight)
            {
                for (int o = 0; o < this.Width; o += tuileWidth)
                {

                    tuile = TilesetImageGenerator.GetTile(map.getTypeTuile(o / tuileWidth, i / tuileHeight));

                    g.DrawImage(tuile, o, i);
                }
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
            this.Refresh();
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
                Player.Y += tuile.Height;
                PicJoe.Location = new Point(Player.X, Player.Y);
            }
            if (e.KeyCode == Keys.W)
            {
                Player.Y -= tuile.Height;
                PicJoe.Location = new Point(Player.X, Player.Y);
            }
            if (e.KeyCode == Keys.D)
            {
                Player.X += tuile.Width;
                PicJoe.Location = new Point(Player.X, Player.Y);
            }
            if (e.KeyCode == Keys.A)
            {
                Player.X -= tuile.Width;
                PicJoe.Location = new Point(Player.X, Player.Y);
            }
        }
    }
}
