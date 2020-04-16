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

            DrawBaseMap(screenWidth, screenHeight, tuileExemple);
            
        }

        private void DrawBaseMap(int screenWidth, int screenHeight, Bitmap tuileExemple)
        {
            Bitmap tuile = TilesetImageGenerator.GetTile(0);

            int tuileWidth = tuile.Width;
            int tuileHeight = tuile.Height;

            for (int i = 0; i < screenHeight; i += tuileHeight)
            {
                {
                    listeTuiles.Add(new List<Tuile>());
                    for (int o = 0; o < screenWidth; o += tuileWidth)
                        listeTuiles[i / tuileHeight].Add(new Tuile(0));
                }
            }
        }

        private void drawFarm()
        {
            setTypeTuile(10, 10, 6);
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
