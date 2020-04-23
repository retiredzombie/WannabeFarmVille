using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        private Bitmap ImgJoe = new Bitmap(Properties.Resources.joeExotic);
        private Graphics g;
        private System.Windows.Forms.PictureBox Joe;
        Bitmap tuile;


        public Jeu()
        {
            InitializeComponent();

            map = new Map(this.Width, this.Height, TilesetImageGenerator.GetTile(0));

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            tuile = TilesetImageGenerator.GetTile(0);
            Player.Y += tuile.Height;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;

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
                    tuile = TilesetImageGenerator.GetTile(map.getTypeTuile(o/tuileWidth, i/tuileHeight));
                    e.Graphics.DrawImage(tuile, o, i);
                }
            }

            
            PicJoe.Size = new Size(Player.Width, Player.Height);
            PicJoe.Location = new Point(Player.X, Player.Y);
        }

        private void Jeu_Load(object sender, EventArgs e)
        {
            // FPS timer
            Timer timer = new Timer();
            timer.Interval = (1 * 1000); // FPS
            timer.Tick += new EventHandler(TickTick);
            timer.Start();
        }

        // Roule à chaque fois que le timer tick.
        private void TickTick(object sender, EventArgs e)
        {
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
