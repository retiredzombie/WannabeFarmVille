using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WannabeFarmVille
{
    public enum Genre
    {
        Homme,
        Femme
    }
    class Visiteur
    {
        /// <summary>
        /// Cette classe représente les visiteurs 
        /// </summary>
        private String[] ListePrenomHommes = { "Scott", "John", "Denis", "Hugo", "Gabriel",
                                                     "William", "Logan", "Liam", "Thomas", "Noah",
                                                     "Jacob", "Leo", "Felix", "Marc", "André",
                                                     "Pierre", "Jack", "Clément", "Edouard"};
        private String[] ListePrenomFemmes = { "Sarah", "Alexa", "Aurélie", "Megan", "Anna",
                                                     "Laura", "Fatima", "Emma", "Alice", "Olivia",
                                                     "Léa", "Florence", "Charlotte", "Zoé", "Béatrice",
                                                     "Virginie", "Joannie", "Tania", "Laurie"};
        private String[] ListeNom = { "Lapointe", "Shepard", "Duplessis", "Lavoie", "Meloche", "Morissette",
                                      "Brodeur", "Kenway", "Halitim", "Palpatine", "Tremblay", "Obitsa", 
                                      "Giron", "Couillard", "Trudeau", "Trump", "Pratt", "Dostoyevski", 
                                      "Spasov", "Rivas", "Mandel", "Paquet", "Loyer", "Deffes", "Dulac", "Ménassé",
                                      "Gill", "Fontaine", "Parent", "Magnan", "Montpetit", "Deschamps", "Levesque",
                                      "Nelson", "Robitaille", "Rheault", "Bridelle", "Desormeaux", "Brown", "Mirandette", "Désilet",
                                      "Belhumeur", "Gontar", "Bray" };
        private List<Image> images;

        private Genre genre;

        public Visiteur(int x, int y, Random rand)
        {
            Init(x, y);

            int random;



            if (rand.Next(2) == 1)
            {
                genre = Genre.Femme;
            } else
            {
                genre = Genre.Homme;
            }

            if (genre.Equals(Genre.Homme))
            {
                random = rand.Next(0, ListePrenomHommes.Length);
                this.Nom = ListePrenomHommes[random] + " ";

                this.imageVisiteur = Properties.Resources.HomUpLeft;
            }
            else if (genre.Equals(Genre.Femme))
            {
                random = rand.Next(0, ListePrenomFemmes.Length);
                this.Nom = ListePrenomFemmes[random] + " ";

                this.imageVisiteur = Properties.Resources.FemUpLeft;
            }
            random = rand.Next(0, ListeNom.Length - 1);
            this.Nom += ListeNom[random];
        }

        private void Init(int x, int y)
        {
            X = x;
            Y = y;
            Width = 32;
            Height = 32;
            MovingX = 0;
            MovingY = 0;

            images = new List<Image>();
            
        }

        public void ReloadImages()
        {
            if (this.genre == Genre.Homme)
            {
                ReloadImageHomme();
            }
            else if (this.genre == Genre.Femme)
            {
                ReloadImageFemme();
            }
        }

        private void ReloadImageHomme()
        {
            if (movingX == 1 && movingY == 0)
            {
                this.imageVisiteur = Properties.Resources.HomRightRight;
            }
            else if (movingX == -1 && movingY == 0)
            {
                this.imageVisiteur = Properties.Resources.HomLeftLeft;
            }

            else if (movingX == 0 && movingY == 1)
            {
                this.imageVisiteur = Properties.Resources.HomUpLeft;
            }
            else if (movingX == 0 && movingY == -1)
            {
                this.imageVisiteur = Properties.Resources.HomDownLeft;
            }
        }

        private void ReloadImageFemme()
        {
            if (movingX == 1 && movingY == 0)
            {
                this.imageVisiteur = Properties.Resources.FemRightRight;
            }
            else if (movingX == -1 && movingY == 0)
            {
                this.imageVisiteur = Properties.Resources.FemLeftLeft;
            }

            else if (movingX == 0 && movingY == 1)
            {
                this.imageVisiteur = Properties.Resources.FemUpLeft;
            }
            else if (movingX == 0 && movingY == -1)
            {
                this.imageVisiteur = Properties.Resources.FemDownLeft;
            }
        }

        public String Nom { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int CurrentRow { get; set; } = 24;
        public int CurrentColumn { get; set; } = 19;

        public int Width { get; set; }
        public int Height { get; set; }

        private int movingX;
        private int movingY;

        public Image imageVisiteur { get; set; }
        public int MovingX { get => movingX; set => movingX = value; }
        public int MovingY { get => movingY; set => movingY = value; }
    }
}