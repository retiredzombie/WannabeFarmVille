using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WannabeFarmVille
{
    class Concierge
    {
        Image image;
        int x, y;

        public Concierge(int x, int y)
        {
            this.Image = Properties.Resources.ConDownRight;
            this.x = x;
            this.y = y;
        }

        public Image Image { get => image; set => image = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
    }
}
