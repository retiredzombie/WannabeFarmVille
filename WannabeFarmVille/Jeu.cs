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
using WannabeFarmVille.Animaux;
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
        int intFPS;
        private Joueur Player;
        private List<Visiteur> visiteurs;
        private Bitmap ImgJoe = new Bitmap(Properties.Resources.joeExotic);
        private Graphics g;
        private System.Media.SoundPlayer snd;
        Stopwatch stopwatchPayerConcierges;
        ThreadStart thStart; 
        Bitmap tuile;
        List<PictureBox> visiteursPicBox;
        MenuDepart menuDepart;
        Random rand;
        private bool gameover;
        Stopwatch stopWatch;
        DelegateRefresh refreshFormDelegate;
        int tailleTuile;
        List<Dechet> dechets;
        List<Concierge> concierges;
        Thread bouclePrincipale;

        public Jeu(MenuDepart menuDepart)
        {
            InitializeComponent();

            this.menuDepart = menuDepart;
            Init();

            Thread bouclePrincipale = new Thread(BouclePrincipaleDuJeu);
            bouclePrincipale.Start();
        }

        /*
         * Initialiser les composants du jeu/
         */
        private void Init()
        {
            stopWatch = new Stopwatch();
            stopwatchPayerConcierges = new Stopwatch();
            stopwatchPayerConcierges.Start();
            concierges = new List<Concierge>();
            stopWatch.Start();
            rand = new Random();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
            map = new Map(this.Width, this.Height, TilesetImageGenerator.GetTile(0));
            tuile = TilesetImageGenerator.GetTile(0);
            dechets = new List<Dechet>();
            for (int row = 0; row < 28; row++)
            {
                for (int column = 0; column < 40; column++)
                {
                    Carte[row, column] = new Tuile();
                    Carte[row, column].Ligne = row;
                    Carte[row, column].Colonne = column;
                }
            }
            refreshFormDelegate = new DelegateRefresh(Refresh);
            FPS = 1 / FPS * 1000;
            intFPS = Convert.ToInt32(FPS);
            RendreClotureSolide(2, 4);
            RendreClotureSolide(15, 4);
            RendreClotureSolide(15, 25);
            RendreClotureSolide(2, 25);
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
            visiteursPicBox = new List<PictureBox>();
            for (int i = 0; i < 10; i++)
            {
               AjouterVisiteurSpawn();
            }
        }

        private void RendreClotureSolide(int row, int column)
        {   
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                column++;
            }
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                row++;
            }
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
                column--;
            }
            for (int i = 0; i < 9; i++)
            {
                Carte[row, column].EstUnObstacle = true;
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


        /* Tout ce qui est dessiner sur l'écran.
         */
        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;

            this.affichageArgent.Text = this.Player.Argent.ToString() + "$";

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
                
                if (randX == 0 && !(vX >= 3 && vX <= 13 && vY >= 2 && vY <= 12) && !(vX >= 3 && vX <= 13 && vY >= 15 && vY <= 25) && !(vX >= 24 && vX <= 34 && vY >= 2 && vY <= 12) && !(vX >= 24 && vX <= 34 && vY >= 15 && vY <= 25))
                { 
                    visiteurs[i].X -= tuile.Width;
                    visiteurs[i].MovingX = -1;
                    visiteurs[i].MovingY = 0;
                } 
                else if (randX == 1 && !(vX >= 4 && vX <= 14 && vY >= 2 && vY <= 12) && !(vX >= 4 && vX <= 14 && vY >= 15 && vY <= 25) && !(vX >= 25 && vX <= 35 && vY >= 2 && vY <= 12) && !(vX >= 25 && vX <= 35 && vY >= 15 && vY <= 25))
                { 
                    visiteurs[i].X += tuile.Width;
                    visiteurs[i].MovingX = 1;
                    visiteurs[i].MovingY = 0;
                }

                if (randY == 0 && !(vY >= 3 && vY <= 13 && vX >= 4 && vX <= 13) && !(vY >= 3 && vY <= 13 && vX >= 25 && vX <= 34) && !(vY >= 16 && vY <= 26 && vX >= 4 && vX <= 13) && !(vY >= 16 && vY <= 26 && vX >= 25 && vX <= 34))
                {
                    visiteurs[i].Y -= tuile.Height;
                    visiteurs[i].MovingY = 1;
                    visiteurs[i].MovingX = 0;
                }
                else if (randY == 1 && !(vY >= 1 && vY <= 10 && vX >= 4 && vX <= 13) && !(vY >= 1 && vY <= 10 && vX >= 25 && vX <= 34) && !(vY >= 14 && vY <= 24 && vX >= 4 && vX <= 13) && !(vY >= 14 && vY <= 24 && vX >= 25 && vX <= 34))
                {
                    visiteurs[i].Y += tuile.Height;
                    visiteurs[i].MovingY = -1;
                    visiteurs[i].MovingX = 0;
                }

                visiteurs[i].ReloadImages();

                EchapeDechet(vX, vY);

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
         * Un visiteur a certaines chances d'échapper un déchet.
         */
        private void EchapeDechet(int x, int y)
        {
            int chance = this.rand.Next(0, 100);
            int limite = 1;

            if (chance < limite)
            {
                x *= tailleTuile;
                y *= tailleTuile;

                this.dechets.Add(new Dechet(x, y));
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
        }

        private void NewConcierge()
        {
            int cX = Player.X * tailleTuile;
            int cY = Player.Y * tailleTuile;

            concierges.Add(new Concierge(cX, cY));
        }

        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Jeu_KeyDown(object sender, KeyEventArgs e)
        {
            BougerJoueur(e);
        }

        private void BougerJoueur(KeyEventArgs e)
        {
            Console.WriteLine("KeyDown");
            if (e.KeyCode == Keys.U)
            {
                MessageBox.Show("(" + Player.CurrentRow + "," + Player.CurrentColumn + ")");
            }
            if(e.KeyCode == Keys.M)
            {
                snd.Stop();
            }
            if (e.KeyCode == Keys.S)
            {
                Player.MoveDown();
            }
            if (e.KeyCode == Keys.W)
            {
                Player.MoveUp();
            }
            if (e.KeyCode == Keys.D)
            {
                Player.MoveRight();
            }
            if (e.KeyCode == Keys.A)
            {
                Player.MoveLeft();
            }
            if(e.KeyCode == Keys.N)
            {
                int row = Player.CurrentRow, column = Player.CurrentColumn;
                try
                {
                    if (Carte[row + 1, column].EstUnObstacle || Carte[row - 1, column].EstUnObstacle
                       || Carte[row, column + 1].EstUnObstacle || Carte[row, column - 1].EstUnObstacle)
                    {
                        MessageBox.Show("Num NUnm nuM");
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
                Mouton mouton = new Mouton(Mouton.Nombre_Moutons);
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
                Grizzly grizzly = new Grizzly(Grizzly.Nombre_Grizzlys);
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
                Licorne licorne = new Licorne(Licorne.Nombre_Licornes);
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
                Rhino rhino = new Rhino(Rhino.Nombre_Rhinos);
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
                Buffle buffle = new Buffle(Buffle.Nombre_Buffles);
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
            int IntArgent;
            bool AjoutReussi = true;

            if (Modificateur)
            {
                Player.AjouterArgent(Montant);
            }
            else
            {
                if (Player.Argent - Montant >= 0)
                {
                    Player.RetirerArgent(Montant);
                }
                else
                {
                    AjoutReussi = false;
                }
            }

            Player.Argent -= 35;
            IntArgent = Player.Argent;
            TextArgent = IntArgent.ToString();
            
            affichageArgent.Text = TextArgent + "$";
            
            Lion lion = new Lion(0);
            Console.WriteLine("Un lion a été ajouté");
        }
        private void Jeu_FormClosing(object sender, FormClosingEventArgs e)
        {
            menuDepart.Dispose();
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
