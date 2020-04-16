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
            drawFarm();
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

        // Dessine la ferme (4 enclos).
        private void drawFarm()
        {
            // Enclos En haut à gauche.
            int[] enclos1 = { 4, 4, 10, 10 };
            for (int y = enclos1[1]; y < enclos1[1] + enclos1[3]; y++)
            {
                for (int x = enclos1[0]; x < enclos1[0] + enclos1[2]; x++)
                {
                    if (y == enclos1[1] || y == enclos1[1] + enclos1[3] - 1)
                    {
                        setTypeTuile(x, y, 6);
                    }
                    if (x == enclos1[0] || x == enclos1[0] + enclos1[2] - 1)
                    {
                        setTypeTuile(x, y, 6);
                    }
                }
            }

            // Enclos En haut à droite.
            enclos1 = new int[] { 25, 4, 10, 10 };
            for (int y = enclos1[1]; y < enclos1[1] + enclos1[3]; y++)
            {
                for (int x = enclos1[0]; x < enclos1[0] + enclos1[2]; x++)
                {
                    if (y == enclos1[1] || y == enclos1[1] + enclos1[3] - 1)
                    {
                        setTypeTuile(x, y, 6);
                    }
                    if (x == enclos1[0] || x == enclos1[0] + enclos1[2] - 1)
                    {
                        setTypeTuile(x, y, 6);
                    }
                }
            }

            // Enclos En bas à gauche.
            enclos1 = new int[] { 4, 16, 10, 10 };
            for (int y = enclos1[1]; y < enclos1[1] + enclos1[3]; y++)
            {
                for (int x = enclos1[0]; x < enclos1[0] + enclos1[2]; x++)
                {
                    if (y == enclos1[1] || y == enclos1[1] + enclos1[3] - 1)
                    {
                        setTypeTuile(x, y, 6);
                    }
                    if (x == enclos1[0] || x == enclos1[0] + enclos1[2] - 1)
                    {
                        setTypeTuile(x, y, 6);
                    }
                }
            }

            // Enclos En bas à droite.
            enclos1 = new int[] { 25, 16, 10, 10 };
            for (int y = enclos1[1]; y < enclos1[1] + enclos1[3]; y++)
            {
                for (int x = enclos1[0]; x < enclos1[0] + enclos1[2]; x++)
                {
                    if (y == enclos1[1] || y == enclos1[1] + enclos1[3] - 1)
                    {
                        setTypeTuile(x, y, 6);
                    }
                    if (x == enclos1[0] || x == enclos1[0] + enclos1[2] - 1)
                    {
                        setTypeTuile(x, y, 6);
                    }
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
