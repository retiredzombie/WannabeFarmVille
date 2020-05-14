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
        private Image image;
        private int x, y;
        private int movingX;
        private int movingY;

        public Concierge(int x, int y)
        {
            this.Image = Properties.Resources.ConDownRight;
            this.x = x;
            this.y = y;
        }

        public Image Image { get => image; set => image = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int MovingX { get => movingX; set => movingX = value; }
        public int MovingY { get => movingY; set => movingY = value; }

        public void ReloadImages()
        {
            ReloadImage();
        }

        private void ReloadImage()
        {
            if (movingX == 1 && movingY == 0)
            {
                this.image = Properties.Resources.ConRightRight;
            }
            else if (movingX == -1 && movingY == 0)
            {
                this.image = Properties.Resources.ConLeftLeft;
            }

            else if (movingX == 0 && movingY == 1)
            {
                this.image = Properties.Resources.ConUpLeft;
            }
            else if (movingX == 0 && movingY == -1)
            {
                this.image = Properties.Resources.ConDownLeft;
            }
        }
    }
}
