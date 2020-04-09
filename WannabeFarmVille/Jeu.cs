using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WannabeFarmVille
{
    public partial class Jeu : Form
    {
        public static String[] ListePrenomHommes = { "Scott", "John", "Denis", "Foudil", "Gabriel", 
                                                     "William", "Logan", "Liam", "Thomas", "Noah",
                                                     "Jacob", "Leo", "Felix", "Marc", "André", 
                                                     "Pierre", "Jack", "Clément", "Edouard"};
        public static String[] ListePrenomFemmes = { "Sarah", "Alexa", "Aurélie", "Megan", "Anna",
                                                     "Laura", "Fatimna", "Emma", "Alice", "Olivia",
                                                     "Léa", "Florence", "Charlotte", "Zoé", "Béatrice",
                                                     "Virginie", "Joannie", "Tania", "Laurie"};
        public Jeu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
