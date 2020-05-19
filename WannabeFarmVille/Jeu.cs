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
using TestTilesetMario;
using WannabeFarmVille.Properties;

namespace WannabeFarmVille
{

    public partial class Jeu : Form
    {
        // OPTIONS

        double FPS = 1;


        // VARIABLES
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
        private ThreadStart thStart;
        private Bitmap tuile;
        private List<PictureBox> visiteursPicBox;
        private MenuDepart menuDepart;
        private Random rand;
        private bool gameover;
        private Stopwatch stopWatch;
        private DelegateRefresh refreshFormDelegate;
        private int tailleTuile;
        private List<Dechet> dechets;
        private List<Concierge> concierges;
        private Thread bouclePrincipale;
        private DateTime datejeu;
        private int NbConcierge = 0;
        private int NombreAnimaux = 0;
        

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
            stopWatch = new Stopwatch();
            stopwatchJeu = new Stopwatch();
            stopwatchPayerConcierges = new Stopwatch();
            stopwatchPayerConcierges.Start();
            concierges = new List<Concierge>();
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
            thStart = delegate { this.VisiteurThread(); };
            gameover = false;
            tailleTuile = 32;
            //Stream str = Properties.Resources.rd2;
            //System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            //snd.Play();
            Stream str = Properties.Resources.rd2;
            snd = new System.Media.SoundPlayer(str);
            snd.Play();
            Player.CurrentSprite = Player.JoeUpRight;
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
            visiteursPicBox = new List<PictureBox>();
            for (int i = 0; i < 10; i++)
            {
               AjouterVisiteurSpawn();
            }
        }
        /// <summary>
        /// Event qui se déclenche lorsque l'utilisateur clique avec sa sourie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Jeu_MouseClick(object sender, MouseEventArgs e)
        {
                for (int ligne = 0; ligne < 28; ligne++)
                {
                    for (int colonne = 0; colonne < 40; colonne++)
                    {
                        if (e.X > Carte[ligne, colonne].X && e.X < (32 + Carte[ligne, colonne].X) && e.Y > Carte[ligne, colonne].Y && e.Y < (32 + Carte[ligne, colonne].Y))
                        {
                            if (Carte[ligne, colonne].AnimalSurLaCase != null)
                            {
                                if (Player.PeutNourrir)
                                {
                                    if (Carte[ligne, colonne].PositionEnclo == Player.EncloChoisi)
                                    {
                                        Carte[ligne, colonne].AnimalSurLaCase.AFaim = false;
                                        MessageBox.Show("L'animal cri de joie et est rassasié !");
                                        Player.Argent -= 1;
                                        affichageArgent.Text = Player.Argent + "$";
                                    }
                                    else
                                    {
                                        MessageBox.Show("Vous ne pouvez pas nourrir un animal qui ne se trouve \n pas dans l'enclo à côté de vous.");
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Il n'y a pas d'animal sur cette case");
                            }
                        }
                    }
                }
        }
        /// <summary>
        /// Cette méthode définie les tuiles situées 
        /// à l'intérieur d'un enclo
        /// </summary>
        private void DefinirInterieurEnclos()
        {
            bool enclo = false;
            int compt = 0;
            for (int ligne = 0; ligne < 28; ligne++)
            {
                for (int colonne = 0; colonne < 40; colonne++)
                {
                    if (enclo && !Carte[ligne, colonne].EstUnObstacle)
                    {
                        Carte[ligne, colonne].EstDansUnEnclo = true;
                        if(compt == 0)
                        {
                            Lion lion = new Lion(117);
                            Carte[ligne, colonne].AnimalSurLaCase = lion;
                            compt++;
                        }
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
                Carte[row, column].EstUnObstacle = true;
                Carte[row, column].PositionEnclo = enclo;
                column++;
            }
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                Carte[row, column].PositionEnclo = enclo;
                row++;
            }
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                Carte[row, column].PositionEnclo = enclo;
                column--;
            }
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                Carte[row, column].PositionEnclo = enclo;
                row--;
            }
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
            this.visiteurs.Add(new Visiteur(tuile.Width * 19, tuile.Height * 25, rand));
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

                g.DrawString(nom, font, drawBrush, new Point(nomX, nomY));
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

            if (stopwatchJeu.Elapsed.TotalMilliseconds >= 5 / 365 * 3600)
            {
                this.datejeu = this.datejeu.AddDays(1);

                this.stopwatchJeu.Restart();
            }
        }

        /*
         * Fait bouger les visiteurs.
         */
        private void LogicVisiteurs()
        {
            Console.WriteLine("LogicVisiteurs.");
            
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
                    visiteurs[i].X -= tuile.Width;
                    visiteurs[i].MovingX = -1;
                    visiteurs[i].MovingY = 0;
                    visiteurs[i].ReloadImages();
                    if (visiteurs[i].CurrentColumn != 0)
                    {
                        Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = false;
                        visiteurs[i].CurrentColumn--;
                        Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = true;
                    }
                } 
                //DROITE
                else if (randX == 1 && !(vX >= 4 && vX <= 14 && vY >= 2 && vY <= 12) && !(vX >= 4 && vX <= 14 && vY >= 15 && vY <= 25) && !(vX >= 25 && vX <= 35 && vY >= 2 && vY <= 12) && !(vX >= 25 && vX <= 35 && vY >= 15 && vY <= 25))
                { 
                    visiteurs[i].X += tuile.Width;
                    visiteurs[i].MovingX = 1;
                    visiteurs[i].MovingY = 0;
                    visiteurs[i].ReloadImages();
                    if (visiteurs[i].CurrentColumn != 39)
                    {
                        Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = false;
                        visiteurs[i].CurrentColumn++;
                        Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = true;
                    }
                }
                //HAUT
                if (randY == 0 && !(vY >= 3 && vY <= 13 && vX >= 5 && vX <= 12) && !(vY >= 3 && vY <= 13 && vX >= 26 && vX <= 33) && !(vY >= 16 && vY <= 26 && vX >= 5 && vX <= 12) && !(vY >= 16 && vY <= 26 && vX >= 26 && vX <= 33))
                {
                    visiteurs[i].Y -= tuile.Height;
                    visiteurs[i].MovingY = 1;
                    visiteurs[i].MovingX = 0;
                    visiteurs[i].ReloadImages();
                    if (visiteurs[i].CurrentRow != 0)
                    {
                        Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = false;
                        visiteurs[i].CurrentRow--;
                        Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = true;
                    }
                }
                //BAS
                else if (randY == 1 && !(vY >= 1 && vY <= 10 && vX >= 5 && vX <= 12) && !(vY >= 1 && vY <= 10 && vX >= 26 && vX <= 33) && !(vY >= 14 && vY <= 24 && vX >= 5 && vX <= 12) && !(vY >= 14 && vY <= 24 && vX >= 26 && vX <= 33))
                {
                    visiteurs[i].Y += tuile.Height;
                    visiteurs[i].MovingY = -1;
                    visiteurs[i].MovingX = 0;
                    visiteurs[i].ReloadImages();
                    if (visiteurs[i].CurrentRow != 27)
                    {
                        Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = false;
                        visiteurs[i].CurrentRow++;
                        Carte[visiteurs[i].CurrentRow, visiteurs[i].CurrentColumn].EstUnObstacle = true;
                    }
                }

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
                    concierges[i].X -= tuile.Width;
                    concierges[i].MovingX = -1;
                    concierges[i].MovingY = 0;
                    concierges[i].ReloadImages();
                    if (concierges[i].CurrentColumn != 0)
                    {
                        Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = false;
                        concierges[i].CurrentColumn--;
                        Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = true;
                    }
                }
                //DROITE
                else if (randX == 1 && !(vX >= 4 && vX <= 14 && vY >= 2 && vY <= 12) && !(vX >= 4 && vX <= 14 && vY >= 15 && vY <= 25) && !(vX >= 25 && vX <= 35 && vY >= 2 && vY <= 12) && !(vX >= 25 && vX <= 35 && vY >= 15 && vY <= 25))
                {
                    concierges[i].X += tuile.Width;
                    concierges[i].MovingX = 1;
                    concierges[i].MovingY = 0;
                    concierges[i].ReloadImages();
                    if (concierges[i].CurrentColumn != 39)
                    {
                        Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = false;
                        concierges[i].CurrentColumn++;
                        Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = true;
                    }
                }
                //HAUT
                if (randY == 0 && !(vY >= 3 && vY <= 13 && vX >= 4 && vX <= 13) && !(vY >= 3 && vY <= 13 && vX >= 25 && vX <= 34) && !(vY >= 16 && vY <= 26 && vX >= 4 && vX <= 13) && !(vY >= 16 && vY <= 26 && vX >= 25 && vX <= 34))
                {
                    concierges[i].Y -= tuile.Height;
                    concierges[i].MovingY = 1;
                    concierges[i].MovingX = 0;
                    concierges[i].ReloadImages();
                    if (concierges[i].CurrentRow != 0)
                    {
                        Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = false;
                        concierges[i].CurrentRow--;
                        Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = true;
                    }
                }
                //BAS
                else if (randY == 1 && !(vY >= 1 && vY <= 10 && vX >= 4 && vX <= 13) && !(vY >= 1 && vY <= 10 && vX >= 25 && vX <= 34) && !(vY >= 14 && vY <= 24 && vX >= 4 && vX <= 13) && !(vY >= 14 && vY <= 24 && vX >= 25 && vX <= 34))
                {
                    concierges[i].Y += tuile.Height;
                    concierges[i].MovingY = -1;
                    concierges[i].MovingX = 0;
                    concierges[i].ReloadImages();
                    if (concierges[i].CurrentRow != 27)
                    {
                        Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = false;
                        concierges[i].CurrentRow++;
                        Carte[concierges[i].CurrentRow, concierges[i].CurrentColumn].EstUnObstacle = true;
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
                Carte[row, column].DechetSurTuile = Trash;
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

        private void VisiteurThread()
        {

        }

        private void embaucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewConcierge();
            NbConcierge++;
            conciergesToolStripMenuItem.Text = NbConcierge + " Concierges";
        }

        private void NewConcierge()
        {
            int cX = Player.CurrentColumn * tailleTuile;
            int cY = Player.CurrentRow * tailleTuile;
            try
            {
                concierges.Add(new Concierge(cX, cY, Player.CurrentRow - 1, Player.CurrentColumn));
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
                Player.MoveDown();
                try
                {
                    if (Carte[row + 1, column].EstUnObstacle || Carte[row - 1, column].EstUnObstacle
                       || Carte[row, column + 1].EstUnObstacle || Carte[row, column - 1].EstUnObstacle)
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
            if (e.KeyCode == Keys.W)
            {
                Player.MoveUp();
                try
                {
                    if (Carte[row + 1, column].EstUnObstacle || Carte[row - 1, column].EstUnObstacle
                       || Carte[row, column + 1].EstUnObstacle || Carte[row, column - 1].EstUnObstacle)
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
            if (e.KeyCode == Keys.D)
            {
                Player.MoveRight();
                try
                {
                    if (Carte[row + 1, column].EstUnObstacle || Carte[row - 1, column].EstUnObstacle
                       || Carte[row, column + 1].EstUnObstacle || Carte[row, column - 1].EstUnObstacle)
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
            if (e.KeyCode == Keys.A)
            {
                Player.MoveLeft();
                try
                {
                    if (Carte[row + 1, column].EstUnObstacle || Carte[row - 1, column].EstUnObstacle
                       || Carte[row, column + 1].EstUnObstacle || Carte[row, column - 1].EstUnObstacle)
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
            if(e.KeyCode == Keys.N)
            {
                try
                {
                    if (Carte[row + 1, column].EstUnObstacle || Carte[row - 1, column].EstUnObstacle
                       || Carte[row, column + 1].EstUnObstacle || Carte[row, column - 1].EstUnObstacle)
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
         * Déduit 20$ du joueur et instancie un nouveau Mouton
         */
        private void mouton20ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(20, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                //Mouton mouton = new Mouton(Mouton.Nombre_Moutons);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter un mouton.");
            }
        }

        /**
         * Déduit 30$ du joueur et instancie un nouveau Grizzly
         */
        private void grizzly30ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(30, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                //Grizzly grizzly = new Grizzly(Grizzly.Nombre_Grizzlys);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter un grizzly.");
            }
        }

        /**
         * Déduit 35$ du joueur et instancie un nouveau Lion
         */
        private void lion35ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(35, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                Lion lion = new Lion(Lion.Nombre_Lions);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter un lion.");
            }
        }

        /**
         * Déduit 50$ du joueur et instancie une nouvelle Licorne
         */
        private void licorne50ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(50, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                //Licorne licorne = new Licorne(Licorne.Nombre_Licornes);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter une licorne.");
            }
        }

        /**
         * Déduit 40$ du joueur et instancie un nouveau Rhinocéros
         */
        private void rhinocéros40ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(40, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                //Rhino rhino = new Rhino(Rhino.Nombre_Rhinos);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter un rhinocéros.");
            }
        }

        /**
         * Déduit 40$ du joueur et instancie un nouveau Buffle
         */
        private void buffle40ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool PeutAjouter;

            PeutAjouter = Modifier_Argent(40, false);

            if (PeutAjouter)
            {
                Ajouter_Animal();
                //Buffle buffle = new Buffle(Buffle.Nombre_Buffles);
            }
            else
            {
                Console.WriteLine("Tu n'as pas assez d'argent pour acheter un buffle.");
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

            affichageArgent.Text = TextArgent + " $";

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
            MessageBox.Show("Vous avez fait un profit de " + this.Player.ProfitTotal.ToString() + "$.");

            this.bouclePrincipale.Abort();

            snd.Stop();
            snd.Dispose();
            menuDepart.Dispose();
            this.Dispose();
        }

        private void PicUpRight_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

        }

        private void aideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Se déplacer: W,A,S,D \n" + 
                            "Arrêter la musique: M \n" +
                            "Nourrir un animal: N");
        }
    }
}
