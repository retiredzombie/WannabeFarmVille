using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WannabeFarmVille
{
    class Map
    {
        List<List<Tuile>> listeTuiles;
        Map()
        {
            listeTuiles = new List<List<Tuile>>();
        }

        int getType(int x, int y)
        {
            return listeTuiles[y][x].Type;
        }

        void setType(int x, int y, int newType)
        {
            listeTuiles[y][x].Type = newType;
        }
    }
}
