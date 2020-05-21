using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TestTilesetMario;
using WannabeFarmVille.Animaux;
using WannabeFarmVille.Properties;

namespace WannabeFarmVille
{

    public partial class Jeu : Form
    {
        // OPTIONS

        double FPS = 1;


        // CONFIGURABLE


        double PAYE_PAR_VISITEUR = 1.0; // Chaque visiteur paye ce montant fois le nombre d'animaux.
        double MALUS_PAR_DECHET = 0.10;


        // VARIABLES
        private InfoVisiteur InfoVis;
        private InfoAnimal InfoAni;
        private static Tuile[,] Carte = new Tuile[28, 40];
        private Map map;
        int intFPS, coutConcierge;
        private Joueur Player;
        private List<Visiteur> visiteurs;
        private Bitmap ImgJoe = new Bitmap(Properties.Resources.joeExotic);
        private Graphics g;
        private System.Media.SoundPlayer snd;
        private Stopwatch stopwatchPayerConcierges;
        private Stopwatch stopwatchJeu;
        private Bitmap tuile;
        private List<PictureBox> visiteursPicBox;
        private MenuDepart menuDepart;
        private Random rand;
        private bool gameover;
        private Stopwatch stopWatch;
        private DateTime dt_remuneration;
        private DelegateRefresh refreshFormDelegate;
        private int tailleTuile;
        private List<Dechet> dechets;
        private List<Concierge> concierges;
        private Thread bouclePrincipale;
        private DateTime datejeu;
        private int NbConcierge = 0;
        private int NombreAnimaux = 0;
        private List<Animal> animaux;
        private int typeAnimalSelectionne; // 0: aucun 1-6:animal.
        

        public Jeu(MenuDepart menuDepart)
        {
            InitializeComponent();

            this.menuDepart = menuDepart;
            Init();

            bouclePrincipale = new Thread(BouclePrincipaleDuJeu);
            bouclePrincipale.Start();
        }

