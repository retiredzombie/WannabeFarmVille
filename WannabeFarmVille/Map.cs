using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTilesetMario;

namespace WannabeFarmVille
{
    class Map
    {
        List<List<Tuile>> listeTuiles;
        public Map(int screenWidth, int screenHeight, Bitmap tuileExemple)
        {
            listeTuiles = new List<List<Tuile>>();

            Bitmap tuile = TilesetImageGenerator.GetTile(0);

            int tuileWidth = tuile.Width;
            int tuileHeight = tuile.Height;

            for (int i = 0; i < screenHeight; i += tuileHeight)
            {
                for (int o = 0; o < screenWidth; o += tuileWidth)
                {
                    listeTuiles.Add(new List<Tuile>());
                    listeTuiles[i / screenHeight].Add(new Tuile(0));
                }
            }
        }

        public int getTypeTuile(int x, int y)
        {
            return listeTuiles[y][x].Type;
        }

        public void setTypeTuile(int x, int y, int newType)
        {
            listeTuiles[y][x].Type = newType;
        }

        public int GetMapWidth()
        {
            return listeTuiles[0].Count;
        }

        public int GetMapHeight()
        {
            return listeTuiles.Count;
        }
    }
}
