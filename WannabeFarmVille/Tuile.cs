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

        public Tuile(int type)
        {
            this.type = type;
        }

       public int Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
