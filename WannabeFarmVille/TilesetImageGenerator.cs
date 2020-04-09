using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    class TilesetImageGenerator
    {
        // Différentes tailles concernant les images dans le fichier de tuiles de jeu
        public const int IMAGE_WIDTH = 32, IMAGE_HEIGHT = 32;
        private const int TILE_LEFT = 20, TILE_TOP = 20;
        private const int SEPARATEUR_TILE = 2;

        // La valeur entière correspond "par hasard" à la position de l'image dans la List<TileCoord>
        public static int PLANCHER = 0;
        public static int BRIQUE_BRUNE = 1;
        public static int CIEL_BLEU = 2;
        public static int COIN_BLOCK = 3;
        public static int NUAGE = 4;
        public static int HACHE = 5;

        private static List<TileCoord> listeCoord = new List<TileCoord>();
        private static List<Bitmap> listeBitmap = new List<Bitmap>();

        /// <summary>
        /// Constructeur statique
        /// </summary>
        static TilesetImageGenerator()
        {
            listeCoord.Add(new TileCoord() { Ligne = 0, Colonne = 0 }); // PLANCHER
            listeCoord.Add(new TileCoord() { Ligne = 0, Colonne = 5 }); // BRIQUE_BRUNE
            listeCoord.Add(new TileCoord() { Ligne = 6, Colonne = 12 }); // CIEL_BLEU
            listeCoord.Add(new TileCoord() { Ligne = 9, Colonne = 8 }); // COIN_BLOCK
            listeCoord.Add(new TileCoord() { Ligne = 3, Colonne = 12 }); // NUAGE
            listeCoord.Add(new TileCoord() { Ligne = 0, Colonne = 7 }); // HACHE

            listeBitmap.Add(LoadTile(PLANCHER)); // PLANCHER
            listeBitmap.Add(LoadTile(BRIQUE_BRUNE)); // BRIQUE_BRUNE
            listeBitmap.Add(LoadTile(CIEL_BLEU)); // CIEL_BLEU
            listeBitmap.Add(LoadTile(COIN_BLOCK)); // COIN_BLOCK
            listeBitmap.Add(LoadTile(NUAGE)); // TUYAU_TOP_LEFT
            listeBitmap.Add(LoadTile(HACHE)); // TUYAU_TOP_RIGHT
        }

        private static Bitmap LoadTile(int posListe)
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory + "Ressources\\Tileset\\zoo_tileset.png";
            Image source = Image.FromFile(directory);
            TileCoord coord = listeCoord[posListe];
            Rectangle crop = new Rectangle(TILE_LEFT + (coord.Colonne * (IMAGE_WIDTH + SEPARATEUR_TILE)), TILE_TOP + coord.Ligne * (IMAGE_HEIGHT + SEPARATEUR_TILE), IMAGE_WIDTH, IMAGE_HEIGHT);

            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            return bmp;
        }

        public static Bitmap GetTile(int posListe)
        {
            return listeBitmap[posListe];
        }

    }

    public class TileCoord
    {
        public int Ligne { get; set; }
        public int Colonne { get; set; }
    };
