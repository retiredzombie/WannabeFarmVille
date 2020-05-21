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
    public partial class InfoVisiteur : Form
    {
        public InfoVisiteur()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.CenterToScreen();
        }
        public void SetInformations(String nom, String genre, String temps)
        {
            this.Text = nom;
            lblVisNom.Text = nom;
            if (genre.Equals("Homme"))
            {
                IconVisFem.Visible = false;
                IconVisHom.Visible = true;
            }
            else
            {
                IconVisFem.Visible = true;
                IconVisHom.Visible = false;
            }
            lblGenre.Text = genre;
            lblTemps.Text = temps;
        }
    }
}
