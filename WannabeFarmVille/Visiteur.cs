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
                                                     "Laura", "Fatimna", "Emma", "Alice", "Olivia",
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

        public Visiteur()
        {
            init();
        }
        public Visiteur(Enum genre)
        {
            init();

            Random rand = new Random();
            int random;
            if(genre.Equals(Genre.Homme))
            {
                random = rand.Next(0, ListePrenomHommes.Length - 1);
                Nom = ListePrenomHommes[random] + " ";
                
            }
            else if (genre.Equals(Genre.Femme))
            {
                random = rand.Next(0, ListePrenomFemmes.Length - 1);
                Nom = ListePrenomFemmes[random] + " ";
            }
            random = rand.Next(0, ListeNom.Length - 1);
            Nom += ListeNom[random];
        }

        private void init()
        {
            X = 0;
            Y = 0;
            Width = 50;
            Height = 20;

            images = new List<Image>();
            
        }

        public String Nom { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Image image { get; set; }

    }
}