        /*
         * Initialiser les composants du jeu/
         */
        private void Init()
        {
            datejeu = DateTime.Now;
            typeAnimalSelectionne = 0;
            stopWatch = new Stopwatch();
            dt_remuneration = DateTime.Now;
            stopwatchJeu = new Stopwatch();
            stopwatchPayerConcierges = new Stopwatch();
            stopwatchPayerConcierges.Start();
            concierges = new List<Concierge>();
            animaux = new List<Animal>();
            coutConcierge = 2;
            stopWatch.Start();
            rand = new Random();
            this.MouseClick += new MouseEventHandler(this.Jeu_MouseClick);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
            map = new Map(this.Width, this.Height, TilesetImageGenerator.GetTile(0));
            tuile = TilesetImageGenerator.GetTile(0);
            dechets = new List<Dechet>();
            int x = 0, y = 32;
            for (int row = 0; row < 28; row++)
            {
                for (int column = 0; column < 40; column++)
                {
                    Carte[row, column] = new Tuile();
                    Carte[row, column].Ligne = row;
                    Carte[row, column].Colonne = column;
                    Carte[row, column].X = x;
                    Carte[row, column].Y = y;
                    x += 32;
                }
                x = 0;
                y += 32;
            }
            refreshFormDelegate = new DelegateRefresh(Refresh);
            FPS = 1 / FPS * 1000;
            intFPS = Convert.ToInt32(FPS);
            RendreClotureSolide(2, 4, Enclo.UpLeft);
            RendreClotureSolide(15, 4, Enclo.DownLeft);
            RendreClotureSolide(15, 25, Enclo.DownRight);
            RendreClotureSolide(2, 25, Enclo.UpRight);
            DefinirInterieurEnclos();
            PicUpRight.Size = new Size(32, 32);
            PicUpRight.Location = new Point(0, 32);
            Player = new Joueur(PicUpLeft, PicUpRight, PicDownLeft, PicDownRight, 
                PicLeftLeft, PicLeftRight, PicRightLeft, PicRightRight, 0, 0, 0, 32, Carte);
            Player.Y += tuile.Height;
            visiteurs = new List<Visiteur>();
            Player.Width = tuile.Width;
            Player.Height = tuile.Height;
            Player.JoeDownLeft = PicDownLeft;
            Player.JoeDownRight = PicDownRight;
            Player.JoeUpLeft = PicUpLeft;
            Player.JoeUpRight = PicUpRight;
            Player.JoeRightLeft = PicRightLeft;
            Player.JoeRightRight = PicRightRight;
            Player.JoeLeftLeft = PicLeftLeft;
            Player.JoeLeftRight = PicLeftRight;
            Player.CurrentSprite = Player.JoeUpRight;
            gameover = false;
            tailleTuile = 32;
            Stream str = Properties.Resources.rd2;
            snd = new System.Media.SoundPlayer(str);
            snd.Play();
            Player.CurrentSprite = Player.JoeUpRight;
            Carte[0, 0].EstUnObstacle = true;
            MettreTuilesAdjacentes();
            visiteursPicBox = new List<PictureBox>();
            for (int i = 0; i < 10; i++)
            {
               AjouterVisiteurSpawn();
            }
        }
        /// <summary>
        /// Rendre les tuiles de la cartes autour de soit, adjacentes
        /// </summary>
        private void MettreTuilesAdjacentes()
        {
            try
            {
                Carte[Player.CurrentRow + 1, Player.CurrentColumn].EstAdjacente = true;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow + 1, Player.CurrentColumn + 1].EstAdjacente = true;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow + 1, Player.CurrentColumn - 1].EstAdjacente = true;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow, Player.CurrentColumn - 1].EstAdjacente = true;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow, Player.CurrentColumn + 1].EstAdjacente = true;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow - 1, Player.CurrentColumn].EstAdjacente = true;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow - 1, Player.CurrentColumn + 1].EstAdjacente = true;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow - 1, Player.CurrentColumn - 1].EstAdjacente = true;
            }
            catch (IndexOutOfRangeException)
            {
            }
        }
        /// <summary>
        /// Enlever les tuiles adjacentes
        /// </summary>
        private void EnleverTuilesAdjacentes()
        {
            try
            {
                Carte[Player.CurrentRow + 1, Player.CurrentColumn].EstAdjacente = false;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow + 1, Player.CurrentColumn + 1].EstAdjacente = false;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow + 1, Player.CurrentColumn - 1].EstAdjacente = false;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow, Player.CurrentColumn - 1].EstAdjacente = false;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow, Player.CurrentColumn + 1].EstAdjacente = false;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow - 1, Player.CurrentColumn].EstAdjacente = false;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow - 1, Player.CurrentColumn + 1].EstAdjacente = false;
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                Carte[Player.CurrentRow - 1, Player.CurrentColumn - 1].EstAdjacente = false;
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        /// <summary>
        /// Event qui se déclenche lorsque l'utilisateur clique avec sa sourie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Jeu_MouseClick(object sender, MouseEventArgs e)
        {
            for (int c = 0; c < visiteurs.Count; c++)
            {
                if (visiteurs[c].IsSelected)
                {
                    visiteurs[c].IsSelected = false;
                }
            }
            for (int ligne = 0; ligne < 28; ligne++)
                {
                    for (int colonne = 0; colonne < 40; colonne++)
                    {
                        if (e.X > Carte[ligne, colonne].X && e.X < (32 + Carte[ligne, colonne].X) && e.Y > Carte[ligne, colonne].Y && e.Y < (32 + Carte[ligne, colonne].Y))
                        {
                        if (e.Button == MouseButtons.Right)
                        {
                            for (int i = 0; i < visiteurs.Count; i++)
                            {
                                if (Carte[ligne, colonne].X == visiteurs[i].X && Carte[ligne, colonne].Y == visiteurs[i].Y)
                                {
                                    TimeSpan ts = visiteurs[i].TempsDansLeZoo.Elapsed;
                                    double minutes = ts.TotalMinutes;
                                    minutes = Math.Truncate(100 * minutes) / 100;
                                    String temps = minutes + " Minutes";
                                    InfoVis = new InfoVisiteur();
                                    InfoVis.SetInformations(visiteurs[i].Nom, visiteurs[i].Sexe, temps);
                                    InfoVis.Show();
                                    i = visiteurs.Count + 1;
                                }
                            }
                            for (int i = 0; i < animaux.Count; i++)
                            {
                                if (e.X > animaux[i].X && e.X < (32 + animaux[i].X) && e.Y > animaux[i].Y && e.Y < (32 + animaux[i].Y))
                                {
                                    String race = "";
                                    String sexe = "Non-Binaire";
                                    String age = "Adulte";
                                    String food;
                                    String enceinte = " ";
                                    if(animaux[i] is Lion)
                                    {
                                        race = "Lion";
                                    }
                                    else if (animaux[i] is Buffle)
                                    {
                                        race = "Buffle";
                                    }
                                    else if (animaux[i] is Grizzly)
                                    {
                                        race = "Grizzly";
                                    }
                                    else if (animaux[i] is Licorne)
                                    {
                                        race = "Licorne";
                                    }
                                    else if (animaux[i] is Mouton)
                                    {
                                        race = "Mouton";
                                    }
                                    else if (animaux[i] is Rhino)
                                    {
                                        race = "Rhinocéros";
                                    }
                                    if(animaux[i].genre == Animal.Genre.Male)
                                    {
                                        sexe = "Mâle";
                                    }
                                    else
                                    {
                                        sexe = "Femelle";
                                        if (animaux[i].EnGestation)
                                        {
                                            enceinte = "Attend un bébé";
                                        }
                                        else
                                        {
                                            enceinte = "N'attend pas de bébé";
                                        }
                                    }
                                    double temps =  (DateTime.Now - animaux[i].DernierRepas).TotalSeconds;
                                    temps = temps / 60;
                                    temps = Math.Truncate(100 * temps) / 100;
                                    food = "A mangé il y a " + temps + " minutes";
                                    InfoAni = new InfoAnimal();
                                    InfoAni.SetInformation(race, sexe, age, food, enceinte);
                                    InfoAni.Show();
                                    i = animaux.Count + 1;
                                }
                            }
                        }
                        else
                        {
                            if (Player.PeutNourrir)
                            {
                                bool trouve = false;
                                for (int i = 0; i < animaux.Count; i++) {
                                    if (e.X > animaux[i].X && e.X < (32 + animaux[i].X) && e.Y > animaux[i].Y && e.Y < (32 + animaux[i].Y))
                                    {
                                        if (Carte[ligne, colonne].PositionEnclo == Player.EncloChoisi)
                                        {
                                            animaux[i].DernierRepas = DateTime.Now;

                                            if (animaux[i].Type == 1)
                                            {
                                                MessageBox.Show("Le mouton bêle.");
                                            }
                                            else if (animaux[i].Type == 2)
                                            {
                                                MessageBox.Show("Le grizzly grogne.");
                                            }
                                            else if (animaux[i].Type == 3)
                                            {
                                                MessageBox.Show("Le lion rugit.");
                                            }
                                            else if (animaux[i].Type == 4)
                                            {
                                                MessageBox.Show("La licorne hennit.");
                                            }
                                            else if (animaux[i].Type == 5)
                                            {
                                                MessageBox.Show("Le rhinocéros barète.");
                                            }
                                            else if (animaux[i].Type == 6)
                                            {
                                                MessageBox.Show("Le buffle beugle.");
                                            }

                                            Player.Argent -= 1;
                                            affichageArgent.Text = Player.Argent + "$";
                                            trouve = true;
                                        }
                                        else
                                        {
                                            MessageBox.Show("Vous ne pouvez pas nourrir un animal qui ne se trouve \n pas dans l'enclo à côté de vous.");
                                            trouve = true;
                                        }
                                    }
                                }
                                if(!trouve)
                                {
                                    MessageBox.Show("Il n'y a pas d'animal sur cette case");
                                }
                            }
                            if (Player.PeutAjouterAnimal)
                            {
                                if (Carte[ligne, colonne].PositionEnclo == Player.EncloChoisi)
                                {
                                    PlacerAnimal(e.X, e.Y);
                                }
                                else
                                {
                                    MessageBox.Show("Vous ne pouvez pas ajouter un animal dans \n un enclo qui n'est pas à côté de vous ");
                                }
                            }
                            if (Carte[ligne, colonne].EstAdjacente)
                            {
                                if (Player.PeutEngagerConcierge)
                                {
                                    if (!Carte[ligne, colonne].EstUnObstacle)
                                    {
                                        NewConcierge(Carte[ligne, colonne].X, Carte[ligne, colonne].Y, ligne, colonne);
                                        NbConcierge++;
                                        conciergesToolStripMenuItem.Text = NbConcierge + " Concierges";
                                        Player.PeutEngagerConcierge = false;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Vous devez choisir une case vide");
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < dechets.Count; i++)
                                    {
                                        if (Carte[ligne, colonne].X == dechets[i].X && Carte[ligne, colonne].Y == dechets[i].Y)
                                        {
                                            dechets.RemoveAt(i);
                                        }
                                    }
                                }
                            }
                        }
                        }
                    }
                }
        }

        // Place un animal au X, Y choisi en fonction du type choisi.
        private void PlacerAnimal(int x, int y)
        {

            bool placementLegal = VerifierXYEnclos(x, y);
            // MessageBox.Show(x.ToString() + " " + y.ToString());
            if (BonTypeEnclos(typeAnimalSelectionne, x, y))
            {
                switch (typeAnimalSelectionne)
                {
                    case 1:
                        AjouterMouton(x, y);
                        typeAnimalSelectionne = 0;
                        break;
                    case 2:
                        AjouterGrizzly(x, y);
                        typeAnimalSelectionne = 0;
                        break;
                    case 3:
                        AjouterLion(x, y);
                        typeAnimalSelectionne = 0;
                        break;
                    case 4:
                        AjouterLicorne(x, y);
                        typeAnimalSelectionne = 0;
                        break;
                    case 5:
                        AjouterRhino(x, y);
                        typeAnimalSelectionne = 0;
                        break;
                    case 6:
                        AjouterBuffle(x, y);
                        typeAnimalSelectionne = 0;
                        break;
                }
            } else
            {
                typeAnimalSelectionne = 0;
            }
        }

        //Verifie que le joueur à cliqué sur un enclos et retourne lequel si oui.
        private bool BonTypeEnclos(int typeAnimalSelectionne, int x, int y)
        {
            bool bonType = true;

            int enclosClique = GetEncloClique(x, y);

            if (enclosClique == 0 && typeAnimalSelectionne != 0)
            {
                MessageBox.Show("Les animaux doivent êtres placés dans des enclos.");
                return false;
            }

            for (int i = 0; i < animaux.Count; i++)

            {
                if (animaux[i].Enclos == enclosClique)
                {
                    if (animaux[i].Type != typeAnimalSelectionne && typeAnimalSelectionne != 0)
                    {
                        bonType = false;
                        MessageBox.Show("Un seul type d'animal par enclos.");
                        break;
                    }
                }
            }


            return bonType;
        }

        // Accouche (gratuit) un animal au X, Y choisi en fonction du type choisi.
        private void Accoucher(int X, int Y)
        {

            bool placementLegal = VerifierXYEnclos(X, Y);
            // MessageBox.Show(x.ToString() + " " + y.ToString());
            if (BonTypeEnclos(typeAnimalSelectionne, X, Y))
            {
                switch (typeAnimalSelectionne)
                {
                    case 1:
                        Ajouter_Animal();
                        Mouton mouton = new Mouton(X, Y);
                        animaux.Add(mouton);
                        typeAnimalSelectionne = 0;
                        break;
                    case 2:
                        Ajouter_Animal();
                        Grizzly grizzly = new Grizzly(X, Y);
                        animaux.Add(grizzly);
                        typeAnimalSelectionne = 0;
                        break;
                    case 3:
                        Ajouter_Animal();
                        Lion lion = new Lion(X, Y);
                        animaux.Add(lion);
                        typeAnimalSelectionne = 0;
                        break;
                    case 4:
                        Ajouter_Animal();
                        Licorne licorne = new Licorne(X, Y);
                        animaux.Add(licorne);
                        typeAnimalSelectionne = 0;
                        break;
                    case 5:
                        Ajouter_Animal();
                        Rhino rhino = new Rhino(X, Y);
                        animaux.Add(rhino);
                        typeAnimalSelectionne = 0;
                        break;
                    case 6:
                        Ajouter_Animal();
                        Buffle buffle = new Buffle(X, Y);
                        animaux.Add(buffle);
                        typeAnimalSelectionne = 0;
                        break;
                }
            }
            else
            {
                typeAnimalSelectionne = 0;
            }
        }

        //Retourne quel enclos à été cliqué (int 1 à 4).
        private int GetEncloClique(int x, int y)
        {
            int encloNum = 0;

            int vX = x / 32;
            int vY = y / 32;

            //HAUT-GAUCHE
            if ((vX >= 5 && vX <= 12) && (vY >= 4 && vY <= 13))
            {
                encloNum = 1;
            }
            //HAUT-DROITE
            if ((vX >= 25 && vX <= 33) && (vY >= 4 && vY <= 13))
            {
                encloNum = 2;
            }

            //BAS-GAUCHE
            if ((vY >= 16 && vY <= 25) && (vX >= 5 && vX <= 12))
            {
                encloNum = 3;
            }
            //BAS-DROITE
            if ((vX >= 25 && vX <= 33) && (vY >= 16 && vY <= 25))
            {
                encloNum = 4;
            }

            return encloNum;
        }

        // Verifie rapidement si le x, y donné est sur un enclos.
        private bool VerifierXYEnclos(int x, int y)
        {
            bool bon = false;
            // Enclo #1
            if ((x >= 175 && x < 395) && (y >= 155 && y <= 385))
            {
                bon = true;
            }

            // Enclo #2
            if ((x >= 865 && x < 1075) && (y >= 155 && y <= 385))
            {
                bon = true;
            }

            // Enclo #3
            if ((x >= 175 && x < 395) && (y >= 565 && y <= 805))
            {
                bon = true;
            }

            // Enclo #4
            if ((x >= 865 && x < 1075) && (y >= 565 && y <= 805))
            {
                bon = true;
            }

            return bon;
        }
        
        //Ajoute un mouton au X,Y choisi.
        private void AjouterMouton(int X, int Y)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(20, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                Mouton mouton = new Mouton(X, Y);
                animaux.Add(mouton);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter un mouton.");
            }
        }

        //Ajoute un grizzly au X,Y choisi.
        private void AjouterGrizzly (int X, int Y)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(30, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                Grizzly grizzly = new Grizzly(X, Y);
                animaux.Add(grizzly);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter un grizzly.");
            }
            
        }
        
        //Ajoute un lion au X,Y choisi.
        private void AjouterLion(int X, int Y)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(35, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                Lion lion = new Lion(X, Y);
                animaux.Add(lion);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter un lion.");
            }
        }

        //Ajoute une licorne au X,Y choisi.
        private void AjouterLicorne(int X, int Y)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(50, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                Licorne licorne = new Licorne(X, Y);
                animaux.Add(licorne);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter une licorne.");
            }
        }

        //Ajoute un rhinocéros au X,Y choisi.
        private void AjouterRhino(int X, int Y)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(40, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                Rhino rhino = new Rhino(X, Y);
                animaux.Add(rhino);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter un rhinocéros.");
            }
        }

        //Ajoute un buffle au X,Y choisi.
        private void AjouterBuffle(int X, int Y)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(40, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                Buffle buffle = new Buffle(X, Y);
                animaux.Add(buffle);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter un lion.");
            }
        }

        /// <summary>
        /// Cette méthode définie les tuiles situées 
        /// à l'intérieur d'un enclo
        /// </summary>
        private void DefinirInterieurEnclos()
        {
            bool enclo = false;
            for (int ligne = 0; ligne < 28; ligne++)
            {
                for (int colonne = 0; colonne < 40; colonne++)
                {
                    if (enclo && !Carte[ligne, colonne].EstUnObstacle)
                    {
                        Carte[ligne, colonne].EstDansUnEnclo = true;
                        if(ligne < 13 && colonne < 18)
                        {
                            Carte[ligne, colonne].PositionEnclo = Enclo.UpLeft;
                        }
                        else if(ligne < 13 && colonne > 18)
                        {
                            Carte[ligne, colonne].PositionEnclo = Enclo.UpRight;
                        }
                        else if(ligne > 13 && colonne < 18)
                        {
                            Carte[ligne, colonne].PositionEnclo = Enclo.DownLeft;
                        }
                        else
                        {
                            Carte[ligne, colonne].PositionEnclo = Enclo.DownRight;
                        }
                    }
                    if (Carte[ligne, colonne].EstUnObstacle)
                    {
                        if (enclo)
                        {
                            enclo = false;
                        }
                        else
                        {
                            enclo = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Définir les obstacles (clotures) à l'intérieur
        /// du tableau Carte
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="enclo"></param>
        private void RendreClotureSolide(int row, int column, Enclo enclo)
        {   
            
            for (int i = 0; i < 9; i++)
            {
                if(i == 1)
                {
                    Carte[row, column].EstDansUnEnclo = true;
                }
                if(i == 7)
                {
                    Carte[row, column].EstDansUnEnclo = true;
                }
                Carte[row, column].EstUnObstacle = true;
                Carte[row, column].PositionEnclo = enclo;
                column++;
            }
            for (int i = 0; i < 9; i++)
            {
                if (i == 1)
                {
                    Carte[row, column].EstDansUnEnclo = true;
                }
                if (i == 7)
                {
                    Carte[row, column].EstDansUnEnclo = true;
                }
                Carte[row, column].EstUnObstacle = true;
                Carte[row, column].PositionEnclo = enclo;
                row++;
            }
            Carte[row, column].EstUnObstacle = true;
            Carte[row, column].PositionEnclo = enclo;
            row++;
            for (int i = 0; i < 9; i++)
            {
                if (i == 1)
                {
                    Carte[row, column].EstDansUnEnclo = true;
                }
                if (i == 7)
                {
                    Carte[row, column].EstDansUnEnclo = true;
                }
                Carte[row, column].EstUnObstacle = true;
                Carte[row, column].PositionEnclo = enclo;
                column--;
            }
            for (int i = 0; i < 9; i++)
            {
                if (i == 1)
                {
                    Carte[row, column].EstDansUnEnclo = true;
                }
                if (i == 7)
                {
                    Carte[row, column].EstDansUnEnclo = true;
                }
                Carte[row, column].EstUnObstacle = true;
                Carte[row, column].PositionEnclo = enclo;
                row--;
            }
            Carte[row, column].EstUnObstacle = true;
            Carte[row, column].PositionEnclo = enclo;
        }

        /*
         * Ajoute un visiteur à la tuile position X, Y (en tuiles pas en pixels).
         * Le spawn des visiteurs est à (19, 28).
         */
        public void AjouterVisiteur(Genre genre, int x, int y)
        {
            this.visiteurs.Add(new Visiteur(tuile.Width * x, tuile.Height * y, rand));
        }

        /*
         * Ajoute un visiteur directemnet au spawn de visiteur.
         */
        public void AjouterVisiteurSpawn()
        {
            Visiteur vis = new Visiteur(tuile.Width * 19, tuile.Height * 25, rand);
            Carte[24, 19].VisiteurSurLaTuile = vis;
            vis.XInfos = Carte[24, 19].X;
            vis.YInfos = Carte[24, 19].Y + 5;
            this.visiteurs.Add(vis);
            visiteurs[visiteurs.Count - 1].TempsDansLeZoo = new Stopwatch();
            visiteurs[visiteurs.Count - 1].TempsDansLeZoo.Start();
            Carte[24, 19].EstUnObstacle = true;
            CalculerEtFacturerPrixEntree();
            /*
            PictureBox newVisiteur = new PictureBox();
            newVisiteur.BackgroundImage = visiteurs[visiteurs.Count - 1].imageVisiteur;
            newVisiteur.Location = new Point(visiteurs[visiteurs.Count - 1].X, visiteurs[visiteurs.Count - 1].Y);
            newVisiteur.Width = visiteurs[visiteurs.Count - 1].Width;
            newVisiteur.Height = visiteurs[visiteurs.Count - 1].Height;
            newVisiteur.BringToFront();
            newVisiteur.Name = "visiteurPB" + visiteursPicBox.Count.ToString().Trim();
            newVisiteur.BackgroundImageLayout = ImageLayout.Stretch;
            
            this.Controls.Add(newVisiteur);

            visiteursPicBox.Add(newVisiteur);
            */
        }
        /// <summary>
        /// Calcule le prix d'entré du zoo et le facture au visiteur
        /// tout juste ajouté
        /// </summary>
        private void CalculerEtFacturerPrixEntree()
        {
            double prix = 2.0; //On met le prix de base à 2$ même si y'a pas d'animaux, c'est pour tester
            prix = 2.0 * NombreAnimaux;
            prix -= 0.1 * dechets.Count;
            if(prix < 0)
            {
                MessageBox.Show("Votre zoo est tellement dégeulasse que vous payez " + prix + "$ \n " +
                                "aux visiteurs pour qu'ils viennent.");
            }
            Player.Argent += prix;
            affichageArgent.Text = Player.Argent + "$";
        }


        /* Tout ce qui est dessiner sur l'écran.
         */
        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;

            LogiqueMenuBar();

            this.affichageArgent.Text = this.Player.Argent.ToString() + "$";

            this.dateToolStripMenuItem.Text = this.datejeu.Date.ToString("dd MMMM yyyy");

            g.DrawImage(Properties.Resources.Background_game, 0, 0, this.Width, this.Height);

            for (int i = 0; i < visiteurs.Count; i++)
            {
                g.DrawImage(visiteurs[i].imageVisiteur, visiteurs[i].X, visiteurs[i].Y, 32, 32);

                Font font = new Font("Arial", 8);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                int nomX = visiteurs[i].X - visiteurs[i].Width / 2;
                int nomY = visiteurs[i].Y - 20;
                string nom = visiteurs[i].Nom;

            //    g.DrawString(nom, font, drawBrush, new Point(nomX, nomY));
                
            }

            for (int i = 0; i < animaux.Count; i++)
            {
                g.DrawImage(animaux[i].image, animaux[i].X, animaux[i].Y, 32, 32);
            }

            for (int i = 0; i < dechets.Count; i++)
            {
                g.DrawImage(dechets[i].Image, dechets[i].X, dechets[i].Y, 32, 32);
            }

            for (int i = 0; i < concierges.Count; i++)
            {
                g.DrawImage(concierges[i].Image, concierges[i].X, concierges[i].Y, 32, 32);
            }
        }

        /* Logique du jeu (1x par tick).
         */
        public void Logic()
        {
            LogicVisiteurs();
            LogicConcierges();
            LogicContravention();
            LogicAnimaux();

            if (stopwatchJeu.Elapsed.TotalMilliseconds >= 5 / 365 * 3600)
            {
                this.datejeu = this.datejeu.AddDays(1);

                this.stopwatchJeu.Restart();
            }


            //Paye le joueur en fonction du nombre de visiteurs et de déchets (1x par minute).
            if ((DateTime.Now - dt_remuneration).TotalSeconds >= 60)
            {
                PayeJoueur();

                this.dt_remuneration = DateTime.Now;
            }
        }

        // Fait bouger les animaux en restant dans les enclos.
        private void LogicAnimaux()
        {
            for (int i = 0; i < animaux.Count; i++)
            {
                int randX = rand.Next(3);
                int randY = rand.Next(3);

                while ((randX == randY) ||
                        (randY == 0 && animaux[i].Y - tuile.Height <= 0 + tuile.Height) ||
                        (randY == 1 && animaux[i].Y + tuile.Height >= this.Height - tuile.Height) ||
                        (randX == 0 && animaux[i].X - tuile.Width <= 0 + tuile.Width) ||
                        (randX == 1 && animaux[i].X + tuile.Width >= this.Width - tuile.Height) ||
                        (randX != 2 && randY != 2) //||
                                                   //(IsColliding(randX, randY, visiteurs[i]))
                        )
                {
                    randX = rand.Next(3);
                    randY = rand.Next(3);
                }

                int vX = animaux[i].X / this.tailleTuile;
                int vY = animaux[i].Y / this.tailleTuile;

                Console.WriteLine("vX = " + vX);
                Console.WriteLine("vY = " + vY);
                //GAUCHE
                if (randX == 0 && !(vX >= 4 && vX <= 5) && !(vX >= 25 && vX <= 26))
                {
                    animaux[i].X -= tuile.Width;
                    animaux[i].MovingX = -1;
                    animaux[i].MovingY = 0;
                    animaux[i].ReloadImages();
                }
                //DROITE
                else if (randX == 1 && !(vX >= 11 && vX <= 12) && !(vX >= 32 && vX <= 33))
                {
                    animaux[i].X += tuile.Width;
                    animaux[i].MovingX = 1;
                    animaux[i].MovingY = 0;
                    animaux[i].ReloadImages();
                }
                //HAUT
                if (randY == 0 && !(vY >= 3 && vY <= 4) && !(vY >= 16 && vY <= 17))
                {
                    animaux[i].Y -= tuile.Height;
                    animaux[i].MovingY = 1;
                    animaux[i].MovingX = 0;
                    animaux[i].ReloadImages();
                }
                //BAS
                else if (randY == 1 && !(vY >= 12 && vY <= 13) && !(vY >= 24 && vY <= 25))
                {
                    animaux[i].Y += tuile.Height;
                    animaux[i].MovingY = -1;
                    animaux[i].MovingX = 0;
                    animaux[i].ReloadImages();
                }


                int cX = animaux[i].X;
                int cY = animaux[i].Y;

                // Animaux Croissance

                if ((DateTime.Now - animaux[i].DateNaissance).TotalMilliseconds >= 5 / 365 * 3600 && !animaux[i].Adulte)
                {
                    animaux[i].Adulte = true;
                }

                if (animaux[i].genre == Animal.Genre.Femelle && EncloContient(animaux[i].Enclos) && !animaux[i].EnGestation)
                {
                    animaux[i].EnGestation = true;
                    animaux[i].DebutGestation = DateTime.Now;
                }

                if (animaux[i].EnGestation && (DateTime.Now - animaux[i].DebutGestation).TotalMilliseconds * 5 / 365 * 3600 >= animaux[i].Gestation)
                {
                    animaux[i].EnGestation = false;
                    animaux[i].DebutGestation = DateTime.Now;
                    this.typeAnimalSelectionne = animaux[i].Type;
                    this.Accoucher(animaux[i].X, animaux[i].Y);
                }
            }
        }

        private bool EncloContient(int enclos)
        {
            bool contient = false;

            for (int i = 0; i < animaux.Count; i++)
            {
                if (animaux[i].Enclos == enclos && animaux[i].genre == Animal.Genre.Male)
                {
                    contient = true;
                }
            }

            return contient;
        }

        // Nourris les animaux à double le prix si ils ont faim.
        private void LogicContravention()
        {
            for (int i = 0; i < animaux.Count; i++)
            {
                if ((DateTime.Now - animaux[i].DernierRepas).TotalSeconds >= animaux[i].Faim)
                {
                    animaux[i].NourrirDoublePrix(Player);
                }
            }
        }

        // Active ou désactive les options du menu dépendament de l'argent du joueur.
        private void LogiqueMenuBar()
        {
            double argent = Player.Argent;

            if (argent >= 20)
            {
                mouton20ToolStripMenuItem.Enabled = true;
            } else
            {
                mouton20ToolStripMenuItem.Enabled = false;
            }

            if (argent >= 30)
            {
                grizzly30ToolStripMenuItem.Enabled = true;
            }
            else
            {
                grizzly30ToolStripMenuItem.Enabled = false;
            }

            if (argent >= 35)
            {
                lion35ToolStripMenuItem.Enabled = true;
            }
            else
            {
                lion35ToolStripMenuItem.Enabled = false;
            }

            if (argent >= 50)
            {
                buffle30ToolStripMenuItem.Enabled = true;
            }
            else
            {
                buffle30ToolStripMenuItem.Enabled = false;
            }

            if (argent >= 40)
            {
                rhinocéros40ToolStripMenuItem.Enabled = true;
            }
            else
            {
                rhinocéros40ToolStripMenuItem.Enabled = false;
            }

            if (argent >= 40)
            {
                buffle40ToolStripMenuItem.Enabled = true;
            }
            else
            {
                buffle40ToolStripMenuItem.Enabled = false;
            }
        }

        // Paye le joueur (nombre de visiteurs * nombre d'animaux).
        private void PayeJoueur()
        {
            double paye = 0.0;

            for (int i = 0; i < visiteurs.Count; i++)
            {
                paye += PAYE_PAR_VISITEUR * NombreAnimaux;
            }

            for (int i = 0; i < dechets.Count; i++)
            {
                paye -= MALUS_PAR_DECHET;
            }

            this.Player.AjouterArgent(paye);
        }

        /*
         * Fait bouger les visiteurs.
         */
        private void LogicVisiteurs()
        {
            Console.WriteLine("LogicVisiteurs.");
            Visiteur BackUp;
            for (int i = 0; i < visiteurs.Count; i++)
            {
                int randX = rand.Next(3);
                int randY = rand.Next(3);

                while ((randX == randY) ||
                        (randY == 0 && visiteurs[i].Y - tuile.Height <= 0 + tuile.Height) ||
                        (randY == 1 && visiteurs[i].Y + tuile.Height >= this.Height - tuile.Height) ||
                        (randX == 0 && visiteurs[i].X - tuile.Width <= 0 + tuile.Width) ||
                        (randX == 1 && visiteurs[i].X + tuile.Width >= this.Width - tuile.Height) ||
                        (randX != 2 && randY != 2) //||
                        //(IsColliding(randX, randY, visiteurs[i]))
                        )
                {
                    randX = rand.Next(3);
                    randY = rand.Next(3);
                }

                int vX = visiteurs[i].X / this.tailleTuile;
                int vY = visiteurs[i].Y / this.tailleTuile;

                Console.WriteLine("vX = " + vX);
                Console.WriteLine("vY = " + vY);
                //GAUCHE
                if (randX == 0 && !(vX >= 3 && vX <= 13 && vY >= 2 && vY <= 12) && !(vX >= 3 && vX <= 13 && vY >= 15 && vY <= 25) && !(vX >= 24 && vX <= 34 && vY >= 2 && vY <= 12) && !(vX >= 24 && vX <= 34 && vY >= 15 && vY <= 25))
                {
                    if (visiteurs[i].CurrentColumn != 0)
                    {
                        if (!Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn - 1].EstUnObstacle)
                        {
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = false;
                            BackUp = Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile = null;
                            visiteurs[i].CurrentColumn--;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = true;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile = BackUp;
                            visiteurs[i].X -= tuile.Width;
                            visiteurs[i].MovingX = -1;
                            visiteurs[i].MovingY = 0;
                            visiteurs[i].XInfos = visiteurs[i].X;
                            visiteurs[i].YInfos = visiteurs[i].Y + 5;
                        }
                    }
                } 
                //DROITE
                else if (randX == 1 && !(vX >= 4 && vX <= 14 && vY >= 2 && vY <= 12) && !(vX >= 4 && vX <= 14 && vY >= 15 && vY <= 25) && !(vX >= 25 && vX <= 35 && vY >= 2 && vY <= 12) && !(vX >= 25 && vX <= 35 && vY >= 15 && vY <= 25))
                {
                    if (visiteurs[i].CurrentColumn != 39)
                    {
                        if (!Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn + 1].EstUnObstacle)
                        {
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = false;
                            BackUp = Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile = null;
                            visiteurs[i].CurrentColumn++;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = true;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile = BackUp;
                            visiteurs[i].X += tuile.Width;
                            visiteurs[i].MovingX = 1;
                            visiteurs[i].MovingY = 0;
                            visiteurs[i].XInfos = visiteurs[i].X;
                            visiteurs[i].YInfos = visiteurs[i].Y + 5;
                        }
                    }
                }
                //HAUT
                if (randY == 0 && !(vY >= 3 && vY <= 13 && vX >= 5 && vX <= 12) && !(vY >= 3 && vY <= 13 && vX >= 26 && vX <= 33) && !(vY >= 16 && vY <= 26 && vX >= 5 && vX <= 12) && !(vY >= 16 && vY <= 26 && vX >= 26 && vX <= 33))
                {
                    if (visiteurs[i].CurrentRow != 0)
                    {
                        if (!Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentRow - 1].EstUnObstacle)
                        {
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = false;
                            BackUp = Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile = null;
                            visiteurs[i].CurrentRow--;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = true;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile = BackUp;
                            visiteurs[i].Y -= tuile.Height;
                            visiteurs[i].MovingY = 1;
                            visiteurs[i].MovingX = 0;
                            visiteurs[i].XInfos = visiteurs[i].X;
                            visiteurs[i].YInfos = visiteurs[i].Y + 5;
                        }
                    }
                }
                //BAS
                else if (randY == 1 && !(vY >= 1 && vY <= 10 && vX >= 5 && vX <= 12) && !(vY >= 1 && vY <= 10 && vX >= 26 && vX <= 33) && !(vY >= 14 && vY <= 24 && vX >= 5 && vX <= 12) && !(vY >= 14 && vY <= 24 && vX >= 26 && vX <= 33))
                {
                    if (visiteurs[i].CurrentRow != 27)
                    {
                        if (!Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentRow + 1].EstUnObstacle)
                        {
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = false;
                            BackUp = Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile = null;
                            visiteurs[i].CurrentRow++;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = true;
                            Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].VisiteurSurLaTuile = BackUp;
                            visiteurs[i].Y += tuile.Height;
                            visiteurs[i].MovingY = -1;
                            visiteurs[i].MovingX = 0;
                            visiteurs[i].XInfos = visiteurs[i].X;
                            visiteurs[i].YInfos = visiteurs[i].Y + 5;
                        }
                    }
                }
                visiteurs[i].ReloadImages();
                EchapeDechet(vX, vY, visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn);
                /*
                string visiteurPBName = "visiteurPB" + i.ToString().Trim();
                Control[] foundVisiteurs = Controls.Find(visiteurPBName, true);
                PictureBox visiteurPB = (PictureBox)foundVisiteurs.First();
                if (randX == 0) visiteurPB.Location = new Point(visiteurPB.Location.X - tuile.Width, visiteurPB.Location.Y);
                else if (randX == 1) visiteurPB.Location = new Point(visiteurPB.Location.X + tuile.Width, visiteurPB.Location.Y);

                if (randY == 0) visiteurPB.Location = new Point(visiteurPB.Location.X, visiteurPB.Location.Y - tuile.Height);
                else if (randY == 1) visiteurPB.Location = new Point(visiteurPB.Location.X, visiteurPB.Location.Y + tuile.Height);
                */
            }
                
                Console.WriteLine("LogicVisiteurs Fin.");
        }

        /*
         * Fait bouger les concierges.
         */
        private void LogicConcierges()
        {

            for (int i = 0; i < concierges.Count; i++)
            {
                int randX = rand.Next(3);
                int randY = rand.Next(3);

                while ((randX == randY) ||
                        (randY == 0 && concierges[i].Y - tuile.Height <= 0 + tuile.Height) ||
                        (randY == 1 && concierges[i].Y + tuile.Height >= this.Height - tuile.Height) ||
                        (randX == 0 && concierges[i].X - tuile.Width <= 0 + tuile.Width) ||
                        (randX == 1 && concierges[i].X + tuile.Width >= this.Width - tuile.Height) ||
                        (randX != 2 && randY != 2) //||
                                                   //(IsColliding(randX, randY, visiteurs[i]))
                        )
                {
                    randX = rand.Next(3);
                    randY = rand.Next(3);
                }

                int vX = concierges[i].X / this.tailleTuile;
                int vY = concierges[i].Y / this.tailleTuile;

                Console.WriteLine("vX = " + vX);
                Console.WriteLine("vY = " + vY);
                //GAUCHE
                if (randX == 0 && !(vX >= 3 && vX <= 13 && vY >= 2 && vY <= 12) && !(vX >= 3 && vX <= 13 && vY >= 15 && vY <= 25) && !(vX >= 24 && vX <= 34 && vY >= 2 && vY <= 12) && !(vX >= 24 && vX <= 34 && vY >= 15 && vY <= 25))
                {
                    if (concierges[i].CurrentColumn != 0)
                    {
                        if (!Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn - 1].EstUnObstacle)
                        {
                            concierges[i].X -= tuile.Width;
                            concierges[i].MovingX = -1;
                            concierges[i].MovingY = 0;
                            concierges[i].ReloadImages();
                            Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = false;
                            concierges[i].CurrentColumn--;
                            Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = true;
                        }
                    }
                }
                //DROITE
                else if (randX == 1 && !(vX >= 4 && vX <= 14 && vY >= 2 && vY <= 12) && !(vX >= 4 && vX <= 14 && vY >= 15 && vY <= 25) && !(vX >= 25 && vX <= 35 && vY >= 2 && vY <= 12) && !(vX >= 25 && vX <= 35 && vY >= 15 && vY <= 25))
                {
                    if (concierges[i].CurrentColumn != 39)
                    {
                        if (!Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn + 1].EstUnObstacle)
                        {
                            concierges[i].X += tuile.Width;
                            concierges[i].MovingX = 1;
                            concierges[i].MovingY = 0;
                            concierges[i].ReloadImages();
                            Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = false;
                            concierges[i].CurrentColumn++;
                            Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = true;
                        }
                    }
                }
                //HAUT
                if (randY == 0 && !(vY >= 3 && vY <= 13 && vX >= 4 && vX <= 13) && !(vY >= 3 && vY <= 13 && vX >= 25 && vX <= 34) && !(vY >= 16 && vY <= 26 && vX >= 4 && vX <= 13) && !(vY >= 16 && vY <= 26 && vX >= 25 && vX <= 34))
                {
                    if (concierges[i].CurrentRow != 0)
                    {
                        if (!Carte[concierges[i].CurrentRow - 1, concierges[i].CurrentColumn].EstUnObstacle)
                        {
                            concierges[i].Y -= tuile.Height;
                            concierges[i].MovingY = 1;
                            concierges[i].MovingX = 0;
                            concierges[i].ReloadImages();
                            Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = false;
                            concierges[i].CurrentRow--;
                            Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = true;
                        }
                    }
                }
                //BAS
                else if (randY == 1 && !(vY >= 1 && vY <= 10 && vX >= 4 && vX <= 13) && !(vY >= 1 && vY <= 10 && vX >= 25 && vX <= 34) && !(vY >= 14 && vY <= 24 && vX >= 4 && vX <= 13) && !(vY >= 14 && vY <= 24 && vX >= 25 && vX <= 34))
                {
                    if (concierges[i].CurrentRow != 27)
                    {
                        if (!Carte[concierges[i].CurrentRow - 1, concierges[i].CurrentColumn].EstUnObstacle)
                        {
                            concierges[i].Y += tuile.Height;
                            concierges[i].MovingY = -1;
                            concierges[i].MovingX = 0;
                            concierges[i].ReloadImages();
                            Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = false;
                            concierges[i].CurrentRow++;
                            Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = true;
                        }
                    }
                }


                int cX = concierges[i].X;
                int cY = concierges[i].Y;

                //Ramasser déchets.

                for (int d = 0; d < dechets.Count; d++)
                {
                    int dX = dechets[d].X;
                    int dY = dechets[d].Y;

                    if (dX == cX && dY == cY)
                    {
                        Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].DechetSurTuile = null;
                        dechets.RemoveAt(d);
                    }
                }


                PayerConcierges();
            }
