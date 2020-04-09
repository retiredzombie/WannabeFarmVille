using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WannabeFarmVille
{
    class Visiteur
    {
        private String[] ListePrenomHommes = { "Scott", "John", "Denis", "Foudil", "Gabriel",
                                                     "William", "Logan", "Liam", "Thomas", "Noah",
                                                     "Jacob", "Leo", "Felix", "Marc", "André",
                                                     "Pierre", "Jack", "Clément", "Edouard"};
        private String[] ListePrenomFemmes = { "Sarah", "Alexa", "Aurélie", "Megan", "Anna",
                                                     "Laura", "Fatimna", "Emma", "Alice", "Olivia",
                                                     "Léa", "Florence", "Charlotte", "Zoé", "Béatrice",
                                                     "Virginie", "Joannie", "Tania", "Laurie"};
        public Visiteur(String nom)
        {
            Nom = nom;
        }
        public String Nom { get; set; }

    }
}
