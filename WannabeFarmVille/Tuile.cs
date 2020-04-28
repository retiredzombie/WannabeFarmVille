using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WannabeFarmVille
{
    class Tuile
    {
        private int type;
        Bitmap image;

        public Tuile()
        {
            EstUnObstacle = false;
        }
        public Tuile(int type)
        {
            this.type = type;
            EstUnObstacle = false;
        }

       public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public int getWitdth()
        {
            return image.Width;
        }
        public int getHeight()
        {
            return image.Height;
        }
        public bool EstUnObstacle { get; set; }
    }
}