;
        }

        // Retire de l'argent au joueurs pour la paye des concierges.
        private void PayerConcierges()
        {
            if (stopwatchPayerConcierges.Elapsed.TotalSeconds >= 60)
            {
                int cout = this.concierges.Count * coutConcierge;

                this.Player.RetirerArgent(cout);
                affichageArgent.Text = Player.Argent + " $";
                stopwatchPayerConcierges.Restart();
            }
        }

        // Verifie si le joueur à assez d'argent.
        public bool AssezArgent(int cout)
        {
            bool assez = true;

            if (this.Player.Argent < cout)
            {
                assez = false;
            }

            return assez;
        }


        /*
         * Un visiteur a certaines chances d'échapper un déchet.
         */
        private void EchapeDechet(int x, int y, int row, int column)
        {
            int chance = this.rand.Next(0, 100);
            int limite = 1;

            if (chance < limite)
            {
                x *= tailleTuile;
                y *= tailleTuile;
                Dechet Trash = new Dechet(x, y);
                Carte[row , column].DechetSurTuile = Trash;
                this.dechets.Add(Trash);
            }
        }

        /*private bool IsColliding(int randX, int randY, Visiteur visiteur)
        {
            bool colliding = false;

            int oldX = visiteur.X;
            int oldMovingX = visiteur.MovingX;
            int oldMovingY = visiteur.MovingX;

            if (randX == 0)
            {
                visiteur.X -= tuile.Width;
                visiteur.MovingX = -1;
                visiteur.MovingY = 0;
            }
            else if (randX == 1)
            {
                visiteur.X += tuile.Width;
                visiteur.MovingX = 1;
                visiteur.MovingY = 0;
            }

            if (randY == 0)
            {
                visiteur.Y -= tuile.Height;
                visiteur.MovingY = 1;
                visiteur.MovingX = -0;
            }
            else if (randY == 1)
            {
                visiteur.Y += tuile.Height;
                visiteur.MovingY = -1;
                visiteur.MovingX = 0;
            }
            


            for (int i = 0; i < Carte.GetLength(1); i++)
            {
                for (int o = 0; o < Carte.GetLength(0); o++)
                {
                    int vX = visiteur.X;
                    int vY = visiteur.Y;
                    int vW = visiteur.Width;
                    int vH = visiteur.Height;
                    int tS = 32;
                    int tX = Carte[o, i].Colonne * tS;
                    int tY = Carte[o, i].Ligne * tS;
                    bool estObstacle = Carte[o, i].EstUnObstacle;


                    if (vX >= tX && vX <= tX + tS && vY >= tY && vY <= tY + tS && estObstacle)
                    {
                        colliding = true;
                        break;
                    } else
                    {
                        
                    }
                }
            }

            visiteur.X = oldX;
            visiteur.MovingX = oldMovingX;
            visiteur.MovingX = oldMovingY;

            return colliding;
        }*/


        private void Jeu_Load(object sender, EventArgs e)
        {
            /// FPS timer
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = (intFPS); // FPS
            timer.Tick += new EventHandler(TickTick);
            timer.Start();
        }

        public delegate void DelegateRefresh();
        
        // La boucle principale du jeu.
        private void BouclePrincipaleDuJeu()
        {
            while (!gameover)
            {
                if (stopWatch.ElapsedMilliseconds >= FPS)
                {
                    Logic();
                    stopWatch.Restart();
                }
                
            }
        }

        // Roule à chaque fois que le timer tick.
        private void TickTick(object sender, EventArgs e)
        {
             Refresh();
        /*    Thread th = new Thread(thStart);
            th.Start();*/
        }

        private void embaucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Player.PeutEngagerConcierge = true;
            MessageBox.Show("Séléctionnez la tuile adjaçente sur laquelle vous \n voulez faire apparaître le concierge");
        }

        // Ajoute un nouveau concierge à la position donnée.
        private void NewConcierge(int cX, int cY, int ligne, int colonne)
        {
            try
            {
                concierges.Add(new Concierge(cX, cY, ligne, colonne));
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Jeu_KeyDown(object sender, KeyEventArgs e)
        {
            BougerJoueur(e);
        }
        /// <summary>
        /// Event qui se se déclenche lorsqu'un
        /// touche du clavié est enfoncée et fait bouger le joueur
        /// et les autres actions aux clavier
        /// </summary>
        /// <param name="e"></param>
        private void BougerJoueur(KeyEventArgs e)
        {
            int row = Player.CurrentRow, column = Player.CurrentColumn;
            Console.WriteLine("KeyDown");
            if (e.KeyCode == Keys.U)
            {
                MessageBox.Show("(" + Player.CurrentRow + "," + Player.CurrentColumn + ")\n" + 
                                "X: " + Player.CurrentSprite.Location.X + " Y: " + Player.CurrentSprite.Location.Y);
            }
            if(e.KeyCode == Keys.M)
            {
                snd.Stop();
            }
            if (e.KeyCode == Keys.S)
            {
                EnleverTuilesAdjacentes();
                Carte[Player.CurrentRow, Player.CurrentColumn].EstUnObstacle = false;
                Player.MoveDown();
                Carte[Player.CurrentRow, Player.CurrentColumn].EstUnObstacle = true;
                MettreTuilesAdjacentes();
                if (Player.PeutNourrir)
                {
                    try
                    {
                        if (Carte[row + 2, column].EstDansUnEnclo || Carte[row - 2, column].EstDansUnEnclo
                       || Carte[row, column + 2].EstDansUnEnclo || Carte[row, column - 2].EstDansUnEnclo)
                        {
                            Player.PeutNourrir = true;
                        }
                        else
                        {
                            Player.PeutNourrir = false;
                            Player.EncloChoisi = Enclo.PasEnclo;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Player.PeutNourrir = false;
                        Player.EncloChoisi = Enclo.PasEnclo;
                    }
                }
                if (Player.PeutAjouterAnimal)
                {
                    try
                    {
                        if (Carte[row + 2, column].EstDansUnEnclo || Carte[row - 2, column].EstDansUnEnclo
                       || Carte[row, column + 2].EstDansUnEnclo || Carte[row, column - 2].EstDansUnEnclo)
                        {
                            Player.PeutAjouterAnimal = true;
                        }
                        else
                        {
                            Player.PeutAjouterAnimal = false;
                            Player.EncloChoisi = Enclo.PasEnclo;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Player.PeutAjouterAnimal = false;
                        Player.EncloChoisi = Enclo.PasEnclo;
                    }
                }
            }
            if (e.KeyCode == Keys.W)
            {
                EnleverTuilesAdjacentes();
                Carte[Player.CurrentRow, Player.CurrentColumn].EstUnObstacle = false;
                Player.MoveUp();
                Carte[Player.CurrentRow, Player.CurrentColumn].EstUnObstacle = true;
                MettreTuilesAdjacentes();
                if (Player.PeutNourrir)
                {
                    try
                    {
                        if (Carte[row + 2, column].EstDansUnEnclo || Carte[row - 2, column].EstDansUnEnclo
                       || Carte[row, column + 2].EstDansUnEnclo || Carte[row, column - 2].EstDansUnEnclo)
                        {
                            Player.PeutNourrir = true;
                        }
                        else
                        {
                            Player.PeutNourrir = false;
                            Player.EncloChoisi = Enclo.PasEnclo;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Player.PeutNourrir = false;
                        Player.EncloChoisi = Enclo.PasEnclo;
                    }
                }
                if (Player.PeutAjouterAnimal)
                {
                    try
                    {
                        if (Carte[row + 2, column].EstDansUnEnclo || Carte[row - 2, column].EstDansUnEnclo
                       || Carte[row, column + 2].EstDansUnEnclo || Carte[row, column - 2].EstDansUnEnclo)
                        {
                            Player.PeutAjouterAnimal = true;
                        }
                        else
                        {
                            Player.PeutAjouterAnimal = false;
                            Player.EncloChoisi = Enclo.PasEnclo;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Player.PeutAjouterAnimal = false;
                        Player.EncloChoisi = Enclo.PasEnclo;
                    }
                }
            }
            if (e.KeyCode == Keys.D)
            {
                EnleverTuilesAdjacentes();
                Carte[Player.CurrentRow, Player.CurrentColumn].EstUnObstacle = false;
                Player.MoveRight();
                Carte[Player.CurrentRow, Player.CurrentColumn].EstUnObstacle = true;
                MettreTuilesAdjacentes();
                if (Player.PeutNourrir)
                {
                    try
                    {
                        if (Carte[row + 2, column].EstDansUnEnclo || Carte[row - 2, column].EstDansUnEnclo
                       || Carte[row, column + 2].EstDansUnEnclo || Carte[row, column - 2].EstDansUnEnclo)
                        {
                            Player.PeutNourrir = true;
                        }
                        else
                        {
                            Player.PeutNourrir = false;
                            Player.EncloChoisi = Enclo.PasEnclo;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Player.PeutNourrir = false;
                        Player.EncloChoisi = Enclo.PasEnclo;
                    }
                }
                if (Player.PeutAjouterAnimal)
                {
                    try
                    {
                        if (Carte[row + 2, column].EstDansUnEnclo || Carte[row - 2, column].EstDansUnEnclo
                       || Carte[row, column + 2].EstDansUnEnclo || Carte[row, column - 2].EstDansUnEnclo)
                        {
                            Player.PeutAjouterAnimal = true;
                        }
                        else
                        {
                            Player.PeutAjouterAnimal = false;
                            Player.EncloChoisi = Enclo.PasEnclo;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Player.PeutAjouterAnimal = false;
                        Player.EncloChoisi = Enclo.PasEnclo;
                    }
                }
            }
            if (e.KeyCode == Keys.A)
            {
                EnleverTuilesAdjacentes();
                Carte[Player.CurrentRow, Player.CurrentColumn].EstUnObstacle = false;
                Player.MoveLeft();
                Carte[Player.CurrentRow, Player.CurrentColumn].EstUnObstacle = true;
                MettreTuilesAdjacentes();
                if (Player.PeutNourrir)
                {
                    try
                    {
                        if (Carte[row + 2, column].EstDansUnEnclo || Carte[row - 2, column].EstDansUnEnclo
                            || Carte[row, column + 2].EstDansUnEnclo || Carte[row, column - 2].EstDansUnEnclo)
                        {
                            Player.PeutNourrir = true;
                        }
                        else
                        {
                            Player.PeutNourrir = false;
                            Player.EncloChoisi = Enclo.PasEnclo;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Player.PeutNourrir = false;
                        Player.EncloChoisi = Enclo.PasEnclo;
                    }
                }
                if (Player.PeutAjouterAnimal)
                {
                    try
                    {
                        if (Carte[row + 2, column].EstDansUnEnclo || Carte[row - 2, column].EstDansUnEnclo
                       || Carte[row, column + 2].EstDansUnEnclo || Carte[row, column - 2].EstDansUnEnclo)
                        {
                            Player.PeutAjouterAnimal = true;
                        }
                        else
                        {
                            Player.PeutAjouterAnimal = false;
                            Player.EncloChoisi = Enclo.PasEnclo;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Player.PeutAjouterAnimal = false;
                        Player.EncloChoisi = Enclo.PasEnclo;
                    }
                }
            }
            if(e.KeyCode == Keys.N)
            {
                try
                {
                    if (Carte[row + 2, column].EstDansUnEnclo || Carte[row - 2, column].EstDansUnEnclo
                       || Carte[row, column + 2].EstDansUnEnclo || Carte[row, column - 2].EstDansUnEnclo)
                    {
                        MessageBox.Show("Cliquez sur l'animal que vous voulez nourrir.");
                        Player.PeutNourrir = true;
                        if (Carte[row + 1, column].EstUnObstacle)
                        {
                            Player.EncloChoisi = Carte[row + 1, column].PositionEnclo;
                        }
                        else if (Carte[row - 1, column].EstUnObstacle)
                        {
                            Player.EncloChoisi = Carte[row - 1, column].PositionEnclo;
                        }
                        else if (Carte[row, column + 1].EstUnObstacle)
                        {
                            Player.EncloChoisi = Carte[row, column + 1].PositionEnclo;
                        }
                        else
                        {
                            Player.EncloChoisi = Carte[row, column - 1].PositionEnclo;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Vous devez être à côté d'un enclo pour nourrir un animal");
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show("Vous devez être à côté d'un enclo pour nourrir un animal");
                }
            }
        }

        /**
         * Déduit 35$ du joueur et instancie un nouveau Lion
         */
        private void lion35ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Carte[Player.CurrentRow + 2, Player.CurrentColumn].EstDansUnEnclo || Carte[Player.CurrentRow - 2, Player.CurrentColumn].EstDansUnEnclo
                          || Carte[Player.CurrentRow, Player.CurrentColumn + 2].EstDansUnEnclo || Carte[Player.CurrentRow, Player.CurrentColumn - 2].EstDansUnEnclo)
                {
                    Player.PeutAjouterAnimal = true;
                    typeAnimalSelectionne = 3;
                    MessageBox.Show("Appuyez quelque part pour ajouter un lion.");
                    if (Carte[Player.CurrentRow + 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow + 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow - 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow - 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow, Player.CurrentColumn + 1].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn + 1].PositionEnclo;
                    }
                    else
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn - 1].PositionEnclo;
                    }
                }
                else
                {
                    MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
            }
        }

        /**
         * Ajoute ou retire de l'argent du joueur selon le modificateur indiqué.
         * Met à jour l'affichage.
         * 
         * @param Montant - Quantité d'argent
         * @param Modificateur - True => Ajouter / False => Retirer
         */
        private bool Modifier_Argent(int Montant, bool Modificateur)
        {
            String[] TextArray = affichageArgent.Text.Split('$');
            String TextArgent = TextArray[0];
            double DoubleArgent;
            bool AjoutReussi = true;

            if (Modificateur)
            {
                Player.Argent += Montant;
            }
            else
            {
                if (Player.Argent - Montant >= 0)
                {
                    Player.Argent -= Montant;
                }
                else
                {
                    AjoutReussi = false;
                }
            }

            DoubleArgent = Player.Argent;
            TextArgent = DoubleArgent.ToString();

            return AjoutReussi;
        }

        /**
         * Ajoute un animal au compteur visuel.
         */
        private void Ajouter_Animal()
        {
            String[] TextArray = animauxToolStripMenuItem.Text.Split(' ');
            String TextAnimaux = TextArray[0];
            int NombreAnimaux = Int32.Parse(TextAnimaux);
           
            TextAnimaux = (++NombreAnimaux).ToString();
            this.NombreAnimaux = NombreAnimaux;
            animauxToolStripMenuItem.Text = NombreAnimaux + " Animaux";
            
        }
        private void Jeu_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Vous avez fait un profit de " + this.Player.Argent.ToString() + "$.");

            this.bouclePrincipale.Abort();

            snd.Stop();
            snd.Dispose();
            menuDepart.Dispose();
            this.Dispose();
        }

        private void PicUpRight_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

        }

        private void mouton20ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (Carte[Player.CurrentRow + 2, Player.CurrentColumn].EstDansUnEnclo || Carte[Player.CurrentRow - 2, Player.CurrentColumn].EstDansUnEnclo
                           || Carte[Player.CurrentRow, Player.CurrentColumn + 2].EstDansUnEnclo || Carte[Player.CurrentRow, Player.CurrentColumn - 2].EstDansUnEnclo)
                {
                    Player.PeutAjouterAnimal = true;
                    typeAnimalSelectionne = 1;
                    MessageBox.Show("Appuyez quelque part pour ajouter un mouton.");
                    if (Carte[Player.CurrentRow + 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow + 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow - 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow - 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow, Player.CurrentColumn + 1].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn + 1].PositionEnclo;
                    }
                    else
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn - 1].PositionEnclo;
                    }
                }
                else
                {
                    MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
            }
        }

        private void buffle40ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (Carte[Player.CurrentRow + 2, Player.CurrentColumn].EstDansUnEnclo || Carte[Player.CurrentRow - 2, Player.CurrentColumn].EstDansUnEnclo
                          || Carte[Player.CurrentRow, Player.CurrentColumn + 2].EstDansUnEnclo || Carte[Player.CurrentRow, Player.CurrentColumn - 2].EstDansUnEnclo)
                {
                    Player.PeutAjouterAnimal = true;
                    typeAnimalSelectionne = 6;
                    MessageBox.Show("Appuyez quelque part pour ajouter un buffle.");
                    if (Carte[Player.CurrentRow + 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow + 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow - 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow - 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow, Player.CurrentColumn + 1].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn + 1].PositionEnclo;
                    }
                    else
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn - 1].PositionEnclo;
                    }
                }
                else
                {
                    MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
            }
        }

        private void buffle30ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Carte[Player.CurrentRow + 2, Player.CurrentColumn].EstDansUnEnclo || Carte[Player.CurrentRow - 2, Player.CurrentColumn].EstDansUnEnclo
                          || Carte[Player.CurrentRow, Player.CurrentColumn + 2].EstDansUnEnclo || Carte[Player.CurrentRow, Player.CurrentColumn - 2].EstDansUnEnclo)
                {
                    Player.PeutAjouterAnimal = true;
                    typeAnimalSelectionne = 4;
                    MessageBox.Show("Appuyez quelque part pour ajouter une licorne.");
                    if (Carte[Player.CurrentRow + 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow + 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow - 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow - 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow, Player.CurrentColumn + 1].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn + 1].PositionEnclo;
                    }
                    else
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn - 1].PositionEnclo;
                    }
                }
                else
                {
                    MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
            }
        }

        private void grizzly30ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (Carte[Player.CurrentRow + 2, Player.CurrentColumn].EstDansUnEnclo || Carte[Player.CurrentRow - 2, Player.CurrentColumn].EstDansUnEnclo
                           || Carte[Player.CurrentRow, Player.CurrentColumn + 2].EstDansUnEnclo || Carte[Player.CurrentRow, Player.CurrentColumn - 2].EstDansUnEnclo)
                {
                    Player.PeutAjouterAnimal = true;
                    typeAnimalSelectionne = 2;
                    MessageBox.Show("Appuyez quelque part pour ajouter un grizzly.");
                    if (Carte[Player.CurrentRow + 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow+ 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow - 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow - 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow, Player.CurrentColumn + 1].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn + 1].PositionEnclo;
                    }
                    else
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn- 1].PositionEnclo;
                    }
                }
                else
                {
                    MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
            }
        }

        private void nourrirUnAnimalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = Player.CurrentRow;
            int column = Player.CurrentColumn;
            try
            {
                if (Carte[row + 2, column].EstDansUnEnclo || Carte[row - 2, column].EstDansUnEnclo
                       || Carte[row, column + 2].EstDansUnEnclo || Carte[row, column - 2].EstDansUnEnclo)
                {
                    MessageBox.Show("Cliquez sur l'animal que vous voulez nourrir.");
                    Player.PeutNourrir = true;
                    if (Carte[row + 1, column].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[row + 1, column].PositionEnclo;
                    }
                    else if (Carte[row - 1, column].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[row - 1, column].PositionEnclo;
                    }
                    else if (Carte[row, column + 1].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[row, column + 1].PositionEnclo;
                    }
                    else
                    {
                        Player.EncloChoisi = Carte[row, column - 1].PositionEnclo;
                    }
                }
                else
                {
                    MessageBox.Show("Vous devez être à côté d'un enclo pour nourrir un animal");
                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Vous devez être à côté d'un enclo pour nourrir un animal");
            }
        }

        private void rhinocéros40ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (Carte[Player.CurrentRow + 2, Player.CurrentColumn].EstDansUnEnclo || Carte[Player.CurrentRow - 2, Player.CurrentColumn].EstDansUnEnclo
                          || Carte[Player.CurrentRow, Player.CurrentColumn + 2].EstDansUnEnclo || Carte[Player.CurrentRow, Player.CurrentColumn - 2].EstDansUnEnclo)
                {
                    Player.PeutAjouterAnimal = true;
                    typeAnimalSelectionne = 5;
                    MessageBox.Show("Appuyez quelque part pour ajouter un rhinocéros.");
                    if (Carte[Player.CurrentRow + 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow + 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow - 1, Player.CurrentColumn].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow - 1, Player.CurrentColumn].PositionEnclo;
                    }
                    else if (Carte[Player.CurrentRow, Player.CurrentColumn + 1].EstUnObstacle)
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn + 1].PositionEnclo;
                    }
                    else
                    {
                        Player.EncloChoisi = Carte[Player.CurrentRow, Player.CurrentColumn - 1].PositionEnclo;
                    }
                }
                else
                {
                    MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Vous devez être à côté d'un enclo pour \n ajouter un animal");
            }
        }

        private void aideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Se déplacer: W,A,S,D \n" + 
                            "Arrêter la musique: M \n" +
                            "Nourrir un animal: N");
        }
    }
}
