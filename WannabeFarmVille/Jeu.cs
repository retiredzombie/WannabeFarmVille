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

namespace WannabeFarmVille
{
    public partial class Jeu : Form
    {
        Map map;
        public Jeu()
        {
            InitializeComponent();

            map = new Map(this.Width, this.Height, TilesetImageGenerator.GetTile(0));

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            map.setTypeTuile(20, 20, 2);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Bitmap bitmap = new Bitmap(Properties.Resources.zoo_tileset);

            Bitmap tuile = TilesetImageGenerator.GetTile(0);

            int tuileWidth = tuile.Width;
            int tuileHeight = tuile.Height;

            for (int i = 0; i < this.Height; i += tuileHeight)
            {
                for (int o = 0; o < this.Width; o += tuileWidth)
                {
                    tuile = TilesetImageGenerator.GetTile(map.getTypeTuile(o/tuileWidth, i/tuileHeight));
                    e.Graphics.DrawImage(tuile, o, i);
                }
            }
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
    }
}
